import { getProjects } from "@/lib/actions/project.actions";
import { ProjectResponse } from "@/types/api/responses/project";
import { useEffect, useState } from "react";

export const useProject = (): [ProjectResponse[], React.Dispatch<React.SetStateAction<ProjectResponse[]>>] => {
    const [projects, setProjects] = useState<ProjectResponse[]>([]);

    useEffect(() => {
        const fetchData = async () => {
            const data = await getProjects();

            setProjects(() => data);
        }

        fetchData()
            .catch(console.error);
    }, []);

    return [projects, setProjects];
}