using SmartPMS.EmployeeService.DTOs;
using SmartPMS.Shared.Common;

namespace SmartPMS.EmployeeService.Services;

public interface IEmployeeService
{
    Task<ApiResponse<EmployeeResponseDto>> CreateEmployeeAsync(CreateEmployeeDto request);

    Task<ApiResponse<List<EmployeeResponseDto>>> GetEmployeesAsync();

    Task<ApiResponse<EmployeeResponseDto>> GetEmployeeByIdAsync(long id);

    Task<ApiResponse<EmployeeResponseDto>> GetEmployeeByCodeAsync(string employeeCode);

    Task<ApiResponse<EmployeeResponseDto>> UpdateEmployeeAsync(long id, UpdateEmployeeDto request);

    Task<ApiResponse<bool>> DeleteEmployeeAsync(long id);
}