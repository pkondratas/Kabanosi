'use server'

import { getErrorMessage } from "../utils";
import { cookies, headers } from "next/headers";
import { AssignmentLabelResponse } from "@/types/api/responses/assignment-label";
import { AssignmentLabelRequest } from "@/types/api/requests/assignment-label";

const API_URL = process.env.NEXT_PUBLIC_API_URL;

const handleResponse = async (response: Response) => {
    if (!response.ok) {
        const errorMessage = await getErrorMessage(response);
        throw new Error(errorMessage);
    }
}

export const getAssignmentLabels = async (projectId: string): Promise<AssignmentLabelResponse[]> => {
    const cookieStore = await cookies();

    const response = await fetch(`${API_URL}/api/v1/assignment-labels`, {
        headers: {
            'Authorization': `Bearer ${cookieStore.get('token')?.value}`,
            'Content-Type': 'application/json',
            'X-Project-Id': projectId
        },
    });

    await handleResponse(response);

    return await response.json();
}

export const createAssignmentLabel = async (projectId: string, data: AssignmentLabelRequest): Promise<AssignmentLabelResponse> => {
    const cookieStore = await cookies();

    const response = await fetch(`${API_URL}/api/v1/assignment-labels`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${cookieStore.get('token')?.value}`,
            'X-Project-Id': projectId,
        },
        body: JSON.stringify(data),
    });

    await handleResponse(response);

    return await response.json();
}

export const editAssignmentLabel = async (labelId: number, projectId: string, data: AssignmentLabelRequest): Promise<AssignmentLabelResponse> => {
    const cookieStore = await cookies();

    const response = await fetch(`${API_URL}/api/v1/assignment-labels/${labelId}/rename`, {
        method: 'PATCH',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${cookieStore.get('token')?.value}`,
            'X-Project-Id': projectId,
        },
        body: JSON.stringify(data),
    });

    await handleResponse(response);

    return await response.json();
}

export const deleteAssignmentLabel = async (labelId: number, projectId: string): Promise<void> => {
    const cookieStore = await cookies();

    const response = await fetch(`${API_URL}/api/v1/assignment-labels/${labelId}`, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${cookieStore.get('token')?.value}`,
            'X-Project-Id': projectId,
        }
    });

    await handleResponse(response);
}