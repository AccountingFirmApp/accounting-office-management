using AccountingSystem.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountingSystem.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FrequencyController : ControllerBase
    {
      
    }
}
