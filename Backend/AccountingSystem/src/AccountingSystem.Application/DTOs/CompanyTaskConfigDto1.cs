//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace AccountingSystem.Application.DTOs

//    {
//        public class CompanyTaskConfigDto
//        {
//            // מזהה השורה בטבלה (יכול להיות 0 אם זו הגדרה חדשה שטרם נשמרה)
//            public int Id { get; set; }

//            // פרטי החברה (לקריאה בלבד בתצוגה)
//            public int CompanyId { get; set; }
//            public string CompanyName { get; set; } = string.Empty;

//            // פרטי סוג המשימה (לקריאה בלבד בתצוגה)
//            public int TaskTypeId { get; set; }
//            public string TaskTypeName { get; set; } = string.Empty;

//            // פרטי ביצוע (ניתנים לעריכה)
//            public int? assignedWorkerId { get; set; }
//            public string? WorkerName { get; set; } // לנוחות תצוגה במטריצה

//            public int Frequency { get; set; } // תדירות בחודשים
//            public int DueDay { get; set; }    // יום יעד (1-31)

//            public bool IsActive { get; set; } // סטטוס פעיל/לא פעיל
//        }
//    }