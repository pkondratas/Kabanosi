import ProjectNav from "@/components/features/protected/project/shared/ProjectNav";
import { getProjectById } from "@/lib/actions/project.actions";

export default async function ProjectLayout({
  children,
  params,
}: {
  children: React.ReactNode;
  params: Promise<{ projectId: string }>;
}) {
  const { projectId } = await params;
  const project = await getProjectById(projectId);
  return (
    <div key={projectId} className="min-h-screen flex flex-col">
      <ProjectNav
        key={projectId}
        projectId={projectId}
        projectName={project.name}
      />
      <main className="flex-1 pt-[124px]">{children}</main>
    </div>
  );
}
