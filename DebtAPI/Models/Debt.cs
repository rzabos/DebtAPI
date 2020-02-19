using System;
using MongoDB.Bson.Serialization.Attributes;

namespace DebtAPI.Models
{
    [BsonIgnoreExtraElements]
    public class Debt
    {
        public int Amount { get; set; }

        public string Asset { get; set; }

        public DateTime Date { get; set; }
    }
}