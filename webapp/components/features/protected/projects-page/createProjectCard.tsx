"use client";

import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
} from "@/components/ui/card";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogTitle,
  DialogTrigger,
  DialogFooter,
  DialogHeader,
} from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { createProject } from "@/lib/actions/project.actions";
import { Label } from "@radix-ui/react-label";
import { Loader2, Plus } from "lucide-react";
import { useRef, useState } from "react";
import { useQueryClient } from "@tanstack/react-query";

const errorMessages: Record<number, string> = {
  1: "Please provide both name and description.",
  2: "Request was not processed correctly.",
};

export const CreateProjectCard = () => {
  const [isOpen, setOpen] = useState(false);
  const [error, setError] = useState({ isError: false, messageIdx: 0 });
  const [isLoading, setLoading] = useState(false);
  const queryClient = useQueryClient();

  const nameRef = useRef<HTMLInputElement>(null);
  const descriptionRef = useRef<HTMLInputElement>(null);

  const handleClose = () => {
    setError((prev) => {
      return { ...prev, isError: false, messageIdx: 0 };
    });
    setOpen(false);
  };

  const handleCreate = async () => {
    const name = nameRef.current?.value;
    const description = descriptionRef.current?.value;

    if (!name || !description) {
      setError((prev) => {
        return { ...prev, isError: true, messageIdx: 1 };
      });
      return;
    }

    setLoading(true);
    createProject({ name, description })
      .catch((_) =>
        setError((prev) => {
          return { ...prev, isError: true, messageIdx: 2 };
        })
      )
      .then((data) => {
        if (data) {
          queryClient.invalidateQueries({ queryKey: ["projects"] });
          setError((prev) => {
            return { ...prev, isError: false, messageIdx: 0 };
          });
          setOpen(false);
        } else {
          setError((prev) => {
            return { ...prev, isError: true, messageIdx: 2 };
          });
        }
      })
      .finally(() => {
        setLoading(false);
      });
  };

  return (
    <div className="flex flex-col items-center w-full">
      <div className="flex flex-col w-full max-w-[90%]">
        <Card className="w-110 min-h-55 hover:bg-gray-200 bg-gray-100 p-4 rounded-lg shadow-lg ">
          <CardHeader className="text-xl font-bold text-center">
            Beginning of your legendary project...
          </CardHeader>
          <CardDescription className="pl-6">Click to start</CardDescription>
          <Dialog open={isOpen} onOpenChange={setOpen}>
            <DialogTrigger asChild>
              <Button
                variant="ghost"
                className="self-center hover:bg-gray-300 size-12"
              >
                <Plus />
              </Button>
            </DialogTrigger>
            <DialogContent
              onClose={() => handleClose()}
              className="sm:max-w-[425px]"
            >
              <DialogHeader>
                <DialogTitle>Create project</DialogTitle>
                <DialogDescription>
                  Provide details about your project.
                </DialogDescription>
              </DialogHeader>
              <div className="grid gap-4 py-4">
                <div className="grid grid-cols-4 items-center gap-4">
                  <Label htmlFor="name" className="text-right">
                    Name
                  </Label>
                  <Input id="name" className="col-span-3" ref={nameRef} />
                </div>
                <div className="grid grid-cols-4 items-center gap-4">
                  <Label htmlFor="description" className="text-right">
                    Description
                  </Label>
                  <Input
                    id="description"
                    className="col-span-3"
                    ref={descriptionRef}
                  />
                </div>
              </div>
              {error.isError && (
                <Label className="text-sm text-red-500">
                  {errorMessages[error.messageIdx]}
                </Label>
              )}
              {isLoading && (
                <div className="fixed inset-0 z-50 flex items-center justify-center backdrop-blur-sm bg-background/70 rounded-md">
                  <Loader2 className="h-10 w-10 animate-spin text-primary" />
                </div>
              )}
              <Button onClick={async () => await handleCreate()}>Create</Button>
            </DialogContent>
          </Dialog>
        </Card>
      </div>
    </div>
  );
};
