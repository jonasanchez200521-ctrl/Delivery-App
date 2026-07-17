using Nami.API.Models;

namespace Nami.API.Services
{
    public interface ITokenService
    {
        (string Token, DateTime ExpiresAt) GenerateToken(User user, string userType);
    }
}
