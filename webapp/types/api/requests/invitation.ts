export interface CreateInvitationRequestDto {
    targetEmail: string;
    targetRole: 'ProjectAdmin' | 'ProjectMember';
    validDays?: number;
} 