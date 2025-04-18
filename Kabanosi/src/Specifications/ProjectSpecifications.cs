using System.Linq.Expressions;
using Kabanosi.Entities;

namespace Kabanosi.Specifications;

public static class ProjectSpecifications
{
    public static Expression<Func<Project, bool>> MemberBy(string userId) =>
        p => p.ProjectMembers.Any(pm => pm.UserId == userId);
}