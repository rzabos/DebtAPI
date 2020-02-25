namespace DebtAPI.Models
{
    public class AuthenticationResponse
    {
        public AuthenticationResponse(bool isSuccessful, string message, string token = null)
        {
            IsSuccessful = isSuccessful;
            Message = message;
            Token = token;
        }

        public bool IsSuccessful { get; set; }

        public string Message { get; set; }

        public string Token { get; set; }
    }
}