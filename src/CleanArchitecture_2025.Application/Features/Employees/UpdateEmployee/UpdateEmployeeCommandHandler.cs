using CleanArchitecture_2025.Application.Services;
using CleanArchitecture_2025.Domain.Entities;
using CleanArchitecture_2025.Domain.Repositories;
using GenericRepository;
using Mapster;
using MediatR;
using TS.Result;

namespace CleanArchitecture_2025.Application.Features.Employees.UpdateEmployee;

internal sealed class UpdateEmployeeCommandHandler(
    IEmployeeRepository employeeRepository,
    IUnitOfWork unitOfWork,
    ICacheService cacheService) : IRequestHandler<UpdateEmployeeCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        Employee? employee = await employeeRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);

        if (employee is null)
        {
            return Result<string>.Failure("Personel bulunamadı.");
        }

        request.Adapt(employee);

        employeeRepository.Update(employee);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        cacheService.Remove("employees");

        return "Personel kaydı başarıyla güncellendi.";
    }
}