using AccountingSystem.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Application.Commands
{
    public class UpdateCompanyConfigReportCommand : IRequest<CompanyReportConfigDto>
    {
        public int Id { get; set; }
        public int? FrequencyId { get; set; }
        public int? DayOfMonth { get; set; }
        public bool? Isactive { get; set; }
    }
}
