using Microsoft.EntityFrameworkCore;
using SmartPMS.EmployeeService.Data;
using SmartPMS.EmployeeService.DTOs;
using SmartPMS.EmployeeService.Models;
using SmartPMS.Shared.Common;

namespace SmartPMS.EmployeeService.Services;

public class EmployeeService : IEmployeeService
{
    private readonly EmployeeDbContext _context;

    public EmployeeService(EmployeeDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<EmployeeResponseDto>> CreateEmployeeAsync(CreateEmployeeDto request)
    {
        if (string.IsNullOrWhiteSpace(request.EmployeeCode))
        {
            return ApiResponse<EmployeeResponseDto>.Fail("Employee code is required.");
        }

        if (string.IsNullOrWhiteSpace(request.FullName))
        {
            return ApiResponse<EmployeeResponseDto>.Fail("Full name is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return ApiResponse<EmployeeResponseDto>.Fail("Email is required.");
        }

        var employeeCodeExists = await _context.Employees
            .AnyAsync(x => x.EmployeeCode.ToLower() == request.EmployeeCode.ToLower()
                           && !x.IsDeleted);

        if (employeeCodeExists)
        {
            return ApiResponse<EmployeeResponseDto>.Fail("Employee code already exists.");
        }

        var emailExists = await _context.Employees
            .AnyAsync(x => x.Email.ToLower() == request.Email.ToLower()
                           && !x.IsDeleted);

        if (emailExists)
        {
            return ApiResponse<EmployeeResponseDto>.Fail("Email already exists.");
        }

        var employee = new Employee
        {
            EmployeeCode = request.EmployeeCode.Trim(),
            FullName = request.FullName.Trim(),
            Email = request.Email.Trim().ToLower(),
            Department = request.Department.Trim(),
            Designation = request.Designation.Trim(),
            Level = request.Level.Trim(),
            JoiningDate = request.JoiningDate,
            IsPaperPMS = request.IsPaperPMS,
            IsActive = true,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();

        return ApiResponse<EmployeeResponseDto>.Ok(
            MapToResponse(employee),
            "Employee created successfully."
        );
    }

    public async Task<ApiResponse<List<EmployeeResponseDto>>> GetEmployeesAsync()
    {
        var employees = await _context.Employees
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.Id)
            .Select(x => MapToResponse(x))
            .ToListAsync();

        return ApiResponse<List<EmployeeResponseDto>>.Ok(
            employees,
            "Employees fetched successfully."
        );
    }

    public async Task<ApiResponse<EmployeeResponseDto>> GetEmployeeByIdAsync(long id)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        if (employee == null)
        {
            return ApiResponse<EmployeeResponseDto>.Fail("Employee not found.");
        }

        return ApiResponse<EmployeeResponseDto>.Ok(
            MapToResponse(employee),
            "Employee fetched successfully."
        );
    }

    public async Task<ApiResponse<EmployeeResponseDto>> GetEmployeeByCodeAsync(string employeeCode)
    {
        if (string.IsNullOrWhiteSpace(employeeCode))
        {
            return ApiResponse<EmployeeResponseDto>.Fail("Employee code is required.");
        }

        var employee = await _context.Employees
            .FirstOrDefaultAsync(x => x.EmployeeCode.ToLower() == employeeCode.ToLower()
                                      && !x.IsDeleted);

        if (employee == null)
        {
            return ApiResponse<EmployeeResponseDto>.Fail("Employee not found.");
        }

        return ApiResponse<EmployeeResponseDto>.Ok(
            MapToResponse(employee),
            "Employee fetched successfully."
        );
    }

    public async Task<ApiResponse<EmployeeResponseDto>> UpdateEmployeeAsync(long id, UpdateEmployeeDto request)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        if (employee == null)
        {
            return ApiResponse<EmployeeResponseDto>.Fail("Employee not found.");
        }

        if (string.IsNullOrWhiteSpace(request.FullName))
        {
            return ApiResponse<EmployeeResponseDto>.Fail("Full name is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return ApiResponse<EmployeeResponseDto>.Fail("Email is required.");
        }

        var emailExists = await _context.Employees
            .AnyAsync(x => x.Id != id
                           && x.Email.ToLower() == request.Email.ToLower()
                           && !x.IsDeleted);

        if (emailExists)
        {
            return ApiResponse<EmployeeResponseDto>.Fail("Email already exists.");
        }

        employee.FullName = request.FullName.Trim();
        employee.Email = request.Email.Trim().ToLower();
        employee.Department = request.Department.Trim();
        employee.Designation = request.Designation.Trim();
        employee.Level = request.Level.Trim();
        employee.JoiningDate = request.JoiningDate;
        employee.IsPaperPMS = request.IsPaperPMS;
        employee.IsActive = request.IsActive;
        employee.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return ApiResponse<EmployeeResponseDto>.Ok(
            MapToResponse(employee),
            "Employee updated successfully."
        );
    }

    public async Task<ApiResponse<bool>> DeleteEmployeeAsync(long id)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        if (employee == null)
        {
            return ApiResponse<bool>.Fail("Employee not found.");
        }

        employee.IsDeleted = true;
        employee.IsActive = false;
        employee.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return ApiResponse<bool>.Ok(true, "Employee deleted successfully.");
    }

    private static EmployeeResponseDto MapToResponse(Employee employee)
    {
        return new EmployeeResponseDto
        {
            Id = employee.Id,
            EmployeeCode = employee.EmployeeCode,
            FullName = employee.FullName,
            Email = employee.Email,
            Department = employee.Department,
            Designation = employee.Designation,
            Level = employee.Level,
            JoiningDate = employee.JoiningDate,
            IsPaperPMS = employee.IsPaperPMS,
            IsActive = employee.IsActive
        };
    }
}