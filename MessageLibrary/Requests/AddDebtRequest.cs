using MessageLibrary.Database;

namespace MessageLibrary.Requests
{
    public class AddDebtRequest : Request
    {
        public Debt Debt { get; set; }
    }
}