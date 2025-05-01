import { NextResponse } from 'next/server'
import type { NextRequest } from 'next/server'

export function middleware(request: NextRequest) {
    const token = request.cookies.get('token')
    const isAuthPage = request.nextUrl.pathname.startsWith('/login') ||
        request.nextUrl.pathname.startsWith('/signup')

    // If trying to access auth pages while logged in, redirect to home
    if (isAuthPage && token) {
        return NextResponse.redirect(new URL('/', request.url))
    }

    // If trying to access protected routes without being logged in
    if (!isAuthPage && !token) {
        return NextResponse.redirect(new URL('/login', request.url))
    }

    return NextResponse.next()
}

export const config = {
    matcher: [
        '/',
        '/projects/:path*',
        '/settings/:path*',
        '/login',
        '/signup'
    ]
} 