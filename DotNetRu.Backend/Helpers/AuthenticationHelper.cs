using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http.Controllers;

namespace XamarinEvolve.Backend.Helpers
{
    public class AuthenticationHelper
    {
        public static string GetAuthenticatedUserId()
        {
            var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
            var claims = identity.Claims;

            var claimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

            var userId = (from c in claims
                    where c.Type == claimType
                    select c.Value).FirstOrDefault();

            return string.IsNullOrEmpty(userId) ? Guid.Empty.ToString() : userId;
        }

        public static string GetAuthenticatedUserId(HttpRequestContext context)
        {
            var identity = (ClaimsIdentity)context.Principal.Identity;
            var claims = identity.Claims;

            var claimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

            var userId = (from c in claims
                    where c.Type == claimType
                    select c.Value).FirstOrDefault();

            return string.IsNullOrEmpty(userId) ? Guid.Empty.ToString() : userId;
        }
    }
}