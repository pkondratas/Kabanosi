using Kabanosi.Entities;
using Kabanosi.Persistence;

namespace Kabanosi.Repositories;

public class ProjectMemberRepository(DatabaseContext context) : GenericRepository<ProjectMember>(context)
{
}