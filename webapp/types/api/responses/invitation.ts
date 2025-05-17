export interface UserInvitesResponseDto {
    invitationId: string;
    projectId: string;
    projectName: string;
    roleOffered: string;
    validUntil: string;
}

export interface ProjectInviteDto {
    invitationId: string;
    projectId: string;
    targetEmail: string;
    role: string;
    status: string;
    validUntil: string;
}

export interface ProjectInvitesResponseDto {
    items: ProjectInviteDto[];
    totalCount: number;
    pageSize: number;
    pageNumber: number;
} 