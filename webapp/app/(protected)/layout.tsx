import { LoggedInNavbar } from "@/components/features/protected/shared/loggedInNavbar";
import { getQueryClient } from "@/components/providers/getQueryClient";
import { getProjects } from "@/lib/actions/project.actions";
import { getUserInvites } from "@/lib/actions/invites.actions";
import { HydrationBoundary, dehydrate } from "@tanstack/react-query";
import { SelectedProjectProvider } from "@/components/providers/SelectedProjectProvider";
import { SignalRProvider } from "@/components/providers/SignalRProvider";
import { cookies } from "next/headers";
import { ProjectSwitchProvider } from "@/components/providers/ProjectSwitchProvider";

export default async function ProtectedLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const queryClient = getQueryClient();
  const cookieStore = await cookies();
  const token = cookieStore.get("token")?.value;

  if (!token) {
    throw new Error("No token found");
  }

  await queryClient.prefetchQuery({
    queryKey: ["projects"],
    queryFn: getProjects,
  });

  await queryClient.prefetchQuery({
    queryKey: ["invites"],
    queryFn: getUserInvites,
  });

  return (
    <HydrationBoundary state={dehydrate(queryClient)}>
      <SelectedProjectProvider>
        <ProjectSwitchProvider>
          <header className="fixed top-0 left-0 w-full z-50">
            <SignalRProvider token={token}>
              <LoggedInNavbar />
            </SignalRProvider>
          </header>
          <main className="mt-16">{children}</main>
        </ProjectSwitchProvider>
      </SelectedProjectProvider>
    </HydrationBoundary>
  );
}
