using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Middleware
{
    public class UserSessionMiddleware
    {
        private ILogger<UserSessionMiddleware> _logger;

        private readonly RequestDelegate _next;

        public UserSessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext httpContext, 
            ILogger<UserSessionMiddleware> logger, 
            IUserSession userSession)
        {
            _logger = logger;

            try
            {
                if (userSession == null)
                    return;

                if (httpContext == null
                    || httpContext.User == null
                    || httpContext.User.Identity == null
                    || !httpContext.User.Identity.IsAuthenticated)
                {
                    userSession.ClearUserSession();
                }

                userSession.SetUserSession(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            finally
            {
                await _next(httpContext);
            }
        }

    }
}
