using AccountingSystem.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Application.Commands
{
    public class CreateCompanyConfigReportCommand:IRequest<CompanyReportConfigDto>
    {
        public int CompanyId { get; set; }
        public int ReportTypeId { get; set; }
        public int FrequencyId { get; set; }
        public short? DayOfMonth { get; set; }
        public int Year { get; set; }
    }
    public class DeleteCompanyConfigReportCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
