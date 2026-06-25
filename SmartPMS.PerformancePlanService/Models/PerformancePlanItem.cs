namespace SmartPMS.PerformancePlanService.Models
{
    public class PerformancePlanItem
    {
        public long Id { get; set; }

        public long PerformancePlanId { get; set; }

        public string KraTitle { get; set; } = string.Empty;

        public string KpiDescription { get; set; } = string.Empty;

        public decimal Weightage { get; set; }

        public string Target { get; set; } = string.Empty;

        public string MeasurementCriteria { get; set; } = string.Empty;

        public int DisplayOrder { get; set; }

        public PerformancePlan? PerformancePlan { get; set; }
    }
}
