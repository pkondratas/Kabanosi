"use client";

import {
  changeAssignmentStatus,
  createAssignment,
} from "@/lib/actions/assignment.actions";
import { AssignmentResponse } from "@/types/api/responses/assignment";
import { useState } from "react";
import AssignmentSection from "./assignment-section";
import { AssignmentStatusResponse } from "@/types/api/responses/assignment-status";

interface BacklogProps {
  assignments: AssignmentResponse[];
  statuses: AssignmentStatusResponse[];
  projectId: string;
}

export default function Backlog({
  assignments: initialAssignments,
  statuses,
  projectId,
}: BacklogProps) {
  const [assignments, setAssignments] =
    useState<AssignmentResponse[]>(initialAssignments);

  const handleAddAssignment = async (isPlanned: boolean, name: string) => {
    const request: AssignmentRequest = {
      name: name,
      isPlanned: isPlanned,
    };

    const newAssignment = await createAssignment(projectId, request);
    setAssignments([...assignments, newAssignment]);
  };

  const handleStatusChange = async (
    assignmentId: string,
    newStatusId: string
  ) => {
    changeAssignmentStatus(projectId, assignmentId, newStatusId);
  };

  return (
    <div className="p-6 flex flex-col items-center gap-10">
      <AssignmentSection
        title="Working Pane"
        assignments={assignments.filter((a) => a.isPlanned)}
        statuses={statuses}
        onAddAssignment={handleAddAssignment}
        onStatusChange={handleStatusChange}
        isPlanned={true}
      />
      <AssignmentSection
        title="Backlog"
        assignments={assignments.filter((a) => !a.isPlanned)}
        statuses={statuses}
        onAddAssignment={handleAddAssignment}
        onStatusChange={handleStatusChange}
        isPlanned={false}
      />
    </div>
  );
}
