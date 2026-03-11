using AccountingSystem.Application.Commands.Workers;
using AccountingSystem.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;
using Google.Apis.Auth;


namespace AccountingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;
    private readonly string _googleClientId;


    public AuthController(IMediator mediator, ILogger<AuthController> logger, IConfiguration configuration)
    {
        _mediator = mediator;
        _logger = logger;
        _googleClientId = configuration["Authentication:Google:ClientId"];

    }

    /// <summary>
    /// התחברות למערכת
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async System.Threading.Tasks.Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        try
        {
            var command = new LoginCommand
            {
                Email = request.Email,
                Password = request.Password
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "שגיאה פנימית בשרת", detail = ex.Message });
        }
    }
    /// <summary>
    /// התחברות עם Google
    /// </summary>
    [HttpPost("login-google")]
    [AllowAnonymous]
public async Task<ActionResult<LoginResponseDto>> GoogleLogin([FromBody] GoogleLoginRequestDto request)
{
    try
    {
        //  אימות הטוקן מול Google
        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = new[] { _googleClientId } 
        };

        GoogleJsonWebSignature.Payload payload;
        try
        {
            payload = await GoogleJsonWebSignature.ValidateAsync(request.GoogleToken, settings);
        }
        catch (InvalidJwtException)
        {
            _logger.LogInformation("❌ Google login failed: invalid token");
            return Unauthorized(new { message = "Google token לא תקין" });
        }

        // אופציונלי: בדיקה אם המייל מאומת
        if (!payload.EmailVerified)
        {
            _logger.LogInformation($"❌ Google login failed: email not verified ({payload.Email})");
            return Unauthorized(new { message = "Email לא מאומת ב-Google" });
        }

        // 2️⃣ אם הכל תקין, יוצרים את ה-Command ל-MediatR
        var command = new GoogleLoginCommand
        {
            GoogleToken = request.GoogleToken,
        };

        var result = await _mediator.Send(command);

        _logger.LogInformation($"✅ Google login successful for: {result.Worker.Email}");

        return Ok(result);
    }
    catch (UnauthorizedAccessException ex)
    {
        _logger.LogInformation($"❌ Google login failed: {ex.Message}");
        return Unauthorized(new { message = ex.Message });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "💥 Google login error");
        return StatusCode(500, new { message = "שגיאה פנימית בשרת" });
    }
}

}