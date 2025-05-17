"use client";

import { CreateInviteModal } from "./create-invite-modal";
import { InvitationsTable } from "./invitations-table";
import { ProjectInviteDto } from "@/types/api/responses/invitation";
import { useRouter } from "next/navigation";

interface InvitationsContainerProps {
  invitations: ProjectInviteDto[];
  projectId: string;
}

export function InvitationsContainer({
  invitations = [],
  projectId,
}: InvitationsContainerProps) {
  const router = useRouter();

  const handleInviteCreated = () => {
    router.refresh();
  };

  const handleInvitationCancelled = () => {
    router.refresh();
  };

  return (
    <div className="w-full max-w-screen-lg pt-[70px]">
      <div className="mb-6 flex items-center justify-between">
        <h1 className="text-2xl font-bold">Project Invitations</h1>
        <CreateInviteModal
          projectId={projectId}
          onInviteCreated={handleInviteCreated}
        />
      </div>
      <InvitationsTable
        invitations={invitations}
        projectId={projectId}
        onInvitationCancelled={handleInvitationCancelled}
      />
    </div>
  );
}
