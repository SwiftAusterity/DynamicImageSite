using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Site.Routing
{
    public class MobileBaseRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var sectionValue = values["section"] == null ? string.Empty : values["section"].ToString();
            var pageValue = values["pathInfo"] == null ? string.Empty : values["pathInfo"].ToString();

            if (sectionValue.Equals("m") && string.IsNullOrEmpty(pageValue))
            {
                values["section"] = string.Empty;
                return true;
            }

            return false;
        }
    }
}
