using CleanArchitecture_2025.Application.Features.Auth.Login;
using MediatR;
using TS.Result;

namespace CleanArchitecture_2025.WebAPI.Modules.Auth;

public static class AuthModule
{
    public static void RegisterAuthRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/auth").WithTags("Auth");

        group.MapPost("login",
            async (ISender sender, LoginCommand request, CancellationToken cancellationToken) =>
            {
                if (request is null)
                {
                    return Results.BadRequest("Request cannot be null.");
                }

                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<LoginCommandResponse>>()
            .WithName("login");
    }
}
