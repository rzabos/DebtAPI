using AspNetCore.Identity.MongoDbCore.Models;

namespace DebtAPI.Models.Authentication
{
    public class ApplicationRole : MongoIdentityRole
    {
        public ApplicationRole()
            : base()
        {
        }

        public ApplicationRole(string roleName)
            : base(roleName)
        {
        }
    }
}