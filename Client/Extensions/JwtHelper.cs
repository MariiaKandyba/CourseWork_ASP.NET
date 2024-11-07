using Client.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Client.Extensions
{
    public class JwtHelper
    {
        public static ClaimsPrincipal DecodeJwtToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null; 
            }

            var handler = new JwtSecurityTokenHandler();

          
            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJwtToken(token);
                return new ClaimsPrincipal(new ClaimsIdentity(jwtToken.Claims, "jwt"));
            }

            return null; 
        }

        public static User GetUser(string token)
        {
            var claimsPrincipal = DecodeJwtToken(token);
            if (claimsPrincipal != null)
            {
                User user = new User();
                user.Id = claimsPrincipal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
                user.Email = claimsPrincipal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
                user.FirstName = claimsPrincipal.FindFirst("firstName")?.Value;
                user.LastName = claimsPrincipal.FindFirst("lastName")?.Value;
                user.Role= claimsPrincipal.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;
                user.IsAuthenticated = true;
                return user;
                
            }
            else
            {
                return null;
            }
        }
    }
}
