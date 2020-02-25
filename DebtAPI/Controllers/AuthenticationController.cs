using System.Threading.Tasks;
using DebtAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DebtAPI.Controllers
{
    [Route("v1/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthenticationController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<ActionResult> Login([FromBody] UserRequest userRequest)
        {
            var user = await _userManager.FindByNameAsync(userRequest.UserName);
            if (user == null)
            {
                return NotFound();
            }

            var result = _signInManager.PasswordSignInAsync(user, userRequest.Password, true, false);
            if (!result.IsCompletedSuccessfully)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> Register([FromBody] UserRequest userRequest)
        {
            var user = new ApplicationUser(userRequest.UserName);
            var result = await _userManager.CreateAsync(user, userRequest.Password);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}