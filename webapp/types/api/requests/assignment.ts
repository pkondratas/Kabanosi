interface AssignmentRequest {
    name: string;
    description?: string;
    assignmentStatusId?: string;
    assignmentLabelId?: number;
    isPlanned: boolean;
    estimation?: number;
    deadline?: Date;
}

interface UpdateAssignmentRequest {
    name: string;
    description?: string;
    assignmentLabelId?: number;
    isPlanned: boolean;
    estimation?: number;
    deadline?: Date;
    completedDate?: Date;
    assigneeId?: string;
    assignmentStatusId: string;
}