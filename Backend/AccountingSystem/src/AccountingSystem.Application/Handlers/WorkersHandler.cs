//// Application/Handlers/Workers/WorkersHandler.cs
//using AccountingSystem.Application.DTOs;
////using AccountingSystem.Application;
//using AccountingSystem.Application.Queries.Workers;
//using AccountingSystem.Domain.Entities;
//using AccountingSystem.Domain.Interfaces;
//using AutoMapper;
//using MediatR;
//using AccountingSystem.Application.Commands.Workers;
//using Microsoft.AspNetCore.Authentication;
//using CreateWorkerCommand = AccountingSystem.Application.Commands.Workers.CreateWorkerCommand;
//using UpdateWorkerCommand = AccountingSystem.Application.Commands.Workers.UpdateWorkerCommand;
//using DeleteWorkerCommand = AccountingSystem.Application.Commands.Workers.DeleteWorkerCommand;

//namespace AccountingSystem.Application.Handlers.Workers;

//// ========================================
//// GET ALL WORKERS HANDLER
//// ========================================
//public class GetAllWorkersQueryHandler : IRequestHandler<GetAllWorkersQuery, List<WorkerDto>>
//{
//    private readonly IUnitOfWork _unitOfWork;
//    private readonly IMapper _mapper;

//    public GetAllWorkersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
//    {
//        _unitOfWork = unitOfWork;
//        _mapper = mapper;
//    }

//    public async Task<List<WorkerDto>> Handle(GetAllWorkersQuery request, CancellationToken cancellationToken)
//    {
//        var workers = await _unitOfWork.Workers.GetAllAsync();
//        return _mapper.Map<List<WorkerDto>>(workers);
//    }
//}

//// ========================================
//// GET WORKER BY ID HANDLER
//// ========================================
//public class GetWorkerByIdQueryHandler : IRequestHandler<GetWorkerByIdQuery, WorkerDto>
//{
//    private readonly IUnitOfWork _unitOfWork;
//    private readonly IMapper _mapper;

//    public GetWorkerByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
//    {
//        _unitOfWork = unitOfWork;
//        _mapper = mapper;
//    }

//    public async Task<WorkerDto> Handle(GetWorkerByIdQuery request, CancellationToken cancellationToken)
//    {
//        var worker = await _unitOfWork.Workers.GetByIdAsync(request.Id);

//        if (worker == null)
//        {
//            throw new Exception($"עובד עם ID {request.Id} לא נמצא");
//        }

//        return _mapper.Map<WorkerDto>(worker);
//    }
//}

//// ========================================
//// GET WORKERS BY FIRM ID HANDLER
//// ========================================
//public class GetWorkersByFirmIdQueryHandler : IRequestHandler<GetWorkersByFirmIdQuery, List<WorkerDto>>
//{
//    private readonly IUnitOfWork _unitOfWork;
//    private readonly IMapper _mapper;

//    public GetWorkersByFirmIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
//    {
//        _unitOfWork = unitOfWork;
//        _mapper = mapper;
//    }

//    public async Task<List<WorkerDto>> Handle(GetWorkersByFirmIdQuery request, CancellationToken cancellationToken)
//    {
//        var workers = await _unitOfWork.Workers.GetWorkersByFirmIdAsync(request.FirmId);
//        return _mapper.Map<List<WorkerDto>>(workers);
//    }
//}

//// ========================================
//// CREATE WORKER COMMAND HANDLER
//// ========================================
//public class CreateWorkerCommandHandler : IRequestHandler<CreateWorkerCommand, WorkerDto>
//{
//    private readonly IUnitOfWork _unitOfWork;
//    private readonly IMapper _mapper;

//    public CreateWorkerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
//    {
//        _unitOfWork = unitOfWork;
//        _mapper = mapper;
//    }

//    public async Task<WorkerDto> Handle(CreateWorkerCommand request, CancellationToken cancellationToken)
//    {
//        var worker = new Worker
//        {
//            Firstname = request.Firstname,
//            Lastname = request.Lastname,
//            Firmid = request.Firmid,
//            Email = request.Email,
//            Phone = request.Phone,
//            Isactive = request.Isactive,
//            //Role = request.Role,
//            Createdat = DateTime.UtcNow
//        };

//        await _unitOfWork.Workers.AddAsync(worker);
//        await _unitOfWork.SaveChangesAsync(cancellationToken);

//        return _mapper.Map<WorkerDto>(worker);
//    }
//}

//// ========================================
//// UPDATE WORKER COMMAND HANDLER
//// ========================================
//public class UpdateWorkerCommandHandler : IRequestHandler<UpdateWorkerCommand, WorkerDto>
//{
//    private readonly IUnitOfWork _unitOfWork;
//    private readonly IMapper _mapper;

//    public UpdateWorkerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
//    {
//        _unitOfWork = unitOfWork;
//        _mapper = mapper;
//    }

//    public async Task<WorkerDto> Handle(UpdateWorkerCommand request, CancellationToken cancellationToken)
//    {
//        var worker = await _unitOfWork.Workers.GetByIdAsync(request.Id);

//        if (worker == null)
//        {
//            throw new Exception($"עובד עם ID {request.Id} לא נמצא");
//        }

//        worker.Firstname = request.Firstname;
//        worker.Lastname = request.Lastname;
//        worker.Email = request.Email;
//        worker.Phone = request.Phone;
//        worker.Isactive = request.Isactive;
//        //worker.Role = request.Role;
//        worker.Updatedat = DateTime.UtcNow;

//        await _unitOfWork.Workers.UpdateAsync(worker);
//        await _unitOfWork.SaveChangesAsync(cancellationToken);

//        return _mapper.Map<WorkerDto>(worker);
//    }
//}

//// ========================================
//// DELETE WORKER COMMAND HANDLER
//// ========================================
//public class DeleteWorkerCommandHandler : IRequestHandler<DeleteWorkerCommand, Unit>
//{
//    private readonly IUnitOfWork _unitOfWork;

//    public DeleteWorkerCommandHandler(IUnitOfWork unitOfWork)
//    {
//        _unitOfWork = unitOfWork;
//    }

//    public async Task<Unit> Handle(DeleteWorkerCommand request, CancellationToken cancellationToken)
//    {
//        var worker = await _unitOfWork.Workers.GetByIdAsync(request.Id);

//        if (worker == null)
//        {
//            throw new Exception($"עובד עם ID {request.Id} לא נמצא");
//        }

//        await _unitOfWork.Workers.DeleteAsync(request.Id);
//        await _unitOfWork.SaveChangesAsync(cancellationToken);

//        return Unit.Value;
//    }
//}



////using AccountingSystem.Application.DTOs;
////using AccountingSystem.Application.Queries.Workers;
////using AccountingSystem.Domain.Interfaces;
////using MediatR;

////namespace AccountingSystem.Application.Handlers.Workers;

///// <summary>
///// Handler לקבלת כל החברות של עובדת
///// </summary>
//public class GetWorkerCompaniesHandler : IRequestHandler<GetWorkerCompaniesQuery, List<CompanyDto>>
//{
//    private readonly IUnitOfWork _unitOfWork;

//    public GetWorkerCompaniesHandler(IUnitOfWork unitOfWork)
//    {
//        _unitOfWork = unitOfWork;
//    }

//    public async Task<List<CompanyDto>> Handle(
//        GetWorkerCompaniesQuery request,
//        CancellationToken cancellationToken)
//    {
//        // בדיקה שהעובדת קיימת
//        var workerExists = await _unitOfWork.Workers.ExistsAsync(request.WorkerId);
//        if (!workerExists)
//        {
//            throw new Exception($"עובדת עם ID {request.WorkerId} לא נמצאה");
//        }

//        // קבלת כל השיוכים של העובדת לחברות
//        var companyWorkers = await _unitOfWork.CompanyWorkers.GetByWorkerIdAsync(request.WorkerId);

//        // המרה ל-DTOs
//        var result = new List<CompanyDto>();

//        foreach (var cw in companyWorkers)
//        {
//            result.Add(new CompanyDto
//            {
//                Id = cw.Id,
//                Name = cw.Company.Name,
//                TaxId = cw.Company.Taxid ?? string.Empty,
//                Address = cw.Company.Address ?? string.Empty,
//                Phone = cw.Company.Phone ?? string.Empty,
//                Notes = cw.Company.Notes ?? string.Empty,
//                FirmId = cw.Company.Firmid,
//                Email = cw.Worker.Email,
//                IsActive = cw.Isactive ?? true,
//            });
//        }

//        return result
//            .OrderByDescending(x => x.IsActive)
//            .ThenBy(x => x.Name)
//            .ToList();
//    }
//}
////=================Login========
//public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
//{
//    private readonly AccountingSystem.Application.Intrefaces.IAuthenticationService _authService;

//    public LoginCommandHandler(AccountingSystem.Application.Intrefaces.IAuthenticationService authService)
//    {
//        _authService = authService;
//    }

//    public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
//    {
//        return await _authService.LoginAsync(request.Email, request.Password, cancellationToken);
//    }
//}

//// ========================================
//// GOOGLE LOGIN HANDLER
//// ========================================
//public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommand, LoginResponseDto>
//{
//    private readonly AccountingSystem.Application.Intrefaces.IAuthenticationService _authService;

//    public GoogleLoginCommandHandler(AccountingSystem.Application.Intrefaces.IAuthenticationService authService)
//    {
//        _authService = authService;
//    }

//    public async Task<LoginResponseDto> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
//    {
//        return await _authService.GoogleLoginAsync(request.GoogleToken, cancellationToken);
//    }
//}




// Application/Handlers/Workers/WorkersHandler.cs
using AccountingSystem.Application.DTOs;
//using AccountingSystem.Application;
using AccountingSystem.Application.Queries.Workers;
using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Interfaces;
using AutoMapper;
using MediatR;
using AccountingSystem.Application.Commands.Workers;
using Microsoft.AspNetCore.Authentication;
using CreateWorkerCommand = AccountingSystem.Application.Commands.Workers.CreateWorkerCommand;
using UpdateWorkerCommand = AccountingSystem.Application.Commands.Workers.UpdateWorkerCommand;
using DeleteWorkerCommand = AccountingSystem.Application.Commands.Workers.DeleteWorkerCommand;
//using AccountingSystem.Application.Intrefaces; // ✅ ודאי שיש את זה
using IAuthService = AccountingSystem.Application.Intrefaces.IAuthenticationService;

namespace AccountingSystem.Application.Handlers.Workers;

// ========================================
// GET ALL WORKERS HANDLER
// ========================================
public class GetAllWorkersQueryHandler : IRequestHandler<GetAllWorkersQuery, List<WorkerDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllWorkersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<WorkerDto>> Handle(GetAllWorkersQuery request, CancellationToken cancellationToken)
    {
        var workers = await _unitOfWork.Workers.GetAllAsync();

        var workerDtos = workers.Select(w => new WorkerDto
        {
            Id = w.Id,
            FirstName = w.Firstname,
            LastName = w.Lastname,
            Email = w.Email,
            Employeeid = w.Employeeid,
            RoleId = w.Roleid,
            RoleName = w.Role?.Name ?? "לא הוגדר", // ⭐ הוסף את זה!
            IsActive = w.Isactive ?? true,
            Phone = w.Phone
        }).ToList();

        return workerDtos;
    }
}


// ========================================
// GET WORKER BY ID HANDLER
// ========================================
public class GetWorkerByIdQueryHandler : IRequestHandler<GetWorkerByIdQuery, WorkerDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetWorkerByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<WorkerDto> Handle(GetWorkerByIdQuery request, CancellationToken cancellationToken)
    {
        var worker = await _unitOfWork.Workers.GetByIdAsync(request.Id);

        if (worker == null)
        {
            throw new Exception($"עובד עם ID {request.Id} לא נמצא");
        }

        return _mapper.Map<WorkerDto>(worker);
    }
}

// ========================================
// GET WORKERS BY FIRM ID HANDLER
// ========================================
public class GetWorkersByFirmIdQueryHandler : IRequestHandler<GetWorkersByFirmIdQuery, List<WorkerDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetWorkersByFirmIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<WorkerDto>> Handle(GetWorkersByFirmIdQuery request, CancellationToken cancellationToken)
    {
        var workers = await _unitOfWork.Workers.GetWorkersByFirmIdAsync(request.FirmId);
        return _mapper.Map<List<WorkerDto>>(workers);
    }
}

// ========================================
// CREATE WORKER COMMAND HANDLER
// ========================================
public class CreateWorkerCommandHandler : IRequestHandler<CreateWorkerCommand, WorkerDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAuthService _authService; // ✅ נוסיף את זה


    public CreateWorkerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _authService = authService;


    }

    //public async Task<WorkerDto> Handle(CreateWorkerCommand request, CancellationToken cancellationToken)
    //{
    //    var worker = new Worker
    //    {
    //        Firstname = request.Firstname,
    //        Lastname = request.Lastname,
    //        Firmid = request.Firmid,
    //        Email = request.Email,
    //        Phone = request.Phone,
    //        Isactive = request.Isactive,
    //        //Role = request.Role,
    //        Roleid = request.Roleid,  // ← הוסיפי את השורה הזו!
    //        Employeeid = request.Employeeid,  // ← גם את זה כדאי להוסיף
    //        Createdat = DateTime.UtcNow
    //    };

    //    await _unitOfWork.Workers.AddAsync(worker);
    //    await _unitOfWork.SaveChangesAsync(cancellationToken);

    //    return _mapper.Map<WorkerDto>(worker);
    //}



    public async Task<WorkerDto> Handle(CreateWorkerCommand request, CancellationToken cancellationToken)
    {
        var passwordHash = await _authService.HashPasswordAsync(request.Password);

        // ✅ יצירת העובד
        var worker = new Worker
            {
                Firmid = request.Firmid, // ✅ בא מה-Token!
                Roleid = request.Roleid,
                Firstname = request.Firstname,
                Lastname = request.Lastname,
                Email = request.Email,
                Phone = request.Phone,
                Employeeid = request.Employeeid,
                Isactive = request.Isactive,
                Hiredate = request.Hiredate,
            PasswordHash = passwordHash, // ✅ הוספה!

            Createdat = DateTime.UtcNow
            };

            await _unitOfWork.Workers.AddAsync(worker);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 🔥 אם נבחרו חברות - צור קשרים בטבלת Companyworkers
            if (request.CompanyIds != null && request.CompanyIds.Any())
            {
                foreach (var companyId in request.CompanyIds)
                {
                    var companyWorker = new Companyworker
                    {
                        Companyid = companyId,
                        Workerid = worker.Id, // ✅ ה-Id נוצר אחרי SaveChanges
                        Isactive = true,
                        Assignedat = DateTime.UtcNow
                    };

                    await _unitOfWork.CompanyWorkers.AddAsync(companyWorker); // ✅ נניח שיש לך Repository
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return _mapper.Map<WorkerDto>(worker);
        }



    // ========================================
    // UPDATE WORKER COMMAND HANDLER
    // ========================================
    //public class UpdateWorkerCommandHandler : IRequestHandler<UpdateWorkerCommand, WorkerDto>
    //{
    //    private readonly IUnitOfWork _unitOfWork;
    //    private readonly IMapper _mapper;

    //    public UpdateWorkerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    //    {
    //        _unitOfWork = unitOfWork;
    //        _mapper = mapper;
    //    }

    //    public async Task<WorkerDto> Handle(UpdateWorkerCommand request, CancellationToken cancellationToken)
    //    {
    //        var worker = await _unitOfWork.Workers.GetByIdAsync(request.Id);

    //        if (worker == null)
    //        {
    //            throw new Exception($"עובד עם ID {request.Id} לא נמצא");
    //        }

    //        worker.Firstname = request.Firstname;
    //        worker.Lastname = request.Lastname;
    //        worker.Email = request.Email;
    //        worker.Phone = request.Phone;
    //        worker.Isactive = request.Isactive;
    //        //worker.Role = request.Role;
    //        worker.Updatedat = DateTime.UtcNow;

    //        await _unitOfWork.Workers.UpdateAsync(worker);
    //        await _unitOfWork.SaveChangesAsync(cancellationToken);

    //        return _mapper.Map<WorkerDto>(worker);
    //    }
    //}

    public class UpdateWorkerCommandHandler : IRequestHandler<UpdateWorkerCommand, WorkerDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateWorkerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<WorkerDto> Handle(UpdateWorkerCommand request, CancellationToken cancellationToken)
        {
            var worker = await _unitOfWork.Workers.GetByIdAsync(request.Id);
            if (worker == null)
            {
                throw new Exception($"עובד עם ID {request.Id} לא נמצא");
            }

            // ✅ עדכן רק שדות שנשלחו (לא null)
            if (!string.IsNullOrEmpty(request.Firstname))
                worker.Firstname = request.Firstname;

            if (!string.IsNullOrEmpty(request.Lastname))
                worker.Lastname = request.Lastname;

            if (!string.IsNullOrEmpty(request.Email))
                worker.Email = request.Email;

            if (!string.IsNullOrEmpty(request.Phone))
                worker.Phone = request.Phone;

            if (request.Roleid.HasValue)
                worker.Roleid = request.Roleid.Value;

            if (!string.IsNullOrEmpty(request.Employeeid))
                worker.Employeeid = request.Employeeid;

            if (request.Hiredate.HasValue)
                worker.Hiredate = request.Hiredate.Value;

            worker.Isactive = request.Isactive;
            worker.Updatedat = DateTime.UtcNow;

            // ✅ טיפול בחברות (many-to-many)
            if (request.CompanyIds != null)
            {
                await UpdateWorkerCompanies(worker.Id, request.CompanyIds, cancellationToken);
            }

            await _unitOfWork.Workers.UpdateAsync(worker);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<WorkerDto>(worker);
        }

        // ✅ פונקציה לעדכון חברות
        private async Task UpdateWorkerCompanies(int workerId, List<int> companyIds, CancellationToken cancellationToken)
        {
            // הסר את כל החברות הקיימות
            await _unitOfWork.CompanyWorkers.DeleteByWorkerIdAsync(workerId);

            // הוסף את החברות החדשות
            foreach (var companyId in companyIds)
            {
                await _unitOfWork.CompanyWorkers.AddAsync(new Companyworker
                {
                    Workerid = workerId,
                    Companyid = companyId
                });
            }
        }
    }


    // ========================================
    // DELETE WORKER COMMAND HANDLER
    // ========================================
    public class DeleteWorkerCommandHandler : IRequestHandler<DeleteWorkerCommand, Unit>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteWorkerCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Unit> Handle(DeleteWorkerCommand request, CancellationToken cancellationToken)
            {
                var worker = await _unitOfWork.Workers.GetByIdAsync(request.Id);

                if (worker == null)
                {
                    throw new Exception($"עובד עם ID {request.Id} לא נמצא");
                }

                await _unitOfWork.Workers.DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }



        //using AccountingSystem.Application.DTOs;
        //using AccountingSystem.Application.Queries.Workers;
        //using AccountingSystem.Domain.Interfaces;
        //using MediatR;

        //namespace AccountingSystem.Application.Handlers.Workers;

        /// <summary>
        /// Handler לקבלת כל החברות של עובדת
        /// </summary>
        public class GetWorkerCompaniesHandler : IRequestHandler<GetWorkerCompaniesQuery, List<CompanyDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetWorkerCompaniesHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<CompanyDto>> Handle(
                GetWorkerCompaniesQuery request,
                CancellationToken cancellationToken)
            {
                // בדיקה שהעובדת קיימת
                var workerExists = await _unitOfWork.Workers.ExistsAsync(request.WorkerId);
                if (!workerExists)
                {
                    throw new Exception($"עובדת עם ID {request.WorkerId} לא נמצאה");
                }

                // קבלת כל השיוכים של העובדת לחברות
                var companyWorkers = await _unitOfWork.CompanyWorkers.GetByWorkerIdAsync(request.WorkerId);

                // המרה ל-DTOs
                var result = new List<CompanyDto>();

                foreach (var cw in companyWorkers)
                {
                    result.Add(new CompanyDto
                    {
                        Id = cw.Companyid,
                        Name = cw.Company.Name,
                        TaxId = cw.Company.Taxid ?? string.Empty,
                        Address = cw.Company.Address ?? string.Empty,
                        Phone = cw.Company.Phone ?? string.Empty,
                        Notes = cw.Company.Notes ?? string.Empty,
                        FirmId = cw.Company.Firmid,
                        Email = cw.Worker.Email,
                        IsActive = cw.Isactive ?? true,
                    });
                }

                return result
                    .OrderByDescending(x => x.IsActive)
                    .ThenBy(x => x.Name)
                    .ToList();
            }
        }
        //=================Login========
        public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
        {
            private readonly AccountingSystem.Application.Intrefaces.IAuthenticationService _authService;

            public LoginCommandHandler(AccountingSystem.Application.Intrefaces.IAuthenticationService authService)
            {
                _authService = authService;
            }

            public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                return await _authService.LoginAsync(request.Email, request.Password, cancellationToken);
            }
        }

        // ========================================
        // GOOGLE LOGIN HANDLER
        // ========================================
        public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommand, LoginResponseDto>
        {
            private readonly AccountingSystem.Application.Intrefaces.IAuthenticationService _authService;

            public GoogleLoginCommandHandler(AccountingSystem.Application.Intrefaces.IAuthenticationService authService)
            {
                _authService = authService;
            }

            public async Task<LoginResponseDto> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
            {
                return await _authService.GoogleLoginAsync(request.GoogleToken, cancellationToken);
            }
        }
    }