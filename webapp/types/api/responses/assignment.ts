export interface AssignmentResponse {
    id: string;
    name: string;
    description?: string;
    projectId: string;
    assignmentLabelId?: number;
    assignmentStatusId: string;
    assigneeId?: string;
    isPlanned: boolean;
    estimation?: number;
    deadline?: Date;
    completedDate?: Date;
    assignmentLabelName?: string;
    assigneeUsername?: string;
    reporterUsername: string; 
}