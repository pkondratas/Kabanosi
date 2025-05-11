interface AssignmentRequest {
    name: string;
    description?: string;
    assignmentStatusId?: string;
    assignmentLabelId?: string;
    isPlanned: boolean;
    estimation?: number;
    deadline?: Date;
}