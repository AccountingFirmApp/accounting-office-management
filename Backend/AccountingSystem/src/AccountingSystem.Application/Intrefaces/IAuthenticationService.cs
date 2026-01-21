using AccountingSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Application.Intrefaces
{
    public interface IAuthenticationService
    {
        AccountingSystem.Domain.Entities.Task<LoginResponseDto> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
        AccountingSystem.Domain.Entities.Task<LoginResponseDto> GoogleLoginAsync(string googleToken, CancellationToken cancellationToken = default);

        AccountingSystem.Domain.Entities.Task<string> HashPasswordAsync(string password);
        AccountingSystem.Domain.Entities.Task<bool> VerifyPasswordAsync(string password, string passwordHash);
    }
}