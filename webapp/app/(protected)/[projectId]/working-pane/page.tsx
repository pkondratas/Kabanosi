import { WorkingPane } from "@/components/features/protected/project/working-pane/working-pane";
import { getAssignmentStatuses } from "@/lib/actions/assignment-status.actions";
import { getPlannedAssignments } from "@/lib/actions/assignment.actions";

export default async function Page({
  params,
}: {
  params: Promise<{ projectId: string }>;
}) {
  const { projectId } = await params;
  const [statuses, assignments] = await Promise.all([
    getAssignmentStatuses(projectId),
    getPlannedAssignments(projectId),
  ]);

  return (
    <WorkingPane
      statuses={statuses}
      assignments={assignments}
      projectId={projectId}
    />
  );
}
