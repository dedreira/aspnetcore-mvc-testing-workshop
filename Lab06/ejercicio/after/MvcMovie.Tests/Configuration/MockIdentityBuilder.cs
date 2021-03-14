using System.Collections.Generic;
using System.Security.Claims;

namespace MvcMovie.Tests.Configuration
{
    public class MockIdentityBuilder
    {
        private List<Claim> claims;

        public MockIdentityBuilder()
        {
            this.claims = new List<Claim>();
            this.claims.Add(new Claim(ClaimTypes.NameIdentifier, string.Empty));
            this.claims.Add(new Claim(ClaimTypes.Role, "Mock"));
        }
        public MockIdentityBuilder WithName(string name)
        {
            this.claims.Add(new Claim(ClaimTypes.NameIdentifier, name));
            return this;
        }
        public MockIdentityBuilder WithRole(string role)
        {
            this.claims.Add(new Claim(ClaimTypes.Role, role));
            return this;
        }
        public ClaimsIdentity Identity
        {
            get
            {
                return new ClaimsIdentity(claims);
            }
        }
    }
}
