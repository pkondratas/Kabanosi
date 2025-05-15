"use client";
import React, { createContext, useContext, useState } from "react";

type SelectedProjectContextType = {
  selectedId: string | null;
  setSelectedId: (id: string | null) => void;
};

const SelectedProjectContext = createContext<
  SelectedProjectContextType | undefined
>(undefined);

export function SelectedProjectProvider({
  children,
}: {
  children: React.ReactNode;
}) {
  const [selectedId, setSelectedId] = useState<string | null>(null);
  return (
    <SelectedProjectContext.Provider value={{ selectedId, setSelectedId }}>
      {children}
    </SelectedProjectContext.Provider>
  );
}

export function useSelectedProject() {
  const ctx = useContext(SelectedProjectContext);
  if (!ctx)
    throw new Error(
      "useSelectedProject must be used within SelectedProjectProvider"
    );
  return ctx;
}
