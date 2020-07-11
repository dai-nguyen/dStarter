using Infrastructure.Data;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Infrastructure
{
    public sealed class BackgroundFactory
    {
        private static readonly Lazy<BackgroundFactory> lazy = 
            new Lazy<BackgroundFactory>(() => new BackgroundFactory());

        public static BackgroundFactory Instance
        {
            get { return lazy.Value; }
        }

        ServiceProvider ServiceProvider;
        IConfigurationRoot Configuration;

        private BackgroundFactory()
        {
            IServiceCollection services = new ServiceCollection();
            
            var builder = new ConfigurationBuilder()
                .SetBasePath(Helper.CurrentFolder)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();

            services.AddLogging();

            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.UseInfrastructure(Configuration);

            ServiceProvider = services.BuildServiceProvider();
        }

        public T GetRequiredService<T>()
        {
            try
            {
                return ServiceProvider.GetRequiredService<T>();
            }
            catch (Exception)
            { }
            return default(T);
        }

        public IEnumerable<T> GetServices<T>()
        {
            try
            {
                return ServiceProvider.GetServices<T>();
            }
            catch (Exception)
            { }
            return null;
        }

        public T GetConfigurationValue<T>(string key)
        {
            try
            {
                return Configuration.GetValue<T>(key);
            }
            catch (Exception)
            { }
            return default(T);
        }
    }
}
