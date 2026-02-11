using AccountingSystem.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Application.Commands
{
    public class GenerateAutoReportInstanceCommand:IRequest<List<ReportInstanceDto>>
    {
        public GenerateAutoReportInstanceCommand(DateTime date)
        {
            this.date = date;
        }

        public DateTime date { get; set; }
    }
}
