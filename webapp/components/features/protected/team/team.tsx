"use client"

import { useEffect, useState } from 'react';
import { getProjectMembers, editProjectMemberRole } from '@/lib/actions/project-member.actions';
import { ProjectMemberResponse } from '@/types/api/responses/project-member';
import { usePathname } from "next/navigation";

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
        try {
            await editProjectMemberRole(projectId as string, memberId, { projectRole: newRole });
            setMembers(prev =>
                prev.map(m => m.id === memberId ? { ...m, projectRole: newRole } : m)
            );
        } catch (error) {
            console.error('Failed to update role:', error);
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
                    <div key={member.id} className="bg-white p-4 shadow rounded-lg flex justify-between items-center">
                <div>
                    <h3 className="text-lg font-semibold">{member.id}</h3>
                </div>
                <div className="flex items-center space-x-4">
                <select
                    value={member.projectRole}
                    onChange={(e) => handleRoleChange(member.id, parseInt(e.target.value))}
        className="border rounded px-2 py-1"
        >
            <option value={0}>Admin</option>
            <option value={1}>Member</option>
        </select>
        </div>
        </div>
    ))}
        </div>
    )}
    </div>
);
}