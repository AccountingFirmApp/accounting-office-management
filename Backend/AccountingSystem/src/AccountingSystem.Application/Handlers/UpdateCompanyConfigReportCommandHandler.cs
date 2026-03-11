using AccountingSystem.Application.Commands;
using AccountingSystem.Application.DTOs;
using AccountingSystem.Domain.Interfaces;
using AccountingSystem.Domain.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Application.Handlers
{
    public class UpdateCompanyConfigReportCommandHandler: IRequestHandler<UpdateCompanyConfigReportCommand, CompanyReportConfigDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UpdateCompanyConfigReportCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<CompanyReportConfigDto> Handle( UpdateCompanyConfigReportCommand request,
            CancellationToken cancellationToken)
        {
            // מצא רשומה קיימת
            var config = await _unitOfWork.CompanyReportConfigs.GetByIdAsync(request.Id);
            if (config == null)
                throw new Exception($"Company report config with ID {request.Id} not found");

            if (request.FrequencyId!=0)
            {
                var frequencyExists = await _unitOfWork.Frequencies.GetByIdAsync(request.FrequencyId.Value);
                if (frequencyExists == null)
                {
                    throw new Exception($"Frequency {request.FrequencyId.Value} does not exist");
                }
                config.Frequencyid = request.FrequencyId.Value;
            }

            if (request.DayOfMonth.HasValue)
            {
                // ולידציה אופציונלית
                if (request.DayOfMonth.Value < 1 || request.DayOfMonth.Value > 31)
                    throw new Exception("Day of month must be between 1 and 31");

                config.Dayofmonth = (short?)request.DayOfMonth.Value;
            }

            config.Isactive = request.IsActive;
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CompanyReportConfigDto>(config);
        }
    }
}
