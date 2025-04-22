using Kabanosi.Dtos.Invitation;

namespace Kabanosi.Services.Interfaces;

public interface INotificationService
{
    Task InviteReceivedAsync(string userId, UserInvitesResponseDto newInviteDto);
}