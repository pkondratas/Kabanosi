"use client";

import { AssignmentResponse } from "@/types/api/responses/assignment";
import { AssignmentStatusResponse } from "@/types/api/responses/assignment-status";
import { useState } from "react";
import { Dialog, DialogContent, DialogTrigger } from "@/components/ui/dialog";
import AssignmentDetailsDialog from "../shared/AssignmentDetailsDialog";
import { AssignmentLabelResponse } from "@/types/api/responses/assignment-label";
import { ProjectMemberResponse } from "@/types/api/responses/project-member";

interface AssignmentCardProps {
  projectId: string;
  assignment: AssignmentResponse;
  projectMembers: ProjectMemberResponse[];
  statuses: AssignmentStatusResponse[];
  labels: AssignmentLabelResponse[];
  onStatusChange: (assignmentId: string, newStatusId: string) => void;
}

export default function AssignmentCard({
  projectId,
  assignment,
  projectMembers,
  statuses,
  labels,
  onStatusChange,
}: AssignmentCardProps) {
  const [dialogOpened, setDialogOpened] = useState(false);
  const [localAssignment, setLocalAssignment] = useState(assignment);
  const [status, setStatus] = useState(assignment.assignmentStatusId);

  const handleStatusChange = (newStatusId: string) => {
    setStatus(newStatusId);
    onStatusChange(localAssignment.id, newStatusId);
  };

  const handleSaveChanges = (updatedAssignment: AssignmentResponse) => {
    if (updatedAssignment.assignmentStatusId !== localAssignment.assignmentStatusId)
      handleStatusChange(updatedAssignment.assignmentStatusId);
    setLocalAssignment(updatedAssignment);
    setDialogOpened(false);
  };

  return (
    <Dialog open={dialogOpened} onOpenChange={setDialogOpened}>
      <div className="bg-white p-4 rounded-md shadow-sm hover:bg-gray-50 cursor-pointer">
        <DialogTrigger asChild>
          <div>
            <p className="text-lg font-medium text-gray-700">
              {localAssignment.name}
            </p>
            <p className="text-sm text-gray-500">
              Estimation: {localAssignment.estimation ?? "None"}
            </p>
            <p className="text-sm text-gray-500">
              Label:{" "}
              {localAssignment.assignmentLabelId
                ? labels.find((l) => l.id == localAssignment.assignmentLabelId)
                    ?.name
                : "None"}
            </p>
            <p className="text-sm text-gray-500">
              Assignee:{" "}
              {localAssignment.assigneeId
                ? projectMembers.find(
                    (pm) => pm.id == localAssignment.assigneeId
                  )?.username
                : "Unassigned"}
            </p>
          </div>
        </DialogTrigger>
        <div className="mt-2">
          <label className="text-sm text-gray-600 mr-1" htmlFor="status">
            Move to:
          </label>
          <select
            id="status"
            value={status}
            onChange={(e) => handleStatusChange(e.target.value)}
            className="border rounded pl-2 py-1 text-sm"
          >
            {statuses.map((s) => (
              <option key={s.id} value={s.id}>
                {s.name}
              </option>
            ))}
          </select>
        </div>
      </div>

      <DialogContent
        onClose={() => setDialogOpened(false)}
        className="max-w-[600px]"
      >
        <AssignmentDetailsDialog
          projectId={projectId}
          assignment={localAssignment}
          projectMembers={projectMembers}
          statuses={statuses}
          labels={labels}
          onChangesSaved={(updatedAssignment) =>
            handleSaveChanges(updatedAssignment)
          }
        />
      </DialogContent>
    </Dialog>
  );
}
