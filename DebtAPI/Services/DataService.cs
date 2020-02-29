using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DebtAPI.Models.Settings;
using MessageLibrary.Database;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DebtAPI.Services
{
    public class DataService : IDataService
    {
        private readonly IMongoDatabase _mongoDatabase;
        private readonly int _pageSize;

        public DataService(IOptions<DatabaseSettings> databaseSettings)
        {
            _mongoDatabase = new MongoClient().GetDatabase(databaseSettings.Value.DatabaseName);
            _pageSize = databaseSettings.Value.PageSize;
        }

        public async Task AddDebt(Debt debt, Contract contract)
        {
            if (debt == null)
            {
                throw new ArgumentNullException(nameof(debt));
            }

            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            var collectionName = contract.ToString();
            if (!Exists(collectionName))
            {
                throw new KeyNotFoundException($"{collectionName} is not found in the database!");
            }

            var collection = _mongoDatabase.GetCollection<Debt>(contract.ToString());
            await collection.InsertOneAsync(debt);
        }

        public async Task<IEnumerable<Debt>> GetDebts(int page, Contract contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            var collection = GetCollection(contract);
            if (collection == null)
            {
                throw new KeyNotFoundException("The given contract is missing from the database!");
            }

            return await collection.AsQueryable().ToAsyncEnumerable()
                .OrderByDescending(d => d.Date)
                .Skip(_pageSize * (page - 1))
                .Take(_pageSize)
                .ToList();
        }

        public async Task<int> GetFinance(Contract contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            var collection = GetCollection(contract);
            if (collection == null)
            {
                throw new KeyNotFoundException("The given contract is missing from the database!");
            }

            var finance = 0;
            await collection.AsQueryable().ForEachAsync(debt =>
            {
                if (debt.UserName == contract.UserName)
                {
                    finance += debt.Amount;
                }
                else
                {
                    finance -= debt.Amount;
                }
            });

            return finance;
        }

        private bool Exists(string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            var options = new ListCollectionNamesOptions { Filter = filter };

            return _mongoDatabase.ListCollectionNames(options).Any();
        }

        private IMongoCollection<Debt> GetCollection(Contract contract)
        {
            var contractName = contract.ToString();
            return Exists(contractName) ? _mongoDatabase.GetCollection<Debt>(contractName) : null;
        }
    }
}