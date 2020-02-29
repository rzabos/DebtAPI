using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DebtAPI.Models.Authentication;
using DebtAPI.Services;
using MessageLibrary.Helpers;
using MessageLibrary.Requests;
using MessageLibrary.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DebtAPI.Controllers
{
    [Route("v1/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthenticationController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] AuthenticationRequest userRequest)
        {
            var requestValidation = ValidateRequests.Validate(userRequest);
            if (requestValidation != null)
            {
                return BadRequest(new AuthenticationResponse(requestValidation));
            }

            try
            {
                var user = await _userManager.FindByNameAsync(userRequest.UserName);
                if (user == null)
                {
                    return Unauthorized(new AuthenticationResponse($"{userRequest.UserName} is an invalid username!"));
                }

                var result = _signInManager.PasswordSignInAsync(user, userRequest.Password, true, false);
                if (!result.IsCompletedSuccessfully)
                {
                    return Unauthorized(new AuthenticationResponse("The password is invalid!"));
                }

                return Ok(new AuthenticationResponse("Login was successful!", true, _tokenService.GenerateToken(user)));
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new AuthenticationResponse("Something went wrong!"));
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register([FromBody] AuthenticationRequest userRequest)
        {
            var requestValidation = ValidateRequests.Validate(userRequest);
            if (requestValidation != null)
            {
                return BadRequest(new AuthenticationResponse(requestValidation));
            }

            try
            {
                var user = new ApplicationUser(userRequest.UserName);
                var result = await _userManager.CreateAsync(user, userRequest.Password);
                if (result.Errors?.Any() == true)
                {
                    return Unauthorized(new AuthenticationResponse(AppendErrors(result.Errors)));
                }

                return Ok(new AuthenticationResponse("Register was successful!"));
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new AuthenticationResponse("Something went wrong!"));
            }
        }

        private string AppendErrors(IEnumerable<IdentityError> errors)
        {
            var stringBuilder = new StringBuilder();

            foreach (var error in errors)
            {
                stringBuilder.AppendLine(error.Description);
            }

            return stringBuilder.ToString();
        }
    }
}