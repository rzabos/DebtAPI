using DebtAPI.Models;

namespace DebtAPI.Services
{
    public interface ITokenService
    {
        string GenerateToken(ApplicationUser applicationUser);
    }
}