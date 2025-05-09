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

const projects = [
  {
    value: "Project1",
    label: "Project1",
  },
  {
    value: "Project2",
    label: "Project2",
  },
  {
    value: "Project3",
    label: "Project3",
  },
  {
    value: "Project4",
    label: "Project4",
  },
  {
    value: "Project5",
    label: "Project5",
  },
  {
    value: "Project6",
    label: "Project6",
  },
  {
    value: "Project7",
    label: "Project7",
  },
  {
    value: "Project8",
    label: "Project8",
  },
  {
    value: "Project9",
    label: "Project9",
  },
  {
    value: "Project10",
    label: "Project10",
  },
  {
    value: "Project11",
    label: "Project11",
  },
];

export function ProjectsDropdownSearch() {
  const [open, setOpen] = React.useState(false);
  const [value, setValue] = React.useState("");

  return (
    <Popover open={open} onOpenChange={setOpen}>
      <PopoverTrigger asChild>
        <Button
          variant="outline"
          role="combobox"
          aria-expanded={open}
          className="w-[200px] justify-between"
        >
          {value
            ? projects.find((project) => project.value === value)?.label
            : "Select project..."}
          <ChevronDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
        </Button>
      </PopoverTrigger>
      <PopoverContent className="w-[200px] p-0">
        <Command>
          <CommandInput placeholder="Search project..." />
          <CommandList>
            <CommandEmpty>No framework found.</CommandEmpty>
            <CommandGroup>
              {projects.map((project) => (
                <CommandItem
                  key={project.value}
                  value={project.value}
                  onSelect={(currentValue) => {
                    setValue(currentValue === value ? "" : currentValue);
                    setOpen(false);
                  }}
                >
                  <Check
                    className={cn(
                      "mr-2 h-4 w-4",
                      value === project.value ? "opacity-100" : "opacity-0"
                    )}
                  />
                  {project.label}
                </CommandItem>
              ))}
            </CommandGroup>
          </CommandList>
        </Command>
      </PopoverContent>
    </Popover>
  );
}
