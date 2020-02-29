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

        public static string Validate(Debt debt)
        {
            if (debt == null)
            {
                return "Invalid request!";
            }

            if (string.IsNullOrWhiteSpace(debt.Asset))
            {
                return "Invalid asset has been specified!";
            }

            if (debt.Amount < 0)
            {
                return "Invalid amount of debt has been specified!";
            }

            return null;
        }
    }
}