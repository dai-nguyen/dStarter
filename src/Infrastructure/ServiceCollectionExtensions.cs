using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Middleware;
using Infrastructure.Services;
using Infrastructure.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(builder => GetDbContextOption(builder, configuration));
            services.AddScoped<IAppDbContext>(_ => _.GetRequiredService<AppDbContext>() as IAppDbContext);
            
            services.AddAutoMapper(
               typeof(ServiceCollectionExtensions));

            services.AddTransient(typeof(IStore<,>), typeof(GenericStore<,>));
            services.AddTransient<Interfaces.IUserStore, UserStore>();
            services.AddTransient<Interfaces.IRoleStore, RoleStore>();
            services.AddScoped<IUserSession, UserSession>();

            // background services
            services.AddHostedService<TimedHostedService>();
            services.AddHostedService<QueuedHostedService>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

            return services;
        }

        public static IApplicationBuilder UseUserSession(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserSessionMiddleware>();
        }

        public static void GetDbContextOption(
            DbContextOptionsBuilder builder,
            IConfiguration configuration)
        {
            var migrationsAssembly = typeof(AppDbContext).Assembly.GetName();

            builder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(migrationsAssembly.Name));
                //.UseSnakeCaseNamingConvention();

            //var useSqlServer = configuration.GetValue<bool>("Infrastructure:UseSqlServer");
            //var dbConnString = useSqlServer
            //    ? configuration.GetConnectionString("DefaultConnection")
            //    : $"Filename={configuration.GetConnectionString("SqliteConnection")}";

            //if (useSqlServer)
            //{
            //    builder.UseSqlServer(dbConnString, sql => sql.MigrationsAssembly(migrationsAssembly.Name));
            //}
            //else if (configuration.GetValue<bool>("Infrastructure:UsePostgresServer"))
            //{
            //    builder.UseNpgsql(configuration.GetConnectionString("PostgresConnection"), sql => sql.MigrationsAssembly(migrationsAssembly.Name));
            //}
            //else
            //{
            //    builder.UseSqlite(dbConnString, sql => sql.MigrationsAssembly(migrationsAssembly.Name));
            //}

        }
    }
}
