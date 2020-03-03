using System.Collections.Generic;
using MessageLibrary.Database;

namespace MessageLibrary.Responses
{
    public class HistoryResponse : Response
    {
        public HistoryResponse(string message, bool isSuccessful, IEnumerable<DebtWrapper> debts)
            : this(message, isSuccessful)
        {
            Debts = debts;
        }

        public HistoryResponse(string message)
            : base(message)
        {
        }

        public HistoryResponse(string message, bool isSuccessful)
            : base(message, isSuccessful)
        {
        }

        public IEnumerable<DebtWrapper> Debts { get; private set; }
    }
}