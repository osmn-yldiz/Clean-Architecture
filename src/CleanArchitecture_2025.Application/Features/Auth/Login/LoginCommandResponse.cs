namespace CleanArchitecture_2025.Application.Features.Auth.Login;

public sealed record LoginCommandResponse(
    string Token,
    string RefreshToken,
    DateTimeOffset RefreshTokenExpires);
