using Kabanosi.Entities;

namespace Kabanosi.Services.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}