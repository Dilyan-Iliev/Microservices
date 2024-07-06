using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using Carter;
using Catalog.API.Data;
using FluentValidation;
using HealthChecks.UI.Client;
using Marten;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Catalog.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var assembly = typeof(Program).Assembly;
            builder.Services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(assembly);
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
                config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });
            builder.Services.AddValidatorsFromAssembly(assembly);

            builder.Services.AddCarter();
            builder.Services.AddMarten(opts =>
            {
                opts.Connection(builder.Configuration.GetConnectionString("Database")!);
            }).UseLightweightSessions();

            if (builder.Environment.IsDevelopment())
            {
                builder.Services.InitializeMartenWith<CatalogInitialData>();
            }

            builder.Services.AddExceptionHandler<CustomExcetpionHandler>();

            //Health check for the microservice
            builder.Services.AddHealthChecks()
                .AddNpgSql(builder.Configuration.GetConnectionString("Database")!); //health check for the DB as well

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapCarter();
            app.UseExceptionHandler(options => { });

            //health check status will be available on this path
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.Run();
        }
    }
}
