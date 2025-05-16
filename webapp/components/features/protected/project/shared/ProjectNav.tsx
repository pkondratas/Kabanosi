"use client";
import Link from "next/link";
import { usePathname } from "next/navigation";
import {
  HomeIcon,
  ViewColumnsIcon,
  WindowIcon,
  UsersIcon,
  EnvelopeIcon,
  Cog6ToothIcon,
} from "@heroicons/react/24/outline";
import { useEffect } from "react";
import { useProjectSwitch } from "@/components/providers/ProjectSwitchProvider";

const navLinks = [
  { name: "Backlog", icon: HomeIcon, path: "backlog" },
  { name: "Board", icon: ViewColumnsIcon, path: "working-pane" },
  { name: "Team", icon: UsersIcon, path: "team" },
  { name: "Invitations", icon: EnvelopeIcon, path: "invitations" },
];

export default function ProjectNav({
  projectId,
  projectName,
}: {
  projectId: string;
  projectName: string;
}) {
  const pathname = usePathname();
  const { isSwitching, setIsSwitching } = useProjectSwitch();

  useEffect(() => {
    setIsSwitching(false);
  }, [projectId]);

  return (
    <nav
      className={`fixed top-16 left-0 right-0 z-40 flex items-center justify-between px-8 py-3 bg-white border-b shadow-sm h-[60px] transition-opacity duration-200 ${
        isSwitching ? "pointer-events-none opacity-50" : ""
      }`}
    >
      <div className="flex items-center gap-4 text-xl font-bold text-gray-800">
        <WindowIcon className="h-6 w-6 text-blue-500" />
        {projectName}
      </div>
      <div className="flex-1 flex justify-center">
        <div className="flex gap-8">
          {navLinks.map((link) => {
            const href = `/${projectId}/${link.path}`;
            const isActive = pathname.startsWith(href);
            const Icon = link.icon;
            return (
              <Link
                key={link.name}
                href={href}
                className={`flex items-center gap-2 font-medium px-3 py-1.5 rounded transition-colors duration-150 ${
                  isActive
                    ? "bg-blue-100 text-blue-700"
                    : "hover:bg-gray-200 text-gray-700"
                }`}
                tabIndex={isSwitching ? -1 : 0}
                aria-disabled={isSwitching}
                style={{ pointerEvents: isSwitching ? "none" : undefined }}
              >
                <Icon className="h-5 w-5" />
                {link.name}
              </Link>
            );
          })}
        </div>
      </div>
      <div className="flex-1 flex justify-end">
        <Link
          href={`/${projectId}/settings`}
          className={`flex items-center gap-2 font-medium px-3 py-1.5 rounded transition-colors duration-150 ${
            pathname.startsWith(`/${projectId}/settings`)
              ? "bg-blue-100 text-blue-700"
              : "hover:bg-gray-200 text-gray-700"
          }`}
          tabIndex={isSwitching ? -1 : 0}
          aria-disabled={isSwitching}
          style={{ pointerEvents: isSwitching ? "none" : undefined }}
        >
          <Cog6ToothIcon className="h-5 w-5" />
          Settings
        </Link>
      </div>
      {isSwitching && (
        <div className="absolute inset-0 flex items-center justify-center bg-white bg-opacity-60 z-50">
          <svg
            className="animate-spin h-6 w-6 text-blue-500"
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 24 24"
          >
            <circle
              className="opacity-25"
              cx="12"
              cy="12"
              r="10"
              stroke="currentColor"
              strokeWidth="4"
            />
            <path
              className="opacity-75"
              fill="currentColor"
              d="M4 12a8 8 0 018-8v8z"
            />
          </svg>
        </div>
      )}
    </nav>
  );
}
