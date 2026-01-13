using MediatR;
using AccountingSystem.Application.DTOs;

namespace AccountingSystem.Application.Commands
{
    public class CreateReportInstanceCommand : IRequest<ReportInstanceDto>
    {
        public int ConfigId { get; set; }
        public DateTime Period { get; set; }
        public decimal? Amount { get; set; }
        public string? PaymentMethod { get; set; }
        public DateTime? ReceiptDate { get; set; }
        public string Comments { get; set; } = string.Empty;
    }



    public class UpdateReportStatusCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty;
    }




    public class UpdateReportPaymentCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime PaidDate { get; set; }
    }





    public class UpdateReportInstanceCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public decimal? Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? PaymentMethod { get; set; }
        public DateTime? ReceiptDate { get; set; }
        public DateTime? ReportedDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public string Comments { get; set; } = string.Empty;
    }
}