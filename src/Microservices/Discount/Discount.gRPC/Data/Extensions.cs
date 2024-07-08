using Microsoft.EntityFrameworkCore;

namespace Discount.gRPC.Data
{
    public static class Extensions
    {
        //this Extensions class will cover the auto-migration of db

        public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            using var dbCtx = scope.ServiceProvider.GetRequiredService<DiscountDbContext>();
            dbCtx.Database.MigrateAsync();

            return app;
        }
    }
}
