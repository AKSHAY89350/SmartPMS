namespace SmartPMS.PerformancePlanService.DTOs
{
    public class CreatePerformancePlanDto
    {
        public long EmployeeId { get; set; }

        public string EmployeeCode { get; set; } = string.Empty;

        public string EmployeeName { get; set; } = string.Empty;

        public string FinancialYear { get; set; } = string.Empty;

        public DateTime PlanPeriodFrom { get; set; }

        public DateTime PlanPeriodTo { get; set; }

        public List<CreatePerformancePlanItemDto> Items { get; set; } = new();
    }
}
