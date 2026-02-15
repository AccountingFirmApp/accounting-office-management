using AccountingSystem.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Application.Queries.AccountingFirmQuery
{
  

        // ========================================
        // GET ALL COMPANIES
        // ========================================
        public class GetAllAccountingFirmQuery : IRequest<List<AccountingFirmDto>>
        {
        }
    public record GetAccountingFirmByIdQuery(int Id) : IRequest<AccountingFirmDto>;

}

