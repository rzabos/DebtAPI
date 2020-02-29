namespace MessageLibrary.Responses
{
    public class Response
    {
        public Response(string message, bool isSuccessful)
            : this(message)
        {
            IsSuccessful = isSuccessful;
        }

        public Response(string message)
        {
            Message = message;
        }

        public bool IsSuccessful { get; private set; }

        public string Message { get; private set; }
    }
}