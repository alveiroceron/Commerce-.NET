using Clients.WebClient.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Clients.WebClient.Controllers
{
    public class AccountController : Controller
    {
        private readonly string _authenticationUrl;

        public AccountController(IConfiguration configuration)
        {
            _authenticationUrl = configuration.GetValue<string>("AuthenticationUrl");
        }

        [HttpGet]
        public IActionResult Login() 
        {
            return Redirect(_authenticationUrl + $"?ReturnBaseUrl={this.Request.Scheme}://{this.Request.Host}/");
        }

        [HttpGet]
        public async Task<IActionResult> Connect(string access_token)
        {
            var token = access_token.Split('.');

            var jwtPayload = token[1].Replace('-', '+').Replace('_', '/');
            // Pad with '=' if necessary to make it a valid base64 string
            while (jwtPayload.Length % 4 != 0)
            {
                jwtPayload += '=';
            }
            byte[] base64Bytes = Convert.FromBase64String(jwtPayload);

            var user = JsonSerializer.Deserialize<AccessTokenUserInformation>(base64Bytes);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.nameid),
                new Claim(ClaimTypes.Name, user.unique_name),
                new Claim(ClaimTypes.Email, user.email),
                new Claim("access_token", access_token)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IssuedUtc = DateTime.UtcNow.AddHours(10)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Redirect("~/");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("~/");
        }
    }
}
