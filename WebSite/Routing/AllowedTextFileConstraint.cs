using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Site.Routing
{
    public class AllowedTextFilesConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return !httpContext.Request.RawUrl.Equals("/robots.txt", StringComparison.OrdinalIgnoreCase);
        }
    }
}
