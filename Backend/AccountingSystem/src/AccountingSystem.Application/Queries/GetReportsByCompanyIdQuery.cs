using MediatR;
using AccountingSystem.Application.DTOs;

namespace AccountingSystem.Application.Queries
{
    public class GetReportsByCompanyIdQuery : IRequest<List<ReportInstanceDetailDto>>
    {
        public int CompanyId { get; set; }
        public string? Status { get; set; } 
        public DateTime? FromPeriod { get; set; }
        public DateTime? ToPeriod { get; set; }
    }

    public class GetReportByIdQuery : IRequest<ReportInstanceDetailDto?>
    {
        public int ReportId { get; set; }
    }

    public class GetUpcomingReportsQuery : IRequest<List<UpcomingReportDto>>
    {
        public int? CompanyId { get; set; } 
        public int DaysAhead { get; set; } = 30;
    }



    

        /// <summary>
        /// שאילתה לקבלת כל הדיווחים במערכת
        /// </summary>
        public class GetAllReportsQuery : IRequest<List<ReportInstanceDetailDto>>
        {
        public int? WorkerId { get; set; } 
        public bool IsAdminMode { get; set; } = false;
    }


    /// <summary>
    /// שאילתה לקבלת דיווחים לפי Config ID
    /// </summary>
    public class GetReportsByConfigIdQuery : IRequest<List<ReportInstanceDetailDto>>
        {
            public int ConfigId { get; set; }
        }


        /// <summary>
        /// שאילתה לקבלת דיווחים לפי סטטוס
        /// </summary>
        public class GetReportsByStatusQuery : IRequest<List<ReportInstanceDetailDto>>
        {
            public string Status { get; set; } = string.Empty;
        }


        /// <summary>
        /// שאילתה לקבלת דיווחים ממתינים (Pending)
        /// </summary>
        public class GetPendingReportsQuery : IRequest<List<ReportInstanceDetailDto>>
        {
        }


        /// <summary>
        /// שאילתה לקבלת דיווחים לפי תקופה (חודש/שנה)
        /// </summary>
        public class GetReportsByPeriodQuery : IRequest<List<ReportInstanceDetailDto>>
        {
            public DateTime Period { get; set; }
        }


        /// <summary>
        /// שאילתה לקבלת דיווחים בטווח תאריכים
        /// </summary>
        public class GetReportsByDateRangeQuery : IRequest<List<ReportInstanceDetailDto>>
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }


        /// <summary>
        /// שאילתה לקבלת דיווחים באיחור - קריטי!
        /// </summary>
        public class GetOverdueReportsQuery : IRequest<List<ReportInstanceDetailDto>>
        {
        }


        /// <summary>
        /// שאילתה לקבלת דיווחים שצריך להגיש בעוד X ימים
        /// </summary>
        public class GetReportsDueInNextDaysQuery : IRequest<List<ReportInstanceDetailDto>>
        {
            public int Days { get; set; }
        }







    // ========== Report Types Queries ==========

    /// <summary>
    /// שאילתה לקבלת כל סוגי הדיווחים
    /// </summary>
    public class GetAllReportTypesQuery : IRequest<List<ReportTypeDto>>
    {
    }
    public class GetAllReportTypesToEditQuery : IRequest<List<ReportTypeDto>>
    {
    }

    /// <summary>
    /// שאילתה לקבלת סוג דיווח לפי ID
    /// </summary>
    public class GetReportTypeByIdQuery : IRequest<ReportTypeDto?>
    {
        public int Id { get; set; }
    }

    // ========== Company Report Configs Queries ==========

    /// <summary>
    /// שאילתה לקבלת כל הקונפיגורציות
    /// </summary>
    public class GetAllConfigsQuery : IRequest<List<CompanyReportConfigDto>>
    {
    }
    public class GetConfigsByWorkerId : IRequest<List<CompanyReportConfigDto>>
    {
        public int WorkerId { get; set; }

    }

    /// <summary>
    /// שאילתה לקבלת קונפיגורציות לפי חברה
    /// </summary>
    public class GetConfigsByCompanyIdQuery : IRequest<List<CompanyReportConfigDto>>
    {
        public int CompanyId { get; set; }
    }

    /// <summary>
    /// שאילתה לקבלת קונפיגורציה לפי ID
    /// </summary>
    public class GetConfigByIdQuery : IRequest<CompanyReportConfigDto?>
    {
        public int Id { get; set; }
    }

}
