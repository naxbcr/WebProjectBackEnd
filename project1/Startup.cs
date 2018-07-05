using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;

using project1.BLL.Services;
using project1.BLL.Interfaces;

using project1.DAL.Interfaces;
using project1.DAL.UnitOfWork;
using System.Reflection;
using Microsoft.Owin.Security.OAuth;

[assembly: OwinStartup(typeof(project1.Startup))]

namespace project1
{
    public partial class Startup
    {
        // Переносим всю сюда из глобал.асакс, так написано в документации, если хотим использовать авторизацию о-аутх с помощью ОВИН.
        //http://bitoftech.net/2014/10/27/json-web-token-asp-net-web-api-2-jwt-owin-authorization-server/
        public void Configuration(IAppBuilder app)
        {

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll); // esli ne na pervom meste to poluchaem bugi svjazanniye s CORS.
            HttpConfiguration config = new HttpConfiguration();
           // config.Formatters.Add(new UploadMultipartMediaTypeFormatter());
            
            ConfigureAuth(app);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            WebApiConfig.Register(config);


            // далее из тутора http://docs.autofac.org/en/latest/integration/owin.html
            var builder = new ContainerBuilder();

            // STANDARD WEB API SETUP:
         
            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // !!! ВОЗМОЖНОЕ РЕШЕНИЕ отвязки от СЛОЯ ДОСТУПА К ДАННЫМ - http://stackoverflow.com/questions/26838880/autofac-register-assembly-types
            // !!! http://docs.autofac.org/en/latest/register/scanning.html
            // http://stackoverflow.com/questions/31433876/autofac-none-of-the-constructors-found-with-autofac-core-activators-reflection
            // http://stackoverflow.com/questions/26838880/autofac-register-assembly-types
            // http://stackoverflow.com/questions/31477287/autofac-exception-cannot-resolve-parameter-of-constructor-void-ctor

            builder.RegisterType<EFUnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<CommentService>().As<ICommentService>();
            builder.RegisterType<PositionService>().As<IPositionService>();
            builder.RegisterType<TranslateService>().As<ITranslateService>();
            builder.RegisterType<TransTypeService>().As<ITypeService>();
            builder.RegisterType<StatusService>().As<IStatusService>();
            builder.RegisterType<LanguageService>().As<ILanguageService>();
            builder.RegisterType<StatisticsService>().As<IStatistics>();
            // Run other optional steps, like registering filters,
            // per-controller-type services, etc., then set the dependency resolver
            // to be Autofac.
            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);


            // OWIN WEB API SETUP:

            // Register the Autofac middleware FIRST, then the Autofac Web API middleware,
            // and finally the standard Web API middleware.

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
            app.UseWebApi(config);

           



        }
    }
}
