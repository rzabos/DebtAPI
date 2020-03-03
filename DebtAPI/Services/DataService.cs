using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DebtAPI.Models.Settings;
using MessageLibrary.Database;
using MessageLibrary.Requests;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DebtAPI.Services
{
    public class DataService : IDataService
    {
        private readonly IMongoDatabase _mongoDatabase;
        private readonly int _pageSize;

        public DataService(IOptions<DatabaseSettings> databaseSettings)
        {
            _mongoDatabase = new MongoClient().GetDatabase(databaseSettings.Value.HistoryDatabase);
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

            var collection = _mongoDatabase.GetCollection<DebtWrapper>(await GetCollectionName(contract) ?? contract.ToString());
            var wrapper = new DebtWrapper(debt, contract.UserName);
            await collection.InsertOneAsync(wrapper);
        }

        public async Task<IEnumerable<DebtWrapper>> GetDebts(int page, Contract contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            var collectionName = await GetCollectionName(contract);
            if (collectionName == null)
            {
                throw new KeyNotFoundException("The given contract is missing from the database!");
            }

            var collection = _mongoDatabase.GetCollection<DebtWrapper>(collectionName);

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

            var collectionName = await GetCollectionName(contract);
            if (collectionName == null)
            {
                throw new KeyNotFoundException("The given contract is missing from the database!");
            }

            var collection = _mongoDatabase.GetCollection<DebtWrapper>(collectionName);
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

        private async Task<string> GetCollectionName(Contract contract)
        {
            var collectionNames = await _mongoDatabase.ListCollectionNames().ToListAsync();
            return collectionNames.FirstOrDefault(c => c == contract.UserName + contract.OppositeUserName || c == contract.OppositeUserName + contract.UserName);
        }
    }
}