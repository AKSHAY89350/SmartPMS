using SmartPMS.SelfAppraisalService.DTOs;
using SmartPMS.Shared.Common;

namespace SmartPMS.SelfAppraisalService.Services;

public interface ISelfAppraisalService
{
    Task<ApiResponse<SelfAppraisalResponseDto>> CreateAsync(CreateSelfAppraisalDto request);
    Task<ApiResponse<SelfAppraisalResponseDto>> UpdateDraftAsync(long id, UpdateSelfAppraisalDto request);
    Task<ApiResponse<List<SelfAppraisalResponseDto>>> GetAllAsync();
    Task<ApiResponse<SelfAppraisalResponseDto>> GetByIdAsync(long id);
    Task<ApiResponse<List<SelfAppraisalResponseDto>>> GetByEmployeeAsync(long employeeId);
    Task<ApiResponse<List<SelfAppraisalResponseDto>>> GetByFinancialYearAsync(string financialYear);
    Task<ApiResponse<SelfAppraisalResponseDto>> SubmitAsync(long id);
    Task<ApiResponse<bool>> DeleteAsync(long id);
}
