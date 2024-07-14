using Microsoft.AspNetCore.RateLimiting;

namespace YarpApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddReverseProxy()
                .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

            //control the amount of traffic a service can handle
            //rate limiting can be configured per route or globally across all routes
            builder.Services.AddRateLimiter(rateLimiterOpts =>
            {
                rateLimiterOpts.AddFixedWindowLimiter("fixed", options =>
                {
                    options.Window = TimeSpan.FromSeconds(5);
                    options.PermitLimit = 5;
                });
            });

            var app = builder.Build();

            app.UseRateLimiter();
            app.MapReverseProxy();

            app.Run();
        }
    }
}
