using Kabanosi.Entities;

namespace Kabanosi.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}