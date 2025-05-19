import Backlog from "@/components/features/protected/project/backlog/backlog";
import { getAssignments } from "@/lib/actions/assignment.actions";
import { getAssignmentStatuses } from "@/lib/actions/assignment-status.actions";
import { getAssignmentLabels } from "@/lib/actions/assignment-label.actions";
import { getProjectMembers } from "@/lib/actions/project-member.actions";

export default async function Page({
  params,
}: {
  params: Promise<{ projectId: string }>;
}) {
  const { projectId } = await params;
  const [assignments, statuses, labels, projectMembers] = await Promise.all([
    getAssignments(projectId),
    getAssignmentStatuses(projectId),
    getAssignmentLabels(projectId),
    getProjectMembers(projectId)
  ]);

  for (let i = 0; i < assignments.length; i++) {
    console.log(assignments[i].deadline + " " + assignments[i].completedDate);
  }

  return (
    <div className="flex min-h-screen w-full items-start justify-center p-6 md:p-10">
      <div className="w-full max-w-screen-lg">
        <Backlog
          assignments={assignments}
          statuses={statuses}
          labels={labels}
          projectMembers={projectMembers}
          projectId={projectId}
        />
      </div>
    </div>
  );
}
