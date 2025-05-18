import { clsx, type ClassValue } from "clsx"
import { toast } from "sonner"
import { twMerge } from "tailwind-merge"

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}

export async function getErrorMessage(response: { headers: { get(name: string): string | null }, json(): Promise<any>, text(): Promise<string> }): Promise<string> {
  const contentType = response.headers.get('content-type')
  if (contentType?.includes('application/json')) {
      const error = await response.json()
      return error.message || error.errors?.join(', ') || error || 'An error occurred'
  }
  return await response.text()
}

export const showToast = (success: boolean, message: string) => {
  toast(success ? "Success!" : "Failure!", {
    description: message,
  });
}
