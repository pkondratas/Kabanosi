"use client"

import { AssignmentResponse } from "@/types/api/responses/assignment";
import AssignmentCard from "./assignment-card";
import { useState } from "react";
import { AssignmentStatusResponse } from "@/types/api/responses/assignment-status";

interface AssignmentSectionProps {
	title: string;
	assignments: AssignmentResponse[];
	statuses: AssignmentStatusResponse[];
	onAddAssignment: (isPlanned: boolean, name: string) => void;
	onStatusChange: (assignmentId: string, newStatusId: string) => void;
	isPlanned: boolean;
}

export default function AssignmentSection({ title, assignments, statuses, onAddAssignment, onStatusChange, isPlanned }: AssignmentSectionProps) {
  const [isAdding, setIsAdding] = useState(false);
  const [newName, setNewName] = useState("");

  const handleAddClick = () => {
    if (isAdding && newName.trim()) {
      onAddAssignment(isPlanned, newName.trim());
      setNewName("");
      setIsAdding(false);
    } else {
      setIsAdding(true);
    }
  };

	const handleCancelClick = () => {
		setNewName("");
		setIsAdding(false);
	}

	return (
    <div className="w-full max-w-5xl">
      <div className="flex justify-between items-center mb-4">
        <h2 className="text-xl font-bold">{title}</h2>
        <div className="flex gap-2 items-center">
          {isAdding && (
            <input
              type="text"
              value={newName}
              onChange={(e) => setNewName(e.target.value)}
              className="border rounded px-2 py-1"
              placeholder="Task title"
            />
          )}
          <button
            onClick={handleAddClick}
            className="bg-blue-500 text-white px-4 py-1 rounded hover:bg-blue-600"
          >
            {isAdding ? "Create" : "+ Add Task"}
          </button>
					{isAdding ? <button
						onClick={handleCancelClick}
						className="bg-blue-500 text-white px-4 py-1 rounded hover:bg-blue-600">Cancel</button> : (<></>)}
        </div>
      </div>
      <div>{assignments.map(a => 
				<AssignmentCard 
					key={a.id} 
					assignment={a}
					statuses={statuses}
					onStatusChange={onStatusChange} />)}
			</div>
    </div>
	);
}