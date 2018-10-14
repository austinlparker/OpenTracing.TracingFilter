using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using OpenTracing;
using OpenTracing.Util;
using System.Diagnostics;

namespace OpenTracing.TracingFilter
{
    public class WebApiTrace : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var scope = GlobalTracer.Instance.BuildSpan(actionContext.ActionDescriptor.ActionName).StartActive();
            scope.Span.SetTag("controller", actionContext.ControllerContext.ControllerDescriptor.ControllerName);
            Debug.WriteLine($"Created span {scope.Span.Context.SpanId}");
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var scope = GlobalTracer.Instance.ScopeManager.Active;
            Debug.WriteLine($"Finishing span {scope.Span.Context.SpanId}");
            scope.Span.Finish();
        }
    }
}
