using AccountingSystem.Application.DTOs;
using AccountingSystem.Application.Intrefaces;
using AccountingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;

namespace AccountingSystem.Infrastructure.Services;

/// <summary>
/// השירות שמטפל באימות משתמשים
/// </summary>
public class AuthenticationService : IAuthenticationService
{
    private readonly AccountingDbContext _context;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;

    public AuthenticationService(AccountingDbContext context, ITokenService tokenService, IConfiguration configuration)
    {
        _context = context;
        _tokenService = tokenService;
        _configuration = configuration;
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
    public async Task<LoginResponseDto> GoogleLoginAsync(
    string googleToken,
    CancellationToken cancellationToken = default)
    {
        try
        {
            // 1️⃣ אימות Google Token
            var payload = await GoogleJsonWebSignature.ValidateAsync(googleToken, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _configuration["Authentication:Google:ClientId"] }
            });

            if (payload == null)
            {
                throw new UnauthorizedAccessException("Google token לא תקין");
            }

            Console.WriteLine($"✅ Google user validated: {payload.Email}");

            // 2️⃣ חיפוש משתמש קיים
            var worker = await _context.Workers
                .Include(w => w.Role)
                .Include(w => w.Firm)
                .FirstOrDefaultAsync(
                    w => w.Email == payload.Email || w.GoogleId == payload.Subject,
                    cancellationToken);

            if (worker == null)
            {
                // אפשרות 1: שגיאה - משתמש לא קיים
                throw new UnauthorizedAccessException(
                    "משתמש לא נמצא במערכת. אנא פנה למנהל המערכת להוספת המשתמש.");

                // אפשרות 2: יצירה אוטומטית (לא מומלץ - סכנת אבטחה!)
                // worker = await CreateWorkerFromGoogleAsync(payload, cancellationToken);
            }

            // 3️⃣ בדיקה שהחשבון פעיל
            if (worker.Isactive != true)
            {
                throw new UnauthorizedAccessException("חשבון המשתמש אינו פעיל");
            }

            // 4️⃣ עדכון Google ID אם חסר
            if (string.IsNullOrEmpty(worker.GoogleId))
            {
                worker.GoogleId = payload.Subject;
                worker.AuthProvider = "Google";
                worker.Updatedat = DateTime.Now;
                await _context.SaveChangesAsync(cancellationToken);

                Console.WriteLine($"📝 Updated worker {worker.Email} with Google ID");
            }

            // 5️⃣ יצירת Token והחזרת תשובה
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
        catch (InvalidJwtException ex)
        {
            Console.WriteLine($"❌ Invalid Google token: {ex.Message}");
            throw new UnauthorizedAccessException("Google token לא תקין");
        }
    }
}