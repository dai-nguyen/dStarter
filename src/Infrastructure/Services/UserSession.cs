using Infrastructure.Data;
using Infrastructure.Extensions;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Security.Claims;

namespace Infrastructure.Services
{
    public class UserSession : IUserSession
    {
        public bool IsAuthenticated { get; private set; }
        public string UserId { get; private set; }
        public string UserName { get; private set; } = "?";
        public string Email { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public IEnumerable<string> Roles { get; private set; }
        public IEnumerable<Claim> Claims { get; private set; }

        private readonly ILogger _logger;
        //private readonly IUserStore _userStore;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserSession(
            ILogger<UserSession> logger,
            //Interfaces.IUserStore userStore,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            //_userStore = userStore;
            _httpContextAccessor = httpContextAccessor;

            if (httpContextAccessor != null && httpContextAccessor.HttpContext != null)
                this.SetUserSession(httpContextAccessor.HttpContext);
        }

        public UserSession(AppUser user)
        {
            UserId = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            FirstName = user.FirstName;
            LastName = user.LastName;
        }

        public void SetUserSession(HttpContext httpContext)
        {
            if (httpContext == null
                || httpContext.User == null
                || httpContext.User.Identity == null)
            {
                this.ClearUserSession();
                return;
            }

            IsAuthenticated = httpContext.User.Identity.IsAuthenticated;

            if (httpContext.User.Identity.IsAuthenticated)
            {
                UserName = httpContext.User.Identity.Name;

                UserId = httpContext.User.Claims.GetClaim(Constants.Claims.UserId);
                Email = httpContext.User.Claims.GetClaim(Constants.Claims.Email);
                FirstName = httpContext.User.Claims.GetClaim(Constants.Claims.FirstName);
                LastName = httpContext.User.Claims.GetClaim(Constants.Claims.LastName);

                Claims = httpContext.User.Claims;
                Roles = httpContext.User.Claims.GetRoles();
            }
            else
                this.ClearUserSession();
        }

        public void ClearUserSession()
        {
            UserName = "?";
            UserId = "";
            Email = "";
            FirstName = "";
            LastName = "";

            Claims = new List<Claim>();
            Roles = new List<string>();
        }
    }
}
