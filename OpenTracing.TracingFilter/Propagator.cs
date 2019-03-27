using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using OpenTracing.Propagation;
using OpenTracing.Util;

namespace OpenTracing.TracingFilter
{
    public class Propagator : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ISpanContext context = null;
            IScope scope;
            
            var routeValues = filterContext.RouteData.Values;
            var headers = filterContext.HttpContext.Request.Headers;
            var headerDictionary = headers.AllKeys.ToDictionary(k => k, k => headers[k]);
            
            try
            {
                context = GlobalTracer.Instance.Extract(BuiltinFormats.TextMap,
                    new TextMapExtractAdapter(headerDictionary));
            }
            catch (ArgumentException)
            {
                // this space left blank
            }
            
            scope = context != null ? GlobalTracer.Instance.BuildSpan($"{routeValues["controller"]}").AsChildOf(context).StartActive() 
                : GlobalTracer.Instance.BuildSpan($"{routeValues["controller"]}").StartActive();
            
            scope.Span.SetTag("action", routeValues["action"].ToString());
            Debug.WriteLine($"Created span {scope.Span.Context.SpanId}");
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var scope = GlobalTracer.Instance.ScopeManager.Active;
            Debug.WriteLine($"Finishing span {scope.Span.Context.SpanId}");
            scope.Span.Finish();
            
            var headersDictionary = new Dictionary<string, string>();
            GlobalTracer.Instance.Inject(scope.Span.Context, BuiltinFormats.TextMap, new TextMapInjectAdapter(headersDictionary));

            var contextNvc = headersDictionary.Aggregate(new NameValueCollection(), (v, item) =>
            {
                v.Add(item.Key, item.Value);
                return v;
            });
            
            filterContext.HttpContext.Response.Headers.Add(contextNvc);
        }
    }
}