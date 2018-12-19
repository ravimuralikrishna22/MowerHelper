using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;

namespace MowerHelper.App_Start
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class,
                AllowMultiple = false, Inherited = true)]
    public sealed class ValidateJsonAntiForgeryTokenAttribute
                                : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            
            var httpContext = filterContext.HttpContext;
            //if (ConfigurationManager.AppSettings["TwoFactorAuthentication"].ToUpper() == "YES")
            //{
            //    if (httpContext.Request.Cookies["_ValidUserVerifiedAuthyGrabCookie"] == null)
            //    {
            //        filterContext.Result = new RedirectToRouteResult(
            //                           new RouteValueDictionary {
            //                    { "Controller", "Account" },
            //                    { "Action", "Login" }});
            //        return;
            //    }
            //}
            var cookie = httpContext.Request.Cookies[AntiForgeryConfig.CookieName];
            if (cookie != null)
            {
                AntiForgery.Validate(cookie != null ? cookie.Value : null,
                                     httpContext.Request.Headers["__RequestVerificationToken"]);
            }
        }
    }
}