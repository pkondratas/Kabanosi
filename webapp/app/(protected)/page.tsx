import { PageTitle } from "@/components/ui/title";
import { ProjectsCarousel } from "@/components/features/protected/projects-page/projectsCarousel";
import { CreateProjectCard } from "@/components/features/protected/projects-page/createProjectCard";

export default function ProjectsPage() {
  return (
    <div className="items-center justify-center h-screen">
      <PageTitle className="flex p-8 text-4xl justify-center">
        Welcome back to JiGa!
      </PageTitle>
      <PageTitle className="p-10 pl-20">Your Projects</PageTitle>
      <ProjectsCarousel />
      <PageTitle className="p-10 pl-20">Create a new project</PageTitle>
      <CreateProjectCard />
    </div>
  );
}
