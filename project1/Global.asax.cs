using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

namespace project1
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
         // look to Startup.cs (OWIN)
        }
    }
}
