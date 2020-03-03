using System;

namespace MessageLibrary.Requests
{
    public class Debt
    {
        public int Amount { get; set; }

        public string Asset { get; set; }

        public DateTime Date { get; set; }
    }
}