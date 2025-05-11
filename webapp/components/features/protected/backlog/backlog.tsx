"use client"

import { changeAssignmentStatus, createAssignment, getAssignments } from "@/lib/actions/assignment.actions";
import { AssignmentResponse } from "@/types/api/responses/assignment";
import { usePathname } from "next/navigation";
import { useEffect, useState } from "react";
import AssignmentSection from "./assignment-section";
import { AssignmentStatusResponse } from "@/types/api/responses/assignment-status";
import { getAssignmentStatuses } from "@/lib/actions/assignment-status.actions";

export default function Backlog() {
	const pathname = usePathname();
	const projectId = pathname.split("/")[1];

	const [assignments, setAssignments] = useState<AssignmentResponse[]>([]);
	const [statuses, setStatuses] = useState<AssignmentStatusResponse[]>([]);

	useEffect(() => {
		if (!projectId) {
			return;
		}

		const fetchData = async () => {
			const [assignmentsData, statusesData] = await Promise.all([
				getAssignments(projectId),
				getAssignmentStatuses(projectId)
			]);

			setAssignments(assignmentsData);
			setStatuses(statusesData);
		};

		fetchData();
	}, [projectId]);

	const handleAddAssignment = async (isPlanned: boolean, name: string) => {
		const request: AssignmentRequest = {
			name: name,
			isPlanned: isPlanned
		};

		const newAssignment = await createAssignment(projectId, request);

		setAssignments([...assignments, newAssignment]);
	};

	const handleStatusChange = async (assignmentId: string, newStatusId: string) => {
		changeAssignmentStatus(projectId, assignmentId, newStatusId);
	}

	return (
		<div className="p-6 flex flex-col items-center gap-10">
			<AssignmentSection
				title="Working Pane"
				assignments={assignments.filter(a => a.isPlanned)}
				statuses={statuses}
				onAddAssignment={handleAddAssignment}
				onStatusChange={handleStatusChange}
				isPlanned={true} />
			<AssignmentSection
				title="Backlog"
				assignments={assignments.filter(a => !a.isPlanned)}
				statuses={statuses}
				onAddAssignment={handleAddAssignment}
				onStatusChange={handleStatusChange}
				isPlanned={false}
			/>
		</div>
	);
}