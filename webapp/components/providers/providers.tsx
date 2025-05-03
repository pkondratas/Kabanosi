"use client";

import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { ProgressProvider } from "@bprogress/next/app";
import { useState } from "react";

// TODO: why useState here? betterman does not use it (?)
export function Providers({ children }: { children: React.ReactNode }) {
  const [queryClient] = useState(
    () =>
      new QueryClient({
        defaultOptions: {
          queries: {
            staleTime: 60 * 1000, // 1 minute
            retry: 3,
          },
        },
      })
  );

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
