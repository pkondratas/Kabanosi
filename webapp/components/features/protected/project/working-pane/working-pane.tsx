"use client";

import { AssignmentColumn } from "./assignment-column";
import { AssignmentStatusResponse } from "@/types/api/responses/assignment-status";
import { AssignmentResponse } from "@/types/api/responses/assignment";
import { useState } from "react";
import {
  createAssignmentStatus,
  reorderAssignmentStatuses,
} from "@/lib/actions/assignment-status.actions";
import { changeAssignmentStatus } from "@/lib/actions/assignment.actions";
import { AssignmentLabelResponse } from "@/types/api/responses/assignment-label";
import { ProjectMemberResponse } from "@/types/api/responses/project-member";

interface WorkingPaneProps {
  initialStatuses: AssignmentStatusResponse[];
  assignments: AssignmentResponse[];
  projectMembers: ProjectMemberResponse[];
  labels: AssignmentLabelResponse[];
  projectId: string;
}

export function WorkingPane({
  initialStatuses,
  assignments,
  projectMembers,
  labels,
  projectId,
}: WorkingPaneProps) {
  const [statuses, setStatuses] = useState(initialStatuses);
  const [statusOrder, setStatusOrder] = useState<string[]>(initialStatuses.map((s) => s.id));
  const [assignmentsByStatus, setAssignmentsByStatus] = useState<{
    [key: string]: AssignmentResponse[];
  }>(() => {
    const map: { [key: string]: AssignmentResponse[] } = {};
    assignments.forEach((assignment) => {
      if (!map[assignment.assignmentStatusId]) {
        map[assignment.assignmentStatusId] = [];
      }
      map[assignment.assignmentStatusId].push(assignment);
    });
    return map;
  });

  const [isCreatingStatus, setIsCreatingStatus] = useState(false);
  const [newStatusName, setNewStatusName] = useState("");

  const moveStatusLeft = (index: number) => {
    if (index === 0) return;
    const newOrder = [...statusOrder];
    [newOrder[index - 1], newOrder[index]] = [
      newOrder[index],
      newOrder[index - 1],
    ];
    setStatusOrder(newOrder);
    reorderAssignmentStatuses(projectId, { idsInOrder: newOrder });
  };

  const moveStatusRight = (index: number) => {
    if (index === statusOrder.length - 1) return;
    const newOrder = [...statusOrder];
    [newOrder[index], newOrder[index + 1]] = [
      newOrder[index + 1],
      newOrder[index],
    ];
    setStatusOrder(newOrder);
    reorderAssignmentStatuses(projectId, { idsInOrder: newOrder });
  };

  const handleAssignmentStatusChange = async (assignmentId: string, newStatusId: string) => {
    const updatedAssignment = await changeAssignmentStatus(projectId, assignmentId, newStatusId);

    setAssignmentsByStatus((prev) => {
      const updated = { ...prev };
      const currentStatusId = Object.keys(updated).find((key) =>
        updated[key].some((a) => a.id === assignmentId)
      );

      if (!currentStatusId || currentStatusId === newStatusId) 
        return prev;

      updated[currentStatusId] = updated[currentStatusId].filter((a) => a.id !== assignmentId);

      if (!updated[newStatusId]) 
        updated[newStatusId] = [];

      updated[newStatusId] = [...updated[newStatusId], updatedAssignment];

      return updated;
    });
  };

  const handleCreateStatus = async () => {
    if (!newStatusName.trim()) return;

    const newStatus = await createAssignmentStatus(projectId, {
      name: newStatusName.trim(),
    });

    setStatuses((prev) => [...prev, newStatus]);
    setStatusOrder((prev) => [...prev, newStatus.id]);
    setAssignmentsByStatus((prev) => ({
      ...prev,
      [newStatus.id]: [],
    }));

    setIsCreatingStatus(false);
    setNewStatusName("");
  };

  return (
    <div className="p-4">
      <div className="mb-4">
        {isCreatingStatus ? (
          <div className="flex items-center gap-2">
            <input
              className="border px-2 py-1 rounded"
              placeholder="New status name"
              value={newStatusName}
              onChange={(e) => setNewStatusName(e.target.value)}
            />
            <button
              className="bg-blue-500 text-white px-3 py-1 rounded"
              onClick={handleCreateStatus}
            >
              Create
            </button>
            <button
              className="bg-gray-300 text-black px-3 py-1 rounded"
              onClick={() => {
                setIsCreatingStatus(false);
                setNewStatusName("");
              }}
            >
              Cancel
            </button>
          </div>
        ) : (
          <button
            className="bg-green-500 text-white px-4 py-2 rounded"
            onClick={() => setIsCreatingStatus(true)}
          >
            Create New Status
          </button>
        )}
      </div>

      <div className="flex gap-4 overflow-x-auto">
        {statusOrder.map((statusId, index) => {
          const status = statuses.find((s) => s.id === statusId);
          if (!status) return null;
          return (
            <AssignmentColumn
              projectId={projectId}
              key={status.id}
              status={status}
              assignments={assignmentsByStatus[status.id] || []}
              projectMembers={projectMembers}
              onMoveLeft={() => moveStatusLeft(index)}
              onMoveRight={() => moveStatusRight(index)}
              canMoveLeft={index > 0}
              canMoveRight={index < statusOrder.length - 1}
              statuses={statuses}
              labels={labels}
              onAssignmentStatusChange={handleAssignmentStatusChange}
            />
          );
        })}
      </div>
    </div>
  );
}
