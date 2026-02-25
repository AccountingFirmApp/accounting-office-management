using AccountingSystem.Application.DTOs;
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
using IAuthService = AccountingSystem.Application.Intrefaces.IAuthenticationService;
using AccountingSystem.Application.Intrefaces;

namespace AccountingSystem.Application.Handlers.Workers;

// ========================================
// GET ALL WORKERS HANDLER
// =======================================
public class GetAllWorkersQueryHandler : IRequestHandler<GetAllWorkersQuery, List<WorkerDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetAllWorkersQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<List<WorkerDto>> Handle(GetAllWorkersQuery request, CancellationToken cancellationToken)
    {
        int firmId = _currentUserService.GetFirmId();
        var workers = await _unitOfWork.Workers.GetAllByFirmIdAsync(firmId, request.IsActive);

        //var workers = await _unitOfWork.Workers.GetAllByFirmIdAsync(firmId);

        var workerDtos = workers.Select(w => new WorkerDto
        {
            Id = w.Id,
            FirmId = w.Firmid,
            RoleId = w.Roleid,
            FirstName = w.Firstname,
            LastName = w.Lastname,
            Email = w.Email,
            Employeeid = w.Employeeid ?? "",
            Phone = w.Phone ?? "",
            RoleName = w.Role?.Name ?? "לא הוגדר",
            FirmName = w.Firm?.Name ?? "",
            Isactive = w.Isactive ?? true,
            HireDate = w.Hiredate.HasValue ? w.Hiredate.Value.ToDateTime(TimeOnly.MinValue) : null,
            CreatedAt = w.Createdat ?? DateTime.Now,
            UpdatedAt = w.Updatedat ?? DateTime.Now
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
    private readonly IAuthService _authService;


    public CreateWorkerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _authService = authService;


    }

    public async Task<WorkerDto> Handle(CreateWorkerCommand request, CancellationToken ct)
    {
        var existingWorker = await _unitOfWork.Workers.GetByEmailAsync(request.Email, request.Firmid);
        Worker worker;

        if (existingWorker != null)
        {
            if (existingWorker.Isactive == true)
                throw new Exception("Worker already exists");

            existingWorker.Isactive = true;
            existingWorker.Firstname = request.Firstname;
            existingWorker.Lastname = request.Lastname;
            existingWorker.Phone = request.Phone;
            existingWorker.Roleid = request.Roleid;
            existingWorker.Employeeid = request.Employeeid;
            existingWorker.Hiredate = request.Hiredate;
            existingWorker.Updatedat = DateTime.UtcNow;
            worker = existingWorker;

            await _unitOfWork.SaveChangesAsync(ct);

            //if (request.RestoreExistingData)
            //{
            //    var existingLinks = await _unitOfWork.CompanyWorkers
            //        .FindAsync(cw => cw.Workerid == worker.Id);
            //    foreach (var cw in existingLinks)
            //    {
            //        var company = await _unitOfWork.Companies.GetByIdAsync(cw.Companyid);
            //        if (company != null && company.Isactive == true)
            //            cw.Isactive = true;
            //    }
            //
            //}
            if (request.RestoreExistingData)
            {
                // שחזור קשרים קיימים - רק לחברות פעילות
                var existingLinks = await _unitOfWork.CompanyWorkers
                    .FindAsync(cw => cw.Workerid == worker.Id);
                foreach (var cw in existingLinks)
                {
                    var company = await _unitOfWork.Companies.GetByIdAsync(cw.Companyid);
                    if (company != null && company.Isactive == true)
                        cw.Isactive = true;
                }

                // הוספת חברות חדשות שנבחרו ואין להן קשר קיים
                if (request.CompanyIds != null && request.CompanyIds.Any())
                {
                    foreach (var companyId in request.CompanyIds)
                    {
                        var existingLink = await _unitOfWork.CompanyWorkers
                            .GetByWorkerAndCompany(worker.Id, companyId);
                        if (existingLink == null)
                        {
                            var link = new Companyworker
                            {
                                Workerid = worker.Id,
                                Companyid = companyId,
                                Isactive = true,
                                Assignedat = DateTime.UtcNow
                            };
                            await _unitOfWork.CompanyWorkers.AddAsync(link);
                        }
                    }
                }
            }
            else
            {
                if (request.CompanyIds != null && request.CompanyIds.Any())
                {
                    foreach (var companyId in request.CompanyIds)
                    {
                        var existingLink = await _unitOfWork.CompanyWorkers
                            .GetByWorkerAndCompany(worker.Id, companyId);
                        if (existingLink != null)
                        {
                            existingLink.Isactive = true;
                            existingLink.Assignedat = DateTime.UtcNow;
                        }
                        else
                        {
                            var link = new Companyworker
                            {
                                Workerid = worker.Id,
                                Companyid = companyId,
                                Isactive = true,
                                Assignedat = DateTime.UtcNow
                            };
                            await _unitOfWork.CompanyWorkers.AddAsync(link);
                        }
                    }
                }
            }

            await _unitOfWork.SaveChangesAsync(ct);
            return _mapper.Map<WorkerDto>(worker);
        }
        else
        {
            // 🆕 עובד חדש
            var passwordHash = await _authService.HashPasswordAsync(request.Password);
            worker = new Worker
            {
                Firmid = request.Firmid,
                Roleid = request.Roleid,
                Firstname = request.Firstname,
                Lastname = request.Lastname,
                Email = request.Email,
                Phone = request.Phone,
                Employeeid = request.Employeeid,
                Hiredate = request.Hiredate,
                PasswordHash = passwordHash,
                Isactive = true,
                Createdat = DateTime.UtcNow
            };
            await _unitOfWork.Workers.AddAsync(worker);
            await _unitOfWork.SaveChangesAsync(ct);

            if (request.CompanyIds != null && request.CompanyIds.Any())
            {
                foreach (var companyId in request.CompanyIds)
                {
                    var link = new Companyworker
                    {
                        Workerid = worker.Id,
                        Companyid = companyId,
                        Isactive = true,
                        Assignedat = DateTime.UtcNow
                    };
                    await _unitOfWork.CompanyWorkers.AddAsync(link);
                }
            }

            await _unitOfWork.SaveChangesAsync(ct);
            return _mapper.Map<WorkerDto>(worker);
        }
    }

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

    //        if (!string.IsNullOrEmpty(request.Firstname))
    //            worker.Firstname = request.Firstname;

    //        if (!string.IsNullOrEmpty(request.Lastname))
    //            worker.Lastname = request.Lastname;

    //        if (!string.IsNullOrEmpty(request.Email))
    //            worker.Email = request.Email;

    //        if (!string.IsNullOrEmpty(request.Phone))
    //            worker.Phone = request.Phone;

    //        if (request.Roleid.HasValue)
    //            worker.Roleid = request.Roleid.Value;

    //        if (!string.IsNullOrEmpty(request.Employeeid))
    //            worker.Employeeid = request.Employeeid;

    //        if (request.Hiredate.HasValue)
    //            worker.Hiredate = request.Hiredate.Value;

    //        worker.Updatedat = DateTime.UtcNow;

    //        if (request.CompanyIds != null)
    //        {
    //            await UpdateWorkerCompanies(worker.Id, request.CompanyIds, cancellationToken);
    //        }

    //        await _unitOfWork.Workers.UpdateAsync(worker);
    //        await _unitOfWork.SaveChangesAsync(cancellationToken);

    //        return _mapper.Map<WorkerDto>(worker);
    //    }
    //    private async Task UpdateWorkerCompanies(int workerId, List<int> companyIds, CancellationToken cancellationToken)
    //    {
    //        await _unitOfWork.CompanyWorkers.DeleteByWorkerIdAsync(workerId);

    //        foreach (var companyId in companyIds)
    //        {
    //            await _unitOfWork.CompanyWorkers.AddAsync(new Companyworker
    //            {
    //                Workerid = workerId,
    //                Companyid = companyId
    //            });
    //        }
    //    }
    //}
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

    //        // ========================
    //        // עדכון שדות
    //        // ========================
    //        if (!string.IsNullOrEmpty(request.Firstname))
    //            worker.Firstname = request.Firstname;

    //        if (!string.IsNullOrEmpty(request.Lastname))
    //            worker.Lastname = request.Lastname;

    //        if (!string.IsNullOrEmpty(request.Email))
    //            worker.Email = request.Email;

    //        if (!string.IsNullOrEmpty(request.Phone))
    //            worker.Phone = request.Phone;

    //        if (request.Roleid.HasValue)
    //            worker.Roleid = request.Roleid.Value;

    //        if (!string.IsNullOrEmpty(request.Employeeid))
    //            worker.Employeeid = request.Employeeid;

    //        if (request.Hiredate.HasValue)
    //            worker.Hiredate = request.Hiredate.Value;

    //        worker.Updatedat = DateTime.UtcNow;

    //        // ========================
    //        // עדכון קשרי חברות (Soft Delete / Reactivation)
    //        // ========================
    //        if (request.CompanyIds != null && request.CompanyIds.Any())
    //        {
    //            foreach (var companyId in request.CompanyIds)
    //            {
    //                var existingLink = await _unitOfWork.CompanyWorkers
    //                    .GetByWorkerAndCompany(worker.Id, companyId);

    //                if (existingLink != null)
    //                {
    //                    // הפעלה מחדש
    //                    existingLink.Isactive = true;
    //                    existingLink.Assignedat = DateTime.UtcNow;
    //                }
    //                else
    //                {
    //                    // יצירת קשר חדש
    //                    var link = new Companyworker
    //                    {
    //                        Workerid = worker.Id,
    //                        Companyid = companyId,
    //                        Isactive = true,
    //                        Assignedat = DateTime.UtcNow
    //                    };
    //                    await _unitOfWork.CompanyWorkers.AddAsync(link);
    //                }
    //            }
    //        }

    //        // ========================
    //        // שמירה אחת בסוף
    //        // ========================
    //        await _unitOfWork.SaveChangesAsync(cancellationToken);

    //        return _mapper.Map<WorkerDto>(worker);
    //    }
    //}

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
    //        // קבלת העובד
    //        var worker = await _unitOfWork.Workers.GetByIdAsync(request.Id);
    //        if (worker == null)
    //            throw new Exception($"עובד עם ID {request.Id} לא נמצא");

    //        // ========================
    //        // עדכון שדות העובד
    //        // ========================
    //        if (!string.IsNullOrEmpty(request.Firstname))
    //            worker.Firstname = request.Firstname;

    //        if (!string.IsNullOrEmpty(request.Lastname))
    //            worker.Lastname = request.Lastname;

    //        if (!string.IsNullOrEmpty(request.Email))
    //            worker.Email = request.Email;

    //        if (!string.IsNullOrEmpty(request.Phone))
    //            worker.Phone = request.Phone;

    //        if (request.Roleid.HasValue)
    //            worker.Roleid = request.Roleid.Value;

    //        if (!string.IsNullOrEmpty(request.Employeeid))
    //            worker.Employeeid = request.Employeeid;

    //        if (request.Hiredate.HasValue)
    //            worker.Hiredate = request.Hiredate.Value;

    //        worker.Updatedat = DateTime.UtcNow;

    //        // ========================
    //        // עדכון קשרי חברות (Soft Reactivation)
    //        // ========================
    //        if (request.CompanyIds != null && request.CompanyIds.Any())
    //        {
    //            // מביא את כל הקשרים הקיימים של העובד
    //            var existingLinks = await _unitOfWork.CompanyWorkers
    //                .FindAsync(cw => cw.Workerid == worker.Id);

    //            foreach (var companyId in request.CompanyIds)
    //            {
    //                var link = existingLinks.FirstOrDefault(cw => cw.Companyid == companyId);

    //                if (link != null)
    //                {
    //                    // אם הקשר קיים אך לא פעיל → הפעלה מחדש
    //                    link.Isactive = true;
    //                    link.Assignedat = DateTime.UtcNow;
    //                }
    //                else
    //                {
    //                    // אם הקשר לא קיים → צור חדש
    //                    await _unitOfWork.CompanyWorkers.AddAsync(new Companyworker
    //                    {
    //                        Workerid = worker.Id,
    //                        Companyid = companyId,
    //                        Isactive = true,
    //                        Assignedat = DateTime.UtcNow
    //                    });
    //                }
    //            }
    //        }

    //        // ========================
    //        // שמירה אחת בסוף
    //        // ========================
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
                throw new Exception($"עובד עם ID {request.Id} לא נמצא");

            // ========================
            // עדכון שדות
            // ========================
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

            worker.Updatedat = DateTime.UtcNow;

            // ========================
            // עדכון קשרי חברות
            // ========================
            if (request.CompanyIds != null && request.CompanyIds.Any())
            {
                // מביא את כל הקשרים של העובד, כולל לא פעילים
                var existingLinks = await _unitOfWork.CompanyWorkers
                    .FindAsync(cw => cw.Workerid == worker.Id);
                Console.WriteLine(existingLinks.Count() + "ccccccccccccccccccccccccccccccccccc");
                foreach (var companyId in request.CompanyIds)
                {
                    var link = existingLinks.FirstOrDefault(cw => cw.Companyid == companyId);

                    if (link != null)
                    {
                        // הקשר כבר קיים → הפעלה מחדש אם צריך
                        link.Isactive = true;
                        link.Assignedat = DateTime.UtcNow;
                    }
                    else
                    {
                        // הקשר לא קיים → צור חדש
                        await _unitOfWork.CompanyWorkers.AddAsync(new Companyworker
                        {
                            Workerid = worker.Id,
                            Companyid = companyId,
                            Isactive = true,
                            Assignedat = DateTime.UtcNow
                        });
                    }
                }
            }

            // ========================
            // שמירה אחת בסוף
            // ========================
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<WorkerDto>(worker);
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

        //    public async Task<Unit> Handle(DeleteWorkerCommand request, CancellationToken cancellationToken)
        //    {
        //        var worker = await _unitOfWork.Workers.GetByIdAsync(request.Id);
        //        if (worker == null)
        //        {
        //            throw new Exception($"עובד עם ID {request.Id} לא נמצא");
        //        }

        //        worker.Isactive = false;
        //        worker.Updatedat = DateTime.UtcNow;
        //        await _unitOfWork.Workers.UpdateAsync(worker);
        //        var companyWorkers = await _unitOfWork.CompanyWorkers
        //.FindAsync(cw => cw.Workerid == worker.Id);

        //        foreach (var cw in companyWorkers)
        //        {
        //            cw.Isactive = false;
        //        }

        //        //await _unitOfWork.CompanyWorkers.DeleteByWorkerIdAsync(request.Id);

        //        await _unitOfWork.SaveChangesAsync(cancellationToken);

        //        return Unit.Value;
        //    }
        //}
        public async Task<Unit> Handle(DeleteWorkerCommand request, CancellationToken cancellationToken)
    {
        var worker = await _unitOfWork.Workers.GetByIdAsync(request.Id);
        if (worker == null)
            throw new Exception($"עובד עם ID {request.Id} לא נמצא");

        // Soft Delete לעובד
        worker.Isactive = false;
        worker.Updatedat = DateTime.UtcNow;
        await _unitOfWork.Workers.UpdateAsync(worker);

        // כיבוי כל ה-CompanyWorkers שלו
        var companyWorkers = await _unitOfWork.CompanyWorkers
            .FindAsync(cw => cw.Workerid == worker.Id);
        foreach (var cw in companyWorkers)
            cw.Isactive = false;

        // ניקוי המשימות - נשארות אבל בלי עובד מוקצה
        //var tasks = await _unitOfWork.CompanyTasks
        //    .FindAsync(t => t.Assignedworkerid == worker.Id);
        //foreach (var t in tasks)
        //    t.Assignedworkerid = null;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    } }

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
            var workerExists = await _unitOfWork.Workers.ExistsAsync(request.WorkerId);
            if (!workerExists)
            {
                throw new Exception($"עובדת עם ID {request.WorkerId} לא נמצאה");
            }
            var companyWorkers = await _unitOfWork.CompanyWorkers.GetByWorkerIdAsync(request.WorkerId);
            var result = new List<CompanyDto>();
            foreach (var cw in companyWorkers.Where(cw => cw.Company.Isactive == true))
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
                    Isactive = cw.Isactive ?? true,
                });
            }
            return result.OrderBy(x => x.Name).ToList();
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