'use server'

import { ProjectResponse } from "@/types/api/responses/project";
import { getErrorMessage } from "../utils";
import { cookies, headers } from "next/headers";
import { CreateProjectRequest, JsonPatchProject } from "@/types/api/requests/project";

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

export const getProjectById = async (projectId: string): Promise<ProjectResponse> => {
    const cookieStore = await cookies();

    const response = await fetch(`${API_URL}/api/v1/projects/${projectId}`, {
        headers: {
            'Authorization': `Bearer ${cookieStore.get('token')?.value}`,
        },
    });

    await handleResponse(response);

    return await response.json();
}

export const patchProject = async (projectId: string, patchData: JsonPatchProject[]): Promise<ProjectResponse> => {
    const cookieStore = await cookies();
  
    const response = await fetch(`${API_URL}/api/v1/projects`, {
      method: "PATCH",
      headers: {
        "Content-Type": "application/json-patch+json",
        "Authorization": `Bearer ${cookieStore.get("token")?.value}`,
        'X-Project-Id': projectId,
      },
      body: JSON.stringify(patchData),
    });
  
    await handleResponse(response);
  
    return await response.json();
  };