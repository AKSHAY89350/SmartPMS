namespace SmartPMS.PerformancePlanService.Models
{
    public class PerformancePlan
    {
        public long Id { get; set; }

        public long EmployeeId { get; set; }

        public string EmployeeCode { get; set; } = string.Empty;

        public string EmployeeName { get; set; } = string.Empty;

        public string FinancialYear { get; set; } = string.Empty;

        public DateTime PlanPeriodFrom { get; set; }

        public DateTime PlanPeriodTo { get; set; }

        public string Status { get; set; } = "Draft";

        public bool IsSubmitted { get; set; } = false;

        public DateTime? SubmittedAt { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public List<PerformancePlanItem> Items { get; set; } = new();
    }
}
