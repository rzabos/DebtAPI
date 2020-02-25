using System.Collections.Generic;
using System.Linq;
using DebtAPI.Models;
using DebtAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DebtAPI.Controllers
{
    [Authorize]
    [Route("v1/debt")]
    public class DebtController : ControllerBase
    {
        private readonly IDataService _dataService;

        public DebtController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpPost]
        public void AddDebt([FromBody] Debt debt)
        {
            _dataService.AddDebt(debt);
        }

        [HttpGet]
        public void GetActual()
        {
        }

        [HttpGet]
        [Route("{amount}")]
        public ActionResult<List<Debt>> GetDebts(int amount)
        {
            var debts = _dataService.GetDebts(amount);
            if (debts?.Any() != true)
            {
                return NotFound();
            }

            return debts;
        }
    }
}