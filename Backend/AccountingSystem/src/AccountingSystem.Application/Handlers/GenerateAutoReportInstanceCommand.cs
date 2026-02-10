using AccountingSystem.Application.Commands;
using AccountingSystem.Application.DTOs;
using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Application.Handlers
{
    public class GenerateRepoetInsanceHandler : IRequestHandler<GenerateAutoReportInstanceCommand, List<ReportInstanceDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GenerateRepoetInsanceHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<ReportInstanceDto>> Handle(GenerateAutoReportInstanceCommand request, CancellationToken cancellationToken)
        {
            var r =await _unitOfWork.ReportInstances.GenerateReportsAsync(request.date);
            var res = _mapper.Map<List<ReportInstanceDto>>(r);
            return res;
        }
    }
}
