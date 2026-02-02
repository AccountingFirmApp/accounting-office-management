using MediatR;
using AccountingSystem.Application.DTOs;

namespace AccountingSystem.Application.Queries
{
    public class GetReportsByCompanyIdQuery : IRequest<List<ReportInstanceDetailDto>>
    {
        public int CompanyId { get; set; }
        public string? Status { get; set; } // אופציונלי - סינון לפי סטטוס
        public DateTime? FromPeriod { get; set; } // מתקופה
        public DateTime? ToPeriod { get; set; } // עד תקופה
    }

    public class GetReportByIdQuery : IRequest<ReportInstanceDetailDto?>
    {
        public int ReportId { get; set; }
    }

    public class GetUpcomingReportsQuery : IRequest<List<UpcomingReportDto>>
    {
        public int? CompanyId { get; set; } // אופציונלי
        public int DaysAhead { get; set; } = 30; // ברירת מחדל - 30 יום קדימה
    }



    
        // ========== Query 1: קבלת כל הדיווחים ==========

        /// <summary>
        /// שאילתה לקבלת כל הדיווחים במערכת
        /// </summary>
        public class GetAllReportsQuery : IRequest<List<ReportInstanceDetailDto>>
        {
        public int? WorkerId { get; set; } // 🆕 הוספנו WorkerId אופציונלי

    }

    // ========== Query 2: דיווחים לפי Config ==========

    /// <summary>
    /// שאילתה לקבלת דיווחים לפי Config ID
    /// </summary>
    public class GetReportsByConfigIdQuery : IRequest<List<ReportInstanceDetailDto>>
        {
            public int ConfigId { get; set; }
        }

        // ========== Query 3: דיווחים לפי סטטוס ==========

        /// <summary>
        /// שאילתה לקבלת דיווחים לפי סטטוס
        /// </summary>
        public class GetReportsByStatusQuery : IRequest<List<ReportInstanceDetailDto>>
        {
            public string Status { get; set; } = string.Empty;
        }

        // ========== Query 4: דיווחים ממתינים ==========

        /// <summary>
        /// שאילתה לקבלת דיווחים ממתינים (Pending)
        /// </summary>
        public class GetPendingReportsQuery : IRequest<List<ReportInstanceDetailDto>>
        {
        }

        // ========== Query 5: דיווחים לפי תקופה ==========

        /// <summary>
        /// שאילתה לקבלת דיווחים לפי תקופה (חודש/שנה)
        /// </summary>
        public class GetReportsByPeriodQuery : IRequest<List<ReportInstanceDetailDto>>
        {
            public DateTime Period { get; set; }
        }

        // ========== Query 6: דיווחים בטווח תאריכים ==========

        /// <summary>
        /// שאילתה לקבלת דיווחים בטווח תאריכים
        /// </summary>
        public class GetReportsByDateRangeQuery : IRequest<List<ReportInstanceDetailDto>>
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }

        // ========== Query 7: דיווחים באיחור (OVERDUE) ==========

        /// <summary>
        /// שאילתה לקבלת דיווחים באיחור - קריטי!
        /// </summary>
        public class GetOverdueReportsQuery : IRequest<List<ReportInstanceDetailDto>>
        {
        }

        // ========== Query 8: דיווחים שמגיעים בקרוב ==========

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