using AccountingSystem.Application.DTOs;
using AccountingSystem.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AccountingSystem.Application.Queries.Workers
{
    // ========================================
    // GET WORKER BY ID
    // ========================================

    public class GetWorkerByIdQuery : IRequest<WorkerDto>
    {
        public int Id { get; set; }

        public GetWorkerByIdQuery(int id)
        {
            Id = id;
        }
    }

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
            var worker = await _unitOfWork.Workers.GetWorkerWithRoleAsync(request.Id);

            if (worker == null)
            {
                throw new Exception($"עובד עם ID {request.Id} לא נמצא");
            }

            return _mapper.Map<WorkerDto>(worker);
        }
    }

    // ========================================
    // GET ALL WORKERS
    // ========================================

    public class GetAllWorkersQuery : IRequest<List<WorkerDto>>
    {
    }

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
            return _mapper.Map<List<WorkerDto>>(workers.ToList());
        }
    }

    // ========================================
    // GET WORKERS BY FIRM ID
    // ========================================

    public class GetWorkersByFirmIdQuery : IRequest<List<WorkerDto>>
    {
        public int FirmId { get; set; }

        public GetWorkersByFirmIdQuery(int firmId)
        {
            FirmId = firmId;
        }
    }

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
            return _mapper.Map<List<WorkerDto>>(workers.ToList());
        }
    }
}