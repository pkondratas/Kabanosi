using Kabanosi.Entities;
using Kabanosi.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Kabanosi.Repositories;

public class ProjectRepository(DatabaseContext context) : GenericRepository<Project>(context)
{
    public async Task<string> GetProjectNameAsync(Guid projectId, CancellationToken ct)
    {
        return await context.Projects
            .Where(p => p.Id == projectId)
            .Select(p => p.Name)
            .SingleAsync(ct);
    }

    public async Task<Project?> GetProjectByIdAsync(Guid projectId, CancellationToken ct)
    {
        return await context.Projects
            .Include(p => p.ProjectMembers) // Include related data if needed
            .FirstOrDefaultAsync(p => p.Id == projectId, ct);
    }
}