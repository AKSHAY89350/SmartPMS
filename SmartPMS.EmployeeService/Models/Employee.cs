namespace SmartPMS.EmployeeService.Models;

public class Employee
{
    public long Id { get; set; }

    public string EmployeeCode { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public string Designation { get; set; } = string.Empty;

    public string Level { get; set; } = string.Empty;

    public DateTime JoiningDate { get; set; }

    public bool IsPaperPMS { get; set; } = false;

    public bool IsActive { get; set; } = true;

    public bool IsDeleted { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}