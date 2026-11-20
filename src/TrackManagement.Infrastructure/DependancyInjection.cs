using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TrackManagement.Infrastructure;

public static class DependancyInjection
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var defaultConnectionString = configuration.GetConnectionString("Database");
        var writeConnectionString = configuration.GetConnectionString("WriteDatabase")
            ?? defaultConnectionString
            ?? throw new InvalidOperationException(
                "Connection string 'WriteDatabase' or 'Database' is not configured.");
        var readConnectionString = configuration.GetConnectionString("ReadDatabase")
            ?? defaultConnectionString
            ?? writeConnectionString;

        services.AddDbContext<TrackManagementDbContext>(options =>
            options.UseSqlServer(writeConnectionString));
        services.AddDbContext<TrackManagementReadonlyDbContext>(options =>
            options.UseSqlServer(readConnectionString));

        return services;
    }
}
