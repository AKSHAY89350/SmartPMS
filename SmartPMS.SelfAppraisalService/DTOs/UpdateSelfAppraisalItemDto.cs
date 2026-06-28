namespace SmartPMS.SelfAppraisalService.DTOs;

public class UpdateSelfAppraisalItemDto
{
    public long SelfAppraisalItemId { get; set; }
    public string Achievement { get; set; } = string.Empty;
    public decimal SelfRating { get; set; }
    public string EmployeeComments { get; set; } = string.Empty;
}
