
using AccountingSystem.Application.DTOs;
using AccountingSystem.Application.DTOs.Tasks;
using AccountingSystem.Domain.Enums;
using MediatR;
using System.Collections.Generic;

namespace AccountingSystem.Application.Queries.Tasks
{

    public class GetTasksByCompanyIdQuery : IRequest<List<CompanyTaskDto>>
    {
        public int CompanyId { get; }

        public GetTasksByCompanyIdQuery(int companyId)
        {
            CompanyId = companyId;
        }
    }
    // ==========================================
    // QUERY: קבלת משימה לפי ID
    // ==========================================

    public class GetTaskByIdQuery : IRequest<CompanyTaskDetailDto?>
    {
        public int TaskId { get; }

        public GetTaskByIdQuery(int taskId)
        {
            TaskId = taskId;
        }
    }

    // ==========================================
    // QUERY: קבלת משימות של חברה
    // ==========================================

    public class GetCompanyTasksQuery : IRequest<List<CompanyTaskDto>>
    {
        public int CompanyId { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public TaskStatus1? Status { get; set; }
    }

    // ==========================================
    // QUERY: קבלת משימות של עובד
    // ==========================================

    public class GetWorkerTasksQuery : IRequest<List<CompanyTaskDto>>
    {
        public int WorkerId { get; set; }
        public TaskStatus1? Status { get; set; }
    }

    // ==========================================
    // QUERY: קבלת משימות באיחור
    // ==========================================

    public class GetOverdueTasksQuery : IRequest<List<ActiveCompanyTaskDto>>
    {
    }

    // ==========================================
    // QUERY: קבלת משימות לפי סטטוס
    // ==========================================

    public class GetTasksByStatusQuery : IRequest<List<ActiveCompanyTaskDto>>
    {
        public TaskStatus1 Status { get; set; }
    }

    // ==========================================
    // QUERY: קבלת פריטי Checklist
    // ==========================================

    public class GetTaskChecklistItemsQuery : IRequest<List<ChecklistItemDto>>
    {
        public int TaskId { get; set; }
    }
    public record GetChecklistTemplateByTaskTypeQuery(int TaskTypeId) : IRequest<ChecklistTemplateDto>;
}
public record GetTaskTypesQuery() : IRequest<List<TaskTypeDto>>;

public class GetTaskMatrixQuery : IRequest<List<CompanyTaskConfigDto>>;