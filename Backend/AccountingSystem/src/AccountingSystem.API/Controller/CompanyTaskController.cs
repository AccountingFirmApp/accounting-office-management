using AccountingSystem.Application.Commands.Tasks;
using AccountingSystem.Application.DTOs.Tasks;
using AccountingSystem.Application.Queries.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccountingSystem.API.Controller
{

    [ApiController]
    [Route("api/[controller]")]
    public class CompanyTaskController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompanyTaskController(IMediator mediator)
        {
            _mediator = mediator;
        }


     

        /// <summary>
        /// קבלת פרטי משימה ספציפית כולל הצ'קליסט
        /// GET: api/tasks/5
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyTaskDetailDto>> GetTaskById(int id)
        {
            try
            {
                var query = new GetTaskByIdQuery(id);
                var result = await _mediator.Send(query);

                if (result == null)
                    return NotFound(new { message = $"משימה עם מזהה {id} לא נמצאה" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// סימון פריט בצ'קליסט כהושלם
        /// PATCH: api/tasks/checklist-item/5/complete
        /// </summary>
        [HttpPatch("checklist-item/{itemId}/complete")]
        public async Task<ActionResult<CompleteChecklistItemResult>> CompleteChecklistItem(int itemId, [FromBody] CompleteItemRequest request)
        {
            try
            {
                var command = new CompleteChecklistItemCommand
                {
                    ItemId = itemId,
                    CompletedByWorkerId = request.WorkerId,
                    Notes = request.Notes
                };

                var result = await _mediator.Send(command);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// ביטול סימון פריט בצ'קליסט (החזרה ללא בוצע)
        /// PATCH: api/tasks/checklist-item/5/uncomplete
        /// </summary>
        [HttpPatch("checklist-item/{itemId}/uncomplete")]
        public async Task<ActionResult> UncompleteChecklistItem(int itemId)
        {
            try
            {
                var command = new UncompleteChecklistItemCommand { ItemId = itemId };
                var result = await _mediator.Send(command);

                if (!result)
                    return BadRequest(new { message = "לא ניתן היה לבטל את סימון הפריט" });

                return Ok(new { message = "הסימון בוטל בהצלחה" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DTO פנימי עבור הבקשה מה-Client
        public class CompleteItemRequest
        {
            public int WorkerId { get; set; }
            public string? Notes { get; set; }
        }
    }
}
    

