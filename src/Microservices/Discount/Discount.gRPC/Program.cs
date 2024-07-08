using Discount.gRPC.Data;
using Discount.gRPC.Services;
using Microsoft.EntityFrameworkCore;

namespace Discount.gRPC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddGrpc();

            builder.Services.AddDbContext<DiscountDbContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("Database"));
            });

            var app = builder.Build();

            app.UseMigration();
            app.MapGrpcService<DiscountService>();
            //app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}