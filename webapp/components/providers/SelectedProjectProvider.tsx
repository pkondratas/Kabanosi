"use client";
import { ProjectResponse } from "@/types/api/responses/project";
import React, { createContext, useContext, useState } from "react";

type SelectedProjectContextType = {
  project: ProjectResponse | null;
  setProject: (id: ProjectResponse | null) => void;
};

const SelectedProjectContext = createContext<
  SelectedProjectContextType | undefined
>(undefined);

export function SelectedProjectProvider({
  children,
}: {
  children: React.ReactNode;
}) {
  const [project, setProject] = useState<ProjectResponse | null>(null);
  return (
    <SelectedProjectContext.Provider value={{ project, setProject }}>
      {children}
    </SelectedProjectContext.Provider>
  );
}

export function useProject() {
  const ctx = useContext(SelectedProjectContext);
  if (!ctx)
    throw new Error(
      "useSelectedProject must be used within SelectedProjectProvider"
    );
  return ctx;
}
