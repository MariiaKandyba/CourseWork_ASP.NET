using Client.Extensions;
using Client.Models;
using Client.Services.Auth;
using DTOs.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Client.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }
        // GET: AuthController
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (ModelState.IsValid)
            {
                var authResult = await _authService.LoginAsync(model);

                if (!authResult.IsSuccess)
                {
                    ModelState.AddModelError(string.Empty, authResult.ErrorMessage);
                    return View(model);
                }

                HttpContext.Response.Cookies.Append("jwtToken", authResult.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                });
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(authResult.Token);

                var claims = jwtToken.Claims.ToList();

                var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync("Cookies", claimsPrincipal);

                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Response.Cookies.Delete("jwtToken");
            await HttpContext.SignOutAsync("Cookies");

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                var registerResult = await _authService.RegisterAsync(model);

                if (!registerResult.IsSuccess)
                {
                    ModelState.AddModelError(string.Empty, registerResult.ErrorMessage);
                    return View(model);
                }

                var loginResult = await _authService.LoginAsync(new LoginDto { Email = model.Email, Password = model.Password });

                if (loginResult.IsSuccess)
                {
                    HttpContext.Response.Cookies.Append("jwtToken", loginResult.Token, new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = DateTimeOffset.UtcNow.AddHours(1)
                    });
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(loginResult.Token);

                    var claims = jwtToken.Claims.ToList();

                    var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync("Cookies", claimsPrincipal);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, loginResult.ErrorMessage);
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult UpdateProfile()
        {
            var token = HttpContext.Request.Cookies["jwtToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login");
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var firstName = jwtToken.Claims.FirstOrDefault(c => c.Type == "firstName")?.Value;
            var lastName = jwtToken.Claims.FirstOrDefault(c => c.Type == "lastName")?.Value;
            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;

            var model = new UpdateDto
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateDto model)
        {
            if (ModelState.IsValid)
            {
                var token = HttpContext.Request.Cookies["jwtToken"]; 

                if (string.IsNullOrEmpty(token))
                {
                    ModelState.AddModelError(string.Empty, "Необхідно увійти для оновлення профілю.");
                    return View(model);
                }

                var updateResult = await _authService.UpdateProfileAsync(model, token);
                HttpContext.Response.Cookies.Append("jwtToken", updateResult.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                });

                if (!updateResult.IsSuccess)
                {
                   
                    ModelState.AddModelError(string.Empty, updateResult.ErrorMessage);
                    return View(model);
                }

                return RedirectToAction("UpdateProfile", "Auth");
            }
            return View(model);
        }

    }
}
