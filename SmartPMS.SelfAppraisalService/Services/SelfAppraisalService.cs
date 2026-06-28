using Microsoft.EntityFrameworkCore;
using SmartPMS.SelfAppraisalService.Data;
using SmartPMS.SelfAppraisalService.DTOs;
using SmartPMS.SelfAppraisalService.Models;
using SmartPMS.Shared.Common;

namespace SmartPMS.SelfAppraisalService.Services;

public class SelfAppraisalService : ISelfAppraisalService
{
    private readonly SelfAppraisalDbContext _context;

    public SelfAppraisalService(SelfAppraisalDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<SelfAppraisalResponseDto>> CreateAsync(CreateSelfAppraisalDto request)
    {
        var validationError = ValidateCreateRequest(request);
        if (validationError is not null)
        {
            return ApiResponse<SelfAppraisalResponseDto>.Fail(validationError);
        }

        var alreadyExists = await _context.SelfAppraisals.AnyAsync(x =>
            !x.IsDeleted &&
            (x.PerformancePlanId == request.PerformancePlanId ||
             (x.EmployeeId == request.EmployeeId && x.FinancialYear == request.FinancialYear.Trim())));

        if (alreadyExists)
        {
            return ApiResponse<SelfAppraisalResponseDto>.Fail(
                "A self appraisal already exists for this performance plan or financial year.");
        }

        var appraisal = new SelfAppraisal
        {
            PerformancePlanId = request.PerformancePlanId,
            EmployeeId = request.EmployeeId,
            EmployeeCode = request.EmployeeCode.Trim(),
            EmployeeName = request.EmployeeName.Trim(),
            FinancialYear = request.FinancialYear.Trim(),
            Status = "Draft",
            CreatedAt = DateTime.UtcNow,
            Items = request.Items.Select(item => new SelfAppraisalItem
            {
                PerformancePlanItemId = item.PerformancePlanItemId,
                KraTitle = item.KraTitle.Trim(),
                KpiDescription = item.KpiDescription.Trim(),
                Weightage = item.Weightage,
                Target = item.Target.Trim(),
                Achievement = item.Achievement.Trim(),
                SelfRating = item.SelfRating,
                EmployeeComments = item.EmployeeComments.Trim(),
                DisplayOrder = item.DisplayOrder
            }).ToList()
        };

        await _context.SelfAppraisals.AddAsync(appraisal);
        await _context.SaveChangesAsync();

        return ApiResponse<SelfAppraisalResponseDto>.Ok(
            MapToResponse(appraisal),
            "Self appraisal created as draft successfully.");
    }

    public async Task<ApiResponse<SelfAppraisalResponseDto>> UpdateDraftAsync(
        long id,
        UpdateSelfAppraisalDto request)
    {
        var appraisal = await _context.SelfAppraisals
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        if (appraisal is null)
        {
            return ApiResponse<SelfAppraisalResponseDto>.Fail("Self appraisal not found.");
        }

        if (appraisal.IsSubmitted)
        {
            return ApiResponse<SelfAppraisalResponseDto>.Fail("Submitted self appraisal cannot be edited.");
        }

        if (request.Items is null || request.Items.Count == 0)
        {
            return ApiResponse<SelfAppraisalResponseDto>.Fail("At least one appraisal item is required.");
        }

        if (request.Items.Select(x => x.SelfAppraisalItemId).Distinct().Count() != request.Items.Count)
        {
            return ApiResponse<SelfAppraisalResponseDto>.Fail("Duplicate appraisal item ids are not allowed.");
        }

        var existingItemIds = appraisal.Items.Select(x => x.Id).OrderBy(x => x).ToArray();
        var requestedItemIds = request.Items.Select(x => x.SelfAppraisalItemId).OrderBy(x => x).ToArray();

        if (!existingItemIds.SequenceEqual(requestedItemIds))
        {
            return ApiResponse<SelfAppraisalResponseDto>.Fail(
                "Every appraisal item must be included and must belong to this self appraisal.");
        }

        if (request.Items.Any(x => x.SelfRating < 0 || x.SelfRating > 5))
        {
            return ApiResponse<SelfAppraisalResponseDto>.Fail("Self rating must be between 0 and 5.");
        }

        var updatesById = request.Items.ToDictionary(x => x.SelfAppraisalItemId);
        foreach (var item in appraisal.Items)
        {
            var update = updatesById[item.Id];
            item.Achievement = update.Achievement.Trim();
            item.SelfRating = update.SelfRating;
            item.EmployeeComments = update.EmployeeComments.Trim();
        }

        appraisal.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return ApiResponse<SelfAppraisalResponseDto>.Ok(
            MapToResponse(appraisal),
            "Self appraisal draft updated successfully.");
    }

    public async Task<ApiResponse<List<SelfAppraisalResponseDto>>> GetAllAsync()
    {
        var appraisals = await BaseQuery()
            .OrderByDescending(x => x.Id)
            .ToListAsync();

        return ApiResponse<List<SelfAppraisalResponseDto>>.Ok(
            appraisals.Select(MapToResponse).ToList(),
            "Self appraisals fetched successfully.");
    }

    public async Task<ApiResponse<SelfAppraisalResponseDto>> GetByIdAsync(long id)
    {
        var appraisal = await BaseQuery().FirstOrDefaultAsync(x => x.Id == id);

        return appraisal is null
            ? ApiResponse<SelfAppraisalResponseDto>.Fail("Self appraisal not found.")
            : ApiResponse<SelfAppraisalResponseDto>.Ok(
                MapToResponse(appraisal),
                "Self appraisal fetched successfully.");
    }

    public async Task<ApiResponse<List<SelfAppraisalResponseDto>>> GetByEmployeeAsync(long employeeId)
    {
        if (employeeId <= 0)
        {
            return ApiResponse<List<SelfAppraisalResponseDto>>.Fail("Employee id is required.");
        }

        var appraisals = await BaseQuery()
            .Where(x => x.EmployeeId == employeeId)
            .OrderByDescending(x => x.Id)
            .ToListAsync();

        return ApiResponse<List<SelfAppraisalResponseDto>>.Ok(
            appraisals.Select(MapToResponse).ToList(),
            "Employee self appraisals fetched successfully.");
    }

    public async Task<ApiResponse<List<SelfAppraisalResponseDto>>> GetByFinancialYearAsync(string financialYear)
    {
        if (string.IsNullOrWhiteSpace(financialYear))
        {
            return ApiResponse<List<SelfAppraisalResponseDto>>.Fail("Financial year is required.");
        }

        var normalizedYear = financialYear.Trim();
        var appraisals = await BaseQuery()
            .Where(x => x.FinancialYear == normalizedYear)
            .OrderByDescending(x => x.Id)
            .ToListAsync();

        return ApiResponse<List<SelfAppraisalResponseDto>>.Ok(
            appraisals.Select(MapToResponse).ToList(),
            "Financial year self appraisals fetched successfully.");
    }

    public async Task<ApiResponse<SelfAppraisalResponseDto>> SubmitAsync(long id)
    {
        var appraisal = await _context.SelfAppraisals
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        if (appraisal is null)
        {
            return ApiResponse<SelfAppraisalResponseDto>.Fail("Self appraisal not found.");
        }

        if (appraisal.IsSubmitted)
        {
            return ApiResponse<SelfAppraisalResponseDto>.Fail("Self appraisal is already submitted.");
        }

        if (appraisal.Items.Count == 0)
        {
            return ApiResponse<SelfAppraisalResponseDto>.Fail("Self appraisal has no items.");
        }

        if (appraisal.Items.Any(x => string.IsNullOrWhiteSpace(x.Achievement)))
        {
            return ApiResponse<SelfAppraisalResponseDto>.Fail(
                "Achievement is required for every KRA/KPI before submission.");
        }

        if (appraisal.Items.Any(x => x.SelfRating <= 0 || x.SelfRating > 5))
        {
            return ApiResponse<SelfAppraisalResponseDto>.Fail(
                "A self rating between 1 and 5 is required for every item before submission.");
        }

        appraisal.Status = "Submitted";
        appraisal.IsSubmitted = true;
        appraisal.SubmittedAt = DateTime.UtcNow;
        appraisal.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return ApiResponse<SelfAppraisalResponseDto>.Ok(
            MapToResponse(appraisal),
            "Self appraisal submitted successfully.");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(long id)
    {
        var appraisal = await _context.SelfAppraisals
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        if (appraisal is null)
        {
            return ApiResponse<bool>.Fail("Self appraisal not found.");
        }

        if (appraisal.IsSubmitted)
        {
            return ApiResponse<bool>.Fail("Submitted self appraisal cannot be deleted.");
        }

        appraisal.IsDeleted = true;
        appraisal.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return ApiResponse<bool>.Ok(true, "Self appraisal deleted successfully.");
    }

    private IQueryable<SelfAppraisal> BaseQuery()
    {
        return _context.SelfAppraisals
            .AsNoTracking()
            .Include(x => x.Items)
            .Where(x => !x.IsDeleted);
    }

    private static string? ValidateCreateRequest(CreateSelfAppraisalDto request)
    {
        if (request.PerformancePlanId <= 0) return "Performance plan id is required.";
        if (request.EmployeeId <= 0) return "Employee id is required.";
        if (string.IsNullOrWhiteSpace(request.EmployeeCode)) return "Employee code is required.";
        if (string.IsNullOrWhiteSpace(request.EmployeeName)) return "Employee name is required.";
        if (string.IsNullOrWhiteSpace(request.FinancialYear)) return "Financial year is required.";
        if (request.Items is null || request.Items.Count == 0) return "At least one appraisal item is required.";

        if (request.Items.Any(x => x.PerformancePlanItemId <= 0))
            return "Every item must reference a performance plan item.";

        if (request.Items.Select(x => x.PerformancePlanItemId).Distinct().Count() != request.Items.Count)
            return "Duplicate performance plan item ids are not allowed.";

        if (request.Items.Any(x => string.IsNullOrWhiteSpace(x.KraTitle) ||
                                   string.IsNullOrWhiteSpace(x.KpiDescription)))
            return "KRA title and KPI description are required for every item.";

        if (request.Items.Any(x => x.Weightage <= 0) || request.Items.Sum(x => x.Weightage) != 100)
            return "Item weightages must be positive and total exactly 100.";

        if (request.Items.Any(x => x.SelfRating < 0 || x.SelfRating > 5))
            return "Self rating must be between 0 and 5.";

        return null;
    }

    private static SelfAppraisalResponseDto MapToResponse(SelfAppraisal appraisal)
    {
        return new SelfAppraisalResponseDto
        {
            Id = appraisal.Id,
            PerformancePlanId = appraisal.PerformancePlanId,
            EmployeeId = appraisal.EmployeeId,
            EmployeeCode = appraisal.EmployeeCode,
            EmployeeName = appraisal.EmployeeName,
            FinancialYear = appraisal.FinancialYear,
            Status = appraisal.Status,
            IsSubmitted = appraisal.IsSubmitted,
            SubmittedAt = appraisal.SubmittedAt,
            WeightedSelfScore = Math.Round(
                appraisal.Items.Sum(x => x.Weightage * x.SelfRating) / 100m,
                2),
            Items = appraisal.Items
                .OrderBy(x => x.DisplayOrder)
                .Select(item => new SelfAppraisalItemResponseDto
                {
                    Id = item.Id,
                    PerformancePlanItemId = item.PerformancePlanItemId,
                    KraTitle = item.KraTitle,
                    KpiDescription = item.KpiDescription,
                    Weightage = item.Weightage,
                    Target = item.Target,
                    Achievement = item.Achievement,
                    SelfRating = item.SelfRating,
                    EmployeeComments = item.EmployeeComments,
                    DisplayOrder = item.DisplayOrder
                })
                .ToList()
        };
    }
}
