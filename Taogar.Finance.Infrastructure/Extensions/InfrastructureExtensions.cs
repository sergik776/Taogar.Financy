using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Taogar.Finance.Infrastructure.Extensions
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddFinancyDBContext(this IServiceCollection services)
        {
            services.AddDbContext<FinancyDBContext>(options =>
            {
                options.UseTriggers(triggerOptions =>
                {
                    //triggerOptions.AddTriggersFromAssemblyAndReferences(assembly);
                });
            }, ServiceLifetime.Scoped);
            return services;
        }
    }
}
