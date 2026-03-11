
using AccountingSystem.Application.Commands.Workers;
using AccountingSystem.Application.DTOs;
using AccountingSystem.Application.Queries.Workers;
using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Interfaces;
using AccountingSystem.Domain.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using CreateWorkerCommand = AccountingSystem.Application.Commands.Workers.CreateWorkerCommand;
using DeleteWorkerCommand = AccountingSystem.Application.Commands.Workers.DeleteWorkerCommand;
using UpdateWorkerCommand = AccountingSystem.Application.Commands.Workers.UpdateWorkerCommand;

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
            EmployeeId = w.Employeeid,
            RoleId = w.Roleid,
            RoleName = w.Role?.Name ?? "לא הוגדר", // ⭐ הוסף את זה!
            IsActive = w.Isactive ?? true,
            Phone = w.Phone
        }).ToList();

        return workerDtos;
    }
}
public class GetWorkerTasksQueryHandler : IRequestHandler<GetWorkerTasksQuery, IEnumerable<CompanyTask>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetWorkerTasksQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CompanyTask>> Handle(GetWorkerTasksQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Tasks.GetTasksByWorkerIdAsync(request.WorkerId);
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

    public CreateWorkerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<WorkerDto> Handle(CreateWorkerCommand request, CancellationToken cancellationToken)
    {
        var worker = new Worker
        {
            Firstname = request.Firstname,
            Lastname = request.Lastname,
            Firmid = request.Firmid,
            Email = request.Email,
            Phone = request.Phone,
            Isactive = request.Isactive,
            Createdat = DateTime.UtcNow
        };

        await _unitOfWork.Workers.AddAsync(worker);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<WorkerDto>(worker);
    }
}

// ========================================
// UPDATE WORKER COMMAND HANDLER
// ========================================
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

        worker.Firstname = request.Firstname;
        worker.Lastname = request.Lastname;
        worker.Email = request.Email;
        worker.Phone = request.Phone;
        worker.Isactive = request.Isactive;
        worker.Updatedat = DateTime.UtcNow;

        await _unitOfWork.Workers.UpdateAsync(worker);
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

        foreach (var cw in companyWorkers)
        {
            result.Add(new CompanyDto
            {
                Id = cw.Id,
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
    public class GetWorkersByCompanyHandler : IRequestHandler<GetWorkersByCompanyQuery, IEnumerable<WorkerLookupDto>>
    {
        private readonly IWorkerRepository _workerRepository;

        public GetWorkersByCompanyHandler(IWorkerRepository workerRepository)
        {
            _workerRepository = workerRepository;
        }

        public async Task<IEnumerable<WorkerLookupDto>> Handle(GetWorkersByCompanyQuery request, CancellationToken cancellationToken)
        {
            var workers = await _workerRepository.GetWorkersByCompanyIdAsync(request.CompanyId);

            return workers.Select(w => new WorkerLookupDto
            {
                Id = w.Id,
                FullName = $"{w.Firstname} {w.Lastname}"
            });
        }
    }
}



