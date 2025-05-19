import { WorkingPane } from "@/components/features/protected/project/working-pane/working-pane";
import { getAssignmentLabels } from "@/lib/actions/assignment-label.actions";
import { getAssignmentStatuses } from "@/lib/actions/assignment-status.actions";
import { getPlannedAssignments } from "@/lib/actions/assignment.actions";
import { getProjectMembers } from "@/lib/actions/project-member.actions";

export default async function Page({
  params,
}: {
  params: Promise<{ projectId: string }>;
}) {
  const { projectId } = await params;
  const [statuses, assignments, projectMembers, labels] = await Promise.all([
    getAssignmentStatuses(projectId),
    getPlannedAssignments(projectId),
    getProjectMembers(projectId),
    getAssignmentLabels(projectId)
  ]);

  return (
    <WorkingPane
      initialStatuses={statuses}
      assignments={assignments}
      projectMembers={projectMembers}
      labels={labels}
      projectId={projectId}
    />
  );
}
