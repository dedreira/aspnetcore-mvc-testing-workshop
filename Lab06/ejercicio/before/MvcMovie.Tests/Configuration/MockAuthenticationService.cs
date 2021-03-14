using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcMovie.Tests.Configuration
{
    public class MockAuthenticationService : IAuthenticationService
    {
        public async Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string scheme)
        {
            var principal = new ClaimsPrincipal();
            principal.AddIdentity(new ClaimsIdentity(new Claim[]{
                new Claim(ClaimTypes.NameIdentifier,"TestIdentity")
                },scheme));
            return AuthenticateResult.Success(new AuthenticationTicket(principal,
                new AuthenticationProperties(), scheme));
        }

        public Task ChallengeAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }

        public Task ForbidAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            throw new NotImplementedException();
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
