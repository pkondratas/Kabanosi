"use client";

import Link from "next/link";
import { useQuery } from "@tanstack/react-query";
import { getProjects } from "@/lib/actions/project.actions";
import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import {
  Carousel,
  CarouselApi,
  CarouselContent,
  CarouselItem,
  CarouselNext,
  CarouselPrevious,
} from "@/components/ui/carousel";
import {
  Tooltip,
  TooltipContent,
  TooltipProvider,
  TooltipTrigger,
} from "@/components/ui/tooltip";
import { ChevronRight } from "lucide-react";
import { useEffect, useState } from "react";
import { useProject } from "@/components/providers/SelectedProjectProvider";

export const ProjectsCarousel = () => {
  const { data: projects = [] } = useQuery({
    queryKey: ["projects"],
    queryFn: getProjects,
  });
  const [api, setApi] = useState<CarouselApi>();
  const [current, setCurrent] = useState(0);
  const { setProject } = useProject();

  useEffect(() => {
    if (!api) return;

    setCurrent(api.selectedScrollSnap() + 1);

    api.on("select", () => {
      setCurrent(api.selectedScrollSnap() + 1);
    });
  }, [api]);

  return (
    <div className="flex flex-col items-center w-full">
      <Carousel
        opts={{
          align: "start",
        }}
        className="w-full max-w-[90%]"
        setApi={setApi}
      >
        <CarouselContent>
          {projects.map((project, index) => (
            <CarouselItem key={index} className="lg:basis-1/3 group">
              <Link
                href={`/${project.id}/backlog`}
                className="block p-1"
                onClick={() => setProject(project)}
              >
                <Card className="min-h-55 bg-gray-100 p-4 rounded-lg shadow-lg hover:bg-gray-200">
                  <TooltipProvider>
                    <Tooltip>
                      <TooltipTrigger asChild>
                        <CardHeader className="text-xl font-bold break-words text-ellipsis line-clamp-1 text-center">
                          {project.name}
                        </CardHeader>
                      </TooltipTrigger>
                      <TooltipContent>{project.name}</TooltipContent>
                    </Tooltip>
                  </TooltipProvider>
                  <CardDescription className="pl-6">
                    Project description:
                  </CardDescription>
                  <CardContent className="w-full">
                    <div className="flex items-center w-full min-h-25 gap-x-4">
                      <p className="break-words text-ellipsis line-clamp-3 flex-1">
                        {project.description}
                      </p>
                      <Button
                        variant="ghost"
                        className="opacity-0 group-hover:opacity-100 hover:bg-gray-300"
                      >
                        <ChevronRight />
                      </Button>
                    </div>
                  </CardContent>
                </Card>
              </Link>
            </CarouselItem>
          ))}
        </CarouselContent>
        <CarouselPrevious />
        <CarouselNext />
      </Carousel>
      {projects.length == 3 ? (
        <div className="py-2 text-center text-sm text-muted-foreground">
          All {projects.length} of your projects.
        </div>
      ) : (
        <div className="py-2 text-center text-sm text-muted-foreground">
          Projects {current} - {current + 2} of {projects.length}
        </div>
      )}
    </div>
  );
};
