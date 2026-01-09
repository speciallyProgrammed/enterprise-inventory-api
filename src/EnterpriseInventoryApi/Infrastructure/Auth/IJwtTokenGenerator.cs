using EnterpriseInventoryApi.Domain.Entities;

namespace EnterpriseInventoryApi.Infrastructure.Auth;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
