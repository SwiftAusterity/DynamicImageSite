using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

using Ninject;

using Site.Data.API;
using Site.Data.API.Repository;

namespace Site.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CategoryAuthorize : AuthorizeAttribute
    {
        [Inject]
        IUserRepository UserRepository { get; set; }

        public string CategoryParameter { get; set; }

        protected PermissionsRole[] _splitRoles;
        public PermissionsRole[] SplitRoles
        {
            get
            {
                if (_splitRoles == null)
                {
                    if (String.IsNullOrEmpty(Roles))
                    {
                        _splitRoles = new PermissionsRole[0];
                    }
                    else
                    {
                        _splitRoles = Roles.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(roleString => AsAdminRole(roleString, PermissionsRole.None))
                            .Where(role => role != PermissionsRole.None)
                            .ToArray();
                    }
                }

                return _splitRoles;
            }
        }

        protected override HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
        {
            return OnCacheAuthorization(httpContext, null);
        }

        // this (non-override) version takes the AuthorizationContext (original filterContext) during callback
        protected HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext, AuthorizationContext filterContext)
        {
            return AuthorizeCore(httpContext, filterContext)
                ? HttpValidationStatus.Valid
                : HttpValidationStatus.IgnoreThisRequest;
        }

        // this has to replace the existing code, because we need to pass the entire 
        // filterContext to the Allowed method and also pass it to the cache validation callback
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");

            if (this.AuthorizeCore(filterContext.HttpContext, filterContext))
            {
                HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
                cache.SetProxyMaxAge(new TimeSpan(0L));
                cache.AddValidationCallback(this.CacheValidateHandler, null);
            }
            else
            {
                HandleUnauthorizedRequest(filterContext);
            }
        }

        protected void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = this.OnCacheAuthorization(new HttpContextWrapper(context), data as AuthorizationContext);
        }

        protected PermissionsRole AsAdminRole(string value, PermissionsRole defaultRole)
        {
            PermissionsRole role;

            if (!value.Trim().TryParse(out role))
                role = defaultRole;

            return role;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return AuthorizeCore(httpContext, null);
        }

        // this is a new virtual version of AuthorizeCore, because we need filterContext to get to
        // RouteData but depending on the call-path, we may not have it...
        protected virtual bool AuthorizeCore(HttpContextBase httpContext, AuthorizationContext filterContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            try
            {
                IPrincipal user = httpContext.User;

                if (!user.Identity.IsAuthenticated)
                    return false;

                var userId = new Guid(user.Identity.Name);

                var allowedRoles = SplitRoles;
                if (allowedRoles.Length == 0)
                    return true;

                // hmm, should probably do the category roll up in the people repository eventually
                var profile = UserRepository.Get(userId);

                return allowedRoles.Any(role => profile.Permissions.Contains(role.ToString()));
            }
            catch
            {
            }

            return false;
        }
    }
}
