using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CommonLibrary.Extensions
{
    public static class MigrateDbContextExtensions
    {
        public static async Task MigrateDbContextAsync<TContext>(this WebApplicationBuilder builder,
            Func<TContext, IServiceProvider, Task> seeder) where TContext : DbContext
        {
            var services = builder.Services.BuildServiceProvider();
            var context = services.GetRequiredService<TContext>();
            if ((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                await context.Database.MigrateAsync();
            }
            await seeder(context, services);
        }
    }
}
