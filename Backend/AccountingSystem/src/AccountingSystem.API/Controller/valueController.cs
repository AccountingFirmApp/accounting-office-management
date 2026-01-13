using AccountingSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountingSystem.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MigrationController : ControllerBase
    {
        private readonly AccountingDbContext _context;

        public MigrationController(AccountingDbContext context)
        {
            _context = context;
        }

        [HttpPost("fix-passwords")]
        public async Task<IActionResult> FixPasswordsAsync()
        {
            var workersWithoutPassword = await _context.Workers
                .Where(w => string.IsNullOrEmpty(w.PasswordHash) && string.IsNullOrEmpty(w.GoogleId))
                .ToListAsync();

            if (!workersWithoutPassword.Any())
            {
                return Ok("No workers need password updates");
            }

            foreach (var worker in workersWithoutPassword)
            {
                worker.PasswordHash = BCrypt.Net.BCrypt.HashPassword("ChangeMe123!");
                worker.AuthProvider = "Local";
                worker.Updatedat = DateTime.Now;
            }

            await _context.SaveChangesAsync();

            return Ok($"Updated {workersWithoutPassword.Count} workers with default passwords");
        }
    }
}
