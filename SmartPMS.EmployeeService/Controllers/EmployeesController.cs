using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartPMS.EmployeeService.DTOs;
using SmartPMS.EmployeeService.Services;

namespace SmartPMS.EmployeeService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeDto request)
        {
            var result = await _employeeService.CreateEmployeeAsync(request);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var result = await _employeeService.GetEmployeesAsync();

            return Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetEmployeeById(long id)
        {
            var result = await _employeeService.GetEmployeeByIdAsync(id);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpGet("by-code/{employeeCode}")]
        public async Task<IActionResult> GetEmployeeByCode(string employeeCode)
        {
            var result = await _employeeService.GetEmployeeByCodeAsync(employeeCode);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateEmployee(long id, UpdateEmployeeDto request)
        {
            var result = await _employeeService.UpdateEmployeeAsync(id, request);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteEmployee(long id)
        {
            var result = await _employeeService.DeleteEmployeeAsync(id);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok("Employee Service is running.");
        }
    }
}
