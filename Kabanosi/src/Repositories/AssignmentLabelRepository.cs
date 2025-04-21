using Kabanosi.Entities;
using Kabanosi.Persistence;

namespace Kabanosi.Repositories;

public class AssignmentLabelRepository(DatabaseContext context) : GenericRepository<AssignmentLabel>(context)
{
    
}