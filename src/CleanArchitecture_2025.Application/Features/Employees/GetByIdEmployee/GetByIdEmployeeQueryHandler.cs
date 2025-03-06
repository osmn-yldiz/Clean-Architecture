using CleanArchitecture_2025.Application.Services;
using CleanArchitecture_2025.Domain.Entities;
using CleanArchitecture_2025.Domain.Repositories;
using MediatR;
using TS.Result;

namespace CleanArchitecture_2025.Application.Features.Employees.GetByIdEmployee;

internal sealed class GetByIdEmployeeQueryHandler(
    IEmployeeRepository employeeRepository,
    ICacheService cacheService) : IRequestHandler<GetByIdEmployeeQuery, Result<Employee>>
{
    public async Task<Result<Employee>> Handle(GetByIdEmployeeQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"employee_{request.Id}";

        var cachedEmployee = cacheService.Get<Employee>(cacheKey);
        if (cachedEmployee is not null)
        {
            return cachedEmployee;
        }

        Employee employee = await employeeRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (employee is null)
        {
            return Result<Employee>.Failure("Personel bulunamadı.");
        }

        cacheService.Set(cacheKey, employee, TimeSpan.FromMinutes(3));

        return employee;
    }
}