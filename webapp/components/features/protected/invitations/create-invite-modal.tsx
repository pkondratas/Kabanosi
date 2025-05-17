"use client";

import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { createProjectInvite } from "@/lib/actions/invites.actions";
import { useState } from "react";
import { toast } from "sonner";

interface CreateInviteModalProps {
  projectId: string;
  onInviteCreated: () => void;
}

export function CreateInviteModal({
  projectId,
  onInviteCreated,
}: CreateInviteModalProps) {
  const [open, setOpen] = useState(false);
  const [email, setEmail] = useState("");
  const [role, setRole] = useState<"ProjectAdmin" | "ProjectMember">(
    "ProjectMember"
  );
  const [validDays, setValidDays] = useState(7);
  const [isLoading, setIsLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (validDays < 1 || validDays > 31) {
      toast.error("Valid days must be between 1 and 31");
      return;
    }
    setIsLoading(true);

    try {
      const result = await createProjectInvite(projectId, {
        targetEmail: email,
        targetRole: role,
        validDays,
      });
      console.log("Invite created:", result);
      toast.success("Invitation sent successfully");
      setOpen(false);
      onInviteCreated();
    } catch (error) {
      toast.error("Failed to send invitation");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <Button>Create Invite</Button>
      </DialogTrigger>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Create New Invitation</DialogTitle>
        </DialogHeader>
        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="space-y-2">
            <label htmlFor="email" className="text-sm font-medium">
              Email
            </label>
            <Input
              id="email"
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </div>
          <div className="space-y-2">
            <label htmlFor="role" className="text-sm font-medium">
              Role
            </label>
            <Select
              value={role}
              onValueChange={(value: "ProjectAdmin" | "ProjectMember") =>
                setRole(value)
              }
              required
            >
              <SelectTrigger>
                <SelectValue placeholder="Select a role" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="ProjectMember">Member</SelectItem>
                <SelectItem value="ProjectAdmin">Admin</SelectItem>
              </SelectContent>
            </Select>
          </div>
          <div className="space-y-2">
            <label htmlFor="validDays" className="text-sm font-medium">
              Valid Days
            </label>
            <Input
              id="validDays"
              type="number"
              min={1}
              max={31}
              value={validDays}
              onChange={(e) => setValidDays(Number(e.target.value))}
              required
            />
          </div>
          <Button type="submit" className="w-full" disabled={isLoading}>
            {isLoading ? "Sending..." : "Send Invitation"}
          </Button>
        </form>
      </DialogContent>
    </Dialog>
  );
}
