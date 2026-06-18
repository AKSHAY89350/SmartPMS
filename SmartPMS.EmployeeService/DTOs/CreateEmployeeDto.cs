namespace SmartPMS.EmployeeService.DTOs
{
    public class CreateEmployeeDto
    {
        public string EmployeeCode { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public string Designation { get; set; } = string.Empty;

        public string Level { get; set; } = string.Empty;

        public DateTime JoiningDate { get; set; }

        public bool IsPaperPMS { get; set; } = false;
    }
}
