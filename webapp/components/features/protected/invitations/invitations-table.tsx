"use client";

import { Button } from "@/components/ui/button";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { ProjectInviteDto } from "@/types/api/responses/invitation";
import { cancelProjectInvite } from "@/lib/actions/invites.actions";
import { toast } from "sonner";
import { useState } from "react";

interface InvitationsTableProps {
  invitations: ProjectInviteDto[];
  projectId: string;
  onInvitationCancelled: () => void;
}

const statusMap: Record<number, string> = {
  0: "Pending",
  1: "Accepted",
  2: "Declined",
  3: "Cancelled",
};

const roleMap: Record<number, string> = {
  0: "Admin",
  1: "Member",
};

export function InvitationsTable({
  invitations = [],
  projectId,
  onInvitationCancelled,
}: InvitationsTableProps) {
  const [cancellingId, setCancellingId] = useState<string | null>(null);

  const handleCancel = async (invitationId: string) => {
    setCancellingId(invitationId);
    try {
      await cancelProjectInvite(projectId, invitationId);
      toast.success("Invitation cancelled successfully");
      onInvitationCancelled();
    } catch (error) {
      toast.error("Failed to cancel invitation");
    } finally {
      setCancellingId(null);
    }
  };

  return (
    <div className="rounded-md border">
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead>Email</TableHead>
            <TableHead>Status</TableHead>
            <TableHead>Role</TableHead>
            <TableHead>Valid Until</TableHead>
            <TableHead>Actions</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {invitations.map((invitation) => (
            <TableRow key={invitation.invitationId}>
              <TableCell>{invitation.targetEmail}</TableCell>
              <TableCell>
                {statusMap[Number(invitation.status)] ?? invitation.status}
              </TableCell>
              <TableCell>
                {roleMap[Number(invitation.role)] ?? invitation.role}
              </TableCell>
              <TableCell>{invitation.validUntil.slice(0, 10)}</TableCell>
              <TableCell>
                {Number(invitation.status) === 0 && (
                  <Button
                    variant="destructive"
                    size="sm"
                    onClick={() => handleCancel(invitation.invitationId)}
                    disabled={cancellingId === invitation.invitationId}
                  >
                    {cancellingId === invitation.invitationId
                      ? "Cancelling..."
                      : "Cancel"}
                  </Button>
                )}
              </TableCell>
            </TableRow>
          ))}
          {invitations.length === 0 && (
            <TableRow>
              <TableCell colSpan={5} className="text-center">
                No invitations found
              </TableCell>
            </TableRow>
          )}
        </TableBody>
      </Table>
    </div>
  );
}
