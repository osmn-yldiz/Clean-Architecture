using CleanArchitecture_2025.Domain.Entities;
using CleanArchitecture_2025.Domain.Repositories;
using MediatR;
using TS.Result;

namespace CleanArchitecture_2025.Application.Features.Employees.GetByIdEmployee;

internal sealed class GetByIdEmployeeQueryHandler(
    IEmployeeRepository employeeRepository) : IRequestHandler<GetByIdEmployeeQuery, Result<Employee>>
{
    public async Task<Result<Employee>> Handle(GetByIdEmployeeQuery request, CancellationToken cancellationToken)
    {
        Employee employee = await employeeRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (employee is null)
        {
            return Result<Employee>.Failure("Personel bulunamadı.");
        }

        return employee;
    }
}