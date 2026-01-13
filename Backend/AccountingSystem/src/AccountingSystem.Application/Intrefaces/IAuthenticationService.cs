//using AccountingSystem.Application.DTOs;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace AccountingSystem.Application.Intrefaces
//{
//    public interface IAuthenticationService
//    {
//        Task<LoginResponseDto> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
//        Task<LoginResponseDto> GoogleLoginAsync(string googleToken, CancellationToken cancellationToken = default);

//        Task<string> HashPasswordAsync(string password);
//        Task<bool> VerifyPasswordAsync(string password, string passwordHash);
//    }
//}
