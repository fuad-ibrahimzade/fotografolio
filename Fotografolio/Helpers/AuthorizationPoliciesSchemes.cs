using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fotografolio.Helpers
{
    public static class AuthorizationPolicies
    {
        public const string JWT = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
        public const string Cookie = Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme;
        public const string JWTorCookieNormalAuthorize = Cookie + "," + JWT;
        public const string JWTandCookie = Cookie + ";" + JWT;
        public const string CookieAndJWT = JWT + ";" + Cookie;
        public const string MIX = "MIX";
    }
    public static class AuthenticationSchemes
    {
        public const string JWT = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
        public const string Cookie = Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme;
        public const string JWTorCookieNormalAuthorize = Cookie + "," + JWT;
        public const string JWTandCookie = Cookie + ";" + JWT;
        public const string CookieAndJWT = JWT + ";" + Cookie;
        public const string MIX = "MIX";
    }
}
