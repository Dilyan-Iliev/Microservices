using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Data.Interceptors;

namespace Ordering.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
            IConfiguration config)
        {
            var connectionString = config.GetConnectionString("Database");

            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

            services.AddDbContext<ApplicationDbContext>((serviceProvider, options)=>
            {
                options.AddInterceptors(serviceProvider.GetService<ISaveChangesInterceptor>()!);
                options.UseSqlServer(connectionString);
            });

            return services;
        }
    }
}
