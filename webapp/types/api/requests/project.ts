export interface CreateProjectRequest {
    name: string,
    description: string,
}

export type JsonPatchProject = {
    op: "replace";
    path: string;
    value?: any;
};