using Infrastructure.Modules.Bank.Interfaces;
using Infrastructure.Modules.Bank.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Modules.Bank
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseBank(this IServiceCollection services)
        {
            services.AddTransient<IBankAccountService, BankAccountService>();

            return services;
        }
    }
}
