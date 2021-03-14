using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class AuthorizationBehaviour 
    {
        
        public AuthorizationBehaviour()
        {
        }

        public void ConfigureAuthorization(AuthenticationSchemeOptions options, IConfiguration configuration, ILogger logger)
        {
            options.Events = new OpenIdConnectEvents
            {
                OnRedirectToIdentityProvider = (ctx) => Redirect(configuration, ctx),
                OnTokenValidated = (ctx) => AfterTokenValidated(configuration, ctx),
                OnAuthenticationFailed = (ctx) => LogAuthenticationFailed(logger, ctx),
            };
        }

        private Task AfterTokenValidated(IConfiguration configuration,
            Microsoft.AspNetCore.Authentication.OpenIdConnect.TokenValidatedContext ctx)
        {
           //ctx.Properties.RedirectUri = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}{1}", configuration.GetValue<String>(ConfigurationSectionKeys.RedirectHost), "/");
           //
           ////To Do: Logic to get the User GUID from the ID Token.
           //string id = ctx.Principal.Claims.First(x => x.Type == ClaimTypeKeys.ObjectIdentifier).Value;
           //
           //var idToken = ctx.SecurityToken.RawData;
           //Task<List<string>> userGroups = membershipProvider.GetGroupsByUserAsync(id, idToken);
           //
           //List<string> allowedGroups = configuration.GetValue<String>(ConfigurationSectionKeys.AllowedGroups).Split(';').ToList<string>();
           //
           //var claims = new List<Claim>
           //{
           //    new Claim(ClaimTypes.Role, Roles.EchoWebApiUserRequester)
           //
           //};
           //
           //foreach (var allowedGroup in allowedGroups)
           //{
           //    //Determine claims based on group
           //    if (userGroups.Result.Contains(allowedGroup))
           //    {
           //        Claim group = new Claim(ClaimTypes.GroupSid, allowedGroup);
           //        claims.Add(group);
           //        var appIdentity = new ClaimsIdentity(claims);
           //        ctx.Principal.AddIdentity(appIdentity);
           //        break;
           //    }
           //}

            return Task.CompletedTask;
        }

        private static Task Redirect(IConfiguration configuration,
            Microsoft.AspNetCore.Authentication.OpenIdConnect.RedirectContext ctx)
        {
            //ctx.ProtocolMessage.RedirectUri = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}{1}", configuration.GetValue<String>(ConfigurationSectionKeys.RedirectHost), configuration.GetValue<String>("AzureAd:CallbackPath"));
            return Task.CompletedTask;
        }



        private static Task LogAuthenticationFailed(ILogger logger, Microsoft.AspNetCore.Authentication.OpenIdConnect.AuthenticationFailedContext ctx)
        {
            logger.LogError(ctx.Exception, ctx.Exception.Message, null);
            return Task.CompletedTask;
        }
    }
}
