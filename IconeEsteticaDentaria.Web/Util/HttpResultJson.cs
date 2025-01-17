using System.Net;
using System.Web.Mvc;

namespace IconeEsteticaDentaria.Web.Util
{
    public class HttpResultJson : JsonResult
    {
        public HttpResultJson(HttpStatusCode statusCode, string msg, object data = null)
        {
            Data = new
            {
                statusCode = statusCode,
                msg = msg,
                data = data
            };
            JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            MaxJsonLength = int.MaxValue;
        }
    }
}