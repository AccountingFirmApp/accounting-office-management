using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Application.Intrefaces
{
    public interface ITokenService
    {
        string GenerateToken(Worker worker);

    }
}
