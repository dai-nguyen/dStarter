using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Modules.Bank
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseBank(this IServiceCollection services)
        {

            return services;
        }
    }
}
