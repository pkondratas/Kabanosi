"use client";

import { useRef, useState } from "react";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Button } from "@/components/ui/button";
import { AssignmentResponse } from "@/types/api/responses/assignment";
import { AssignmentStatusResponse } from "@/types/api/responses/assignment-status";
import { ProjectMemberResponse } from "@/types/api/responses/project-member";
import { DialogHeader, DialogTitle } from "@/components/ui/dialog";
import { AssignmentLabelResponse } from "@/types/api/responses/assignment-label";
import { getAssignment, updateAssignment } from "@/lib/actions/assignment.actions";

interface AssignmentDetailsDialogProps {
  projectId: string;
  assignment: AssignmentResponse;
  projectMembers: ProjectMemberResponse[];
  statuses: AssignmentStatusResponse[];
  labels: AssignmentLabelResponse[];
  onChangesSaved: (assignment: AssignmentResponse) => void;
}

export default function AssignmentDetailsDialog({
  projectId,
  assignment,
  projectMembers,
  statuses,
  labels,
  onChangesSaved
}: AssignmentDetailsDialogProps) {
  const [name, setName] = useState(assignment.name);
  const [description, setDescription] = useState(assignment.description);

  const [statusId, setStatusId] = useState(assignment.assignmentStatusId);
  const [labelId, setLabelId] = useState(assignment.assignmentLabelId);

  const [isPlanned, setIsPlanned] = useState(assignment.isPlanned);
  const [estimation, setEstimation] = useState(assignment.estimation);

  // const [hasDeadline, setHasdeadline] = useState(assignment.deadline != undefined);
  // const [deadline, setDeadline] = useState(assignment.deadline);

  // const [hasCompletedDate, setHasCompletedDate] = useState(assignment.completedDate != undefined);
  // const [completedDate, setCompletedDate] = useState(assignment.completedDate);

  const [assigneeId, setAssigneeId] = useState(assignment.assigneeId);

  const [conflictOccured, setConflictOccured] = useState(false);

  const originalRequestRef = useRef<UpdateAssignmentRequest | null>(null);

  const handleSave = async () => {
    setConflictOccured(false);

    const request: UpdateAssignmentRequest = {
      name: name,
      description: description,
      assignmentLabelId: labelId,
      assignmentStatusId: statusId,
      isPlanned: isPlanned,
      estimation: estimation,
      assigneeId: assigneeId
      //deadline: hasDeadline ? deadline : undefined,
      //completedDate: hasCompletedDate ? completedDate : undefined,
    };

    originalRequestRef.current = request;

    try {
      const updatedAssignment = await updateAssignment(projectId, assignment.id, request);
      onChangesSaved(updatedAssignment);
    } catch (error: any) {
      if (error?.name == 'ConflictError')
        setConflictOccured(true);
    }
  };

  const handleForceSave = async () => {
    if (!originalRequestRef.current) 
      return;
    
    const updatedAssignment = await updateAssignment(projectId, assignment.id, originalRequestRef.current);
    onChangesSaved(updatedAssignment);

    setConflictOccured(false); 
  };

  const handleUpdateInformation = async () => {
    const updatedAssignment = await getAssignment(projectId, assignment.id);

    setName(updatedAssignment.name);
    setDescription(updatedAssignment.description);
    setStatusId(updatedAssignment.assignmentStatusId);
    setLabelId(updatedAssignment.assignmentLabelId);
    setIsPlanned(updatedAssignment.isPlanned);
    setEstimation(updatedAssignment.estimation);
    setAssigneeId(updatedAssignment.assigneeId);
    //setDeadline(updatedAssignment.deadline);
    //setHasdeadline(updatedAssignment.deadline != undefined);
    //setCompletedDate(updatedAssignment.completedDate);
    //setHasCompletedDate(updatedAssignment.completedDate != undefined);

    setConflictOccured(false);
  }

  return (
    <div className="flex flex-col gap-4">
      <DialogHeader>
        <DialogTitle>Assignment information</DialogTitle>
      </DialogHeader>

      <div>
        <Label className="mb-1">Name</Label>
        <input
          type="text"
          value={name}
          onChange={(e) => setName(e.target.value)}
          className="text-md font-medium w-full bg-transparent border-none focus:outline-none"
        />
      </div>

      <div>
        <Label className="mb-1">Description</Label>
        <input
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          className="text-sm w-full bg-transparent border-none resize-none focus:outline-none"
          placeholder="No description"
        />
      </div>

      <div>
        <Label className="mb-1">Label</Label>
        <select
          value={labelId ?? ""}
          onChange={(e) =>
            setLabelId(
              e.target.value == "" ? undefined : Number(e.target.value)
            )
          }
          className="border rounded w-full p-1"
        >
          <option value="">None</option>
          {labels.map((l) => (
            <option key={l.id} value={l.id.toString()}>
              {l.name}
            </option>
          ))}
        </select>
      </div>

      <div>
        <Label className="mb-1">Status</Label>
        <select
          value={statusId}
          onChange={(e) => setStatusId(e.target.value)}
          className="border rounded w-full p-1"
        >
          {statuses.map((s) => (
            <option key={s.id} value={s.id}>
              {s.name}
            </option>
          ))}
        </select>
      </div>

      <div>
        <Label className="mb-1 flex items-center gap-2">
          Is Planned
          <input
            type="checkbox"
            checked={isPlanned}
            onChange={(e) => setIsPlanned(e.target.checked)}
            className="h-4 w-4 text-primary border rounded focus:ring-0"
          />
        </Label>
      </div>

      <div>
        <Label className="mb-1 flex items-center gap-2">Estimation</Label>
        <Input
          type="text"
          value={estimation == undefined ? "None" : estimation}
          onChange={(e) => {
            const input = e.target.value.trim().toLowerCase();
            if (input === "" || input === "none") {
              setEstimation(undefined);
            } else {
              const num = Number(input);
              setEstimation(isNaN(num) ? undefined : num);
            }
          }}
        />
      </div>

      {/* <div>
        <Label className="mb-1 flex items-center gap-2">
          Deadline Scheduled
          <input
            type="checkbox"
            checked={hasDeadline}
            onChange={(e) => setHasdeadline(e.target.checked)}
            className="h-4 w-4 text-primary border rounded focus:ring-0"
          />
        </Label>
        {hasDeadline ? (
          <div>
            <Label className="mt-3 mb-1">Deadline</Label>
            <Input
              type="date"
              value={deadline ? deadline.toISOString().slice(0, 10) : ""}
              onChange={(e) =>
                setDeadline(
                  e.target.value ? new Date(e.target.value) : undefined
                )
              }
            />
          </div>
        ) : (
          <></>
        )}
      </div>

      <div>
        <Label className="mb-3 flex items-center gap-2">
          Is Completed
          <input
            type="checkbox"
            checked={hasCompletedDate}
            onChange={(e) => setHasCompletedDate(e.target.checked)}
            className="h-4 w-4 text-primary border rounded focus:ring-0"
          />
        </Label>
        {hasCompletedDate ? (
          <div>
            <Label className="mt-3 mb-1">Completed Date</Label>
            <Input
              type="date"
              value={
                completedDate ? completedDate.toISOString().slice(0, 10) : ""
              }
              onChange={(e) =>
                setCompletedDate(
                  e.target.value ? new Date(e.target.value) : undefined
                )
              }
            />
          </div>
        ) : (
          <></>
        )}
      </div> */}

      <div>
        <Label className="mb-1">Assignee</Label>
        <select
          value={assigneeId ?? ""}
          onChange={(e) =>
            setAssigneeId(e.target.value == "" ? undefined : e.target.value)
          }
          className="border rounded w-full p-1"
        >
          <option value="">Unassigned</option>
          {projectMembers.map((pm: any) => (
            <option key={pm.id} value={pm.id}>
              {pm.username}
            </option>
          ))}
        </select>
      </div>

      <div>
        <Label className="mb-1">Reporter</Label>
        <span>{assignment.reporterUsername}</span>
      </div>

      {conflictOccured && (
        <div className="bg-red-100 text-red-700 p-3 rounded shadow">
          <p className="mb-2 font-medium">A conflict occurred. The assignment was updated by someone else.</p>
          <Button variant="outline" className="mr-2" onClick={handleForceSave}>
            Force Save Changes
          </Button>
          <Button variant="outline" onClick={handleUpdateInformation}>
            Update Information
          </Button>
        </div>
      )}

      {!conflictOccured && (<Button onClick={handleSave}>Save Changes</Button>)}
    </div>
  );
}
