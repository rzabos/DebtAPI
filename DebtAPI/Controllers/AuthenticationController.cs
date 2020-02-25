using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebtAPI.Models;
using DebtAPI.Services;
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

        [HttpGet]
        public async Task<ActionResult> Login([FromBody] AuthenticationRequest userRequest)
        {
            if (!IsValidRequest(userRequest))
            {
                return BadRequest(new AuthenticationResponse(false, "The request is invalid!"));
            }

            var user = await _userManager.FindByNameAsync(userRequest.UserName);
            if (user == null)
            {
                return Unauthorized(new AuthenticationResponse(false, $"{userRequest.UserName} is an invalid username!"));
            }

            var result = _signInManager.PasswordSignInAsync(user, userRequest.Password, true, false);
            if (!result.IsCompletedSuccessfully)
            {
                return Unauthorized(new AuthenticationResponse(false, "The password is invalid!"));
            }

            return Ok(new AuthenticationResponse(true, "Login was successful!", _tokenService.GenerateToken(user)));
        }

        [HttpPost]
        public async Task<ActionResult> Register([FromBody] AuthenticationRequest userRequest)
        {
            if (!IsValidRequest(userRequest))
            {
                return BadRequest(new AuthenticationResponse(false, "The request is invalid!"));
            }

            var user = new ApplicationUser(userRequest.UserName);
            var result = await _userManager.CreateAsync(user, userRequest.Password);
            if (result.Errors?.Any() == true)
            {
                return Unauthorized(new AuthenticationResponse(false, AppendErrors(result.Errors)));
            }

            return Ok(new AuthenticationResponse(true, "Register was successful!"));
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

        private bool IsValidRequest(AuthenticationRequest userRequest) => userRequest != null && !string.IsNullOrWhiteSpace(userRequest.UserName) && !string.IsNullOrWhiteSpace(userRequest.Password);
    }
}