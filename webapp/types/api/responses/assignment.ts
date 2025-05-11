export interface AssignmentResponse {
    id: string;
    name: string;
    description?: string;
    projectId: string;
    assignmentLabelId?: number;
    assignmentStatusId: string;
    isPlanned: boolean;
    estimation?: number;
    deadline?: Date;
    completedDate?: Date;
}