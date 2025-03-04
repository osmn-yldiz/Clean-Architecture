using CleanArchitecture_2025.Domain.Entities;
using CleanArchitecture_2025.Domain.Repositories;
using GenericRepository;
using Mapster;
using MediatR;
using TS.Result;

namespace CleanArchitecture_2025.Application.Features.Employees.UpdateEmployee;

internal sealed class UpdateEmployeeCommandHandler(
    IEmployeeRepository employeeRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateEmployeeCommand, Result<string>>
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

        int changes = await unitOfWork.SaveChangesAsync(cancellationToken);
        if (changes == 0)
        {
            return Result<string>.Failure("Personel güncellemesi başarısız oldu.");
        }
        return "Personel kaydı başarıyla güncellendi.";
    }
}