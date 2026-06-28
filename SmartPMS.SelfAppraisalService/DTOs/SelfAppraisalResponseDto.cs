namespace SmartPMS.SelfAppraisalService.DTOs;

public class SelfAppraisalResponseDto
{
    public long Id { get; set; }
    public long PerformancePlanId { get; set; }
    public long EmployeeId { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string EmployeeName { get; set; } = string.Empty;
    public string FinancialYear { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool IsSubmitted { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public decimal WeightedSelfScore { get; set; }
    public List<SelfAppraisalItemResponseDto> Items { get; set; } = new();
}
