using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

using Ninject;

using Site.Models;
using Site.Data.API.Repository;

namespace Site.Routing
{
    public class AdminOnlyRouteConstraint : IRouteConstraint
    {
        [Inject]
        public UserDataProvider UserProvider { get; set; }

        [Inject]
        public IUserRepository UserRepository { get; set; }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return false;

            var user = UserRepository.Get(UserProvider.GetUserId(httpContext));

            return false;// user.IsAdmin;
        }
    }

    public class MembersOnlyRouteConstraint : IRouteConstraint
    {
        [Inject]
        public UserDataProvider UserProvider { get; set; }

        [Inject]
        public IUserRepository UserRepository { get; set; }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
           if (!httpContext.User.Identity.IsAuthenticated)
                return false;

            var user = UserRepository.Get(UserProvider.GetUserId(httpContext));

            return user != null;
        }
    }
}
