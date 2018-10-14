using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OpenTracing;
using OpenTracing.Util;
using System.Diagnostics;

namespace OpenTracing.TracingFilter
{
    public class Traced : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var routeValues = filterContext.RouteData.Values;
            var scope = GlobalTracer.Instance.BuildSpan($"{routeValues["controller"]}").StartActive();
            scope.Span.SetTag("action", routeValues["action"].ToString());
            Debug.WriteLine($"Created span {scope.Span.Context.SpanId}");
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var scope = GlobalTracer.Instance.ScopeManager.Active;
            Debug.WriteLine($"Finishing span {scope.Span.Context.SpanId}");
            scope.Span.Finish();
        }
    }
}
