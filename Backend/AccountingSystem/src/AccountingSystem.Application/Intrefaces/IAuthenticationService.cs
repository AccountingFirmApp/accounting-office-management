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
        System.Threading.Tasks.Task<LoginResponseDto> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
        System.Threading.Tasks.Task<LoginResponseDto> GoogleLoginAsync(string googleToken, CancellationToken cancellationToken = default);

        //System.Threading.Tasks.Task<string> HashPasswordAsync(string password);
        //System.Threading.Tasks.Task<bool> VerifyPasswordAsync(string password, string passwordHash);
        Task<string> HashPasswordAsync(string password);
        Task<bool> VerifyPasswordAsync(string password, string passwordHash);
    }
}