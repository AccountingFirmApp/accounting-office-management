using AccountingSystem.Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace AccountingSystem.Application.Queries.Tasks
{
    /// <summary>
    /// Query לקבלת כל המשימות של חברה
    /// </summary>
    public class GetTasksByCompanyIdQuery : IRequest<List<TaskDto>>
    {
        public int CompanyId { get; set; }

        public GetTasksByCompanyIdQuery(int companyId)
        {
            CompanyId = companyId;
        }
    }
}
