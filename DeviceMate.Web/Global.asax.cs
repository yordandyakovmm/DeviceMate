using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using Microsoft.Owin;
using Owin;
using System.Net.Http.Formatting;

[assembly: OwinStartup(typeof(DeviceMate.Web.Startup))]

namespace DeviceMate.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MappingConfig.RegisterMappings();

            //GlobalConfiguration.Configuration.Formatters.Clear();
            //GlobalConfiguration.Configuration.Formatters.Add(new JsonMediaTypeFormatter());

            InitializeDependencyInjectionContainer();
        }

        private void InitializeDependencyInjectionContainer()
        {
            IUnityContainer container = new UnityContainer();
            List<Type> types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.Namespace != null && t.Namespace.StartsWith("DeviceMate.Web.Models")).ToList();
            foreach (Type type in types)
            {
                container.RegisterType(type);
            }

            types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.Namespace != null && t.Namespace.StartsWith("DeviceMate.Web.Models.Security")).ToList();
            foreach (Type type in types)
            {
                container.RegisterType(type);
            }

            types = Assembly.Load("DeviceMate.Objects").GetTypes()
                .Where(t => t.Namespace != null && t.Namespace.StartsWith("DeviceMate.Objects.Repositories")).ToList();
            foreach (Type type in types)
            {
                container.RegisterType(type);
            }

            Application["container"] = container;
        }
        
        /// <summary>
        /// On Application begin request.
        /// </summary>
        //protected void Application_BeginRequest(object sender, EventArgs e)
        //{
        //    HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
        //    HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "DELETE, POST, GET, OPTIONS, PUT");
        //    HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Disposition, Origin, X-Requested-With, Content-Type, Accept, TokenID");
        //}

        /// <summary>
        /// On application end request.
        /// </summary>
        //protected void Application_EndRequest(object sender, EventArgs e)
        //{
        //    if (Context.Items["UnauthorizedAjaxRequest"] is bool)
        //    {
        //        Context.Response.StatusCode = 401;
        //        Context.Response.End();
        //    }
        //}
    }
}