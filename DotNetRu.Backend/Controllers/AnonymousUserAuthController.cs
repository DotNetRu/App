using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Login;
using Newtonsoft.Json.Linq;
using XamarinEvolve.Backend.Identity;

namespace XamarinEvolve.Backend.Controllers
{
    public class AnonymousUserAuthController : ApiController
    {
        private const string AuthSigningKeyVariableName = "WEBSITE_AUTH_SIGNING_KEY";
        private const string HostNameVariableName = "WEBSITE_HOSTNAME";

        public IHttpActionResult Post([FromBody] JObject assertion)
        {
            var userId = Guid.NewGuid().ToString();

            var cred = assertion.ToObject<AnonymousUserCredentials>();
            if (!string.IsNullOrEmpty(cred.AnonymousUserId))
            {
                Guid impersonate;
                if (Guid.TryParse(cred.AnonymousUserId, out impersonate))
                {
                    userId = impersonate.ToString();
                }
            }

            IEnumerable<Claim> claims = GetAccountClaims(userId);
            string websiteUri = $"https://{WebsiteHostName}/";

            JwtSecurityToken token = AppServiceLoginHandler.CreateToken(claims, TokenSigningKey, websiteUri, websiteUri, TimeSpan.FromDays(10));

            return Ok(new LoginResult { RawToken = token.RawData, User = new User { UserId = userId } });
        }

        private IEnumerable<Claim> GetAccountClaims(string user) => new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user),
                new Claim(JwtRegisteredClaimNames.GivenName, "anonymous"),
                new Claim(JwtRegisteredClaimNames.FamilyName, "anonymous")
            };

        private string TokenSigningKey => Environment.GetEnvironmentVariable(AuthSigningKeyVariableName) ?? "test_key";

        public string WebsiteHostName => Environment.GetEnvironmentVariable(HostNameVariableName) ?? "localhost";
    }
}
