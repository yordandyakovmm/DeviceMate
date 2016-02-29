using Microsoft.Owin;
using Owin;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

[assembly: OwinStartup(typeof(DeviceMate.Web.Startup))]

namespace DeviceMate.Web
{
    /// <summary>
    /// Startup Owin Configuration Class.
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Initial Configuration Method.
        /// </summary>
        public static void Configuration(IAppBuilder app)
        {
            WindsorConfig.Setup();
            ConfigureAuth(app);
        }
    }
}