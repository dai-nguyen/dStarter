using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Infrastructure.Extensions
{
    public static class ClaimExtensions
    {
        public static string GetClaim(this IEnumerable<Claim> claims, string type)
        {
            string result = string.Empty;

            if (claims == null || !claims.Any())
                return result;

            var found = claims.FirstOrDefault(_ => _.Type == type);

            if (found != null)
                result = found.Value;

            return result;
        }

        public static IEnumerable<string> GetRoles(this IEnumerable<Claim> claims)
        {
            var results = new List<string>();

            if (claims == null || !claims.Any())
                return results;

            var found = claims.FirstOrDefault(_ => _.Type == Constants.Claims.Roles);

            if (found != null)
            {
                results.AddRange(found.Value.Split("|"));
            }

            return results;
        }

        public static Claim ToClaim(this IEnumerable<string> roles)
        {
            if (roles == null || !roles.Any())
                return null;

            return new Claim(Constants.Claims.Roles, string.Join("|", roles));
        }
    }
}
