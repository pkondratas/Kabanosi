import Link from "next/link";
import { SubmitButton } from "@/components/ui/submit-button";
import { logout } from "@/lib/actions/auth.actions";
import { cookies } from "next/headers";
import { ProjectsDropdownSearch } from "./projectsDropdownSearch";
import { NotificationSearchDropdown } from "./notificationSearchDropdown";

export async function LoggedInNavbar() {
  const cookieStore = await cookies();
  const email = cookieStore.get("email")?.value || "Unknown user";

  return (
    <div className="border-b bg-background">
      <div className="container flex h-16 items-center px-4 justify-between">
        <div className="flex items-center gap-4 flex-1">
          <Link href="/" className="text-xl font-bold">
            JiGa
          </Link>
        </div>

        <div className="flex items-center gap-4">
          <ProjectsDropdownSearch />
          <span className="border rounded-md px-4 py-2 text-sm font-medium bg-background flex items-center gap-2">
            {email}
          </span>
          <NotificationSearchDropdown />
          <form action={logout}>
            <SubmitButton pendingText="Logging out..." className="w-full">
              Logout
            </SubmitButton>
          </form>
        </div>
      </div>
    </div>
  );
}
