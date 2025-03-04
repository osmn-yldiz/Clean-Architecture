using CleanArchitecture_2025.Domain.Entities;
using CleanArchitecture_2025.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace CleanArchitecture_2025.Application.Features.Employees.DeleteEmployeeById;

internal sealed class DeleteEmployeeByIdCommandHandler(
    IEmployeeRepository employeeRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteEmployeeByIdCommand, Result<string>>
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

        int changes = await unitOfWork.SaveChangesAsync(cancellationToken);
        if (changes == 0)
        {
            return Result<string>.Failure("Personel silinmesi başarısız oldu.");
        }

        return "Personel kaydı başarıyla silindi.";
    }
}