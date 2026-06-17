using SmartPMS.IdentityService.Data;
using SmartPMS.IdentityService.DTOs;
using SmartPMS.IdentityService.Models;
using SmartPMS.Shared.Common;
using Microsoft.EntityFrameworkCore;

namespace SmartPMS.IdentityService.Services
{
    public class AuthService : IAuthService
    {
        private readonly IdentityDbContext _context;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(IdentityDbContext context,IJwtTokenService jwtTokenService)
            {
                _context = context;
                _jwtTokenService = jwtTokenService;
            }

        public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.EmployeeCode))
            {
                return ApiResponse<AuthResponseDto>.Fail("Employee code is required.");
            }

            if (string.IsNullOrWhiteSpace(request.FullName))
            {
                return ApiResponse<AuthResponseDto>.Fail("Full name is required.");
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return ApiResponse<AuthResponseDto>.Fail("Email is required.");
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return ApiResponse<AuthResponseDto>.Fail("Password is required.");
            }

            var emailAlreadyExists = await _context.Users
                .AnyAsync(x => x.Email.ToLower() == request.Email.ToLower());

            if (emailAlreadyExists)
            {
                return ApiResponse<AuthResponseDto>.Fail("Email already exists.");
            }

            var employeeCodeAlreadyExists = await _context.Users
                .AnyAsync(x => x.EmployeeCode.ToLower() == request.EmployeeCode.ToLower());

            if (employeeCodeAlreadyExists)
            {
                return ApiResponse<AuthResponseDto>.Fail("Employee code already exists.");
            }

            var user = new AppUser
            {
                EmployeeCode = request.EmployeeCode.Trim(),
                FullName = request.FullName.Trim(),
                Email = request.Email.Trim().ToLower(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = string.IsNullOrWhiteSpace(request.Role) ? "Employee" : request.Role.Trim(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var token = _jwtTokenService.GenerateToken(user);

            var response = new AuthResponseDto
            {
                UserId = user.Id,
                EmployeeCode = user.EmployeeCode,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                Token = token
            };

            return ApiResponse<AuthResponseDto>.Ok(response, "User registered successfully.");
        }

        public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return ApiResponse<AuthResponseDto>.Fail("Email is required.");
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return ApiResponse<AuthResponseDto>.Fail("Password is required.");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email.ToLower() == request.Email.ToLower());

            if (user == null)
            {
                return ApiResponse<AuthResponseDto>.Fail("Invalid email or password.");
            }

            if (!user.IsActive)
            {
                return ApiResponse<AuthResponseDto>.Fail("User account is inactive.");
            }

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.PasswordHash
            );

            if (!isPasswordValid)
            {
                return ApiResponse<AuthResponseDto>.Fail("Invalid email or password.");
            }

            var token = _jwtTokenService.GenerateToken(user);

            var response = new AuthResponseDto
            {
                UserId = user.Id,
                EmployeeCode = user.EmployeeCode,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                Token = token
            };

            return ApiResponse<AuthResponseDto>.Ok(response, "Login successful.");
        }
    }
}
