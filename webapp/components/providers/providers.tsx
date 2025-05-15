"use client";

import { QueryClientProvider } from "@tanstack/react-query";
import { ProgressProvider } from "@bprogress/next/app";
import { getQueryClient } from "./getQueryClient";

export function Providers({ children }: { children: React.ReactNode }) {
  const queryClient = getQueryClient();

  return (
    <QueryClientProvider client={queryClient}>
      <ProgressProvider
        height="3px"
        color="#000000"
        options={{ showSpinner: false }}
      >
        {children}
      </ProgressProvider>
    </QueryClientProvider>
  );
}
