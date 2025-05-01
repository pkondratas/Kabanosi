'use server'

import { cookies } from 'next/headers'
import { redirect } from 'next/navigation'
import type { LoginRequest, RegisterRequest } from '@/types/api/requests/auth'

const API_URL = process.env.NEXT_PUBLIC_API_URL

export async function login(data: LoginRequest) {
    const response = await fetch(`${API_URL}/auth/login`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })

    if (!response.ok) {
        throw new Error('Login failed')
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
    const response = await fetch(`${API_URL}/auth/register`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })

    if (!response.ok) {
        throw new Error('Registration failed')
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

export async function logout() {
    const cookieStore = await cookies()
    cookieStore.delete('token')
    redirect('/login')
} 