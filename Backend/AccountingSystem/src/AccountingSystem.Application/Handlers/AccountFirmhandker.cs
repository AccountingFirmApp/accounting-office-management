using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Application.Handlers
{
    using AccountingSystem.Application.DTOs;
    //using AccountingSystem.Application.Pagination;
    using AccountingSystem.Application.Queries.AccountingFirmQuery;
    using AccountingSystem.Domain.Interfaces;
    using AutoMapper;
    using MediatR;

    public class AccountingFirmQueryHandler :
        IRequestHandler<GetAllAccountingFirmQuery, List<AccountingFirmDto>>,
        IRequestHandler<GetAccountingFirmByIdQuery, AccountingFirmDto>
        //IRequestHandler<GetAccountingFirmPageQuery, PagedResult<AccountingFirmDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountingFirmQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // =======================
        // Get All Accounting Firms
        // =======================
        public async Task<List<AccountingFirmDto>> Handle(GetAllAccountingFirmQuery request, CancellationToken cancellationToken)
        {
            var firms = await _unitOfWork.AccountingFirms.GetAllAsync();
            return _mapper.Map<List<AccountingFirmDto>>(firms);
        }

        // =======================
        // Get Accounting Firm by ID
        // =======================
        public async Task<AccountingFirmDto> Handle(GetAccountingFirmByIdQuery request, CancellationToken cancellationToken)
        {
            var firm = await _unitOfWork.AccountingFirms.GetByIdAsync(request.Id);
            if (firm == null)
                throw new Exception($"משרד עם ID {request.Id} לא נמצא");

            return _mapper.Map<AccountingFirmDto>(firm);
        }

        // =======================
        // Get Accounting Firms with Pagination
        // =======================
        //public async Task<PagedResult<AccountingFirmDto>> Handle(GetAccountingFirmPageQuery request, CancellationToken cancellationToken)
        //{
        //    var pagedResult = await _unitOfWork.AccountingFirms.GetPageAsync(request.Page, request.PageSize);
        //    return new PagedResult<AccountingFirmDto>
        //    {
        //        Items = _mapper.Map<List<AccountingFirmDto>>(pagedResult.Items),
        //        TotalCount = pagedResult.TotalCount
        //    };
        //}
    }
}