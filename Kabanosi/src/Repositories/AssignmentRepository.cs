using Kabanosi.Entities;
using Kabanosi.Persistence;

namespace Kabanosi.Repositories;

public class AssignmentRepository(DatabaseContext context) : GenericRepository<Assignment>(context)
{
} 