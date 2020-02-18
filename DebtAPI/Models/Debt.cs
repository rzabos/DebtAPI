using System;
using MongoDB.Bson.Serialization.Attributes;

namespace DebtAPI.Models
{
    [BsonIgnoreExtraElements]
    public class Debt
    {
        public int Amount { get; set; }

        public DateTime Date { get; set; }

        public string Name { get; set; }
    }
}