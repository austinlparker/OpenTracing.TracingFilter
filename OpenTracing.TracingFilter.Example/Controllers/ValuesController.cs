using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OpenTracing;
using OpenTracing.Util;

namespace OpenTracing.TracingFilter.Example.Controllers
{
    [WebApiTrace]
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            var returnValue = getCurrentTime();
            return new string[] { returnValue };
        }

        private string getCurrentTime()
        {
            using (var scope = GlobalTracer.Instance.BuildSpan("getCurrentTime").StartActive())
            {
                return DateTime.Now.ToShortDateString();
            }
                
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
