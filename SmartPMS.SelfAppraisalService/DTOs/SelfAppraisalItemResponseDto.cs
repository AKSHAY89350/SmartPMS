namespace SmartPMS.SelfAppraisalService.DTOs;

public class SelfAppraisalItemResponseDto
{
    public long Id { get; set; }
    public long PerformancePlanItemId { get; set; }
    public string KraTitle { get; set; } = string.Empty;
    public string KpiDescription { get; set; } = string.Empty;
    public decimal Weightage { get; set; }
    public string Target { get; set; } = string.Empty;
    public string Achievement { get; set; } = string.Empty;
    public decimal SelfRating { get; set; }
    public string EmployeeComments { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}
