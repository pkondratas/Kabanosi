using Microsoft.AspNetCore.SignalR;

namespace Kabanosi.Hubs;

public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        
        if (!string.IsNullOrEmpty(userId))
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{userId}");

        await base.OnConnectedAsync();
    }
    
    public Task JoinProjectGroup(Guid projectId) =>
        Groups.AddToGroupAsync(Context.ConnectionId, $"project:{projectId}");

    public Task LeaveProjectGroup(Guid projectId) =>
        Groups.RemoveFromGroupAsync(Context.ConnectionId, $"project:{projectId}");
}