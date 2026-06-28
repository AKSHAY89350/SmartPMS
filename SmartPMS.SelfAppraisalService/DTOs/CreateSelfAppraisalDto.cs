namespace SmartPMS.SelfAppraisalService.DTOs;

public class CreateSelfAppraisalDto
{
    public long PerformancePlanId { get; set; }
    public long EmployeeId { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string EmployeeName { get; set; } = string.Empty;
    public string FinancialYear { get; set; } = string.Empty;
    public List<CreateSelfAppraisalItemDto> Items { get; set; } = new();
}
