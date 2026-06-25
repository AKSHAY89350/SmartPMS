using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartPMS.PerformancePlanService.DTOs;
using SmartPMS.PerformancePlanService.Services;

namespace SmartPMS.PerformancePlanService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerformancePlansController : ControllerBase
    {
        private readonly IPerformancePlanService _performancePlanService;

        public PerformancePlansController(IPerformancePlanService performancePlanService)
        {
            _performancePlanService = performancePlanService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlan(CreatePerformancePlanDto request)
        {
            var result = await _performancePlanService.CreatePlanAsync(request);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetPlans()
        {
            var result = await _performancePlanService.GetPlansAsync();

            return Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetPlanById(long id)
        {
            var result = await _performancePlanService.GetPlanByIdAsync(id);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpGet("employee/{employeeId:long}")]
        public async Task<IActionResult> GetPlansByEmployee(long employeeId)
        {
            var result = await _performancePlanService.GetPlansByEmployeeAsync(employeeId);

            return Ok(result);
        }

        [HttpGet("financial-year/{financialYear}")]
        public async Task<IActionResult> GetPlansByFinancialYear(string financialYear)
        {
            var result = await _performancePlanService.GetPlansByFinancialYearAsync(financialYear);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("{id:long}/submit")]
        public async Task<IActionResult> SubmitPlan(long id)
        {
            var result = await _performancePlanService.SubmitPlanAsync(id);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeletePlan(long id)
        {
            var result = await _performancePlanService.DeletePlanAsync(id);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok("Performance Plan Service is running.");
        }
    }
}
