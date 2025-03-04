using FluentValidation;

namespace CleanArchitecture_2025.Application.Features.Employees.UpdateEmployee;

public sealed class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ad alanı boş olamaz.")
            .MinimumLength(3).WithMessage("Ad alanı en az 3 karakter olmalıdır.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Soyad alanı boş olamaz.")
            .MinimumLength(3).WithMessage("Soyad alanı en az 3 karakter olmalıdır.");

        RuleFor(x => x.BirthOfDate)
            .NotEmpty().WithMessage("Doğum tarihi boş olamaz.")
            .LessThan(DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("Doğum tarihi bugünden büyük olamaz.")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow).AddYears(-18))
            .WithMessage("Çalışan en az 18 yaşında olmalıdır.");

        RuleFor(x => x.Salary)
            .GreaterThan(0).WithMessage("Maaş 0'dan büyük olmalıdır.");

        RuleFor(x => x.PersonelInformation.TCNo)
            .NotEmpty().WithMessage("TC Kimlik numarası boş olamaz.")
            .Length(11).WithMessage("Geçerli bir TC numarası yazın.");
    }
}
