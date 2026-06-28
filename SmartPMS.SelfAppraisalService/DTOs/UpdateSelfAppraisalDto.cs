namespace SmartPMS.SelfAppraisalService.DTOs;

public class UpdateSelfAppraisalDto
{
    public List<UpdateSelfAppraisalItemDto> Items { get; set; } = new();
}
