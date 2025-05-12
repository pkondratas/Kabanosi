'use server'

import { ProjectMemberResponse } from "@/types/api/responses/project-member"
import { EditProjectMemberRequest } from "@/types/api/requests/project-member"
import { cookies } from "next/headers"
import { getErrorMessage } from "../utils";

const API_URL = process.env.NEXT_PUBLIC_API_URL

const getToken = async (): Promise<string> => {
    const cookieStore = await cookies()
    const token = cookieStore.get('token')?.value

    if (!token) throw new Error('No token found')
    return token
}

export const getProjectMembers = async (projectId: string): Promise<ProjectMemberResponse[]> => {
    const token = await getToken()
    
    const response = await fetch(`${API_URL}/api/v1/project-members`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
            'X-Project-Id': projectId
        }
    })

    if (!response.ok) {
        const errorMessage = await getErrorMessage(response)
        throw new Error(errorMessage)
    }
    
    return await response.json()
}

export const editProjectMemberRole = async (
    projectId: string,
    memberId: string,
    data: EditProjectMemberRequest
): Promise<ProjectMemberResponse> => {
    const token = await getToken()

    const response = await fetch(`${API_URL}/api/v1/project-members/${memberId}`, {
        method: 'PATCH',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
            'X-Project-Id': projectId
        },
        body: JSON.stringify(data)
    })

    if (!response.ok) {
        const errorMessage = await getErrorMessage(response)
        throw new Error(errorMessage)
    }

    return await response.json()
}