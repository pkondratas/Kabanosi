'use server'

import { cookies } from 'next/headers'
import { redirect } from 'next/navigation'
import type { LoginRequest, RegisterRequest } from '@/types/api/requests/auth'
import fetch from 'node-fetch'

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

export async function login(data: LoginRequest) {
    const response = await fetch(`${API_URL}/api/v1/auth/login`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })

    if (!response.ok) {
        const errorMessage = await getErrorMessage(response)
        throw new Error(errorMessage)
    }

    const result = await response.json()

    // Set token cookie
    const cookieStore = await cookies()
    cookieStore.set('token', result.token, {
        httpOnly: true,
        secure: process.env.NODE_ENV === 'production',
        sameSite: 'strict',
        maxAge: 7 * 24 * 60 * 60, // 7 days
    })

    redirect('/')
}

export async function register(data: RegisterRequest) {
    const response = await fetch(`${API_URL}/api/v1/auth/register`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })

    if (!response.ok) {
        const errorMessage = await getErrorMessage(response)
        throw new Error(errorMessage)
    }

    // Redirect to login page after successful registration
    redirect('/login')
}

export async function logout() {
    const cookieStore = await cookies()
    cookieStore.delete('token')
    redirect('/login')
} 