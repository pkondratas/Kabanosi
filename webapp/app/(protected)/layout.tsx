import { LoggedInNavbar } from "@/components/features/protected/shared/loggedInNavbar";
import { getQueryClient } from "@/components/providers/getQueryClient";
import { getProjects } from "@/lib/actions/project.actions";
import { HydrationBoundary, dehydrate } from "@tanstack/react-query";
import { SelectedProjectProvider } from "@/components/providers/SelectedProjectProvider";

export default async function ProtectedLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const queryClient = getQueryClient();

  await queryClient.prefetchQuery({
    queryKey: ["projects"],
    queryFn: getProjects,
  });

  return (
    <HydrationBoundary state={dehydrate(queryClient)}>
      <SelectedProjectProvider>
        <header className="fixed top-0 left-0 w-full z-50">
          <LoggedInNavbar />
        </header>
        <main className="mt-16">{children}</main>
      </SelectedProjectProvider>
    </HydrationBoundary>
  );
}
