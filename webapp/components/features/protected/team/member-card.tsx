import { ProjectMemberResponse } from "@/types/api/responses/project-member";

interface MemberCardProps {
    member: ProjectMemberResponse;
    isCurrentUser: boolean;
    currentUserRole: number;
    onRoleChange: (memberId: string, newRole: number) => void;
    onDelete: (memberId: string) => void;
}

export default function MemberCard({ member, isCurrentUser, currentUserRole, onRoleChange, onDelete }: MemberCardProps) {
    const roleText = member.projectRole === 0 ? "Admin" : "Member";

    return (
        <div className="bg-white p-4 shadow rounded-lg flex justify-between items-center">
            <div>
                <h3 className="text-lg font-semibold">{member.username}</h3>
                <p className="text-gray-500 text-sm">{member.email}</p>
            </div>
            <div className="flex items-center space-x-4">

                {!isCurrentUser && currentUserRole === 0  ? (
                    <select
                        className="border rounded px-2 py-1"
                        value={member.projectRole}
                        onChange={(e) =>
                            onRoleChange(member.id, parseInt(e.target.value, 10))
                        }
                    >
                        <option value={0}>Admin</option>
                        <option value={1}>Member</option>
                    </select>
                ) : (
                    <span className="px-2 py-1">{roleText}</span>
                )}

                {!isCurrentUser && currentUserRole === 0 ? (
                    <button
                        onClick={() => onDelete(member.id)}
                        className="px-3 py-1 text-sm font-medium text-white bg-red-500 rounded hover:bg-red-600 transition-colors"
                    >
                        Remove
                    </button>
                ) : null}
            </div>
        </div>
    );
}