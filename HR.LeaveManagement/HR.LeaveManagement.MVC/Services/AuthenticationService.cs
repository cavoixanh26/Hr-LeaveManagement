using Hanssens.Net;
using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Models;
using HR.LeaveManagement.MVC.Services.Base;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HR.LeaveManagement.MVC.Services
{
    public class AuthenticationService : BaseHttpService, Contracts.IAuthenticationService
    {
        private readonly IHttpContextAccessor contextAccessor;
        private JwtSecurityTokenHandler tokenHandler;

        public AuthenticationService(IClient client,
            ILocalStorageService localStorage, 
            IHttpContextAccessor contextAccessor) 
            : base(client, localStorage)
        {
            this.contextAccessor = contextAccessor;
            this.tokenHandler = new JwtSecurityTokenHandler();
        }

        public async Task<bool> Authenticate(string email, string password)
        {
            try
            {
                var authenticationRequest = new AuthRequest
                {
                    Email= email,
                    Password= password
                };
                var authenticationResponse = await client.LoginAsync(authenticationRequest);

                if (authenticationResponse.Token != string.Empty) 
                {
                    var tokenContent = tokenHandler.ReadJwtToken(authenticationResponse.Token);
                    var claims = ParseClaims(tokenContent);
                    var user = new ClaimsPrincipal(
                        new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
                    var login = contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);
                    localStorage.SetStorageValue("token", authenticationResponse.Token);

                    return true;
                }

                return false;
            }
            catch 
            {
                return false;
            }
        }

        public async Task Logout()
        {
            localStorage.ClearStorage(new List<string> { "token" });
            await contextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<bool> Register(RegisterVM request)
        {
            var response = await client.RegisterAsync(new RegistrationRequest
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                EmailAddress= request.Email,
                Password = request.Password
            });

            return !string.IsNullOrEmpty(response.UserId);
        }

        private IList<Claim> ParseClaims(JwtSecurityToken tokenContent)
        {
            var claims = tokenContent.Claims.ToList();
            claims.Add(new Claim(ClaimTypes.Name, tokenContent.Subject));
            return claims;
        }
    }
}
