using System;
using System.Web.Mvc;

namespace Site.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AdminGate : CategoryAuthorize
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/missing");
        }
    }
}
