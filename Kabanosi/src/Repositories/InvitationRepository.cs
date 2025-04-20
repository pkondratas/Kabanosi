using Kabanosi.Entities;
using Kabanosi.Persistence;

namespace Kabanosi.Repositories;

public class InvitationRepository(DatabaseContext context) : GenericRepository<Invitation>(context)
{
    
}