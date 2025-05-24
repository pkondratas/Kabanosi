"use client";

import { Dialog, DialogContent, DialogTrigger } from "@/components/ui/dialog";
import { AssignmentResponse } from "@/types/api/responses/assignment";
import { AssignmentLabelResponse } from "@/types/api/responses/assignment-label";
import { AssignmentStatusResponse } from "@/types/api/responses/assignment-status";
import { ProjectMemberResponse } from "@/types/api/responses/project-member";
import { useState } from "react";
import AssignmentDetailsDialog from "../shared/AssignmentDetailsDialog";

interface AssignmentCardProps {
  projectId: string;
  assignment: AssignmentResponse;
  projectMembers: ProjectMemberResponse[];
  statuses: AssignmentStatusResponse[];
  labels: AssignmentLabelResponse[];
  onIsPlannedChange: (updatedAssignment: AssignmentResponse, isPlanned: boolean) => void;
}

export default function AssignmentCard({
  projectId,
  assignment,
  projectMembers,
  statuses,
  labels,
  onIsPlannedChange,
}: AssignmentCardProps) {
  const [dialogOpened, setDialogOpened] = useState(false);
  const [localAssignment, setLocalAssignment] = useState(assignment);

  const handleSaveChanges = (updatedAssignment: AssignmentResponse) => {
		if (updatedAssignment.isPlanned !== localAssignment.isPlanned)
			onIsPlannedChange(updatedAssignment, updatedAssignment.isPlanned);
    setLocalAssignment(updatedAssignment);
    setDialogOpened(false);
  };

  return (
    <Dialog open={dialogOpened} onOpenChange={setDialogOpened}>
      <div className="bg-white px-6 py-2 shadow rounded-lg mb-2 w-full flex items-center justify-between">
        <DialogTrigger asChild>
          <div className="w-full flex items-center justify-between">
            <h3 className="text-base font-medium">{localAssignment.name}</h3>

            <div className="flex items-center space-x-3">
              <p className="text-sm text-gray-600">
                Label: {localAssignment.assignmentLabelId
                ? labels.find((l) => l.id == localAssignment.assignmentLabelId)
                    ?.name
                : "None"}
              </p>
              <p className="text-sm text-gray-600">
                Assignee: {localAssignment.assigneeId
                ? projectMembers.find(
                    (pm) => pm.id == localAssignment.assigneeId
                  )?.username
                : "Unassigned"}
              </p>
							<p className="text-sm text-gray-600">
								Estimation: {localAssignment.estimation ?? "None"}
							</p>
							<p className="text-sm text-gray-600">
								Status: {statuses.find(s => s.id == localAssignment.assignmentStatusId)?.name}
							</p>
            </div>
          </div>
        </DialogTrigger>
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
