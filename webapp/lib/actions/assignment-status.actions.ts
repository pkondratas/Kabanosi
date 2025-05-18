'use server'

import { AssignmentStatusRequest, ReorderAssignmentStatusesRequest } from '@/types/api/requests/assignment-status';
import { AssignmentStatusResponse } from '@/types/api/responses/assignment-status';
import { cookies } from 'next/headers';
import { getErrorMessage } from '../utils';

const API_URL = process.env.NEXT_PUBLIC_API_URL

// Disable SSL verification in development
if (process.env.NODE_ENV === 'development') {
    process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0'
}

async function getToken() : Promise<string> {
    const cookieStore = await cookies()
    const token = cookieStore.get('token')?.value

    if (!token) {
        throw new Error('No token found')
    }

    return token
}

export async function getAssignmentStatuses(projectId: string) : Promise<AssignmentStatusResponse[]> {
    const token = await getToken()

    const response = await fetch(`${API_URL}/api/v1/assignment-statuses`, {
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

export async function reorderAssignmentStatuses(
    projectId: string, 
    request: ReorderAssignmentStatusesRequest) : Promise<AssignmentStatusResponse[]> {
    const token = await getToken();
    
    const response = await fetch(`${API_URL}/api/v1/assignment-statuses/reorder`, {
        method: 'PATCH',
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

export async function createAssignmentStatus(
    projectId: string,
    request: AssignmentStatusRequest) : Promise<AssignmentStatusResponse> {
    const token = await getToken();

    const response = await fetch(`${API_URL}/api/v1/assignment-statuses`, {
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



