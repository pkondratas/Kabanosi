using Kabanosi.Dtos.Invitation;

namespace Kabanosi.Services.Interfaces;

public interface IInvitationService
{
    Task<InvitationResponseDto> CreateInviteAsync(Guid projectId, CreateInvitationDto invitationDto,
        CancellationToken cancellationToken);

    Task<IList<UserInvitesResponseDto>> GetUserInvitesAsync(int pageSize, int pageNumber,
        CancellationToken cancellationToken);

    Task<IList<InvitationResponseDto>> GetProjectInvitesAsync(Guid projectId, int pageSize, int pageNumber,
        CancellationToken cancellationToken);
}