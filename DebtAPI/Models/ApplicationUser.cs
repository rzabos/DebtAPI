using AspNetCore.Identity.MongoDbCore.Models;

namespace DebtAPI.Models
{
    public class ApplicationUser : MongoIdentityUser
    {
        public ApplicationUser(string userName)
            : base(userName)
        {
        }
    }
}