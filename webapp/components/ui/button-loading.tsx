import { Loader2 } from "lucide-react";
import { Button } from "@/components/ui/button";
import { cn } from "@/lib/utils";

interface ButtonLoadingProps extends React.ComponentProps<"button"> {
  loadingText?: string;
  isLoading?: boolean;
  children: React.ReactNode;
}

export function ButtonLoading({
  loadingText = "Please wait",
  isLoading = false,
  children,
  className,
  disabled,
  ...props
}: ButtonLoadingProps) {
  return (
    <Button
      disabled={isLoading || disabled}
      className={cn(className)}
      {...props}
    >
      {isLoading ? (
        <div className="flex items-center">
          <Loader2 className="animate-spin -ml-1 mr-3 h-5 w-5" />
          {loadingText}
        </div>
      ) : (
        children
      )}
    </Button>
  );
}
