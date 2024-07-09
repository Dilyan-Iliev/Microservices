using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure.Data.Extensions
{
    public static class DbExtensions
    {
        public static async Task InitialiseDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            ctx.Database.MigrateAsync().GetAwaiter().GetResult();

            await SeedAsync(ctx);
        }

        private static async Task SeedAsync(ApplicationDbContext context)
        {
            await SeedCustomerAsync(context);
            await SeedProductAsync(context);
            await SeedOrdersWithItemsAsync(context);
        }

        private static async Task SeedCustomerAsync(ApplicationDbContext context)
        {
            if (!await context.Customers.AnyAsync())
            {
                await context.Customers.AddRangeAsync(InitialData.Customers);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedProductAsync(ApplicationDbContext context)
        {
            if (!await context.Products.AnyAsync())
            {
                await context.Products.AddRangeAsync(InitialData.Products);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedOrdersWithItemsAsync(ApplicationDbContext context)
        {
            if (!await context.Orders.AnyAsync())
            {
                await context.Orders.AddRangeAsync(InitialData.OrdersWithItems);
                await context.SaveChangesAsync();
            }
        }
    }
}
