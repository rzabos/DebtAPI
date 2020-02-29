using DebtAPI.Models.Authentication;

namespace DebtAPI.Services
{
    public interface ITokenService
    {
        string GenerateToken(ApplicationUser applicationUser);
    }
}