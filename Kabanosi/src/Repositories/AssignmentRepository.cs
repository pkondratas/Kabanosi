using Kabanosi.Entities;
using Kabanosi.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Kabanosi.Repositories;

public class AssignmentRepository(DatabaseContext context) : GenericRepository<Assignment>(context)
{
    public Task<Assignment?> GetAssignmentByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return context.Assignments
            .Include(a => a.Reporter)
                .ThenInclude(pm => pm.User)
            .Include(a => a.Assignee)
                .ThenInclude(pm => pm.User)
            .Include(a => a.AssignmentLabel)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public Task<List<Assignment>> GetProjectAssignmentsAsync(Guid projectId, CancellationToken cancellationToken)
    {
        return context.Assignments
            .AsNoTracking()
            .Where(a => a.ProjectId == projectId)
            .Include(a => a.Reporter)
                .ThenInclude(pm => pm.User)
            .Include(a => a.Assignee)
                .ThenInclude(pm => pm.User)
            .Include(a => a.AssignmentLabel)
            .ToListAsync(cancellationToken);
    }

    public Task<List<Assignment>> GetProjectPlannedAssignmentsAsync(Guid projectId, CancellationToken cancellationToken)
    {
        return context.Assignments
            .AsNoTracking()
            .Where(a => a.ProjectId == projectId && a.IsPlanned)
            .Include(a => a.Reporter)
                .ThenInclude(pm => pm.User)
            .Include(a => a.Assignee)
                .ThenInclude(pm => pm.User)
            .Include(a => a.AssignmentLabel)
            .ToListAsync(cancellationToken);
    }
} 