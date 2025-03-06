using CleanArchitecture_2025.Application.Services;
using CleanArchitecture_2025.Domain.Entities;
using CleanArchitecture_2025.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture_2025.Application.Features.Employees.GetAllEmployees;

internal sealed class GetAllEmployeesQueryHandler(
    IEmployeeRepository employeeRepository,
    UserManager<AppUser> userManager,
    ICacheService cacheService) : IRequestHandler<GetAllEmployeesQuery, IQueryable<GetAllEmployeesQueryResponse>>
{
    public Task<IQueryable<GetAllEmployeesQueryResponse>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
    {

        #region LINQ 
        //var response = (from employee in employeeRepository.GetAll()
        //                join create_user in userManager.Users.AsQueryable() on employee.CreateUserId equals create_user.Id
        //                join update_user in userManager.Users.AsQueryable() on employee.CreateUserId equals update_user.Id into update_user
        //                from update_users in update_user.DefaultIfEmpty()
        //                select new GetAllEmployeesQueryResponse
        //                {
        //                    FirstName = employee.FirstName,
        //                    LastName = employee.LastName,
        //                    Salary = employee.Salary,
        //                    BirthOfDate = employee.BirthOfDate,
        //                    CreateAt = employee.CreateAt,
        //                    DeleteAt = employee.DeleteAt,
        //                    Id = employee.Id,
        //                    IsDeleted = employee.IsDeleted,
        //                    TCNo = employee.PersonelInformation.TCNo,
        //                    UpdateAt = employee.UpdateAt,
        //                    CreateUserId = employee.CreateUserId,
        //                    CreateUserName = create_user.FirstName + " " + create_user.LastName + " (" + create_user.Email + ")",
        //                    UpdateUserId = employee.UpdateUserId,
        //                    UpdateUserName = employee.UpdateUserId == null ? null : update_users.FirstName + " " + update_users.LastName + " (" + update_users.Email + ")",
        //                });
        //return Task.FromResult(response);
        #endregion

        #region ORM
        //var employees = employeeRepository
        //    .GetAll()
        //    .Include(e => e.PersonelInformation)
        //    .AsQueryable();

        //var users = userManager.Users.AsQueryable();

        //var response = employees.Select(employee => new GetAllEmployeesQueryResponse
        //{
        //    FirstName = employee.FirstName,
        //    LastName = employee.LastName,
        //    Salary = employee.Salary,
        //    BirthOfDate = employee.BirthOfDate,
        //    CreateAt = employee.CreateAt,
        //    DeleteAt = employee.DeleteAt,
        //    Id = employee.Id,
        //    IsDeleted = employee.IsDeleted,
        //    TCNo = employee.PersonelInformation.TCNo,
        //    UpdateAt = employee.UpdateAt,
        //    CreateUserId = employee.CreateUserId,
        //    CreateUserName = users
        //        .Where(u => u.Id == employee.CreateUserId)
        //        .Select(u => u.FirstName + " " + u.LastName + " (" + u.Email + ")")
        //        .First(),
        //    UpdateUserId = employee.UpdateUserId,
        //    UpdateUserName = employee.UpdateUserId == null ? null :
        //        users.Where(u => u.Id == employee.UpdateUserId)
        //             .Select(u => u.FirstName + " " + u.LastName + " (" + u.Email + ")")
        //             .FirstOrDefault(),
        //}).AsQueryable();

        //return Task.FromResult(response);
        #endregion

        #region Memory Cache 
        var cachedEmployees = cacheService.Get<List<GetAllEmployeesQueryResponse>>("employees");

        if (cachedEmployees is not null)
        {
            return Task.FromResult(cachedEmployees.AsQueryable());
        }

        var employees = employeeRepository
            .GetAll()
            .Include(e => e.PersonelInformation)
            .AsQueryable();

        var users = userManager.Users.AsQueryable();

        var response = employees.Select(employee => new GetAllEmployeesQueryResponse
        {
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Salary = employee.Salary,
            BirthOfDate = employee.BirthOfDate,
            CreateAt = employee.CreateAt,
            DeleteAt = employee.DeleteAt,
            Id = employee.Id,
            IsDeleted = employee.IsDeleted,
            TCNo = employee.PersonelInformation.TCNo,
            UpdateAt = employee.UpdateAt,
            CreateUserId = employee.CreateUserId,
            CreateUserName = users
                .Where(u => u.Id == employee.CreateUserId)
                .Select(u => u.FirstName + " " + u.LastName + " (" + u.Email + ")")
                .First(),
            UpdateUserId = employee.UpdateUserId,
            UpdateUserName = employee.UpdateUserId == null ? null :
                users.Where(u => u.Id == employee.UpdateUserId)
                     .Select(u => u.FirstName + " " + u.LastName + " (" + u.Email + ")")
                     .FirstOrDefault(),
        }).ToList();

        cacheService.Set("employees", response, TimeSpan.FromMinutes(3));

        return Task.FromResult(response.AsQueryable());
        #endregion

    }
}