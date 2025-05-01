"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import { Button } from "@/components/ui/button";

export function AuthHeaderNav() {
  const pathname = usePathname();

  return (
    <nav className="flex items-center gap-4">
      <Link href="/login">
        <Button
          variant={pathname === "/login" ? "default" : "ghost"}
          className="text-sm font-medium"
        >
          Sign in
        </Button>
      </Link>
      <Link href="/signup">
        <Button
          variant={pathname === "/signup" ? "default" : "ghost"}
          className="text-sm font-medium"
        >
          Sign up
        </Button>
      </Link>
    </nav>
  );
}
