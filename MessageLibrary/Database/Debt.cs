using System;
using MongoDB.Bson.Serialization.Attributes;

namespace MessageLibrary.Database
{
    [BsonIgnoreExtraElements]
    public class Debt
    {
        public int Amount { get; set; }

        public string Asset { get; set; }

        public DateTime Date { get; set; }

        public string UserName { get; set; }
    }
}