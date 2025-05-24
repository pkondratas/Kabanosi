'use server'

import { AssignmentResponse } from "@/types/api/responses/assignment"
import { cookies } from "next/headers"

const API_URL = process.env.NEXT_PUBLIC_API_URL

// Disable SSL verification in development
if (process.env.NODE_ENV === 'development') {
    process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0'
}

async function getErrorMessage(response: { headers: { get(name: string): string | null }, json(): Promise<any>, text(): Promise<string> }): Promise<string> {
    const contentType = response.headers.get('content-type')
    if (contentType?.includes('application/json')) {
        const error = await response.json()
        return error.message || error.errors?.join(', ') || error || 'An error occurred'
    }
    return await response.text()
}

async function getToken() : Promise<string> {
    const cookieStore = await cookies()
    const token = cookieStore.get('token')?.value

    if (!token) {
        throw new Error('No token found')
    }

    return token
}

class ConflictError extends Error {
    constructor() {
        super("Conflict Error");
        this.name = "ConflictError";
    }
}

export async function getPlannedAssignments(projectId: string) : Promise<AssignmentResponse[]> {
    const token = await getToken()

    const response = await fetch(`${API_URL}/api/v1/assignments/planned?pageSize=50&pageNumber=0`, {
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
    
    return response.json()
}

export async function getAssignments(projectId: string) : Promise<AssignmentResponse[]> {
    const token = await getToken()

    const response = await fetch(`${API_URL}/api/v1/assignments?pageSize=50&pageNumber=0`, {
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
    
    return response.json()
}

export async function getAssignment(projectId: string, assignmentId: string): Promise<AssignmentResponse> {
    const token = await getToken()

    const response = await fetch(`${API_URL}/api/v1/assignments/${assignmentId}`, {
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
    
    return response.json()
}

export async function createAssignment(projectId: string, request: AssignmentRequest) : Promise<AssignmentResponse> {
    const token = await getToken()

    const response = await fetch(`${API_URL}/api/v1/assignments`, {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
          'X-Project-Id': projectId
        },
        body: JSON.stringify(request)
    })
    
    if (!response.ok) {
        const errorMessage = await getErrorMessage(response)
        throw new Error(errorMessage)
    }
    
    return response.json()
}

export async function changeAssignmentStatus(
    projectId: string, 
    assignmentId: string, 
    statusId: string) : Promise<AssignmentResponse> {
    const token = await getToken()

    const response = await fetch(`${API_URL}/api/v1/assignments/${assignmentId}/change-status`, {
        method: 'PATCH',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
          'X-Project-Id': projectId
        },
        body: JSON.stringify({ newAssignmentStatusId: statusId })
    })

    if (!response.ok) {
        const errorMessage = await getErrorMessage(response)
        throw new Error(errorMessage)
    }
    
    return response.json()
}

export async function assignAssignment(
    projectId: string,
    assignmentId: string,
    assigneeId?: string) : Promise<AssignmentResponse> {
    const token = await getToken()

    const response = await fetch(`${API_URL}/api/v1/assignments/${assignmentId}/assign`, {
        method: 'PATCH',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
          'X-Project-Id': projectId
        },
        body: JSON.stringify({ assigneeId: assigneeId })
    })

    if (!response.ok) {
        const errorMessage = await getErrorMessage(response)
        throw new Error(errorMessage)
    }
    
    return response.json()
}

export async function updateAssignment(
    projectId: string,
    assignmentId: string,
    request: UpdateAssignmentRequest) : Promise<AssignmentResponse> {
    const token = await getToken()

    const response = await fetch(`${API_URL}/api/v1/assignments/${assignmentId}`, {
        method: 'PUT',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
          'X-Project-Id': projectId
        },
        body: JSON.stringify(request)
    })

    if (!response.ok) {
        if (response.status == 409) {
            throw new ConflictError()
        }

        const errorMessage = await getErrorMessage(response)
        throw new Error(errorMessage)
    }

    return response.json()
}