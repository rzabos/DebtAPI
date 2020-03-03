using MessageLibrary.Database;

namespace MessageLibrary.Helpers
{
    public class DatabaseHelper
    {
        public static string Validate(Contract contract)
        {
            if (contract == null)
            {
                return "Invalid contract!";
            }

            if (string.IsNullOrWhiteSpace(contract.UserName))
            {
                return "Given contract contains invalid username!";
            }

            if (string.IsNullOrWhiteSpace(contract.OppositeUserName))
            {
                return "Given contract contains invalid opposite username!";
            }

            return null;
        }
    }
}