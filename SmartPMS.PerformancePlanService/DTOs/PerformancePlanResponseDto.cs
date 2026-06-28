namespace SmartPMS.PerformancePlanService.DTOs
{
    public class PerformancePlanResponseDto
    {
        public long Id { get; set; }

        public long EmployeeId { get; set; }

        public string EmployeeCode { get; set; } = string.Empty;

        public string EmployeeName { get; set; } = string.Empty;

        public string FinancialYear { get; set; } = string.Empty;

        public DateTime PlanPeriodFrom { get; set; }

        public DateTime PlanPeriodTo { get; set; }

        public string Status { get; set; } = string.Empty;

        public bool IsSubmitted { get; set; }

        public DateTime? SubmittedAt { get; set; }

        public decimal TotalWeightage { get; set; }

        public List<PerformancePlanItemResponseDto> Items { get; set; } = new();
    }
}
