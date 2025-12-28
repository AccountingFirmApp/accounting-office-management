using AccountingSystem.Application.DTOs;
using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AccountingSystem.Application.Commands.Workers
{
    // ========================================
    // CREATE WORKER
    // ========================================

    public class CreateWorkerCommand : IRequest<WorkerDto>
    {
        public int Firmid { get; set; }
        public int Roleid { get; set; }
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Employeeid { get; set; }
        public bool Isactive { get; set; } = true;
        public DateOnly? Hiredate { get; set; }
    }

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
            if (await _unitOfWork.Workers.EmailExistsAsync(request.Email))
            {
                throw new Exception($"עובד עם אימייל {request.Email} כבר קיים במערכת");
            }

            if (!await _unitOfWork.AccountingFirms.ExistsAsync(request.Firmid))
            {
                throw new Exception($"משרד רואי חשבון לא נמצא");
            }

            if (!await _unitOfWork.Roles.ExistsAsync(request.Roleid))
            {
                throw new Exception($"תפקיד לא נמצא");
            }

            var worker = new Worker
            {
                Firmid = request.Firmid,
                Roleid = request.Roleid,
                Firstname = request.Firstname,
                Lastname = request.Lastname,
                Email = request.Email,
                Phone = request.Phone,
                Employeeid = request.Employeeid,
                Isactive = request.Isactive,
                Hiredate = request.Hiredate,
                Createdat = DateTime.UtcNow,
                Updatedat = DateTime.UtcNow
            };

            await _unitOfWork.Workers.AddAsync(worker);
            await _unitOfWork.SaveChangesAsync();

            var createdWorker = await _unitOfWork.Workers.GetWorkerWithRoleAsync(worker.Id);
            return _mapper.Map<WorkerDto>(createdWorker);
        }
    }

    // ========================================
    // UPDATE WORKER
    // ========================================

    public class UpdateWorkerCommand : IRequest<WorkerDto>
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public bool Isactive { get; set; }
    }

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

            if (await _unitOfWork.Workers.EmailExistsAsync(request.Email, request.Id))
            {
                throw new Exception($"אימייל {request.Email} כבר קיים במערכת");
            }

            worker.Firstname = request.Firstname;
            worker.Lastname = request.Lastname;
            worker.Email = request.Email;
            worker.Phone = request.Phone;
            worker.Isactive = request.Isactive;
            worker.Updatedat = DateTime.UtcNow;

            await _unitOfWork.Workers.UpdateAsync(worker);
            await _unitOfWork.SaveChangesAsync();

            var updatedWorker = await _unitOfWork.Workers.GetWorkerWithRoleAsync(worker.Id);
            return _mapper.Map<WorkerDto>(updatedWorker);
        }
    }

    // ========================================
    // DELETE WORKER
    // ========================================

    public class DeleteWorkerCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteWorkerCommand(int id)
        {
            Id = id;
        }
    }

    public class DeleteWorkerCommandHandler : IRequestHandler<DeleteWorkerCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteWorkerCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteWorkerCommand request, CancellationToken cancellationToken)
        {
            var worker = await _unitOfWork.Workers.GetByIdAsync(request.Id);
            if (worker == null)
            {
                throw new Exception($"עובד עם ID {request.Id} לא נמצא");
            }

            await _unitOfWork.Workers.DeleteAsync(request.Id);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}