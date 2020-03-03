using MessageLibrary.Requests;

namespace MessageLibrary.Helpers
{
    public static class ValidateRequests
    {
        public static string Validate(AuthenticationRequest authenticationRequest)
        {
            if (authenticationRequest == null)
            {
                return "Invalid request!";
            }

            if (string.IsNullOrWhiteSpace(authenticationRequest.UserName))
            {
                return "Invalid username!";
            }

            if (string.IsNullOrWhiteSpace(authenticationRequest.Password))
            {
                return "Invalid password!";
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

        public static string Validate(AddDebtRequest addDebtRequest)
        {
            var requestValidation = Validate(addDebtRequest as Request);
            if (requestValidation != null)
            {
                return requestValidation;
            }

            var debtValidation = Validate(addDebtRequest.Debt);
            if (debtValidation != null)
            {
                return debtValidation;
            }

            return null;
        }

        public static string Validate(Request request)
        {
            if (request == null)
            {
                return "Invalid request!";
            }

            if (string.IsNullOrWhiteSpace(request.OppositeUser))
            {
                return "No opposite user has been specified!";
            }

            return null;
        }
    }
}