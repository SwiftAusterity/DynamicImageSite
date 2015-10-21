using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Site.Utilities
{
    public static class UrlHelperExtensions
    {
        public static string Stylesheet(this UrlHelper helper, string fileName)
        {
            return helper.Content(string.Format("~/Content/css/{0}", fileName));
        }
        public static string Image(this UrlHelper helper, string fileName)
        {
            return helper.Content(string.Format("~/Content/images/{0}", fileName));
        }
        public static string Script(this UrlHelper helper, string fileName)
        {
            return helper.Content(string.Format("~/Content/scripts/{0}", fileName));
        }
    }
}
