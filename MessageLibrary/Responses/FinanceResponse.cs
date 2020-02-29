namespace MessageLibrary.Responses
{
    public class FinanceResponse : Response
    {
        public FinanceResponse(string message, bool isSuccessful, int finance)
            : this(message, isSuccessful)
        {
            Finance = finance;
        }

        public FinanceResponse(string message)
            : base(message)
        {
        }

        public FinanceResponse(string message, bool isSuccessful)
            : base(message, isSuccessful)
        {
        }

        public int Finance { get; private set; }
    }
}