import { getProjectInvites } from "@/lib/actions/invites.actions";
import { InvitationsContainer } from "@/components/features/protected/invitations/invitations-container";

export default async function InvitationsPage({
  params,
}: {
  params: Promise<{ projectId: string }>;
}) {
  const { projectId } = await params;
  let invitations: any[] = [];
  let error: string | null = null;

  try {
    invitations = await getProjectInvites(projectId);
  } catch (e: any) {
    error = "You do not have permission to view project invitations.";
  }

  return (
    <div className="flex min-h-screen w-full items-start justify-center p-6 md:p-10">
      {error ? (
        <div className="text-red-500 text-lg">{error}</div>
      ) : (
        <InvitationsContainer invitations={invitations} projectId={projectId} />
      )}
    </div>
  );
}
