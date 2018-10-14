using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using OpenTracing;
using OpenTracing.Util;

namespace OpenTracing.TracingFilter.Example
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var tracer = new OpenTracing.Mock.MockTracer();
            OpenTracing.Util.GlobalTracer.Register(tracer);
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
