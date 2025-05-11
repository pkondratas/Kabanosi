"use client";

import { usePathname  } from 'next/navigation';
import { AssignmentColumn } from './assignment-column';
import { AssignmentStatusResponse } from '@/types/api/responses/assignment-status';
import { AssignmentResponse } from '@/types/api/responses/assignment';
import { useEffect, useState } from 'react';
import { getAssignmentStatuses, reorderAssignmentStatuses } from '@/lib/actions/assignment-status.actions';
import { changeAssignmentStatus, getPlannedAssignments } from '@/lib/actions/assignment.actions';

export function WorkingPane() {
  const pathname = usePathname();
	const projectId = pathname.split('/')[1];

  const [statuses, setStatuses] = useState<AssignmentStatusResponse[]>([]);
  const [statusOrder, setStatusOrder] = useState<string[]>([]);
  const [assignmentsByStatus, setAssignmentsByStatus] = useState<{ [key: string]: AssignmentResponse[] }>({});

  useEffect(() => {
    if (projectId) {
      const fetchData = async () => {
        const statusesData = await getAssignmentStatuses(projectId as string);
        const assignmentsData = await getPlannedAssignments(projectId as string);

        statusesData.sort((s1, s2) => s1.order - s2.order);

        setStatuses(statusesData);
        setStatusOrder(statusesData.map(s => s.id));

        const assignmentsByStatusMap: { [key: string]: AssignmentResponse[] } = {};

        assignmentsData.forEach((assignment) => {
          if (!assignmentsByStatusMap[assignment.assignmentStatusId]) {
            assignmentsByStatusMap[assignment.assignmentStatusId] = [];
          }
      
          assignmentsByStatusMap[assignment.assignmentStatusId].push(assignment);
        });

        setAssignmentsByStatus(assignmentsByStatusMap);
      };
      
      fetchData();
    }
  }, [projectId]);

  const moveStatusLeft = (index: number) => {
    if (index === 0) 
      return;

    const newOrder = [...statusOrder];
    [newOrder[index - 1], newOrder[index]] = [newOrder[index], newOrder[index - 1]];

    setStatusOrder(newOrder);
    reorderAssignmentStatuses(projectId, { idsInOrder: newOrder });
  };
  
  const moveStatusRight = (index: number) => {
    if (index === statusOrder.length - 1) 
      return;

    const newOrder = [...statusOrder];
    [newOrder[index], newOrder[index + 1]] = [newOrder[index + 1], newOrder[index]];

    setStatusOrder(newOrder);
    reorderAssignmentStatuses(projectId, { idsInOrder: newOrder });
  };

  const handleAssignmentStatusChange = (assignmentId: string, newStatusId: string) => {
    setAssignmentsByStatus((prev) => {
      const updated = { ...prev };
  
      const currentStatusId = Object.keys(updated).find((key) =>
        updated[key].some((a) => a.id === assignmentId)
      );
  
      if (!currentStatusId || currentStatusId === newStatusId) 
        return prev;

      updated[currentStatusId] = updated[currentStatusId].filter(a => a.id !== assignmentId);
  
      const movedAssignment = prev[currentStatusId].find(a => a.id === assignmentId);

      if (movedAssignment) {
        movedAssignment.assignmentStatusId = newStatusId;
        
        if (!updated[newStatusId]) 
          updated[newStatusId] = [];

        updated[newStatusId] = [...updated[newStatusId], movedAssignment];
      }
  
      return updated;
    });
    
    changeAssignmentStatus(projectId, assignmentId, newStatusId);
  }

  return (
    <div className="flex gap-4 overflow-x-auto p-4">
      {statusOrder.map((statusId, index) => {
        const status = statuses.find(s => s.id === statusId);
        if (!status) 
          return null;

        return (
          <AssignmentColumn
            key={status.id}
            status={status}
            assignments={assignmentsByStatus[status.id] || []}
            onMoveLeft={() => moveStatusLeft(index)}
            onMoveRight={() => moveStatusRight(index)}
            canMoveLeft={index > 0}
            canMoveRight={index < statusOrder.length - 1}
            statuses={statuses.map(s => {return {id: s.id, name: s.name}})}
            onAssignmentStatusChange={handleAssignmentStatusChange}
          />
        );
      })}
    </div>
  );
};