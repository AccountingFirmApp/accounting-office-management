// Application/Handlers/Workers/WorkersHandler.cs
using AccountingSystem.Application.DTOs;
using AccountingSystem.Application;
using AccountingSystem.Application.Queries.Workers;
using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Interfaces;
using AutoMapper;
using MediatR;

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
        return _mapper.Map<List<WorkerDto>>(workers);
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
            Firstname = request.FirstName,
            Lastname = request.LastName,
            Firmid = request.FirmId,
            Email = request.Email,
            Phone = request.Phone,
            //Role = request.Role,
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

        worker.Firstname = request.FirstName;
        worker.Lastname = request.LastName;
        worker.Email = request.Email;
        worker.Phone = request.Phone;
        //worker.Role = request.Role;
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
