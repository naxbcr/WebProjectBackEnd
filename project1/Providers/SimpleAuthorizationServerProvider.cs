using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;
using project1.BLL.Services;
using project1.DAL.UnitOfWork;
using project1.BLL.Interfaces;
using project1.BLL.Model;
using System.Security.Claims;

namespace project1.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();

        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            User user;
           

            using (IUserService _repo = new UserService(new EFUnitOfWork()))
            {
                user = await _repo.GetUserByEmailAndPassAsync(context.UserName,context.Password);

                if (user == null || user.emailconfirm == false)
                {
                    // daet bug, pri avtorizacii!!!!
                    // context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" }); 
                    context.SetError("invalid_grant", "The user name or password is incorrect or account is not confirmed.");
                    return;
                }
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);   
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));

            if (user.postionname.Equals("administrator")) { identity.AddClaim(new Claim(ClaimTypes.Role, "administrator")); }
            else if(user.postionname.Equals("translator")) { identity.AddClaim(new Claim(ClaimTypes.Role, "translator")); }
            else { identity.AddClaim(new Claim(ClaimTypes.Role, "user")); }

            context.Validated(identity);

        }
    }
}