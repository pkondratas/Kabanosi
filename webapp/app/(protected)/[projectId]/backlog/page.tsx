import Backlog from "@/components/features/protected/project/backlog/backlog";
import { getAssignments } from "@/lib/actions/assignment.actions";
import { getAssignmentStatuses } from "@/lib/actions/assignment-status.actions";

export default async function Page({
  params,
}: {
  params: Promise<{ projectId: string }>;
}) {
  const { projectId } = await params;
  const [assignments, statuses] = await Promise.all([
    getAssignments(projectId),
    getAssignmentStatuses(projectId),
  ]);

  return (
    <div className="flex min-h-screen w-full items-start justify-center p-6 md:p-10">
      <div className="w-full max-w-screen-lg pt-[70px]">
        <Backlog
          assignments={assignments}
          statuses={statuses}
          projectId={projectId}
        />
      </div>
    </div>
  );
}
