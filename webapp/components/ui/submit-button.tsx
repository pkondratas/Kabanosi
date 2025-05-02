"use client";

import { useFormStatus } from "react-dom";
import { ButtonLoading } from "@/components/ui/button-loading";

interface SubmitButtonProps extends React.ComponentProps<"button"> {
  pendingText?: string;
  formAction?: (formData: FormData) => Promise<void | any>;
}

export function SubmitButton({
  children,
  pendingText = "Please wait...",
  formAction,
  ...props
}: SubmitButtonProps) {
  const { pending } = useFormStatus();

  return (
    <ButtonLoading
      type="submit"
      isLoading={pending}
      loadingText={pendingText}
      formAction={formAction}
      {...props}
    >
      {children}
    </ButtonLoading>
  );
}
