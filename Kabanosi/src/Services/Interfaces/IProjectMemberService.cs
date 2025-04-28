using Kabanosi.Dtos.ProjectMember;

namespace Kabanosi.Services.Interfaces;

public interface IProjectMemberService
{
    Task<IList<ProjectMemberResponseDto>> GetProjectMembersAsync(
        Guid projectId,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken);
    Task DeleteProjectMemberAsync(Guid projectMemberId, CancellationToken cancellationToken);
}