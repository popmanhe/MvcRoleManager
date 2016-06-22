using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MvcRoleManager.Web.Controllers
{
   
    [RoutePrefix("api/values")]
    public class ValuesController : ApiController
    {
        // GET api/values
        [Route("getvalue")]
        //       [Route("getvalue1")]
        [HttpGet][HttpPost]
        public IEnumerable<string> GetV(int id, string name)
        {
            return new string[] { "value1", "value2", id.ToString() };
        }

        // GET api/values/5
      
        //public string Get(int id)
        //{
        //    return id.ToString();
        //}

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
