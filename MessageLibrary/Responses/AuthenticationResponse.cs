namespace MessageLibrary.Responses
{
    public class AuthenticationResponse
    {
        public AuthenticationResponse(string message, bool isSuccessful, string token)
            : this(message)
        {
            IsSuccessful = isSuccessful;
            Token = token;
        }

        public AuthenticationResponse(string message)
        {
            Message = message;
        }

        public bool IsSuccessful { get; private set; }

        public string Message { get; private set; }

        public string Token { get; private set; }
    }
}