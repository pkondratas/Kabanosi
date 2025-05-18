"use client"

import { useProject } from "@/components/providers/SelectedProjectProvider"
import { Table, TableBody, TableCaption, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { PageTitle } from "@/components/ui/title";
import { createAssignmentLabel, deleteAssignmentLabel, editAssignmentLabel, getAssignmentLabels } from "@/lib/actions/assignment-label.actions";
import { AssignmentLabelResponse } from "@/types/api/responses/assignment-label";
import { Dispatch, SetStateAction, useEffect, useRef, useState } from "react";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogTitle,
  DialogTrigger,
  DialogHeader,
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { Loader2, Pencil, Plus, Trash2 } from "lucide-react";
import { Label } from "@/components/ui/label";
import { Input } from "@/components/ui/input";
import { Tooltip, TooltipContent, TooltipProvider, TooltipTrigger } from "@/components/ui/tooltip";
import { patchProject } from "@/lib/actions/project.actions";
import { toast } from "sonner";
import { showToast } from "@/lib/utils";

const errorMessages: Record<number, string> = {
  1: "Please provide both name and description.",
  2: "Request was not processed correctly.",
};

export const ProjectInfo = () => {
  const { project, setProject } = useProject();
  const [assignmentLabels, setAssignmentLabels] = useState<AssignmentLabelResponse[]>();

  const [isCreateOpen, setCreateOpen] = useState(false);
  const [isEditOpen, setEditOpen] = useState(false);
  const [isDeleteOpen, setDeleteOpen] = useState(false);
  const [error, setError] = useState({ isError: false, messageIdx: 0 });
  const [isLoading, setLoading] = useState(false);
  const [selectedLabelId, setSelectedLabelId] = useState(0);
  const [isProjectModified, setIsProjectModified] = useState(false);

  const [editProjectName, setEditProjectName] = useState("");
  const [editProjectDescription, setEditProjectDescription] = useState("");

  const createNameRef = useRef<HTMLInputElement>(null);
  const createDescriptionRef = useRef<HTMLInputElement>(null);
  const editNameRef = useRef<HTMLInputElement>(null);
  const editDescriptionRef = useRef<HTMLInputElement>(null);

  const handleClose = (setOpen: Dispatch<SetStateAction<boolean>>) => {
    setError((prev) => {
      return { ...prev, isError: false, messageIdx: 0 };
    });
    setOpen(false);
  };

  const handleCreate = async (setOpen: Dispatch<SetStateAction<boolean>>) => {
    if (!project) return;

    const name = createNameRef.current?.value;
    const description = createDescriptionRef.current?.value;

    if (!name || !description) {
      setError((prev) => {
        return { ...prev, isError: true, messageIdx: 1 };
      });
      return;
    }

    setLoading(true);
    createAssignmentLabel(project?.id, { name, description })
      .catch((_) => {
        showToast(false, "Opeartion was unsuccessful. Try again later.");
      })
      .then((data) => {
        if (data) {
          setAssignmentLabels(prev => [ ...(prev || []), data ])
          setError((prev) => {
            return { ...prev, isError: false, messageIdx: 0 };
          });
          setOpen(false);
          showToast(true, "Label created successfully.");
        } else {
          showToast(false, "Opeartion was unsuccessful. Try again later.");
        }
      })
      .finally(() => {
        setLoading(false);
      });
  };
  
  useEffect(() => {
    const fetchData = async () => {
      if (!project) return;
      
      const data = await getAssignmentLabels(project.id);

      setAssignmentLabels(data);
    }

    fetchData();
  }, []);

  const handleEdit = async (setOpen: Dispatch<SetStateAction<boolean>>) => {
    if (!project) return;

    const name = editNameRef.current?.value;
    const description = editDescriptionRef.current?.value;

    if (!name || !description) {
      setError((prev) => {
        return { ...prev, isError: true, messageIdx: 1 };
      });
      return;
    }

    setLoading(true);
    editAssignmentLabel(selectedLabelId, project?.id, { name, description })
      .catch((_) => {
        showToast(false, "Opeartion was unsuccessful. Try again later.");
      })
      .then((data) => {
        if (data) {
          setAssignmentLabels(prev => 
            prev?.map(item => 
              item.id === data.id ? data : item
            )
          );
          setError((prev) => {
            return { ...prev, isError: false, messageIdx: 0 };
          });
          setOpen(false);
          showToast(true, "Label modified successfully.");
        } else {
          showToast(false, "Opeartion was unsuccessful. Try again later.");
        }
      })
      .finally(() => {
        setLoading(false);
      });
  };

  const handleDelete = async (setOpen: Dispatch<SetStateAction<boolean>>) => {
    if (!project) return;

    setLoading(true);
    deleteAssignmentLabel(selectedLabelId, project?.id)
      .catch((_) => {
        showToast(false, "Opeartion was unsuccessful. Try again later.");
      })
      .then(() => {
        setAssignmentLabels(prev => prev?.filter(item => item.id !== selectedLabelId));
        setOpen(false);
        showToast(true, "Project deleted successfully.");
      })
      .finally(() => {
        setLoading(false);
      });
  };

  const handleProjectEdit = async (prop: string, value: string) => {
    if (!project) return;

    setIsProjectModified(true);

    patchProject(project?.id, [{ op: "replace", path: `${prop}`, value: value }])
      .catch((_) => {
        showToast(false, "Opeartion was unsuccessful. Try again later.");
      })
      .then((data) => {
        if (data) {
          setProject(data);
          showToast(true, `Project ${prop} updated successfully!`);
        } else {
          showToast(false, "Opeartion was unsuccessful. Try again later.");
        }

        setIsProjectModified(false);
      });
  };

  useEffect(() => {
    const fetchData = async () => {
      if (!project) return;

      const data = await getAssignmentLabels(project.id);

      setAssignmentLabels(data);
    }

    fetchData();
  }, []);

  return (
    <div className="flex justify-center">
      <div className="w-[60%] max-w-[60%] pr-[5%]">
        <div className="pl-16 flex flex-col items-start gap-6 pr-10">
          <PageTitle className="text-4xl w-full break-words text-ellipsis">
            {project?.name}
          </PageTitle>
          <div className="text-base w-full break-words text-ellipsis text-left text-gray-500">
            <p className="text-sm font-medium gap-2 text-gray-700">
              Description:
            </p>
            {project?.description}
          </div>
        </div>
      </div>
      <div className="w-[35%] max-w-[35%] pt-15">
        <div
          className={`flex flex-col w-full text-sm items-start pb-8 pr-4 text-gray-700 transition-all duration-200 ${
            isProjectModified ? "blur-sm pointer-events-none select-none" : ""
          }`}
        >
          Edit project details
          <div className="grid gap-6 py-4">
            <div className="grid grid-cols-5 items-center gap-4">
              <Label htmlFor="name" className="text-right">
                Name
              </Label>
              <Input id="name" className="col-span-3" placeholder={project?.name}  onChange={(e) => setEditProjectName(e.target.value)} />
              <Button disabled={editProjectName ? false : true} onClick={() => handleProjectEdit("name", editProjectName)}>Change</Button>
            </div>
            <div className="grid grid-cols-5 items-center gap-4">
              <Label htmlFor="description" className="text-right">
                Description
              </Label>
              <Input
                id="description"
                className="col-span-3"
                placeholder={project?.description}
                onChange={(e) => setEditProjectDescription(e.target.value)}
              />
              <Button disabled={editProjectDescription ? false : true} onClick={() => handleProjectEdit("description", editProjectDescription)}>Change</Button>
            </div>
          </div>
        </div>
        <div className="w-full flex justify-between text-sm items-center pb-2 pr-4 text-gray-700">
          Assignment labels
          <Dialog open={isCreateOpen} onOpenChange={setCreateOpen}>
            <DialogTrigger asChild>
              <Button
                variant="ghost"
                className="self-center hover:bg-gray-300 size-8"
              >
                <Plus />
              </Button>
            </DialogTrigger>
            <DialogContent
              onClose={() => handleClose(setCreateOpen)}
              className="sm:max-w-[425px]"
            >
              <DialogHeader>
                <DialogTitle>Create assignment label</DialogTitle>
                <DialogDescription>
                  Provide details about new assignment label.
                </DialogDescription>
              </DialogHeader>
              <div className="grid gap-4 py-4">
                <div className="grid grid-cols-4 items-center gap-4">
                  <Label htmlFor="name" className="text-right">
                    Name
                  </Label>
                  <Input id="name" className="col-span-3" ref={createNameRef} />
                </div>
                <div className="grid grid-cols-4 items-center gap-4">
                  <Label htmlFor="description" className="text-right">
                    Description
                  </Label>
                  <Input
                    id="description"
                    className="col-span-3"
                    ref={createDescriptionRef}
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
              <Button onClick={async () => await handleCreate(setCreateOpen)}>Create</Button>
            </DialogContent>
          </Dialog>
        </div>
        <Table className="max-w-full table-fixed">
          <TableHeader>
            <TableRow className="items-center">
              <TableHead className="w-[80px] text-center">Label</TableHead>
              <TableHead className="w-[240px] text-center">Description</TableHead>
              <TableHead className="w-[40px]"/>
              <TableHead className="w-[40px]"/>
            </TableRow>
          </TableHeader>
          {assignmentLabels ? (
            <TableBody>
              {assignmentLabels?.map(label => (
                <TableRow>
                  <TableCell>
                    <TooltipProvider>
                      <Tooltip>
                        <TooltipTrigger asChild>
                          <div className="max-w-80 break-words text-ellipsis line-clamp-1">
                            {label.name}
                          </div>
                        </TooltipTrigger>
                        <TooltipContent>{label.name}</TooltipContent>
                      </Tooltip>
                    </TooltipProvider>
                  </TableCell>
                  <TableCell>
                    <TooltipProvider>
                      <Tooltip>
                        <TooltipTrigger asChild>
                          <div className="max-w-240 break-words text-ellipsis line-clamp-1">
                            {label?.description}
                          </div>
                        </TooltipTrigger>
                        <TooltipContent>{label?.description}</TooltipContent>
                      </Tooltip>
                    </TooltipProvider>
                  </TableCell>
                  <TableCell className="w-[40px] text-center">
                    <div>
                      <Dialog open={isEditOpen} onOpenChange={setEditOpen}>
                        <DialogTrigger asChild>
                          <Button
                            variant="ghost"
                            className="self-center hover:bg-gray-300 size-8"
                            onClick={() => setSelectedLabelId(label.id)}
                          >
                            <Pencil />
                          </Button>
                        </DialogTrigger>
                        <DialogContent
                          onClose={() => handleClose(setEditOpen)}
                          className="sm:max-w-[425px]"
                        >
                          <DialogHeader>
                            <DialogTitle>Edit label</DialogTitle>
                            <DialogDescription>
                              Provide modified label details.
                            </DialogDescription>
                          </DialogHeader>
                          <div className="grid gap-4 py-4">
                            <div className="grid grid-cols-4 items-center gap-4">
                              <Label htmlFor="name" className="text-right">
                                Name
                              </Label>
                              <Input id="name" className="col-span-3" ref={editNameRef} />
                            </div>
                            <div className="grid grid-cols-4 items-center gap-4">
                              <Label htmlFor="description" className="text-right">
                                Description
                              </Label>
                              <Input
                                id="description"
                                className="col-span-3"
                                ref={editDescriptionRef}
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
                          <Button onClick={async () => await handleEdit(setEditOpen)}>Edit</Button>
                        </DialogContent>
                      </Dialog>
                    </div>
                  </TableCell>
                  <TableCell className="w-[40px] text-center">
                    <div>
                      <Dialog open={isDeleteOpen} onOpenChange={setDeleteOpen}>
                        <DialogTrigger asChild>
                          <Button
                            variant="ghost"
                            className="self-center hover:bg-gray-300 size-8"
                            onClick={() => setSelectedLabelId(label.id)}
                          >
                            <Trash2 />
                          </Button>
                        </DialogTrigger>
                        <DialogContent
                          onClose={() => handleClose(setDeleteOpen)}
                          className="sm:max-w-[425px]"
                        >
                          <DialogHeader>
                            <DialogTitle>Confirmation</DialogTitle>
                            <DialogDescription>
                              Are you sure you want to delete this label?
                            </DialogDescription>
                          </DialogHeader>
                          {isLoading && (
                            <div className="fixed inset-0 z-50 flex items-center justify-center backdrop-blur-sm bg-background/70 rounded-md">
                              <Loader2 className="h-10 w-10 animate-spin text-primary" />
                            </div>
                          )}
                          <Button className="hover:bg-red-700" onClick={async () => await handleDelete(setDeleteOpen)}>Delete</Button>
                        </DialogContent>
                      </Dialog>
                    </div>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          ) : (
            <TableCaption className="pt-5 pb-5">Loading...</TableCaption>
          )}
          <TableCaption>Assignment labels</TableCaption>
        </Table>
      </div>
    </div>
  );
}