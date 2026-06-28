using Microsoft.EntityFrameworkCore;
using SmartPMS.PerformancePlanService.Data;
using SmartPMS.PerformancePlanService.DTOs;
using SmartPMS.PerformancePlanService.Models;
using SmartPMS.Shared.Common;

namespace SmartPMS.PerformancePlanService.Services;

public class PerformancePlanService : IPerformancePlanService
{
    private readonly PerformancePlanDbContext _context;

    public PerformancePlanService(PerformancePlanDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<PerformancePlanResponseDto>> CreatePlanAsync(CreatePerformancePlanDto request)
    {
        if (request.EmployeeId <= 0)
        {
            return ApiResponse<PerformancePlanResponseDto>.Fail("Employee id is required.");
        }

        if (string.IsNullOrWhiteSpace(request.EmployeeCode))
        {
            return ApiResponse<PerformancePlanResponseDto>.Fail("Employee code is required.");
        }

        if (string.IsNullOrWhiteSpace(request.EmployeeName))
        {
            return ApiResponse<PerformancePlanResponseDto>.Fail("Employee name is required.");
        }

        if (string.IsNullOrWhiteSpace(request.FinancialYear))
        {
            return ApiResponse<PerformancePlanResponseDto>.Fail("Financial year is required.");
        }

        if (request.PlanPeriodFrom >= request.PlanPeriodTo)
        {
            return ApiResponse<PerformancePlanResponseDto>.Fail("Plan period from date must be less than to date.");
        }

        if (request.Items == null || request.Items.Count == 0)
        {
            return ApiResponse<PerformancePlanResponseDto>.Fail("At least one KRA/KPI item is required.");
        }

        var totalWeightage = request.Items.Sum(x => x.Weightage);

        if (totalWeightage != 100)
        {
            return ApiResponse<PerformancePlanResponseDto>.Fail("Total weightage must be exactly 100.");
        }

        var alreadyExists = await _context.PerformancePlans
            .AnyAsync(x => x.EmployeeId == request.EmployeeId
                           && x.FinancialYear == request.FinancialYear
                           && !x.IsDeleted);

        if (alreadyExists)
        {
            return ApiResponse<PerformancePlanResponseDto>.Fail("Performance plan already exists for this employee and financial year.");
        }

        var plan = new PerformancePlan
        {
            EmployeeId = request.EmployeeId,
            EmployeeCode = request.EmployeeCode.Trim(),
            EmployeeName = request.EmployeeName.Trim(),
            FinancialYear = request.FinancialYear.Trim(),
            PlanPeriodFrom = request.PlanPeriodFrom,
            PlanPeriodTo = request.PlanPeriodTo,
            Status = "Draft",
            IsSubmitted = false,
            CreatedAt = DateTime.UtcNow,
            Items = request.Items.Select(item => new PerformancePlanItem
            {
                KraTitle = item.KraTitle.Trim(),
                KpiDescription = item.KpiDescription.Trim(),
                Weightage = item.Weightage,
                Target = item.Target.Trim(),
                MeasurementCriteria = item.MeasurementCriteria.Trim(),
                DisplayOrder = item.DisplayOrder
            }).ToList()
        };

        await _context.PerformancePlans.AddAsync(plan);
        await _context.SaveChangesAsync();

        return ApiResponse<PerformancePlanResponseDto>.Ok(
            MapToResponse(plan),
            "Performance plan created as draft successfully."
        );
    }

    public async Task<ApiResponse<List<PerformancePlanResponseDto>>> GetPlansAsync()
    {
        var plans = await _context.PerformancePlans
            .Include(x => x.Items)
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.Id)
            .ToListAsync();

        return ApiResponse<List<PerformancePlanResponseDto>>.Ok(
            plans.Select(MapToResponse).ToList(),
            "Performance plans fetched successfully."
        );
    }

    public async Task<ApiResponse<PerformancePlanResponseDto>> GetPlanByIdAsync(long id)
    {
        var plan = await _context.PerformancePlans
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        if (plan == null)
        {
            return ApiResponse<PerformancePlanResponseDto>.Fail("Performance plan not found.");
        }

        return ApiResponse<PerformancePlanResponseDto>.Ok(
            MapToResponse(plan),
            "Performance plan fetched successfully."
        );
    }

    public async Task<ApiResponse<List<PerformancePlanResponseDto>>> GetPlansByEmployeeAsync(long employeeId)
    {
        var plans = await _context.PerformancePlans
            .Include(x => x.Items)
            .Where(x => x.EmployeeId == employeeId && !x.IsDeleted)
            .OrderByDescending(x => x.Id)
            .ToListAsync();

        return ApiResponse<List<PerformancePlanResponseDto>>.Ok(
            plans.Select(MapToResponse).ToList(),
            "Employee performance plans fetched successfully."
        );
    }

    public async Task<ApiResponse<List<PerformancePlanResponseDto>>> GetPlansByFinancialYearAsync(string financialYear)
    {
        if (string.IsNullOrWhiteSpace(financialYear))
        {
            return ApiResponse<List<PerformancePlanResponseDto>>.Fail("Financial year is required.");
        }

        var plans = await _context.PerformancePlans
            .Include(x => x.Items)
            .Where(x => x.FinancialYear == financialYear && !x.IsDeleted)
            .OrderByDescending(x => x.Id)
            .ToListAsync();

        return ApiResponse<List<PerformancePlanResponseDto>>.Ok(
            plans.Select(MapToResponse).ToList(),
            "Financial year performance plans fetched successfully."
        );
    }

    public async Task<ApiResponse<PerformancePlanResponseDto>> SubmitPlanAsync(long id)
    {
        var plan = await _context.PerformancePlans
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        if (plan == null)
        {
            return ApiResponse<PerformancePlanResponseDto>.Fail("Performance plan not found.");
        }

        if (plan.IsSubmitted)
        {
            return ApiResponse<PerformancePlanResponseDto>.Fail("Performance plan is already submitted.");
        }

        if (plan.Items.Count == 0)
        {
            return ApiResponse<PerformancePlanResponseDto>.Fail("Plan must have at least one KRA/KPI item before submit.");
        }

        var totalWeightage = plan.Items.Sum(x => x.Weightage);

        if (totalWeightage != 100)
        {
            return ApiResponse<PerformancePlanResponseDto>.Fail("Total weightage must be exactly 100 before submit.");
        }

        plan.IsSubmitted = true;
        plan.Status = "Submitted";
        plan.SubmittedAt = DateTime.UtcNow;
        plan.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return ApiResponse<PerformancePlanResponseDto>.Ok(
            MapToResponse(plan),
            "Performance plan submitted successfully."
        );
    }

    public async Task<ApiResponse<bool>> DeletePlanAsync(long id)
    {
        var plan = await _context.PerformancePlans
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        if (plan == null)
        {
            return ApiResponse<bool>.Fail("Performance plan not found.");
        }

        if (plan.IsSubmitted)
        {
            return ApiResponse<bool>.Fail("Submitted performance plan cannot be deleted.");
        }

        plan.IsDeleted = true;
        plan.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return ApiResponse<bool>.Ok(true, "Performance plan deleted successfully.");
    }

    private static PerformancePlanResponseDto MapToResponse(PerformancePlan plan)
    {
        return new PerformancePlanResponseDto
        {
            Id = plan.Id,
            EmployeeId = plan.EmployeeId,
            EmployeeCode = plan.EmployeeCode,
            EmployeeName = plan.EmployeeName,
            FinancialYear = plan.FinancialYear,
            PlanPeriodFrom = plan.PlanPeriodFrom,
            PlanPeriodTo = plan.PlanPeriodTo,
            Status = plan.Status,
            IsSubmitted = plan.IsSubmitted,
            SubmittedAt = plan.SubmittedAt,
            TotalWeightage = plan.Items.Sum(x => x.Weightage),
            Items = plan.Items
                .OrderBy(x => x.DisplayOrder)
                .Select(item => new PerformancePlanItemResponseDto
                {
                    Id = item.Id,
                    KraTitle = item.KraTitle,
                    KpiDescription = item.KpiDescription,
                    Weightage = item.Weightage,
                    Target = item.Target,
                    MeasurementCriteria = item.MeasurementCriteria,
                    DisplayOrder = item.DisplayOrder
                })
                .ToList()
        };
    }
}