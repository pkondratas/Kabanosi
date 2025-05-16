"use server";

import { UserInvitesResponseDto } from "@/types/api/responses/invitation";
import { getErrorMessage } from "../utils";
import { cookies } from "next/headers";

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