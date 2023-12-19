using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taogar.Finance.Database.Interfaces;
using Taogar.Finance.Database.Repositories;
using Taogar.Finance.Domain.Models;
using Taogar.Finance.Infrastructure;

namespace Taogar.Finance.Database.Extensions
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddFinancyReposytories(this IServiceCollection services)
        {
            services.AddScoped<IGenericRepository<Person>, GenericRepository<Person>>();
            return services;
        }
    }
}
