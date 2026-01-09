using EnterpriseInventoryApi.Application.DTOs.Auth;

namespace EnterpriseInventoryApi.Application.Services;

public interface IAuthService
{
    Task<UserDto> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
}
