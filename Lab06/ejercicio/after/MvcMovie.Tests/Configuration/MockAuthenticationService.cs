using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcMovie.Tests.Configuration
{
    public class MockAuthenticationService : IAuthenticationService
    {
        private readonly ClaimsIdentity identity;               
        public MockAuthenticationService(ClaimsIdentity _identity)
        {
            identity = _identity;            
        }

        public async Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string scheme)
        {
            var principal = new ClaimsPrincipal();            
            principal.AddIdentity(new ClaimsIdentity(identity.Claims, scheme));
            return AuthenticateResult.Success(new AuthenticationTicket(principal,
                new AuthenticationProperties(), scheme));
        }

        public Task ChallengeAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }
        public async Task ForbidAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            context.Response.Redirect($"http://{context.Request.Host}/Identity/Account/AccessDenied");
            return;
        }

        public Task SignInAsync(HttpContext context, string scheme, ClaimsPrincipal principal, AuthenticationProperties properties)
        {
                throw new NotImplementedException();
            }

        public Task SignOutAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
                throw new NotImplementedException();
            }
    }
}
