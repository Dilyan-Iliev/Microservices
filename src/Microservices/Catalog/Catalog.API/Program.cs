using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using Carter;
using FluentValidation;
using Marten;

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

            builder.Services.AddExceptionHandler<CustomExcetpionHandler>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapCarter();
            app.UseExceptionHandler(options => { });

            app.Run();
        }
    }
}
