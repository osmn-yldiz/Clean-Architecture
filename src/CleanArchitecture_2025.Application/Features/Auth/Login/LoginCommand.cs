using MediatR;
using TS.Result;

namespace CleanArchitecture_2025.Application.Features.Auth.Login;

public sealed record LoginCommand(
    string EmailOrUserName,
    string Password) : IRequest<Result<LoginCommandResponse>>;
