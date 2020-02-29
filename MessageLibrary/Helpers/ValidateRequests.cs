using MessageLibrary.Requests;

namespace MessageLibrary.Helpers
{
    public static class ValidateRequests
    {
        public static string Validate(AddDebtRequest addDebtRequest)
        {
            var requestValidation = Validate(addDebtRequest);
            if (requestValidation == null)
            {
                return requestValidation;
            }

            var debtValidation = DatabaseHelper.Validate(addDebtRequest.Debt);
            if (debtValidation == null)
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