using MediatR;
using AccountingSystem.Domain.Entities;

namespace AccountingSystem.Application.Queries;

public class GetWorkerTasksQuery : IRequest<IEnumerable<CompanyTask>>
{
    public int WorkerId { get; set; }
}
