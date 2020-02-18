using System.Collections.Generic;
using DebtAPI.Models;

namespace DebtAPI.Services
{
    public interface IDataService
    {
        void AddDebt(Debt debt);

        List<Debt> GetDebts(int amount);
    }
}