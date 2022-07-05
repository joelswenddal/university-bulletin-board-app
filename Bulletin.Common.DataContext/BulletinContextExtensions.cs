using Microsoft.EntityFrameworkCore; // UseSqlServer
using Microsoft.Extensions.DependencyInjection; // IServiceCollection

namespace BulletinApp.Shared;

public static class BulletinContextExtensions
{
    /// <summary>
    /// Adds BulletinContext to the specified IServiceCollection. Uses the SqlServer database provider.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="connectionString">Set to override the default.</param>
    /// <returns>An IServiceCollection that can be used to add more services.</returns>
    public static IServiceCollection AddBulletinContext(
      this IServiceCollection services, string connectionString =
        "Data Source=(localdb\\MSSQLLocalDB;Initial Catalog=Bulletin;"
        + "Integrated Security=true;MultipleActiveResultsets=true;")
    {
        services.AddDbContext<BulletinContext>(options =>
          options.UseSqlServer(connectionString)
          //.UseLoggerFactory(new ConsoleLoggerFactory())
        );

        return services;
    }
}
