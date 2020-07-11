using Infrastructure.Data.Configurations;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>, IAppDbContext
    {
        private readonly ILoggerFactory _loggerFactory;
        //private readonly IUserSession _userSession;

        public DbSet<UserAttribute> UserAttributes { get; set; }
        public DbSet<RoleAttribute> RoleAttributes { get; set; }


        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            ILoggerFactory loggerFactory
            //IUserSession userSession
            )
            : base(options)
        {
            _loggerFactory = loggerFactory;
            //_userSession = userSession;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserAttributeConfiguration());
            builder.ApplyConfiguration(new RoleAttributeConfiguration());


            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            //ChangeTracker.SetShadowProperties(_userSession);
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            //ChangeTracker.SetShadowProperties(_userSession);
            return await base.SaveChangesAsync(true, cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseLoggerFactory(_loggerFactory);
    }

    
}
