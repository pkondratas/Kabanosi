import Link from "next/link";
import { AuthHeaderNav } from "./auth-header-nav";

export function AuthHeader() {
  return (
    <div className="border-b bg-background">
      <div className="container flex h-16 items-center px-4">
        <div className="flex-1">
          <Link href="/" className="text-xl font-bold">
            JiGa
          </Link>
        </div>
        <AuthHeaderNav />
      </div>
    </div>
  );
}
