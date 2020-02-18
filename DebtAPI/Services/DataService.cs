using System;
using System.Collections.Generic;
using System.Linq;
using DebtAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DebtAPI.Services
{
    public class DataService : IDataService
    {
        private readonly IMongoCollection<Debt> _debtCollection;

        public DataService(IOptions<AppSettings> appSettings)
        {
            _debtCollection = new MongoClient().GetDatabase(appSettings.Value.DatabaseName).GetCollection<Debt>(appSettings.Value.CollectionName);
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
            return _debtCollection.AsQueryable().OrderByDescending(d => d.Date).Take(amount).ToList();
        }
    }
}