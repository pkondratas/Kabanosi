"use client";
import { createContext, useContext, useState } from "react";

const ProjectSwitchContext = createContext({
  isSwitching: false,
  setIsSwitching: (v: boolean) => {},
});

export function ProjectSwitchProvider({
  children,
}: {
  children: React.ReactNode;
}) {
  const [isSwitching, setIsSwitching] = useState(false);
  return (
    <ProjectSwitchContext.Provider value={{ isSwitching, setIsSwitching }}>
      {children}
    </ProjectSwitchContext.Provider>
  );
}

export function useProjectSwitch() {
  return useContext(ProjectSwitchContext);
}
