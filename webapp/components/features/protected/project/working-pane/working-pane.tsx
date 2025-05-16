"use client";

import { AssignmentColumn } from "./assignment-column";
import { AssignmentStatusResponse } from "@/types/api/responses/assignment-status";
import { AssignmentResponse } from "@/types/api/responses/assignment";
import { useState } from "react";
import { reorderAssignmentStatuses } from "@/lib/actions/assignment-status.actions";
import { changeAssignmentStatus } from "@/lib/actions/assignment.actions";

interface WorkingPaneProps {
  statuses: AssignmentStatusResponse[];
  assignments: AssignmentResponse[];
  projectId: string;
}

export function WorkingPane({
  statuses,
  assignments,
  projectId,
}: WorkingPaneProps) {
  const [statusOrder, setStatusOrder] = useState<string[]>(
    statuses.map((s) => s.id)
  );
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

  const handleAssignmentStatusChange = (
    assignmentId: string,
    newStatusId: string
  ) => {
    setAssignmentsByStatus((prev) => {
      const updated = { ...prev };
      const currentStatusId = Object.keys(updated).find((key) =>
        updated[key].some((a) => a.id === assignmentId)
      );
      if (!currentStatusId || currentStatusId === newStatusId) return prev;
      updated[currentStatusId] = updated[currentStatusId].filter(
        (a) => a.id !== assignmentId
      );
      const movedAssignment = prev[currentStatusId].find(
        (a) => a.id === assignmentId
      );
      if (movedAssignment) {
        movedAssignment.assignmentStatusId = newStatusId;
        if (!updated[newStatusId]) updated[newStatusId] = [];
        updated[newStatusId] = [...updated[newStatusId], movedAssignment];
      }
      return updated;
    });
    changeAssignmentStatus(projectId, assignmentId, newStatusId);
  };

  return (
    <div className="flex gap-4 overflow-x-auto p-4">
      {statusOrder.map((statusId, index) => {
        const status = statuses.find((s) => s.id === statusId);
        if (!status) return null;
        return (
          <AssignmentColumn
            key={status.id}
            status={status}
            assignments={assignmentsByStatus[status.id] || []}
            onMoveLeft={() => moveStatusLeft(index)}
            onMoveRight={() => moveStatusRight(index)}
            canMoveLeft={index > 0}
            canMoveRight={index < statusOrder.length - 1}
            statuses={statuses}
            onAssignmentStatusChange={handleAssignmentStatusChange}
          />
        );
      })}
    </div>
  );
}
