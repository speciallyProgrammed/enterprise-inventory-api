using EnterpriseInventoryApi.Application.DTOs.Auth;
using EnterpriseInventoryApi.Common.Errors;
using EnterpriseInventoryApi.Domain.Entities;
using EnterpriseInventoryApi.Infrastructure.Auth;
using EnterpriseInventoryApi.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;

namespace EnterpriseInventoryApi.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _users;
    private readonly IJwtTokenGenerator _tokens;
    private readonly IPasswordHasher<User> _hasher;

    public AuthService(IUserRepository users, IJwtTokenGenerator tokens, IPasswordHasher<User> hasher)
    {
        _users = users;
        _tokens = tokens;
        _hasher = hasher;
    }

    public async Task<UserDto> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var existing = await _users.GetByEmailAsync(request.Email, cancellationToken);
        if (existing != null)
        {
            throw new ApiException(ErrorCodes.Conflict, "Email already registered", 409);
        }

        var role = string.Equals(request.Role, Roles.Admin, StringComparison.OrdinalIgnoreCase)
            ? Roles.Admin
            : Roles.User;

        var user = new User
        {
            Email = request.Email,
            Name = request.Name,
            Role = role
        };
        user.PasswordHash = _hasher.HashPassword(user, request.Password);

        await _users.AddAsync(user, cancellationToken);
        await _users.SaveChangesAsync(cancellationToken);

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            Role = user.Role,
            CreatedAt = user.CreatedAt
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _users.GetByEmailAsync(request.Email, cancellationToken);
        if (user == null)
        {
            throw new ApiException(ErrorCodes.Unauthorized, "Invalid email or password", 401);
        }

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            throw new ApiException(ErrorCodes.Unauthorized, "Invalid email or password", 401);
        }

        var token = _tokens.GenerateToken(user);

        return new AuthResponse
        {
            Token = token,
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            }
        };
    }
}
