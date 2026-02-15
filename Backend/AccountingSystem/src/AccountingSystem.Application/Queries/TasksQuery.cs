using AccountingSystem.Application.DTOs;
using MediatR;
namespace AccountingSystem.Application.Queries.Tasks


//    /// <summary>
//    /// Query לקבלת כל המשימות של חברה
{
    public class GetTasksByCompanyIdQuery : IRequest<List<CompanyTaskDto>>
    {
        public int CompanyId { get; }

        public GetTasksByCompanyIdQuery(int companyId)
        {
            CompanyId = companyId;
        }
    }
}
