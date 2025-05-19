"use client";

import { AssignmentResponse } from "@/types/api/responses/assignment";
import AssignmentCard from "./assignment-card";
import { AssignmentStatusResponse } from "@/types/api/responses/assignment-status";
import { ProjectMemberResponse } from "@/types/api/responses/project-member";
import { AssignmentLabelResponse } from "@/types/api/responses/assignment-label";

interface AssignmentColumnProps {
  projectId: string;
  assignments: AssignmentResponse[];
  projectMembers: ProjectMemberResponse[];
  status: AssignmentStatusResponse;
  onMoveLeft?: () => void;
  onMoveRight?: () => void;
  canMoveLeft: boolean;
  canMoveRight: boolean;
  statuses: AssignmentStatusResponse[];
  labels: AssignmentLabelResponse[];
  onAssignmentStatusChange: (assignmentId: string, newStatusId: string) => void
}

export function AssignmentColumn({
  projectId,
  assignments,
  projectMembers,
  status,
  onMoveLeft,
  onMoveRight,
  canMoveLeft,
  canMoveRight,
  statuses,
  labels,
  onAssignmentStatusChange
}: AssignmentColumnProps) {
  return (
    <div className="flex flex-col w-72 bg-gray-100 p-4 rounded-lg shadow-lg">
      <div className="flex justify-between items-center mb-2">
        {canMoveLeft ? (<button onClick={onMoveLeft}>&larr;</button>) : (<></>)}
        <h3 className="text-xl font-semibold text-center flex-1">{status.name}</h3>
        {canMoveRight ? (<button onClick={onMoveRight}>&rarr;</button>) : (<></>)}
      </div>
      <div className="space-y-4">
        {assignments.map((assignment) => (
          <AssignmentCard 
            projectId={projectId}
            key={assignment.id} 
            assignment={assignment} 
            projectMembers={projectMembers}
            statuses={statuses} 
            labels={labels}
            onStatusChange={onAssignmentStatusChange} />
        ))}
      </div>
    </div>
  );
};