using AccountingSystem.Application.Commands;
using AccountingSystem.Application.DTOs;
using AccountingSystem.Application.Queries;
using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<GenerateRepoetInsanceHandler> _logger;

        public GenerateRepoetInsanceHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GenerateRepoetInsanceHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<List<ReportInstanceDto>> Handle(GenerateAutoReportInstanceCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting monthly report generation for date: {Date}", request.date);
            try
            {
                var r = await _unitOfWork.ReportInstances.GenerateReportsAsync(request.date);
                _logger.LogInformation("Successfully created {Count} report instances", r.Count);
                var res = _mapper.Map<List<ReportInstanceDto>>(r);
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate monthly reports for date: {Date}", request.date);
                throw;
            }
        }
    }


    public class CheckReportInstanceHandler : IRequestHandler<CheckReportInstanceQuery, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GenerateRepoetInsanceHandler> _logger;

        public CheckReportInstanceHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GenerateRepoetInsanceHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<bool> Handle(CheckReportInstanceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _unitOfWork.ReportInstances.CheckReportsAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check generate monthly reports for date: {Date}", DateTime.Now.Date);
                throw;
            }
        }
    }
}
