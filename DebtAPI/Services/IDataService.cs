using System.Collections.Generic;
using MessageLibrary.Database;

namespace DebtAPI.Services
{
    public interface IDataService
    {
        void AddDebt(Debt debt, Contract contract);

        IEnumerable<Debt> GetDebts(int page, Contract contract);

        int GetFinance(Contract contract);
    }
}