using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Site.ViewModels
{
    public class JsonPResult : ActionResult
    {
        private JsonResult JSON { get; set; }
        private string Callback { get; set; }

        public JsonPResult(string callback, JsonResult json)
        {
            JSON = json;
            Callback = callback;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            if (!String.IsNullOrEmpty(JSON.ContentType))
            {
                response.ContentType = JSON.ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }

            if (JSON.ContentEncoding != null)
            {
                response.ContentEncoding = JSON.ContentEncoding;
            }

            if (JSON.Data != null)
            {
                // The JavaScriptSerializer type was marked as obsolete prior to .NET Framework 3.5 SP1
#pragma warning disable 0618
                HttpRequestBase request = context.HttpContext.Request;

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                if (!String.IsNullOrEmpty(Callback))
                    response.Write(Callback + "(" + serializer.Serialize(JSON.Data) + ")");
                else
                    response.Write(serializer.Serialize(JSON.Data));
#pragma warning restore 0618
            }
        }
    }
}
