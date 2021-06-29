using Infrastructure.Data.Configurations;
using Infrastructure.Entities;
using Infrastructure.Extensions;
using Infrastructure.Interfaces;
using Infrastructure.Modules.CRM.Configurations;
using Infrastructure.Modules.CRM.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>, IAppDbContext
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IUserSession _userSession;

        public DbSet<LogMsg> Logs { get; set; }
        public DbSet<AppConfig> AppConfigs { get; set; }
        public DbSet<Wiki> Wikis { get; set; }

        // CRM module
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<LaborHour> LaborHours { get; set; }


        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            ILoggerFactory loggerFactory,
            IUserSession userSession
            )
            : base(options)
        {
            _loggerFactory = loggerFactory;
            _userSession = userSession;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AppUser>(b =>
            {
                b.Property(_ => _.CustomAttributes)
                    .HasColumnType("jsonb");
                b.HasIndex(_ => new
                {
                    _.UserName,
                    _.FirstName,
                    _.LastName,
                    _.Email
                }).IsTsVectorExpressionIndex("english");
            });

            builder.Entity<AppRole>(b =>
            {
                b.HasIndex(_ => _.Name);
                b.Property(_ => _.CustomAttributes)
                    .HasColumnType("jsonb");
                b.HasIndex(_ => _.Name).IsTsVectorExpressionIndex("english");
            });

            builder.ApplyConfiguration(new LogMsgConfiguration());
            builder.ApplyConfiguration(new AppConfigConfiguration());
            builder.ApplyConfiguration(new WikiConfiguration());

            // CRM module
            builder.ApplyConfiguration(new CustomerConfiguration());
            builder.ApplyConfiguration(new ContactConfiguration());
            builder.ApplyConfiguration(new TicketConfiguration());
            builder.ApplyConfiguration(new LaborHourConfiguration());

            //builder.HasPostgresEnum<Modules.CRM.Enums.TicketStatus>();

            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            ChangeTracker.SetShadowProperties(_userSession);
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            ChangeTracker.SetShadowProperties(_userSession);
            return await base.SaveChangesAsync(true, cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseLoggerFactory(_loggerFactory);
    }

    public class AppDbContextDesignFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
               .AddJsonFile("appsettings.json")
               //.AddEnvironmentVariables()
               .Build();

            var migrationsAssembly = typeof(AppDbContext).Assembly.GetName();

            string connStr = configuration.GetSection("DefaultConnection").Value;

            var builder = new DbContextOptionsBuilder<AppDbContext>();

            builder.UseNpgsql(connStr,
                sql => sql.MigrationsAssembly(migrationsAssembly.Name).UseNodaTime());
          
            return new AppDbContext(builder.Options, null, null);
        }
    }
}
