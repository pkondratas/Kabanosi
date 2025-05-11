"use client";

import { useState } from "react";

interface AssignmentCardProps {
  assignment: { 
    id: string; 
    name: string;
    assignmentStatusId: string;
    estimation?: number;
    label?: string;
  };
  statuses: { 
    id: string; 
    name: string; 
  }[];
  onStatusChange: (assignmentId: string, newStatusId: string) => void;
}
  
export default function AssignmentCard({ assignment, statuses, onStatusChange }: AssignmentCardProps) {
  const [status, setStatus] = useState(assignment.assignmentStatusId);

  const handleStatusChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const newStatusId = e.target.value;

    setStatus(newStatusId);
    onStatusChange(assignment.id, newStatusId);
  };

  return (
    <div className="bg-white p-4 rounded-md shadow-sm hover:bg-gray-50">
      <p className="text-lg font-medium text-gray-700">{assignment.name}</p>
      <p className="text-sm text-gray-500">
        Estimation: { assignment.estimation ? assignment.estimation : "None" }
      </p>
      <p className="text-sm text-gray-500">
        Label: { assignment.label ? assignment.label : "None" }
      </p>
      <div className="mt-2">
        <label className="text-sm text-gray-600 mr-1" htmlFor="status">
          Move to:
        </label>
        <select
          id="status"
          value={status}
          onChange={handleStatusChange}
          className="border rounded pl-2 py-1 text-sm"
        >
          {statuses.map(s => (<option key={s.id} value={s.id}>{s.name}</option>))}
        </select>
      </div>
    </div>
  );
};