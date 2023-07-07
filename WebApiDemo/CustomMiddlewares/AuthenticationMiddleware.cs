using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebApiDemo.CustomMiddlewares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string authHeader = context.Request.Headers["Authorization"];
            if (authHeader == null)
            {
                await _next(context);
                return;
            }
            //basic username:password
            if (authHeader != null && authHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
            { 
                var token = authHeader.Substring(6).Trim();
                var credintialString = "";
                try
                { 
                    credintialString = Encoding.UTF8.GetString((Convert.FromBase64String(token))); 
                }
                catch 
                { 
                     context.Response.StatusCode = 500;
                }
                var credentials = credintialString.Split(':');
                if (credentials[0] == "sercan" && credentials[1] == "4555")
                {
                    var claims = new[]
                    {
                        new Claim("name", credentials[0]),
                        new Claim(ClaimTypes.Role,"Admin")
                    };
                    var identity = new ClaimsIdentity(claims,"Basic");
                    context.User = new ClaimsPrincipal(identity);
                }
            }
            else
            {
                context.Response.StatusCode = 401;
            }
            await _next(context);
        }
    }
}
