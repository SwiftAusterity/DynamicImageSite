using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Site.Routing
{
    public class AgencyExclusionRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var sectionValue = values["pathInfo"] == null ? string.Empty : values["pathInfo"].ToString();

            return !string.IsNullOrEmpty(sectionValue)
                && (sectionValue.StartsWith("culture", StringComparison.InvariantCultureIgnoreCase)
                || sectionValue.StartsWith("services", StringComparison.InvariantCultureIgnoreCase)
                || sectionValue.StartsWith("careers/junior-copywriter", StringComparison.InvariantCultureIgnoreCase)
                || sectionValue.StartsWith("careers/senior-account-executive/", StringComparison.InvariantCultureIgnoreCase)
                || sectionValue.StartsWith("careers/senior-user-experience-designer-architect/", StringComparison.InvariantCultureIgnoreCase)
                || sectionValue.StartsWith("leadership", StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
