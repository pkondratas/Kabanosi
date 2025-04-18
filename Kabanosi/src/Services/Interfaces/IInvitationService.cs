using Kabanosi.Dtos.Invitation;

namespace Kabanosi.Services.Interfaces;

public interface IInvitationService
{
    Task<InvitationResponseDto> CreateInvitationAsync(Guid projectId, CreateInvitationDto invitationDto,
        CancellationToken cancellationToken);
}