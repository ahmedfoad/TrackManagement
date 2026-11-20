using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TrackManagement.Domain.Entities.Auth;

namespace TrackManagement.Infrastructure.Seeders;

public static class UserDataSeeder
{
    public static async Task UserDataSeederAsync(this IServiceProvider services)
    {
        await using var scope = services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TrackManagementDbContext>();

        await dbContext.Database.MigrateAsync();
        await SeedAsync(dbContext);
    }
    private static async Task SeedAsync(TrackManagementDbContext dbContext)
    {
        await SeedRolesAsync(dbContext);
        await SeedUsersAsync(dbContext);
    }

    private static async Task SeedRolesAsync(TrackManagementDbContext dbContext)
    {
        if (await dbContext.Roles.AnyAsync(role => role.Name == "Admin")) return;

        dbContext.Roles.Add(new Role
        {
            Name = "Admin"
        });

        await dbContext.SaveChangesAsync();
    }

    private static async Task SeedUsersAsync(TrackManagementDbContext dbContext)
    {
        const string adminEmail = "admin@dev.local";
        var adminUser = await dbContext.Users
            .SingleOrDefaultAsync(user => user.Email == adminEmail);

        if (adminUser is null)
        {
            adminUser = new User
            {
                Username = "admin",
                Email = adminEmail,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123!"),
                IsActive = true
            };

            dbContext.Users.Add(adminUser);
            await dbContext.SaveChangesAsync();
        }

        var adminRole = await dbContext.Roles
            .SingleAsync(role => role.Name == "Admin");

        if (!await dbContext.UserRoles.AnyAsync(userRole =>
                userRole.UserId == adminUser.Id && userRole.RoleId == adminRole.Id))
        {
            dbContext.UserRoles.Add(new UserRole
            {
                UserId = adminUser.Id,
                RoleId = adminRole.Id
            });

            await dbContext.SaveChangesAsync();
        }
    }
}