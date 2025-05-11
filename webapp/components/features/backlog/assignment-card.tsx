"use client"

import { AssignmentResponse } from "@/types/api/responses/assignment";
import { AssignmentStatusResponse } from "@/types/api/responses/assignment-status";
import { useState } from "react";

interface AssignmentCardProps {
	assignment: AssignmentResponse;
	statuses: AssignmentStatusResponse[];
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
		<div className="bg-white px-6 py-2 shadow rounded-lg mb-2 w-full flex items-center justify-between">
			<h3 className="text-base font-medium">{assignment.name}</h3>

			<div className="flex items-center space-x-3">
				<p className="text-sm text-gray-600">Label: {assignment.assignmentLabelName ? assignment.assignmentLabelName : "None"}</p>
				<select
					className="ml-4 border border-gray-300 rounded px-2 py-1 text-sm"
					value={status}
					onChange={handleStatusChange}
				>
					{statuses.map(s => (
						<option key={s.id} value={s.id}>
							{s.name}
						</option>
					))}
				</select>
			</div>
		</div>
	);
}