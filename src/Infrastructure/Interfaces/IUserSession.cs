using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;

namespace Infrastructure.Interfaces
{
    public interface IUserSession
    {
        string UserId { get; }
        string UserName { get; }
        IEnumerable<string> Roles { get; }
        IEnumerable<Claim> Claims { get; }

        void SetUserSession(HttpContext httpContext);
        void ClearUserSession();
    }
}
