using AccountingSystem.Application.DTOs;
using AccountingSystem.Application.Intrefaces;
using AccountingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AccountingSystem.Infrastructure.Services;

/// <summary>
/// השירות שמטפל באימות משתמשים
/// </summary>
public class AuthenticationService : IAuthenticationService
{
    private readonly AccountingDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthenticationService(AccountingDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<LoginResponseDto> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        var worker = await _context.Workers
            .Include(w => w.Role)    
            .Include(w => w.Firm)  
            .FirstOrDefaultAsync(w => w.Email == email, cancellationToken);

        if (worker == null)
        {
            throw new UnauthorizedAccessException("אימייל או סיסמה שגויים");
        }

        if (worker.Isactive != true)
        {
            throw new UnauthorizedAccessException("חשבון המשתמש אינו פעיל");
        }

        bool isPasswordValid = await VerifyPasswordAsync(password, worker.PasswordHash);
        if (!isPasswordValid)
        {
            throw new UnauthorizedAccessException("אימייל או סיסמה שגויים");
        }

        string token = _tokenService.GenerateToken(worker);

        return new LoginResponseDto
        {
            Token = token,
            TokenType = "Bearer",
            ExpiresIn = 3600, 
            Worker = new WorkerInfoDto
            {
                Id = worker.Id,
                EmployeeId = worker.Employeeid ?? "",
                Firstname = worker.Firstname,
                Lastname = worker.Lastname,
                Email = worker.Email,
                RoleName = worker.Role.Name,
                FirmId = worker.Firmid
            }
        };
    }

    /// <summary>
    /// יוצר Hash מוצפן מסיסמה
    /// </summary>
    public async Task<string> HashPasswordAsync(string password)
    {
        return await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(password));
    }

    /// <summary>
    /// בודק אם סיסמה תואמת ל-Hash
    /// </summary>
    public async Task<bool> VerifyPasswordAsync(string password, string passwordHash)
    {
        return await Task.Run(() => BCrypt.Net.BCrypt.Verify(password, passwordHash));
    }
}