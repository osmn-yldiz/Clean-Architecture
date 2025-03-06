using CleanArchitecture_2025.Application.Services;
using CleanArchitecture_2025.Domain.Entities;
using CleanArchitecture_2025.Domain.Repositories;
using GenericRepository;
using Mapster;
using MediatR;
using TS.Result;

namespace CleanArchitecture_2025.Application.Features.Employees.CreateEmployee;

internal sealed class CreateEmployeeCommandHandler(
    IEmployeeRepository employeeRepository,
    IUnitOfWork unitOfWork,
    ICacheService cacheService) : IRequestHandler<CreateEmployeeCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        bool isEmployeeExists = await employeeRepository.AnyAsync(p => p.PersonelInformation.TCNo == request.PersonelInformation.TCNo, cancellationToken);
        if (isEmployeeExists)
        {
            return Result<string>.Failure("Bu TC numarası daha önce kaydedilmiş.");
        }

        Employee employee = request.Adapt<Employee>();

        employeeRepository.Add(employee);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        cacheService.Remove("employees");

        return "Personel kaydı başarıyla kaydedildi.";
    }
}