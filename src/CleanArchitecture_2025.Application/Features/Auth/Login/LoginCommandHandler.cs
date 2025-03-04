using CleanArchitecture_2025.Application.Services;
using CleanArchitecture_2025.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace CleanArchitecture_2025.Application.Features.Auth.Login;

internal sealed class LoginCommandHandler(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    IJwtProvider jwtProvider) : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
{
    public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        AppUser? user = await userManager.Users
            .FirstOrDefaultAsync(p =>
            p.UserName == request.EmailOrUserName ||
            p.Email == request.EmailOrUserName,
            cancellationToken);

        if (user is null)
        {
            return Result<LoginCommandResponse>.Failure("Kullanıcı bulunamadı.");
        }

        SignInResult signInResult = await signInManager.CheckPasswordSignInAsync(user, request.Password, true);

        if (signInResult.IsLockedOut)
        {
            TimeSpan? timeSpan = user.LockoutEnd - DateTime.UtcNow;
            if (timeSpan is not null)
            {
                return Result<LoginCommandResponse>.Failure($"Şifrenizi 3 defa yanlış girdiğiniz için kullanıcı {Math.Ceiling(timeSpan.Value.TotalMinutes)} dakika süreyle bloke edilmiştir.");
                //return (500, $"Şifrenizi 3 defa yanlış girdiğiniz için kullanıcı {Math.Ceiling(timeSpan.Value.TotalMinutes)} dakika süreyle bloke edilmiştir");
            }
            else
            {
                return Result<LoginCommandResponse>.Failure("Kullanıcınız 3 kez yanlış şifre girdiği için 5 dakika süreyle bloke edilmiştir.");
                //return (500, "Kullanıcınız 3 kez yanlış şifre girdiği için 5 dakika süreyle bloke edilmiştir");
            }
        }

        if (signInResult.IsNotAllowed)
        {
            return Result<LoginCommandResponse>.Failure("Mail adresiniz onaylı değil.");
            //return (500, "Mail adresiniz onaylı değil");
        }

        if (!signInResult.Succeeded)
        {
            return Result<LoginCommandResponse>.Failure("Şifreniz yanlış.");
            //return (500, "Şifreniz yanlış");
        }

        // Token üret
        var response = await jwtProvider.CreateTokenAsync(user, cancellationToken);
        var loginResponse = new LoginCommandResponse(
            Token: response.Token,
            RefreshToken: response.RefreshToken,
            RefreshTokenExpires: response.RefreshTokenExpires // Yenileme belirteci için bir son kullanma tarihi belirleyin
        );

        return loginResponse;
    }
}