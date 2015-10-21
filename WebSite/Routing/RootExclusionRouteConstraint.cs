using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Site.Routing
{
    public class RootExclusionRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var sectionValue = values["section"] == null ? string.Empty : values["section"].ToString();
            var sectionValues = @"missing|error|unauthorized|people|work|clients|expertise|labs|contact|login";

            return !string.IsNullOrEmpty(sectionValue) && !sectionValues.Contains(sectionValue.ToLowerInvariant());
        }
    }
}
