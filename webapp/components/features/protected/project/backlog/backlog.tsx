"use client";

import {
  changeAssignmentStatus,
  createAssignment,
} from "@/lib/actions/assignment.actions";
import { AssignmentResponse } from "@/types/api/responses/assignment";
import { useState } from "react";
import AssignmentSection from "./assignment-section";
import { AssignmentStatusResponse } from "@/types/api/responses/assignment-status";
import { AssignmentLabelResponse } from "@/types/api/responses/assignment-label";
import { ProjectMemberResponse } from "@/types/api/responses/project-member";

interface BacklogProps {
  assignments: AssignmentResponse[];
  statuses: AssignmentStatusResponse[];
  labels: AssignmentLabelResponse[];
  projectMembers: ProjectMemberResponse[];
  projectId: string;
}

export default function Backlog({
  assignments: initialAssignments,
  statuses,
  labels,
  projectMembers,
  projectId,
}: BacklogProps) {
  const [assignments, setAssignments] = useState<AssignmentResponse[]>(initialAssignments);

  const handleAddAssignment = async (isPlanned: boolean, name: string) => {
    const request: AssignmentRequest = {
      name: name,
      isPlanned: isPlanned,
    };

    const newAssignment = await createAssignment(projectId, request);
    setAssignments([...assignments, newAssignment]);
  };

  const handleIsPlannedChange = async (assignmentId: string, isPlanned: boolean) => {
    setAssignments((prev) =>
      prev.map((assignment) => assignment.id === assignmentId ? { ...assignment, isPlanned } : assignment
    ));
  };

  return (
    <div className="p-6 flex flex-col items-center gap-10">
      <AssignmentSection
        title="Working Pane"
        projectId={projectId}
        assignments={assignments.filter((a) => a.isPlanned)}
        statuses={statuses}
        labels={labels}
        projectMembers={projectMembers}
        onAddAssignment={handleAddAssignment}
        onIsPlannedChange={handleIsPlannedChange}
        isPlanned={true}
      />
      <AssignmentSection
        title="Backlog"
        projectId={projectId}
        assignments={assignments.filter((a) => !a.isPlanned)}
        statuses={statuses}
        labels={labels}
        projectMembers={projectMembers}
        onAddAssignment={handleAddAssignment}
        onIsPlannedChange={handleIsPlannedChange}
        isPlanned={false}
      />
    </div>
  );
}
