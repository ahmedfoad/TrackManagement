using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TrackManagement.Infrastructure;

public static class DependancyInjection
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database")
            ?? throw new InvalidOperationException(
                "Connection string 'Database' is not configured. Set ConnectionStrings__Database to the SQL Server connection string.");

        //services.AddDbContext<SubmissionDbContext>(options => options.UseSqlServer(connectionString));
        //services.AddScoped(typeof(Repository<,>));
        //services.AddScoped<ArticleRepository>();

        return services;
    }
}
