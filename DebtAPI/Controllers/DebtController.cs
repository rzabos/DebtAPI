using System;
using System.Collections.Generic;
using System.Linq;
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
        public Response AddDebt([FromBody] AddDebtRequest addDebtRequest)
        {
            var requestValidation = ValidateRequests.Validate(addDebtRequest);
            if (requestValidation != null)
            {
                return new Response(requestValidation);
            }

            try
            {
                var contract = new Contract(User.Identity.Name, addDebtRequest.OppositeUser);
                _dataService.AddDebt(addDebtRequest.Debt, contract);
            }
            catch (KeyNotFoundException ex)
            {
                return new Response(ex.Message);
            }
            catch (Exception)
            {
                return new Response("Something went wrong!");
            }

            return new Response("The given debt informations have been processed successfully!", true);
        }

        [HttpGet]
        public Response GetFinance([FromBody] Request financeRequest)
        {
            var requestValidation = ValidateRequests.Validate(financeRequest);
            if (requestValidation != null)
            {
                return new Response(requestValidation);
            }

            try
            {
                var finance = _dataService.GetFinance(new Contract(User.Identity.Name, financeRequest.OppositeUser));
                return new FinanceResponse("Finance has been calculated successfully!", true, finance);
            }
            catch (KeyNotFoundException ex)
            {
                return new Response(ex.Message);
            }
            catch (Exception)
            {
                return new Response("Something went wrong!");
            }
        }

        [HttpGet]
        [Route("{amount}")]
        public Response GetHistory([FromBody] HistoryRequest historyRequest)
        {
            var page = historyRequest.Page < 1 ? 1 : historyRequest.Page;

            try
            {
                var debts = _dataService.GetDebts(page, new Contract(User.Identity.Name, historyRequest.OppositeUser));
                if (debts?.Any() != true)
                {
                    return new Response("Cannot find any debts!");
                }

                return new HistoryResponse("Debts were fetched successfully!", true, debts);
            }
            catch (Exception)
            {
                return new Response("Something went wrong!");
            }
        }
    }
}