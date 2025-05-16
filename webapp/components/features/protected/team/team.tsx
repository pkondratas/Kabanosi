"use client"

import { useEffect, useState } from "react";
import { usePathname } from "next/navigation";
import { getProjectMembers, editProjectMemberRole, deleteProjectMember } from "@/lib/actions/project-member.actions";
import { ProjectMemberResponse } from "@/types/api/responses/project-member";
import MemberCard from "./member-card";

export default function Team() {
    const pathname = usePathname();
    const projectId = pathname.split("/")[1];

    const [members, setMembers] = useState<ProjectMemberResponse[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        if (!projectId) return;

        const fetchMembers = async () => {
            try {
                const fetchedMembers = await getProjectMembers(projectId as string);
                setMembers(fetchedMembers);
            } catch (error) {
                console.error('Failed to fetch members:', error);
            } finally {
                setLoading(false);
            }
        };

        fetchMembers();
    }, [projectId]);

    const handleRoleChange = async (memberId: string, newRole: number) => {
        const oldRole = members.find(m => m.id === memberId)?.projectRole;

        setMembers(prev =>
            prev.map(m => m.id === memberId ? { ...m, projectRole: newRole } : m)
        );
        try {
            await editProjectMemberRole(projectId as string, memberId, { projectRole: newRole });
        } catch (error) {
            setMembers(prev =>
                prev.map(m => m.id === memberId ? { ...m, projectRole: oldRole ?? 1 } : m)
            );
            console.error('Failed to update role:', error);
        }
    };

    const handleDelete = async (memberId: string) => {
        try {
            setMembers(prev => prev.filter(m => m.id !== memberId));
            await deleteProjectMember(projectId as string, memberId);
        } catch (error) {
            console.error("Failed to delete member:", error);
        }
    };

    return (
        <div className="p-6 flex flex-col items-center gap-10">
            <h1 className="text-2xl font-bold mb-6">Team members</h1>
            {loading ? (
                <p>Loading...</p>
            ) : (
                <div className="w-full max-w-5xl space-y-4">
                    {members.map((member) => (
                        <MemberCard
                            key={member.id}
                            member={member}
                            onRoleChange={handleRoleChange}
                            onDelete={handleDelete}
                        />
                    ))}
                </div>
            )}
        </div>
    );
}