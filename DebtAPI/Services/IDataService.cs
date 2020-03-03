using System.Collections.Generic;
using System.Threading.Tasks;
using MessageLibrary.Database;
using MessageLibrary.Requests;

namespace DebtAPI.Services
{
    public interface IDataService
    {
        Task AddDebt(Debt debt, Contract contract);

        Task<IEnumerable<DebtWrapper>> GetDebts(int page, Contract contract);

        Task<int> GetFinance(Contract contract);
    }
}