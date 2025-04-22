using Kabanosi.Dtos.Invitation;
using Kabanosi.Hubs;
using Kabanosi.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Kabanosi.Services;

public class NotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub> _hub;

    public NotificationService(IHubContext<NotificationHub> hub)
    {
        _hub = hub;
    }

    public Task InviteReceivedAsync(string userId, UserInvitesResponseDto newInviteDto) =>
        _hub.Clients.Group($"user:{userId}").SendAsync("InviteReceived", newInviteDto);
}