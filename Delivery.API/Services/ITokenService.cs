using Delivery.API.Models;

namespace Delivery.API.Services
{
    public interface ITokenService
    {
        (string Token, DateTime ExpiresAt) GenerateToken(User user, string userType);
    }
}
