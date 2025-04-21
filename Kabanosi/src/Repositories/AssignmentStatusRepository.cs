using Kabanosi.Entities;
using Kabanosi.Persistence;

namespace Kabanosi.Repositories;

public class AssignmentStatusRepository(DatabaseContext context) : GenericRepository<AssignmentStatus>(context)
{
}