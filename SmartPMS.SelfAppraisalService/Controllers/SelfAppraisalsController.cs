using Microsoft.AspNetCore.Mvc;
using SmartPMS.SelfAppraisalService.DTOs;
using SmartPMS.SelfAppraisalService.Services;

namespace SmartPMS.SelfAppraisalService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SelfAppraisalsController : ControllerBase
{
    private readonly ISelfAppraisalService _selfAppraisalService;

    public SelfAppraisalsController(ISelfAppraisalService selfAppraisalService)
    {
        _selfAppraisalService = selfAppraisalService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSelfAppraisalDto request)
    {
        var result = await _selfAppraisalService.CreateAsync(request);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateDraft(long id, UpdateSelfAppraisalDto request)
    {
        var result = await _selfAppraisalService.UpdateDraftAsync(id, request);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _selfAppraisalService.GetAllAsync());
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _selfAppraisalService.GetByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpGet("employee/{employeeId:long}")]
    public async Task<IActionResult> GetByEmployee(long employeeId)
    {
        var result = await _selfAppraisalService.GetByEmployeeAsync(employeeId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("financial-year/{financialYear}")]
    public async Task<IActionResult> GetByFinancialYear(string financialYear)
    {
        var result = await _selfAppraisalService.GetByFinancialYearAsync(financialYear);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("{id:long}/submit")]
    public async Task<IActionResult> Submit(long id)
    {
        var result = await _selfAppraisalService.SubmitAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _selfAppraisalService.DeleteAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok("Self Appraisal Service is running.");
    }
}
