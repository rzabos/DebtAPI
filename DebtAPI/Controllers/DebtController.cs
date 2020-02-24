using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DebtAPI.Models;
using DebtAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DebtAPI.Controllers
{
    [Authorize]
    [Route("v1/debt")]
    public class DebtController : ControllerBase
    {
        private readonly IDataService _dataService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public DebtController(IDataService dataService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _dataService = dataService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("")]
        public void Add([FromBody] Debt debt)
        {
            _dataService.AddDebt(debt);
        }

        [HttpGet]
        [Route("{amount}")]
        public ActionResult<List<Debt>> Get(int amount)
        {
            var debts = _dataService.GetDebts(amount);
            if (debts?.Any() != true)
            {
                return NotFound();
            }

            return debts;
        }

        [HttpGet]
        [Route("auth")]
        public async Task<ActionResult> Login([FromBody] string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound();
            }

            var result = _signInManager.PasswordSignInAsync(user, password, true, false);
            if (!result.IsCompletedSuccessfully)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpPost]
        [Route("auth")]
        public async Task<ActionResult> Register([FromBody] string userName, string password)
        {
            var user = new ApplicationUser(userName);
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}