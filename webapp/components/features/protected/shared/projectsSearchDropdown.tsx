"use client";

import * as React from "react";
import { Check, ChevronDown } from "lucide-react";
import { cn } from "@/lib/utils";
import { Button } from "@/components/ui/button";
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
import { useQuery } from "@tanstack/react-query";
import { getProjects } from "@/lib/actions/project.actions";
import { ProjectResponse } from "@/types/api/responses/project";
import { useRouter, usePathname } from "next/navigation";
import { useProject } from "@/components/providers/SelectedProjectProvider";
import { useProjectSwitch } from "@/components/providers/ProjectSwitchProvider";

export function ProjectsDropdownSearch() {
  const { data: projects = [] } = useQuery({
    queryKey: ["projects"],
    queryFn: getProjects,
  });
  const [open, setOpen] = React.useState(false);
  const { project, setProject } = useProject();
  const router = useRouter();
  const pathname = usePathname();
  const { setIsSwitching } = useProjectSwitch();

  React.useEffect(() => {
    if (pathname === "/") {
      setProject(null);
    }
  }, [pathname, setProject]);

  const selectedProject = projects.find((p) => p.id === project?.id);

  return (
    <Popover open={open} onOpenChange={setOpen}>
      <PopoverTrigger asChild>
        <Button
          variant="outline"
          role="combobox"
          aria-expanded={open}
          className="w-[200px] justify-between"
        >
          <span className="truncate max-w-[150px] overflow-hidden text-ellipsis">
            {selectedProject?.name || "Select project"}
          </span>
          <ChevronDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
        </Button>
      </PopoverTrigger>
      <PopoverContent className="w-[200px] p-0">
        <Command>
          <CommandInput placeholder="Search project..." />
          <CommandList>
            <CommandEmpty>No project found.</CommandEmpty>
            <CommandGroup>
              {projects.map((proj: ProjectResponse) => (
                <CommandItem
                  key={proj.id}
                  value={proj.name}
                  onSelect={() => {
                    setIsSwitching(true);
                    setProject(proj);
                    setOpen(false);
                    router.push(`/${proj.id}/backlog`);
                  }}
                >
                  <span className="truncate max-w-[150px] overflow-hidden text-ellipsis">
                    {proj.name}
                  </span>
                  <Check
                    className={cn(
                      "ml-auto",
                      proj.id === project?.id ? "opacity-100" : "opacity-0"
                    )}
                  />
                </CommandItem>
              ))}
            </CommandGroup>
          </CommandList>
        </Command>
      </PopoverContent>
    </Popover>
  );
}
