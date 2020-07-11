using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IAppDbContext
    {
        public DbSet<UserAttribute> UserAttributes { get; set; }
        public DbSet<RoleAttribute> RoleAttributes { get; set; }

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
