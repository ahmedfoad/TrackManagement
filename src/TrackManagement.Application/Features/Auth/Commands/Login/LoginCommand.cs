using MediatR;
using Microsoft.EntityFrameworkCore;
using TrackManagement.Infrastructure;
using TrackManagement.Infrastructure.Services;

namespace TrackManagement.Application.Features.Auth.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<LoginResponse>;
public sealed record LoginResponse(string Token, string Email, string Username, IEnumerable<string> Roles);


public sealed class LoginCommandHandler(
    TrackManagementReadonlyDbContext db,
    IJwtTokenService jwtService) : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await db.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == request.Email && u.IsActive, ct)
            ?? throw new UnauthorizedAccessException("Invalid credentials.");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials.");

        var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();
        var token = jwtService.GenerateToken(user, roles);

        return new LoginResponse(token, user.Email, user.Username, roles);
    }
}