using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

using Ninject;

using Site.ViewModels;

namespace Site.Models
{
    public class UserDataProvider
    {
        [Inject]
        public IKernel Kernel { get; set; }

        private static HttpRequest _request
        {
            get
            {
                return HttpContext.Current.Request;
            }
        }

        public const String UserCookieName = "DocumentGenerator_Visitor";

        public Guid GetUserId(HttpContextBase context)
        {
            Guid userId = Guid.Empty;

            try
            {
                if (context != null || !context.User.Identity.IsAuthenticated)
                    userId = new Guid(context.User.Identity.Name);
            }
            catch
            {
                userId = Guid.Empty;
            }

            return userId;
        }

        public string GetScreenName(HttpContextBase context)
        {
            if (context == null)
                return String.Empty;

            var principal = context.User;
            var identity = principal.Identity as FormsIdentity;

            if (identity != null && identity.IsAuthenticated)
            {
                FormsAuthenticationTicket ticket = identity.Ticket;

                return ticket.UserData;
            }

            return String.Empty;
        }

        public void SetLoginCookies(HttpResponseBase response, Guid userId, string screenName)
        {
            var ticket = new FormsAuthenticationTicket(1, userId.ToString(), DateTime.Now, DateTime.MaxValue, true, screenName);
            var cookieString = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieString);
            SetAnAuthCookie(response, cookie, ticket.Expiration);

            // now set a user cookie for client-side script to use...
            var userCookie = new HttpCookie("userCookie", screenName);
            SetAnAuthCookie(response, userCookie, DateTime.MaxValue);
        }

        public virtual void ForgetLoginCookie(HttpResponseBase response)
        {
            FormsAuthentication.SignOut();  // kills the OAuth credentials

            // now kill the client-side helper cookie
            var deletionCookie = new HttpCookie("userCookie", "");
            SetAnAuthCookie(response, deletionCookie, DateTime.Now.AddDays(-1));
        }

        protected void SetAnAuthCookie(HttpResponseBase response, HttpCookie cookie, DateTime expiration)
        {
            cookie.Expires = expiration;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            cookie.Domain = FormsAuthentication.CookieDomain;
            response.SetCookie(cookie);
        }
    }
}
