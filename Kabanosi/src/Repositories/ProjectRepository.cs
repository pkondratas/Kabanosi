using Kabanosi.Entities;
using Kabanosi.Persistence;

namespace Kabanosi.Repositories;

public class ProjectRepository(DatabaseContext context) : GenericRepository<Project>(context)
{
    
}