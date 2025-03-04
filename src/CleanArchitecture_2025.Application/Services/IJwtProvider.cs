using CleanArchitecture_2025.Application.Features.Auth.Login;
using CleanArchitecture_2025.Domain.Entities;

namespace CleanArchitecture_2025.Application.Services;

public interface IJwtProvider
{
    Task<LoginCommandResponse> CreateTokenAsync(AppUser user, CancellationToken cancellationToken = default);
}
