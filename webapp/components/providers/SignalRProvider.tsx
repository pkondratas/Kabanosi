"use client";

import {
  HubConnectionBuilder,
  HubConnection,
  LogLevel,
} from "@microsoft/signalr";
import { createContext, useContext, useEffect, useState } from "react";
import { toast } from "sonner";
import { useQueryClient } from "@tanstack/react-query";
import { UserInvitesResponseDto } from "@/types/api/responses/invitation";

type SignalRContextType = {
  connection: HubConnection | null;
};

const SignalRContext = createContext<SignalRContextType>({
  connection: null,
});

export const SignalRProvider = ({
  token,
  children,
}: {
  token: string;
  children: React.ReactNode;
}) => {
  const [connection, setConnection] = useState<HubConnection | null>(null);
  const queryClient = useQueryClient();

  useEffect(() => {
    const conn = new HubConnectionBuilder()
      .withUrl("https://localhost:7249/hubs/notify", {
        accessTokenFactory: () => token,
      })
      .configureLogging(LogLevel.Information)
      .withAutomaticReconnect()
      .build();

    conn
      .start()
      .then(() => {
        setConnection(conn);
        conn.on("InviteReceived", (invite: UserInvitesResponseDto) => {
          toast.success(`You were invited to "${invite.projectName}" project!`);
          queryClient.invalidateQueries({ queryKey: ["invites"] });
        });
        conn.onclose(() => {});
      })
      .catch(() => {});

    return () => {
      conn.stop();
    };
  }, [token, queryClient]);

  return (
    <SignalRContext.Provider value={{ connection }}>
      {children}
    </SignalRContext.Provider>
  );
};

export const useSignalR = () => useContext(SignalRContext);
