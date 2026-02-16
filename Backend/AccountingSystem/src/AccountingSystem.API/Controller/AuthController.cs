using AccountingSystem.Application.Commands.Workers;
using AccountingSystem.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;


namespace AccountingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _logger = logger;
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
    public async System.Threading.Tasks.Task<ActionResult<LoginResponseDto>> GoogleLogin([FromBody] GoogleLoginRequestDto request)
    {
        try
        {
            var command = new GoogleLoginCommand
            {
                GoogleToken = request.GoogleToken
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
            _logger.LogInformation($"💥 Google login error: {ex.Message}");
            return StatusCode(500, new { message = "שגיאה פנימית בשרת", detail = ex.Message });
        }
    }
    /// <summary>
    /// בדיקת תקינות Token
    /// </summary>
    [HttpGet("validate")]
    [Authorize]
    public IActionResult ValidateToken()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new
        {
            isValid = true,
            userId,
            email,
            role
        });
    }
}