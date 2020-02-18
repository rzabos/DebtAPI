using System.Collections.Generic;
using System.Linq;
using DebtAPI.Models;
using DebtAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DebtAPI.Controllers
{
    public class DebtController : ControllerBase
    {
        private readonly IDataService _dataService;

        public DebtController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        [Route("api/debt/{amount}")]
        public ActionResult<List<Debt>> Get(int amount)
        {
            var debts = _dataService.GetDebts(amount);
            if (debts?.Any() != true)
            {
                return NotFound();
            }

            return debts;
        }

        [HttpPost]
        [Route("api/debt")]
        public void Post([FromBody] Debt debt)
        {
            _dataService.AddDebt(debt);
        }
    }
}