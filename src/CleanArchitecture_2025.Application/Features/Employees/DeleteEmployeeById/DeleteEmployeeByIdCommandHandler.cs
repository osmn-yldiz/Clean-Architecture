using CleanArchitecture_2025.Application.Services;
using CleanArchitecture_2025.Domain.Entities;
using CleanArchitecture_2025.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace CleanArchitecture_2025.Application.Features.Employees.DeleteEmployeeById;

internal sealed class DeleteEmployeeByIdCommandHandler(
    IEmployeeRepository employeeRepository,
    IUnitOfWork unitOfWork,
    ICacheService cacheService) : IRequestHandler<DeleteEmployeeByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteEmployeeByIdCommand request, CancellationToken cancellationToken)
    {
        Employee? employee = await employeeRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (employee is null)
        {
            return Result<string>.Failure("Personel bulunamadı");
        }

        employee.IsDeleted = true;

        employeeRepository.Update(employee);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        cacheService.Remove("employees");

        return "Personel kaydı başarıyla silindi.";
    }
}