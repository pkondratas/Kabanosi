"use client"

import { CreateProjectCard } from "@/components/features/protected/project/createProjectCard";
import { ProjectsCarousel } from "@/components/features/protected/project/projectsCarousel";
import { PageTitle } from "@/components/ui/title";
import { useProject } from "@/hooks/projects.hook";

export const ProjectsDashboard = () => {
  const [projects, setProjects] = useProject();

  return (
    <div>
      <PageTitle className="flex p-8 text-4xl justify-center">
        Welcome back to JiGa!
      </PageTitle>
      <PageTitle className="p-10 pl-20">
        Your Projects
      </PageTitle>
      <ProjectsCarousel projects={projects} setProjects={setProjects} />
      <PageTitle className="p-10 pl-20">
        Create a new project
      </PageTitle>
      <CreateProjectCard setProjects={setProjects} />
    </div>
  );
}
