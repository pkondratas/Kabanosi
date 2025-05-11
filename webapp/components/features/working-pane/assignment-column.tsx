"use client";

import AssignmentCard from "./assignment-card";

interface AssignmentColumnProps {
  status: { id: string; name: string };
  assignments: { id: string; name: string; assignmentStatusId: string; estimation?: number; label?: string }[];
  onMoveLeft?: () => void;
  onMoveRight?: () => void;
  canMoveLeft: boolean;
  canMoveRight: boolean;
  statuses: { id: string; name: string }[];
  onAssignmentStatusChange: (assignmentId: string, newStatusId: string) => void
}

export function AssignmentColumn({ 
  status, 
  assignments, 
  onMoveLeft, 
  onMoveRight,
  canMoveLeft,
  canMoveRight,
  statuses,
  onAssignmentStatusChange
 }: AssignmentColumnProps) {
  return (
<div className="flex flex-col w-72 bg-gray-100 p-4 rounded-lg shadow-lg">
      <div className="flex justify-between items-center mb-2">
        { canMoveLeft ? (<button onClick={onMoveLeft}>&larr;</button>) : (<></>) }
        <h3 className="text-xl font-semibold text-center flex-1">{status.name}</h3>
        { canMoveRight ? (<button onClick={onMoveRight}>&rarr;</button>) : (<></>)}
      </div>
      <div className="space-y-4">
        {assignments.map((assignment) => (
          <AssignmentCard key={assignment.id} assignment={assignment} statuses={statuses} onStatusChange={onAssignmentStatusChange} />
        ))}
      </div>
    </div>
  );
};