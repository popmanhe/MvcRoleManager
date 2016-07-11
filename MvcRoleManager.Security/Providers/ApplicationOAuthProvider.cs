using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using MvcRoleManager.Security.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcRoleManager.Security.Providers
{
    /// <summary>
    /// Provide user authetication
    /// </summary>
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;
        private static UserManager<IdentityUser> _userManager = null;
        private static UserManager<IdentityUser> UserManager
        {
          get
            {
                if (_userManager == null)
                    _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(RoleManagerDbContext.Create()));

                return _userManager;
            }
        }
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="publicClientId"></param>
    public ApplicationOAuthProvider(string publicClientId)
    {
        if (publicClientId == null)
        {
            throw new ArgumentNullException("publicClientId");
        }

        _publicClientId = publicClientId;
    }

    public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
    {
        IdentityUser user = await UserManager.FindByEmailAsync(context.UserName);

        if (user == null)
        {
            context.SetError("invalid_grant", "The user name or password is incorrect.");
            return;
        }

        ClaimsIdentity oAuthIdentity = await GenerateUserIdentityAsync(user, OAuthDefaults.AuthenticationType);
        ClaimsIdentity cookiesIdentity = await GenerateUserIdentityAsync(user, CookieAuthenticationDefaults.AuthenticationType);

        AuthenticationProperties properties = CreateProperties(user.UserName);
        AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
        context.Validated(ticket);
        context.Request.Context.Authentication.SignIn(cookiesIdentity);
    }

    public override Task TokenEndpoint(OAuthTokenEndpointContext context)
    {
        foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
        {
            context.AdditionalResponseParameters.Add(property.Key, property.Value);
        }

        return Task.FromResult<object>(null);
    }

    public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
    {
        // Resource owner password credentials does not provide a client ID.
        if (context.ClientId == null)
        {
            context.Validated();
        }

        return Task.FromResult<object>(null);
    }

    public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
    {
        if (context.ClientId == _publicClientId)
        {
            Uri expectedRootUri = new Uri(context.Request.Uri, "/");

            if (expectedRootUri.AbsoluteUri == context.RedirectUri)
            {
                context.Validated();
            }
        }

        return Task.FromResult<object>(null);
    }

    public static AuthenticationProperties CreateProperties(string userName)
    {
        IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
        return new AuthenticationProperties(data);
    }

    private async Task<ClaimsIdentity> GenerateUserIdentityAsync(IdentityUser user, string authenticationType)
    {
        // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
        var userIdentity = await UserManager.CreateIdentityAsync(user, authenticationType);
            // Add custom user claims here
            //add user roles to claims
            foreach(var role in user.Roles)
              userIdentity.AddClaim(new Claim(userIdentity.RoleClaimType, role.RoleId));
        return userIdentity;
    }
}
}