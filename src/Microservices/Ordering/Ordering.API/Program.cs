using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;

namespace Ordering.API
{
    public class Program
    {
        public static async void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services
                .AddApplicationServices()
                .AddInfrastructureServices(builder.Configuration)
                .AddApiServices();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                await app.InitialiseDatabaseAsync();
            }

            app.UseApiServices();

            app.Run();
        }
    }
}
