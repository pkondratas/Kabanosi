"use server";

import { UserInvitesResponseDto } from "@/types/api/responses/invitation";
import { getErrorMessage } from "../utils";
import { cookies } from "next/headers";
import { ProjectInvitesResponseDto } from "@/types/api/responses/invitation";
import { CreateInvitationRequestDto } from "@/types/api/requests/invitation";
import { ProjectInviteDto } from "@/types/api/responses/invitation";

const API_URL = process.env.NEXT_PUBLIC_API_URL;

// Disable SSL verification in development
if (process.env.NODE_ENV === 'development') {
    process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0'
}

const handleResponse = async (response: Response) => {
    if (!response.ok) {
        const errorMessage = await getErrorMessage(response);
        throw new Error(errorMessage);
    }
}

export async function getUserInvites(): Promise<UserInvitesResponseDto[]> {
    const cookieStore = await cookies();

    const response = await fetch(`${API_URL}/api/v1/invitations/user-invites`, {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${cookieStore.get('token')?.value}`,
        },
    });

    await handleResponse(response);
    return response.json();
}

export async function acceptInvite(invitationId: string): Promise<void> {
    const cookieStore = await cookies();

    const response = await fetch(`${API_URL}/api/v1/invitations/accept/${invitationId}`, {
        method: "PUT",
        headers: {
            'Authorization': `Bearer ${cookieStore.get('token')?.value}`,
        },
    });

    await handleResponse(response);
}

export async function declineInvite(invitationId: string): Promise<void> {
    const cookieStore = await cookies();

    const response = await fetch(`${API_URL}/api/v1/invitations/decline/${invitationId}`, {
        method: "PUT",
        headers: {
            'Authorization': `Bearer ${cookieStore.get('token')?.value}`,
        },
    });

    await handleResponse(response);
}

export async function getProjectInvites(projectId: string): Promise<ProjectInviteDto[]> {
    const cookieStore = await cookies();

    const response = await fetch(`${API_URL}/api/v1/invitations/project-invites`, {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${cookieStore.get('token')?.value}`,
            'X-Project-Id': projectId,
        },
    });

    await handleResponse(response);
    const data = await response.json();
    return Array.isArray(data) ? data : [];
}

export async function createProjectInvite(projectId: string, data: CreateInvitationRequestDto): Promise<any> {
    const cookieStore = await cookies();

    const response = await fetch(`${API_URL}/api/v1/invitations`, {
        method: "POST",
        headers: {
            'Authorization': `Bearer ${cookieStore.get('token')?.value}`,
            'X-Project-Id': projectId,
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    });

    await handleResponse(response);
    const result = await response.json().catch(() => null);
    console.log("createProjectInvite result:", result); // Debug log
    return result;
}

export async function cancelProjectInvite(projectId: string, invitationId: string): Promise<void> {
    const cookieStore = await cookies();

    const response = await fetch(`${API_URL}/api/v1/invitations/cancel/${invitationId}`, {
        method: "PUT",
        headers: {
            'Authorization': `Bearer ${cookieStore.get('token')?.value}`,
            'X-Project-Id': projectId,
        },
    });

    await handleResponse(response);
} 