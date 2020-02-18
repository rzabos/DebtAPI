using System;
using System.Collections.Generic;
using DebtAPI.Models;
using MongoDB.Driver;

namespace DebtAPI.Services
{
    public class DataService : IDataService
    {
        private readonly IMongoCollection<Debt> _debtCollection;

        public DataService(AppSettings appSettings)
        {
            _debtCollection = new MongoClient().GetDatabase(appSettings.DatabaseName).GetCollection<Debt>(appSettings.CollectionName);
        }

        public void AddDebt(Debt debt)
        {
            if (debt == null)
            {
                throw new ArgumentNullException(nameof(debt));
            }

            _debtCollection.InsertOne(debt);
        }

        public List<Debt> GetDebts(int amount)
        {
            return _debtCollection.Find(x => true).Limit(amount).ToList();
        }
    }
}