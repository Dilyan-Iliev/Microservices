using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlocks.Messaging.MassTransit
{
    //Extension methods for setting up MassTransit with RabbitMQ
    public static class Extensions
    {
        public static IServiceCollection AddMessageBroker(this IServiceCollection services,
            IConfiguration configuration, Assembly? assembly = null)
        {
            //RabbitMQ MassTransit config
            services.AddMassTransit(config =>
            {
                //sets the naming convetion for the endpoints (e.g., order-service instead of OrderService)
                config.SetKebabCaseEndpointNameFormatter(); 

                if (assembly != null)
                {
                    config.AddConsumers(assembly);
                }

                config.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(new Uri(configuration["MessageBroker:Host"]!),
                        host =>
                        {
                            host.Username(configuration["MessageBroker:UserName"]!);
                            host.Password(configuration["MessageBroker:Password"]!);
                        });

                    //MassTransit automatically configures the endpoints for the consumers
                    configurator.ConfigureEndpoints(context);
                });
            });


            return services;
        }
    }
}
