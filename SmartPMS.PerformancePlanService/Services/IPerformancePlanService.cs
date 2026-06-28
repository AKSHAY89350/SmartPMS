using SmartPMS.PerformancePlanService.DTOs;
using SmartPMS.Shared.Common;

namespace SmartPMS.PerformancePlanService.Services
{
    public interface IPerformancePlanService
    {
        Task<ApiResponse<PerformancePlanResponseDto>> CreatePlanAsync(CreatePerformancePlanDto request);

        Task<ApiResponse<List<PerformancePlanResponseDto>>> GetPlansAsync();

        Task<ApiResponse<PerformancePlanResponseDto>> GetPlanByIdAsync(long id);

        Task<ApiResponse<List<PerformancePlanResponseDto>>> GetPlansByEmployeeAsync(long employeeId);

        Task<ApiResponse<List<PerformancePlanResponseDto>>> GetPlansByFinancialYearAsync(string financialYear);

        Task<ApiResponse<PerformancePlanResponseDto>> SubmitPlanAsync(long id);

        Task<ApiResponse<bool>> DeletePlanAsync(long id);
    }
}
