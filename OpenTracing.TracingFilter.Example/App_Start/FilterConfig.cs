using System.Web;
using System.Web.Mvc;

namespace OpenTracing.TracingFilter.Example
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new Propagator());
        }
    }
}
