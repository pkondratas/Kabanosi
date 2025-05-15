"use client";

import * as React from "react";
import { Bell, Plus, Minus } from "lucide-react";
import {
  Command,
  CommandEmpty,
  CommandGroup,
  CommandInput,
  CommandItem,
  CommandList,
} from "@/components/ui/command";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import { Button } from "@/components/ui/button";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import {
  getUserInvites,
  acceptInvite,
  declineInvite,
} from "@/lib/actions/invites.actions";
import { UserInvitesResponseDto } from "@/types/api/responses/invitation";
import { toast } from "sonner";

export function NotificationSearchDropdown() {
  const [open, setOpen] = React.useState(false);
  const [search, setSearch] = React.useState("");
  const [pendingAction, setPendingAction] = React.useState<string | null>(null);

  const { data: invites = [], refetch } = useQuery({
    queryKey: ["invites"],
    queryFn: getUserInvites,
  });
  const queryClient = useQueryClient();

  // Filter invites by search
  const filteredInvites = invites.filter((invite) =>
    invite.projectName.toLowerCase().includes(search.toLowerCase())
  );

  async function handleAccept(invite: UserInvitesResponseDto) {
    try {
      setPendingAction(invite.invitationId);
      await acceptInvite(invite.invitationId);
      toast.success(`Accepted invite to ${invite.projectName}`);
      refetch();
      queryClient.invalidateQueries({ queryKey: ["projects"] });
    } catch (error) {
      toast.error(`Failed to accept invite: ${(error as Error).message}`);
    } finally {
      setPendingAction(null);
    }
  }

  async function handleDecline(invite: UserInvitesResponseDto) {
    try {
      setPendingAction(invite.invitationId);
      await declineInvite(invite.invitationId);
      toast.success(`Declined invite to ${invite.projectName}`);
      refetch();
    } catch (error) {
      toast.error(`Failed to decline invite: ${(error as Error).message}`);
    } finally {
      setPendingAction(null);
    }
  }

  return (
    <Popover open={open} onOpenChange={setOpen}>
      <PopoverTrigger asChild>
        <Button
          variant="outline"
          aria-label="Notifications"
          size="icon"
          className="relative"
        >
          <Bell className="w-4 h-4 text-black" />
          {invites.length > 0 && (
            <span className="absolute -top-1 -right-1 h-4 w-4 rounded-full bg-red-500 text-[10px] font-medium text-white flex items-center justify-center">
              {invites.length}
            </span>
          )}
        </Button>
      </PopoverTrigger>
      <PopoverContent className="w-[300px] p-0">
        <Command>
          <CommandInput
            placeholder="Search invites..."
            value={search}
            onValueChange={setSearch}
          />
          <CommandList>
            <CommandEmpty>No invites found.</CommandEmpty>
            <CommandGroup>
              {filteredInvites.map((invite: UserInvitesResponseDto) => (
                <CommandItem
                  key={invite.invitationId}
                  value={invite.projectName}
                  className="flex items-center justify-between"
                >
                  <span className="max-w-[160px] truncate">
                    {invite.projectName}
                  </span>
                  <span className="flex gap-2">
                    <Button
                      variant="ghost"
                      className="hover:bg-neutral-300 active:bg-neutral-400 rounded-full p-1"
                      aria-label="Accept invite"
                      tabIndex={-1}
                      onClick={() => handleAccept(invite)}
                      disabled={pendingAction === invite.invitationId}
                    >
                      <Plus className="w-4 h-4 text-black" />
                    </Button>
                    <Button
                      variant="ghost"
                      className="hover:bg-neutral-300 active:bg-neutral-400 rounded-full p-1"
                      aria-label="Decline invite"
                      tabIndex={-1}
                      onClick={() => handleDecline(invite)}
                      disabled={pendingAction === invite.invitationId}
                    >
                      <Minus className="w-4 h-4 text-black" />
                    </Button>
                  </span>
                </CommandItem>
              ))}
            </CommandGroup>
          </CommandList>
        </Command>
      </PopoverContent>
    </Popover>
  );
}
