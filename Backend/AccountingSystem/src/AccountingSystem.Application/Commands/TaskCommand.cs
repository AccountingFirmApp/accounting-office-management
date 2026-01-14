using MediatR;

namespace AccountingSystem.Application.Commands.Tasks;

public class UpdateTaskStatusCommand : IRequest<Unit>
{
    public int TaskId { get; set; }
    public string Status { get; set; }
}