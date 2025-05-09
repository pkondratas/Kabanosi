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

const invites = [
  { id: 1, project: "Project Alpha" },
  { id: 2, project: "Project Beta" },
  { id: 3, project: "Project Gamma" },
  { id: 4, project: "Project Delta" },
  { id: 5, project: "Project Epsilon" },
  { id: 6, project: "Project Zeta" },
  { id: 7, project: "Project Eta" },
  { id: 8, project: "Project Theta" },
  { id: 9, project: "Project Iota" },
  { id: 10, project: "Project Kappa" },
];

export function NotificationSearchDropdown() {
  const [open, setOpen] = React.useState(false);
  const [search, setSearch] = React.useState("");

  // Filter invites by search
  const filteredInvites = invites.filter((invite) =>
    invite.project.toLowerCase().includes(search.toLowerCase())
  );

  function handleAccept(project: string) {
    window.alert(`Accepted invite to ${project}`);
  }
  function handleDecline(project: string) {
    window.alert(`Declined invite to ${project}`);
  }

  return (
    <Popover open={open} onOpenChange={setOpen}>
      <PopoverTrigger asChild>
        <Button
          variant="outline"
          aria-label="Notifications"
          size="icon"
        >
          <Bell className="w-4 h-4 text-black" />
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
              {filteredInvites.map((invite) => (
                <CommandItem
                  key={invite.id}
                  value={invite.project}
                  className="flex items-center justify-between"
                >
                  <span>{invite.project}</span>
                  <span className="flex gap-2">
                    <Button
                      variant="ghost"
                      className="hover:bg-neutral-300 active:bg-neutral-400 rounded-full p-1"
                      aria-label="Accept invite"
                      tabIndex={-1}
                      onClick={() => handleAccept(invite.project)}
                    >
                      <Plus className="w-4 h-4 text-black" />
                    </Button>
                    <Button
                      variant="ghost"
                      className="hover:bg-neutral-300 active:bg-neutral-400 rounded-full p-1"
                      aria-label="Decline invite"
                      tabIndex={-1}
                      onClick={() => handleDecline(invite.project)}
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
