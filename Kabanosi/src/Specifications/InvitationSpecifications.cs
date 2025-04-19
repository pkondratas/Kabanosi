using System.Linq.Expressions;
using Kabanosi.Constants;
using Kabanosi.Entities;

namespace Kabanosi.Specifications;

public static class InvitationSpecifications
{
    public static Expression<Func<Invitation, bool>> ForUser(string userId) =>
        i => i.UserId == userId;
    
    public static Expression<Func<Invitation, bool>> ForUserWithStatus(string userId, InvitationStatus status) =>
        i => i.UserId == userId && i.InvitationStatus == status;

    public static Expression<Func<Invitation, bool>> ForProject(Guid projectId) =>
        i => i.ProjectId == projectId;
}