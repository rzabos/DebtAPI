using MessageLibrary.Requests;
using MongoDB.Bson.Serialization.Attributes;

namespace MessageLibrary.Database
{
    [BsonIgnoreExtraElements]
    public class DebtWrapper : Debt
    {
        public DebtWrapper(Debt debt, string userName)
        {
            Amount = debt.Amount;
            Asset = debt.Asset;
            Date = debt.Date;
            UserName = userName;
        }

        public string UserName { get; private set; }
    }
}