using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }

        public string Permission { get; set; }
    }

    public class PermissionRequirementHandler 
        : AuthorizationHandler<PermissionRequirement>,
        IAuthorizationRequirement

    {
        private readonly AppDbContext _context;

        public PermissionRequirementHandler(AppDbContext context)
        {
            _context = context;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            if (context.User == null)
                return;

            AppUser user = await _context
                .Users
                .FirstOrDefaultAsync(u => u.UserName == context.User.Identity.Name);

            if (user == null)
                return;

            var userRoles = _context.UserRoles.AsQueryable().Where(_ => _.UserId == user.Id);

            var roleClaims = from ur in userRoles
                             join r in _context.Roles on ur.RoleId equals r.Id
                             join rc in _context.RoleClaims on r.Id equals rc.RoleId
                             select rc;

            if (await roleClaims.AnyAsync(c => c.ClaimValue == requirement.Permission))
                context.Succeed(requirement);
            else
                context.Fail();
        }
    }
}
