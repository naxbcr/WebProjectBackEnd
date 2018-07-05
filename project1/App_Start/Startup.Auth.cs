using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using project1.Providers;
using project1.BLL.Model;
using project1.BLL.Services;

namespace project1
{
    public partial class Startup

    //http://metanit.com/sharp/aspnet_webapi/5.1.php полезная инфа
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }
        

        public static string PublicClientId { get; private set; }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {

           // app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            OAuthBearerAuthenticationOptions OAuthBearerOptions = new OAuthBearerAuthenticationOptions();
            
            // Configure the application for OAuth based flow
            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                // устанавливает URL, по которому клиент будет получать токен
                TokenEndpointPath = new PathString("/Token"),
                // указывает на вышеопределенный провайдер авторизации
                Provider = new SimpleAuthorizationServerProvider(),  //probuem SVOI !!!
                //Параметр AuthorizeEndpointPath указывает на маршрут,
                //по которому будет перенаправляться пользователь для авторизации.Должен начинаться со слеша.
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                // In production mode set AllowInsecureHttp = false
                AllowInsecureHttp = true
            };

            // Enable the application to use bearer tokens to authenticate users

            //app.UseOAuthBearerTokens(OAuthOptions); 

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthOptions);
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);

            
        }
    }
}
