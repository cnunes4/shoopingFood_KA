using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShoopingFood.Interfaces;
using ShoopingFood.Models;
using System.Security.Claims;

namespace ShoopingFood.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(User model)
        {
            if (ModelState.IsValid)
            { 
                var response= await _loginService.Login(model);

                if (response!= null && response.Success)
                {
                    // Store the token in session
                    HttpContext.Session.SetString("JwtToken", response.Token); // Store the JWT token in session
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Username),
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "KACookieAuthScheme");

                    await HttpContext.SignInAsync("KACookieAuthScheme", new ClaimsPrincipal(claimsIdentity));
                    HttpContext.Session.SetString("Username", model.Username);
                    return RedirectToAction("Index", "Food"); 

                }
                else
                {
                    ViewData["ErrorMessage"] = "Invalid credentials. Please try again.";
                }
            }
            return View("Index");
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("JwtToken");
            HttpContext.Session.Remove("Username");
            await HttpContext.SignOutAsync("KACookieAuthScheme");
            return RedirectToAction("Index", "Login");
        }


    }
}
