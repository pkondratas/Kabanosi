'use server'

import { ProjectResponse } from "@/types/api/responses/project";
import { getErrorMessage } from "../utils";
import { cookies, headers } from "next/headers";
import { CreateProjectRequest } from "@/types/api/requests/project";

const API_URL = process.env.NEXT_PUBLIC_API_URL;

const handleResponse = async (response: Response) => {
    if (!response.ok) {
        const errorMessage = await getErrorMessage(response);
        throw new Error(errorMessage);
    }
}

export const getProjects = async (): Promise<ProjectResponse[]> => {
    const cookieStore = await cookies();

    const response = await fetch(`${API_URL}/api/v1/projects`, {
        headers: {
            'Authorization': `Bearer ${cookieStore.get('token')?.value}`,
        },
    });

    await handleResponse(response);

    return await response.json();
}

export const createProject = async (data: CreateProjectRequest): Promise<ProjectResponse> => {
    const cookieStore = await cookies();
    
    const response = await fetch(`${API_URL}/api/v1/projects`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${cookieStore.get('token')?.value}`,
        },
        body: JSON.stringify(data),
    });

    await handleResponse(response);

    return await response.json();
}