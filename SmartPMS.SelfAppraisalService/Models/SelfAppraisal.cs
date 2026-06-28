namespace SmartPMS.SelfAppraisalService.Models;

public class SelfAppraisal
{
    public long Id { get; set; }
    public long PerformancePlanId { get; set; }
    public long EmployeeId { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string EmployeeName { get; set; } = string.Empty;
    public string FinancialYear { get; set; } = string.Empty;
    public string Status { get; set; } = "Draft";
    public bool IsSubmitted { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public List<SelfAppraisalItem> Items { get; set; } = new();
}
