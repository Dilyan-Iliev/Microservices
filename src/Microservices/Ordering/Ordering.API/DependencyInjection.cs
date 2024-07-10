﻿using BuildingBlocks.Exceptions.Handler;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Ordering.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddExceptionHandler<CustomExcetpionHandler>();
            services.AddHealthChecks()
                .AddSqlServer(config.GetConnectionString("Database")!);

            return services;
        }

        public static WebApplication UseApiServices(this WebApplication app)
        {
            app.UseExceptionHandler(options => { });
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            return app;
        }
    }
}
