using AccountingSystem.Application.Commands.Tasks;
using AccountingSystem.Domain.Enums;
using AccountingSystem.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using AccountingSystem.Application.DTOs;

using AccountingSystem.Application.Queries.Tasks;
using Microsoft.EntityFrameworkCore;
using AccountingSystem.Domain.Interfaces.Repositories;

namespace AccountingSystem.Application.Handlers.Tasks;

public class GetTasksByCompanyIdQueryHandler
    : IRequestHandler<GetTasksByCompanyIdQuery, List<CompanyTaskDto>>
{
    private readonly ICompanyTaskRepository _companyTaskRepository;
    public GetTasksByCompanyIdQueryHandler(ICompanyTaskRepository companyTaskRepository)
    {
        _companyTaskRepository = companyTaskRepository;
    }
    public async Task<List<CompanyTaskDto>> Handle(
    GetTasksByCompanyIdQuery request,
    CancellationToken cancellationToken)
    {
        var tasks = await _companyTaskRepository
            .GetTasksByCompanyIdAsync(request.CompanyId);

        return tasks.Select(t => new CompanyTaskDto
        {
            Id = t.Id,
            CompanyId = t.Companyid,
            TaskTypeId = t.Tasktypeid,
            Period = t.Period,
            DueDate = t.Duedate,
            CompletedDate = t.Completeddate,
            AssignedWorkerId = t.Assignedworkerid,
            Notes = t.Notes ?? string.Empty,
            Status = t.Status.ToString() ?? string.Empty,
            TaskTypeName = t.Tasktype.Name,
            AssignedWorkerName = t.Assignedworker != null
                ? t.Assignedworker.Firstname + " " + t.Assignedworker.Lastname
                : null
        }).ToList();
    }
}

public class UpdateTaskStatusCommandHandler : IRequestHandler<UpdateTaskStatusCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateTaskStatusCommandHandler> _logger;

    public UpdateTaskStatusCommandHandler(IUnitOfWork unitOfWork
       , ILogger<UpdateTaskStatusCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<Unit> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"🎯 Handler נקרא! TaskId={request.TaskId}, Status={request.Status}");

            var taskExists = await _unitOfWork.CompanyTasks.ExistsAsync(request.TaskId);
            if (!taskExists)
            {
                throw new Exception($"משימה עם ID {request.TaskId} לא נמצאה");
            }

            var validStatuses = new[] { "Pending", "InProgress", "Done", "Paid", "NotRequired" };
            if (!validStatuses.Contains(request.Status, StringComparer.OrdinalIgnoreCase))
            {
                throw new Exception($"סטטוס לא חוקי: {request.Status}");
            }

            _logger.LogInformation($"✅ סטטוס תקין: {request.Status}");

            var rowsAffected = await _unitOfWork.UpdateTaskStatusAsync(
                request.TaskId,
                request.Status,
                cancellationToken
            );

            if (rowsAffected == 0)
            {
                throw new Exception($"לא הצלחנו לעדכן את המשימה {request.TaskId}");
            }

            _logger.LogInformation($"✅ שינויים נשמרו בהצלחה!");
            return Unit.Value;
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"❌ שגיאה: {ex.Message}");
            throw;
        }
    }
}
