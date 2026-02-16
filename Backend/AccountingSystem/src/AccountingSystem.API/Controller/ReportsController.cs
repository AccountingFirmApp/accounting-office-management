//using MediatR;
//using Microsoft.AspNetCore.Mvc;
//using AccountingSystem.Application.Commands;
//using AccountingSystem.Application.Queries;
//using AccountingSystem.Application.DTOs;

//namespace AccountingSystem.API.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class ReportsController : ControllerBase
//    {
//        private readonly IMediator _mediator;

//        public ReportsController(IMediator mediator)
//        {
//            _mediator = mediator;
//        }

//        /// <summary>
//        /// קבלת כל הדיווחים של חברה
//        /// </summary>
//        [HttpGet("company/{companyId}")]
//        public async System.Threading.Tasks.Task<ActionResult<List<ReportInstanceDetailDto>>> GetReportsByCompany(
//            int companyId,
//            [FromQuery] string? status = null,
//            [FromQuery] DateTime? fromPeriod = null,
//            [FromQuery] DateTime? toPeriod = null)
//        {
//            var query = new GetReportsByCompanyIdQuery
//            {
//                CompanyId = companyId,
//                Status = status,
//                FromPeriod = fromPeriod,
//                ToPeriod = toPeriod
//            };

//            var reports = await _mediator.Send(query);
//            return Ok(reports);
//        }

//        /// <summary>
//        /// קבלת דיווח ספציפי לפי ID
//        /// </summary>
//        [HttpGet("{id}")]
//        public async System.Threading.Tasks.Task<ActionResult<ReportInstanceDetailDto>> GetReportById(int id)
//        {
//            var query = new GetReportByIdQuery { ReportId = id };
//            var report = await _mediator.Send(query);

//            if (report == null)
//                return NotFound($"Report with ID {id} not found");

//            return Ok(report);
//        }

//        /// <summary>
//        /// קבלת דיווחים קרובים/באיחור
//        /// </summary>
//        [HttpGet("upcoming")]
//        public async System.Threading.Tasks.Task<ActionResult<List<UpcomingReportDto>>> GetUpcomingReports(
//            [FromQuery] int? companyId = null,
//            [FromQuery] int daysAhead = 30)
//        {
//            var query = new GetUpcomingReportsQuery
//            {
//                CompanyId = companyId,
//                DaysAhead = daysAhead
//            };

//            var reports = await _mediator.Send(query);
//            return Ok(reports);
//        }

//        /// <summary>
//        /// יצירת דיווח חדש
//        /// </summary>
//        [HttpPost]
//        public async System.Threading.Tasks.Task<ActionResult<ReportInstanceDto>> CreateReport(
//            [FromBody] CreateReportInstanceDto dto)
//        {
//            var command = new CreateReportInstanceCommand
//            {
//                ConfigId = dto.ConfigId,
//                Period = dto.Period,
//                Amount = dto.Amount,
//                PaymentMethod = dto.PaymentMethod,
//                ReceiptDate = dto.ReceiptDate,
//                Comments = dto.Comments
//            };

//            var report = await _mediator.Send(command);
//            return CreatedAtAction(nameof(GetReportById), new { id = report.Id }, report);
//        }

//        /// <summary>
//        /// עדכון סטטוס דיווח
//        /// </summary>
//        [HttpPut("status")]
//        public async System.Threading.Tasks.Task<ActionResult> UpdateReportStatus(
//            [FromBody] UpdateReportStatusDto dto)
//        {
//            var command = new UpdateReportStatusCommand
//            {
//                Id = dto.Id,
//                Status = dto.Status
//            };

//            var success = await _mediator.Send(command);

//            if (!success)
//                return NotFound($"Report with ID {dto.Id} not found");

//            return Ok(new { message = "Status updated successfully" });
//        }

//        /// <summary>
//        /// עדכון תשלום דיווח
//        /// </summary>
//        [HttpPut("payment")]
//        public async System.Threading.Tasks.Task<ActionResult> UpdateReportPayment(
//            [FromBody] UpdateReportPaymentDto dto)
//        {
//            var command = new UpdateReportPaymentCommand
//            {
//                Id = dto.Id,
//                Amount = dto.Amount,
//                PaymentMethod = dto.PaymentMethod,
//                PaidDate = dto.PaidDate
//            };

//            var success = await _mediator.Send(command);

//            if (!success)
//                return NotFound($"Report with ID {dto.Id} not found");

//            return Ok(new { message = "Payment updated successfully" });
//        }

//        /// <summary>
//        /// עדכון מלא של דיווח
//        /// </summary>
//        [HttpPut("{id}")]
//        public async System.Threading.Tasks.Task<ActionResult> UpdateReport(
//            int id,
//            [FromBody] UpdateReportInstanceDto dto)
//        {
//            if (id != dto.Id)
//                return BadRequest("ID mismatch");

//            var command = new UpdateReportInstanceCommand
//            {
//                Id = dto.Id,
//                Amount = dto.Amount,
//                Status = dto.Status,
//                PaymentMethod = dto.PaymentMethod,
//                ReceiptDate = dto.ReceiptDate,
//                ReportedDate = dto.ReportedDate,
//                PaidDate = dto.PaidDate,
//                Comments = dto.Comments
//            };

//            var success = await _mediator.Send(command);

//            if (!success)
//                return NotFound($"Report with ID {id} not found");

//            return Ok(new { message = "Report updated successfully" });
//        }
//    }
//}



using MediatR;
using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Application.Commands;
using AccountingSystem.Application.Queries;
using AccountingSystem.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountingSystem.Domain.Entities;


namespace AccountingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ========== פונקציות קיימות ==========

        /// <summary>
        /// קבלת כל הדיווחים של חברה
        /// </summary>
        [HttpGet("company/{companyId}")]
        public async System.Threading.Tasks.Task<ActionResult<List<ReportInstanceDetailDto>>> GetReportsByCompany(
            int companyId,
            [FromQuery] string? status = null,
            [FromQuery] DateTime? fromPeriod = null,
            [FromQuery] DateTime? toPeriod = null)
        {
            var query = new GetReportsByCompanyIdQuery
            {
                CompanyId = companyId,
                Status = status,
                FromPeriod = fromPeriod,
                ToPeriod = toPeriod
            };

            var reports = await _mediator.Send(query);
            return Ok(reports);
        }

        /// <summary>
        /// קבלת דיווח ספציפי לפי ID
        /// </summary>
        [HttpGet("{id}")]
        public async System.Threading.Tasks.Task<ActionResult<ReportInstanceDetailDto>> GetReportById(int id)
        {
            var query = new GetReportByIdQuery { ReportId = id };
            var report = await _mediator.Send(query);

            if (report == null)
                return NotFound($"Report with ID {id} not found");

            return Ok(report);
        }

        /// <summary>
        /// קבלת דיווחים קרובים/באיחור
        /// </summary>
        [HttpGet("upcoming")]
        public async System.Threading.Tasks.Task<ActionResult<List<UpcomingReportDto>>> GetUpcomingReports(
            [FromQuery] int? companyId = null,
            [FromQuery] int daysAhead = 30)
        {
            var query = new GetUpcomingReportsQuery
            {
                CompanyId = companyId,
                DaysAhead = daysAhead
            };

            var reports = await _mediator.Send(query);
            return Ok(reports);
        }

        /// <summary>
        /// יצירת דיווח חדש
        /// </summary>
        //[HttpPost]
        //public async System.Threading.Tasks.Task<ActionResult<ReportInstanceDto>> CreateReport(
        //    [FromBody] CreateReportInstanceDto dto)
        //{
        //    var command = new CreateReportInstanceCommand
        //    {
        //        ConfigId = dto.ConfigId,
        //        Period = dto.Period,
        //        Amount = dto.Amount,
        //        PaymentMethod = dto.PaymentMethod,
        //        ReceiptDate = dto.ReceiptDate,
        //        Comments = dto.Comments
        //    };

        //    var report = await _mediator.Send(command);
        //    return CreatedAtAction(nameof(GetReportById), new { id = report.Id }, report);
        //}




        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult<ReportInstanceDto>> CreateReport(
    [FromBody] CreateReportInstanceDto dto)
        {
            var command = new CreateReportInstanceCommand
            {
                CompanyId = dto.CompanyId,           // 🆕
                ReportTypeId = dto.ReportTypeId,     // 🆕
                FrequencyId = dto.FrequencyId,       // 🆕
                Period = dto.Period,
                Amount = dto.Amount,
                PaymentMethod = dto.PaymentMethod,
                ReceiptDate = dto.ReceiptDate,
                Comments = dto.Comments
            };

            var report = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetReportById), new { id = report.Id }, report);
        }
        /// <summary>
        /// עדכון סטטוס דיווח
        /// </summary>
        [HttpPut("status")]
        public async System.Threading.Tasks.Task<ActionResult> UpdateReportStatus(
            [FromBody] UpdateReportStatusDto dto)
        {
            var command = new UpdateReportStatusCommand
            {
                Id = dto.Id,
                Status = dto.Status
            };
            _logger.LogInformation($"Status before save: '{dto.Status}'");
            _logger.LogInformation($"Status length: {dto.Status.ToString().Length}");
            var success = await _mediator.Send(command);

            if (!success)
                return NotFound($"Report with ID {dto.Id} not found");

            return Ok(new { message = "Status updated successfully" });
        }

        /// <summary>
        /// עדכון תשלום דיווח
        /// </summary>
        [HttpPut("payment")]
        public async System.Threading.Tasks.Task<ActionResult> UpdateReportPayment(
            [FromBody] UpdateReportPaymentDto dto)
        {
            var command = new UpdateReportPaymentCommand
            {
                Id = dto.Id,
                Amount = dto.Amount,
                PaymentMethod = dto.PaymentMethod,
                PaidDate = dto.PaidDate
            };

            var success = await _mediator.Send(command);

            if (!success)
                return NotFound($"Report with ID {dto.Id} not found");

            return Ok(new { message = "Payment updated successfully" });
        }

        /// <summary>
        /// עדכון מלא של דיווח
        /// </summary>
        [HttpPut("{id}")]
        public async System.Threading.Tasks.Task<ActionResult> UpdateReport(
            int id,
            [FromBody] UpdateReportInstanceDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch");

            var command = new UpdateReportInstanceCommand
            {
                Id = dto.Id,
                Amount = dto.Amount,
                Status = dto.Status,
                PaymentMethod = dto.PaymentMethod,
                ReceiptDate = dto.ReceiptDate,
                ReportedDate = dto.ReportedDate,
                PaidDate = dto.PaidDate,
                Comments = dto.Comments
            };

            var success = await _mediator.Send(command);

            if (!success)
                return NotFound($"Report with ID {id} not found");

            return Ok(new { message = "Report updated successfully" });
        }

        // ========== פונקציות חדשות מה-Repository ==========

        /// <summary>
        /// קבלת כל הדיווחים במערכת
        /// GET: api/reports/all
        /// </summary>
        //[HttpGet("all")]
        //public async System.Threading.Tasks.Task<ActionResult<List<ReportInstanceDetailDto>>> GetAllReports()
        //{
        //    var query = new GetAllReportsQuery();
        //    var reports = await _mediator.Send(query);
        //    return Ok(reports);
        //}

        //[HttpGet("all")]
        //public async System.Threading.Tasks.Task<ActionResult<List<ReportInstanceDetailDto>>> GetAllReports()
        //{
        //    try
        //    {
        //        // 🔥 מחלץ את ה-WorkerId מה-Token
        //        var workerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //        if (string.IsNullOrEmpty(workerIdClaim) || !int.TryParse(workerIdClaim, out int workerId))
        //        {
        //            return Unauthorized(new { message = "משתמש לא מזוהה" });
        //        }

        //        // שליחת Query עם WorkerId
        //        var query = new GetAllReportsQuery { WorkerId = workerId };
        //        var reports = await _mediator.Send(query);

        //        return Ok(reports);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { message = "שגיאה בטעינת הדוחות", detail = ex.Message });
        //    }
        //}

        //    [HttpGet("all")]
        //    public async System.Threading.Tasks.Task<ActionResult<List<ReportInstanceDetailDto>>> GetAllReports(
        //[FromQuery] bool isAdminMode = false) // 🆕 פרמטר חדש
        //    {
        //        try
        //        {
        //            // 🔥 מחלץ את ה-WorkerId וה-Role מה-Token
        //            var workerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

        //            if (string.IsNullOrEmpty(workerIdClaim) || !int.TryParse(workerIdClaim, out int workerId))
        //            {
        //                return Unauthorized(new { message = "משתמש לא מזוהה" });
        //            }

        //            // 🔥 אם זה מנהל במצב ניהול - לא שולחים WorkerId
        //            int? filterByWorkerId = null;

        //            if (isAdminMode && roleClaim == "Admin")
        //            {
        //                // מנהל במצב ניהול - רואה הכל
        //                filterByWorkerId = null;
        //            }
        //            else
        //            {
        //                // כל המשתמשים האחרים (כולל מנהל שלא במצב ניהול) - רואים רק את שלהם
        //                filterByWorkerId = workerId;
        //            }

        //            var query = new GetAllReportsQuery
        //            {
        //                WorkerId = filterByWorkerId,
        //                IsAdminMode = isAdminMode
        //            };

        //            var reports = await _mediator.Send(query);
        //            return Ok(reports);
        //        }
        //        catch (Exception ex)
        //        {
        //            return StatusCode(500, new { message = "שגיאה בטעינת הדוחות", detail = ex.Message });
        //        }
        //    }


        [HttpGet("all")]
        public async System.Threading.Tasks.Task<ActionResult<List<ReportInstanceDetailDto>>> GetAllReports(
    [FromQuery] bool isAdminMode = false)
        {
            try
            {
                _logger.LogInformation($"🔍 GetAllReports נקרא עם isAdminMode={isAdminMode}");

                var workerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

                _logger.LogInformation($"🔍 WorkerId={workerIdClaim}, Role={roleClaim}");

                if (string.IsNullOrEmpty(workerIdClaim) || !int.TryParse(workerIdClaim, out int workerId))
                {
                    return Unauthorized(new { message = "משתמש לא מזוהה" });
                }

                int? filterByWorkerId = null;

                if (isAdminMode && roleClaim == "Admin")
                {
                    _logger.LogInformation("✅ מנהל במצב ניהול - מחזיר הכל");
                    filterByWorkerId = null;
                }
                else
                {
                    _logger.LogInformation($"✅ מצב רגיל - מסנן לעובד {workerId}");
                    filterByWorkerId = workerId;
                }

                var query = new GetAllReportsQuery
                {
                    WorkerId = filterByWorkerId,
                    IsAdminMode = isAdminMode
                };

                var reports = await _mediator.Send(query);

                _logger.LogInformation($"✅ מחזיר {reports.Count} דוחות");

                return Ok(reports);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"❌ שגיאה: {ex.Message}");
                return StatusCode(500, new { message = "שגיאה בטעינת הדוחות", detail = ex.Message });
            }
        }




        /// <summary>
        /// קבלת דיווחים לפי Config ID
        /// GET: api/reports/config/5
        /// </summary>
        [HttpGet("config/{configId}")]
        public async System.Threading.Tasks.Task<ActionResult<List<ReportInstanceDetailDto>>> GetReportsByConfig(int configId)
        {
            var query = new GetReportsByConfigIdQuery { ConfigId = configId };
            var reports = await _mediator.Send(query);
            return Ok(reports);
        }

        /// <summary>
        /// קבלת דיווחים לפי סטטוס
        /// GET: api/reports/status/Pending
        /// </summary>
        [HttpGet("status/{status}")]
        public async System.Threading.Tasks.Task<ActionResult<List<ReportInstanceDetailDto>>> GetReportsByStatus(string status)
        {
            var query = new GetReportsByStatusQuery { Status = status };
            var reports = await _mediator.Send(query);
            return Ok(reports);
        }

        /// <summary>
        /// קבלת דיווחים ממתינים (Pending)
        /// GET: api/reports/pending
        /// </summary>
        [HttpGet("pending")]
        public async System.Threading.Tasks.Task<ActionResult<List<ReportInstanceDetailDto>>> GetPendingReports()
        {
            var query = new GetPendingReportsQuery();
            var reports = await _mediator.Send(query);
            return Ok(reports);
        }

        /// <summary>
        /// קבלת דיווחים לפי תקופה (חודש/שנה)
        /// GET: api/reports/period?year=2024&month=1
        /// </summary>
        [HttpGet("period")]
        public async System.Threading.Tasks.Task<ActionResult<List<ReportInstanceDetailDto>>> GetReportsByPeriod(
            [FromQuery] int year,
            [FromQuery] int month)
        {
            var period = new DateTime(year, month, 1);
            var query = new GetReportsByPeriodQuery { Period = period };
            var reports = await _mediator.Send(query);
            return Ok(reports);
        }

        /// <summary>
        /// קבלת דיווחים בטווח תאריכים
        /// GET: api/reports/daterange?startDate=2024-01-01&endDate=2024-12-31
        /// </summary>
        [HttpGet("daterange")]
        public async System.Threading.Tasks.Task<ActionResult<List<ReportInstanceDetailDto>>> GetReportsByDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var query = new GetReportsByDateRangeQuery
            {
                StartDate = startDate,
                EndDate = endDate
            };
            var reports = await _mediator.Send(query);
            return Ok(reports);
        }

        /// <summary>
        /// קבלת דיווחים באיחור (OVERDUE) - קריטי!
        /// GET: api/reports/overdue
        /// </summary>
        [HttpGet("overdue")]
        public async System.Threading.Tasks.Task<ActionResult<List<ReportInstanceDetailDto>>> GetOverdueReports()
        {
            var query = new GetOverdueReportsQuery();
            var reports = await _mediator.Send(query);
            return Ok(reports);
        }

        /// <summary>
        /// קבלת דיווחים שצריך להגיש בעוד X ימים
        /// GET: api/reports/due?days=7
        /// דוגמה: דיווחים לשבוע הקרוב
        /// </summary>
        [HttpGet("due")]
        public async System.Threading.Tasks.Task<ActionResult<List<ReportInstanceDetailDto>>> GetReportsDueInDays(
            [FromQuery] int days = 7)
        {
            var query = new GetReportsDueInNextDaysQuery { Days = days };
            var reports = await _mediator.Send(query);
            return Ok(reports);
        }
    }
}