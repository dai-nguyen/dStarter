using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace UnitTest
{
    public sealed class TestFactory
    {
        private static readonly Lazy<TestFactory> lazy = 
            new Lazy<TestFactory>(() => new TestFactory());

        public static TestFactory Instance
        {
            get { return lazy.Value; }
        }

        ServiceProvider ServiceProvider;
        IConfigurationRoot Configuration;

        private TestFactory()
        {
            IServiceCollection services = new ServiceCollection();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Helper.CurrentFolder) //Directory.GetCurrentDirectory())
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
            catch (Exception ex)
            { }
            return default(T);
        }

        public IEnumerable<T> GetServices<T>()
        {
            try
            {
                return ServiceProvider.GetServices<T>();
            }
            catch (Exception ex)
            { }
            return null;
        }

        public T GetConfigurationValue<T>(string key)
        {
            try
            {
                return Configuration.GetValue<T>(key);
            }
            catch (Exception ex)
            { }
            return default(T);
        }

        

        
    }
}
