using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DebtAPI.Models.Authentication;
using DebtAPI.Services;
using MessageLibrary.Database;
using MessageLibrary.Helpers;
using MessageLibrary.Requests;
using MessageLibrary.Responses;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public DebtController(IDataService dataService, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _dataService = dataService;
        }

        [HttpPost]
        public async Task<ActionResult<Response>> AddDebt([FromBody] AddDebtRequest addDebtRequest)
        {
            var requestValidation = ValidateRequests.Validate(addDebtRequest);
            if (requestValidation != null)
            {
                return BadRequest(new Response(requestValidation));
            }

            try
            {
                var contract = new Contract(User.Identity.Name, addDebtRequest.OppositeUser);
                await _dataService.AddDebt(addDebtRequest.Debt, contract);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new Response(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new Response("Something went wrong!"));
            }

            return Ok(new Response("The given debt informations have been processed successfully!", true));
        }

        [HttpGet]
        public async Task<ActionResult<Response>> GetFinance([FromBody] Request financeRequest)
        {
            var requestValidation = ValidateRequests.Validate(financeRequest);
            if (requestValidation != null)
            {
                return BadRequest(new Response(requestValidation));
            }

            var oppositeUser = await _userManager.FindByNameAsync(financeRequest.OppositeUser);
            if (oppositeUser == null)
            {
                return BadRequest(new Response("Opposite user is not registered!"));
            }

            var currentUser = User.Identity.Name;
            if (currentUser == oppositeUser.UserName)
            {
                return BadRequest(new Response("Speficy different user than yours!"));
            }

            try
            {
                var finance = await _dataService.GetFinance(new Contract(currentUser, oppositeUser.UserName));
                return Ok(new FinanceResponse("Finance has been calculated successfully!", true, finance));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new Response(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new Response("Something went wrong!"));
            }
        }

        [HttpGet]
        [Route("history")]
        public async Task<ActionResult<Response>> GetHistory([FromBody] HistoryRequest historyRequest)
        {
            var page = historyRequest.Page < 1 ? 1 : historyRequest.Page;

            var oppositeUser = await _userManager.FindByNameAsync(historyRequest.OppositeUser);
            if (oppositeUser == null)
            {
                return BadRequest(new Response("Opposite user is not registered!"));
            }

            var currentUser = User.Identity.Name;
            if (currentUser == oppositeUser.UserName)
            {
                return BadRequest(new Response("Speficy different user than yours!"));
            }

            try
            {
                var debts = await _dataService.GetDebts(page, new Contract(currentUser, oppositeUser.UserName));
                if (debts?.Any() != true)
                {
                    return NotFound(new Response("Cannot find any debts!"));
                }

                return Ok(new HistoryResponse("Debts were fetched successfully!", true, debts));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new Response(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new Response("Something went wrong!"));
            }
        }
    }
}