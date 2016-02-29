using DeviceMate.Core.Extensions;
using DeviceMate.Web.Common;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Web.Http;
using System.Web.Http.Filters;

namespace DeviceMate.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SerializerSettings =
               new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

            config.Filters.Add(new GeneralExceptionFilterAttribute());

            config.MapHttpAttributeRoutes();

            //var handler = new AuthenticationHandler(config);

            config.Routes.MapHttpRoute(
                name: "AngularApi",
                routeTemplate: "api/v1/{controller}/{action}/{id}",
                defaults: new { area = "Api", id = RouteParameter.Optional }
                //constraints: null,
                //handler: handler
            );

            config.Routes.MapHttpRoute(
                name: "ApiWithoutAction",
                routeTemplate: "api/v1/{controller}/{id}",
                defaults: new { area = "Api", id = RouteParameter.Optional }
                //constraints: null,
                //handler: handler
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private sealed class GeneralExceptionFilterAttribute : ExceptionFilterAttribute
        {
            public override void OnException(HttpActionExecutedContext actionExecutedContext)
            {
                if (actionExecutedContext.Exception is ValidationException)
                {
                    var response = System.Web.HttpContext.Current.Response;
                    response.Clear();
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.StatusDescription = actionExecutedContext.Exception.Message;
                    response.End();
                }

                Exception ex = actionExecutedContext.Exception.RecursiveGetInnerException();

                var nlog = NLog.LogManager.GetLogger("ExeptionHandler");
                nlog.LogException(ex);

                base.OnException(actionExecutedContext);
            }
        }
    }
}
