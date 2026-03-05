using AccountingSystem.Application.DTOs;
using AccountingSystem.Domain.DTOs;
using AccountingSystem.Domain.Entities;
using AutoMapper;
using AccountingSystem.Application.Mappings.Resolvers;
using AccountingSystem.Application.DTOs.Tasks;
namespace AccountingSystem.Application.Mappings
{
    /// <summary>
    /// הגדרת כל המיפויים בין Entities ל-DTOs
    /// מותאם לסכמת הDB של AccountingSystem
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMappings();
        }

        private void CreateMappings()
        {
            // ========================================
            // AccountingFirm Mappings
            // ========================================
            CreateMap<Accountingfirm, AccountingFirmDto>();

            CreateMap<CreateAccountingFirmDto, Accountingfirm>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Createdat, opt => opt.Ignore())
                .ForMember(d => d.Updatedat, opt => opt.Ignore())
                .ForMember(d => d.Companies, opt => opt.Ignore())
                .ForMember(d => d.Workers, opt => opt.Ignore());

            CreateMap<UpdateAccountingFirmDto, Accountingfirm>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Createdat, opt => opt.Ignore())
                .ForMember(d => d.Updatedat, opt => opt.Ignore())
                .ForMember(d => d.Companies, opt => opt.Ignore())
                .ForMember(d => d.Workers, opt => opt.Ignore());

            // ========================================
            // Role Mappings
            // ========================================
            CreateMap<Role, RoleDto>();

            CreateMap<CreateRoleDto, Role>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Createdat, opt => opt.Ignore())
                .ForMember(d => d.Workers, opt => opt.Ignore());

            // ========================================
            // Worker Mappings
            // ========================================
            //CreateMap<Worker, WorkerDto>()
            //    .ForMember(d => d.FirmName,
            //        opt => opt.MapFrom(s => s.Firm != null ? s.Firm.Name : string.Empty))
            //    .ForMember(d => d.RoleName,
            //        opt => opt.MapFrom(s => s.Role != null ? s.Role.Name : string.Empty))
            //    .ForMember(d => d.FullName,
            //        opt => opt.MapFrom(s => $"{s.Firstname} {s.Lastname}"));
            CreateMap<Worker, WorkerDto>()
    .ForMember(d => d.FirmId, opt => opt.MapFrom(s => s.Firmid))
    .ForMember(d => d.RoleId, opt => opt.MapFrom(s => s.Roleid))
    .ForMember(d => d.FirstName, opt => opt.MapFrom(s => s.Firstname))
    .ForMember(d => d.LastName, opt => opt.MapFrom(s => s.Lastname))
    .ForMember(d => d.EmployeeId, opt => opt.MapFrom(s => s.Employeeid ?? string.Empty))
    .ForMember(d => d.IsActive, opt => opt.MapFrom(s => s.Isactive ?? false))
    .ForMember(d => d.CreatedAt, opt => opt.MapFrom(s => s.Createdat ?? DateTime.MinValue))
    .ForMember(d => d.UpdatedAt, opt => opt.MapFrom(s => s.Updatedat ?? DateTime.MinValue))
    .ForMember(d => d.HireDate,
        opt => opt.MapFrom(s =>
            s.Hiredate.HasValue
                ? s.Hiredate.Value.ToDateTime(TimeOnly.MinValue)
                : (DateTime?)null
        ))
    .ForMember(d => d.FirmName,
        opt => opt.MapFrom(s => s.Firm != null ? s.Firm.Name : string.Empty))
    .ForMember(d => d.RoleName,
        opt => opt.MapFrom(s => s.Role != null ? s.Role.Name : string.Empty))
    .ForMember(d => d.FullName, opt => opt.Ignore()); // ⛔ מחושב אוטומטית

            CreateMap<CreateWorkerDto, Worker>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Createdat, opt => opt.Ignore())
                .ForMember(d => d.Updatedat, opt => opt.Ignore())
                .ForMember(d => d.Firm, opt => opt.Ignore())
                .ForMember(d => d.Role, opt => opt.Ignore())
                .ForMember(d => d.Companyworkers, opt => opt.Ignore())
                .ForMember(d => d.CompanyTasks, opt => opt.Ignore())
                .ForMember(d => d.Auditlogs, opt => opt.Ignore());

            CreateMap<UpdateWorkerDto, Worker>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Createdat, opt => opt.Ignore())
                .ForMember(d => d.Updatedat, opt => opt.Ignore())
                .ForMember(d => d.Firm, opt => opt.Ignore())
                .ForMember(d => d.Role, opt => opt.Ignore())
                .ForMember(d => d.Companyworkers, opt => opt.Ignore())
                .ForMember(d => d.CompanyTasks, opt => opt.Ignore())
                .ForMember(d => d.Auditlogs, opt => opt.Ignore());

            // ========================================
            // Frequency Mappings
            // ========================================
            CreateMap<Frequency, FrequencyDto>();

            CreateMap<CreateFrequencyDto, Frequency>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Createdat, opt => opt.Ignore())
                .ForMember(d => d.Companyreportconfigs, opt => opt.Ignore());

            // ========================================
            // Company Mappings
            // ========================================
            CreateMap<Company, CompanyDto>()
                .ForMember(d => d.FirmName,
                    opt => opt.MapFrom(s => s.Firm != null ? s.Firm.Name : string.Empty));

            CreateMap<CreateCompanyDto, Company>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Createdat, opt => opt.Ignore())
                .ForMember(d => d.Updatedat, opt => opt.Ignore())
                .ForMember(d => d.Firm, opt => opt.Ignore())
                .ForMember(d => d.Companycontacts, opt => opt.Ignore())
                .ForMember(d => d.Companyreportconfigs, opt => opt.Ignore())
                .ForMember(d => d.Companyworkers, opt => opt.Ignore())
                .ForMember(d => d.CompanyTasks, opt => opt.Ignore());

            CreateMap<UpdateCompanyDto, Company>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Createdat, opt => opt.Ignore())
                .ForMember(d => d.Updatedat, opt => opt.Ignore())
                .ForMember(d => d.Firm, opt => opt.Ignore())
                .ForMember(d => d.Companycontacts, opt => opt.Ignore())
                .ForMember(d => d.Companyreportconfigs, opt => opt.Ignore())
                .ForMember(d => d.Companyworkers, opt => opt.Ignore())
                .ForMember(d => d.CompanyTasks, opt => opt.Ignore());

            // ========================================
            // CompanyWorker Mappings
            // ========================================
            CreateMap<Companyworker, CompanyWorkerDto>()
                .ForMember(d => d.CompanyName,
                    opt => opt.MapFrom(s => s.Company != null ? s.Company.Name : string.Empty))
                .ForMember(d => d.WorkerFirstName,
                    opt => opt.MapFrom(s => s.Worker != null
                        ? $"{s.Worker.Firstname} {s.Worker.Lastname}"
                        : string.Empty));

         

            // ========================================
            // CompanyContact Mappings
            // ========================================
            CreateMap<Companycontact, CompanyContactDto>()
                .ForMember(d => d.CompanyName,
                    opt => opt.MapFrom(s => s.Company != null ? s.Company.Name : string.Empty))
                .ForMember(d => d.FullName,
                    opt => opt.MapFrom(s => $"{s.Firstname} {s.Lastname}"));

            CreateMap<CreateCompanyContactDto, Companycontact>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Createdat, opt => opt.Ignore())
                .ForMember(d => d.Updatedat, opt => opt.Ignore())
                .ForMember(d => d.Company, opt => opt.Ignore());

            CreateMap<UpdateCompanyContactDto, Companycontact>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Createdat, opt => opt.Ignore())
                .ForMember(d => d.Updatedat, opt => opt.Ignore())
                .ForMember(d => d.Company, opt => opt.Ignore());

            // ========================================
            // ReportType Mappings
            // ========================================
            CreateMap<Reporttype, ReportTypeDto>();

            CreateMap<CreateReportTypeDto, Reporttype>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Createdat, opt => opt.Ignore())
                .ForMember(d => d.Companyreportconfigs, opt => opt.Ignore());

            // ========================================
            // CompanyReportConfig Mappings
            // ========================================

            CreateMap<Companyreportconfig, CompanyReportConfigDto>()
              .ForMember(d => d.CompanyId, opt => opt.MapFrom(s => s.Companyid))
              .ForMember(d => d.ReportTypeId, opt => opt.MapFrom(s => s.Reporttypeid))
              .ForMember(d => d.FrequencyId, opt => opt.MapFrom(s => s.Frequencyid))
              .ForMember(d => d.DayOfMonth, opt => opt.MapFrom(s => s.Dayofmonth))
              .ForMember(d => d.IsActive, opt => opt.MapFrom(s => s.Isactive))
              .ForMember(d => d.CreatedAt, opt => opt.MapFrom(s => s.Createdat ?? DateTime.MinValue))
              .ForMember(d => d.UpdatedAt, opt => opt.MapFrom(s => s.Updatedat ?? DateTime.MinValue))
              .ForMember(d => d.Year, opt => opt.MapFrom(s => s.Year))
              .ForMember(d => d.CompanyName,
                  opt => opt.MapFrom(s => s.Company != null ? s.Company.Name : null))
              .ForMember(d => d.ReportTypeName,
                  opt => opt.MapFrom(s => s.Reporttype != null ? s.Reporttype.Name : null))
              .ForMember(d => d.ReportTypeShortCode,
                  opt => opt.MapFrom(s => s.Reporttype != null ? s.Reporttype.Shortcode : null))
              .ForMember(d => d.FrequencyName,
                  opt => opt.MapFrom(s => s.Frequency != null ? s.Frequency.Name : null));


            CreateMap<CreateCompanyReportConfigDto, Companyreportconfig>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Companyid, opt => opt.MapFrom(s => s.CompanyId))
                .ForMember(d => d.Reporttypeid, opt => opt.MapFrom(s => s.ReportTypeId))
                .ForMember(d => d.Frequencyid, opt => opt.MapFrom(s => s.FrequencyId))
                .ForMember(d => d.Dayofmonth, opt => opt.MapFrom(s => s.DayOfMonth))
                .ForMember(d => d.Year, opt => opt.MapFrom(s => s.Year))
                .ForMember(d => d.Isactive, opt => opt.Ignore())
                .ForMember(d => d.Createdat, opt => opt.Ignore())
                .ForMember(d => d.Updatedat, opt => opt.Ignore())
                .ForMember(d => d.Company, opt => opt.Ignore())
                .ForMember(d => d.Reporttype, opt => opt.Ignore())
                .ForMember(d => d.Frequency, opt => opt.Ignore())
                .ForMember(d => d.Reportinstances, opt => opt.Ignore());

            CreateMap<UpdateCompanyReportConfigDto, Companyreportconfig>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Frequencyid, opt => opt.MapFrom(s => s.FrequencyId))
                .ForMember(d => d.Dayofmonth, opt => opt.MapFrom(s => s.DayOfMonth))
               .ForMember(d => d.Createdat, opt => opt.Ignore())
                .ForMember(d => d.Updatedat, opt => opt.Ignore())
                 .ForMember(d => d.Isactive, opt => opt.Ignore())
                .ForMember(d => d.Company, opt => opt.Ignore())
                .ForMember(d => d.Reporttype, opt => opt.Ignore())
                .ForMember(d => d.Frequency, opt => opt.Ignore())
                .ForMember(d => d.Reportinstances, opt => opt.Ignore());
      

// ========================================
// ReportInstance Mappings
// ========================================
CreateMap<Reportinstance, ReportInstanceDto>()
                .ForMember(d => d.Period, opt => opt.MapFrom(s => s.Period.ToDateTime(TimeOnly.MinValue)))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.PaymentMethod, opt => opt.MapFrom(s => s.PaymentMethod.HasValue ? s.PaymentMethod.Value.ToString() : null))
                .ForMember(d => d.ReceiptDate, opt => opt.MapFrom(s => s.Receiptdate.HasValue ? s.Receiptdate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null))
                .ForMember(d => d.ReportedDate, opt => opt.MapFrom(s => s.Reporteddate.HasValue ? s.Reporteddate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null))
                .ForMember(d => d.PaidDate, opt => opt.MapFrom(s => s.Paiddate.HasValue ? s.Paiddate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null))
                .ForMember(d => d.CreatedAt, opt => opt.MapFrom(s => s.Createdat ?? DateTime.MinValue))
                .ForMember(d => d.UpdatedAt, opt => opt.MapFrom(s => s.Updatedat ?? DateTime.MinValue))
                .ForMember(d => d.ConfigId, opt => opt.MapFrom(s => s.Configid));

            // מיפוי מורחב - ל-ReportInstanceDetailDto (עם כל הפרטים)
            CreateMap<Reportinstance, ReportInstanceDetailDto>()
                .ForMember(d => d.Period, opt => opt.MapFrom(s => s.Period.ToDateTime(TimeOnly.MinValue)))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.PaymentMethod, opt => opt.MapFrom(s => s.PaymentMethod.HasValue ? s.PaymentMethod.Value.ToString() : null))
                .ForMember(d => d.ReceiptDate, opt => opt.MapFrom(s => s.Receiptdate.HasValue ? s.Receiptdate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null))
                .ForMember(d => d.ReportedDate, opt => opt.MapFrom(s => s.Reporteddate.HasValue ? s.Reporteddate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null))
                .ForMember(d => d.PaidDate, opt => opt.MapFrom(s => s.Paiddate.HasValue ? s.Paiddate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null))
                .ForMember(d => d.ConfigId, opt => opt.MapFrom(s => s.Configid))
                .ForMember(d => d.CompanyId, opt => opt.MapFrom(s => s.Config != null && s.Config.Company != null ? s.Config.Company.Id : 0))
                .ForMember(d => d.CompanyName, opt => opt.MapFrom(s => s.Config != null && s.Config.Company != null ? s.Config.Company.Name : string.Empty))
                .ForMember(d => d.CompanyTaxId, opt => opt.MapFrom(s => s.Config != null && s.Config.Company != null ? s.Config.Company.Taxid : string.Empty))
                .ForMember(d => d.ReportTypeId, opt => opt.MapFrom(s => s.Config != null && s.Config.Reporttype != null ? s.Config.Reporttype.Id : 0))
                .ForMember(d => d.ReportTypeName, opt => opt.MapFrom(s => s.Config != null && s.Config.Reporttype != null ? s.Config.Reporttype.Name : string.Empty))
                .ForMember(d => d.ReportTypeShortCode, opt => opt.MapFrom(s => s.Config != null && s.Config.Reporttype != null ? s.Config.Reporttype.Shortcode : string.Empty))
                .ForMember(d => d.FrequencyName, opt => opt.MapFrom(s => s.Config != null && s.Config.Frequency != null ? s.Config.Frequency.Name : string.Empty))
                .ForMember(d => d.DayOfMonth, opt => opt.MapFrom(s => s.Config != null ? s.Config.Dayofmonth : null))
                                .ForMember(d => d.WorkerNames, opt => opt.MapFrom<WorkerNamesResolver>());

            CreateMap<CreateReportInstanceDto, Reportinstance>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Createdat, opt => opt.Ignore())
                .ForMember(d => d.Updatedat, opt => opt.Ignore())
                .ForMember(d => d.Config, opt => opt.Ignore());

            CreateMap<UpdateReportInstanceDto, Reportinstance>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Createdat, opt => opt.Ignore())
                .ForMember(d => d.Updatedat, opt => opt.Ignore())
                .ForMember(d => d.Config, opt => opt.Ignore());

            // ========================================
            // TaskType Mappings
            // ========================================
            CreateMap<Tasktype, TaskTypeDto>();

            CreateMap<CreateTaskTypeDto, Tasktype>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Createdat, opt => opt.Ignore())
                .ForMember(d => d.CompanyTasks, opt => opt.Ignore());

            // ========================================
            // AccountingSystem.Domain.Entities.Task Mappings
            // ========================================
            CreateMap<CompanyTask,CompanyTaskDto>()
     .ForMember(d => d.CompanyName,
         opt => opt.MapFrom(s => s.Company != null ? s.Company.Name : string.Empty))
     .ForMember(d => d.TaskTypeName,
         opt => opt.MapFrom(s => s.Tasktype != null ? s.Tasktype.Name : string.Empty))
     .ForMember(d => d.TaskTypeId,
         opt => opt.MapFrom(s => s.Tasktype != null ? s.Tasktype.Id : 0)) 
     .ForMember(d => d.AssignedWorkerName,
         opt => opt.MapFrom(s => s.Assignedworker != null
             ? $"{s.Assignedworker.Firstname} {s.Assignedworker.Lastname}"
             : string.Empty));

            CreateMap<CreateCompanyTaskDto, CompanyTask>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Createdat, opt => opt.Ignore())
                .ForMember(d => d.Updatedat, opt => opt.Ignore())
                .ForMember(d => d.Company, opt => opt.Ignore())
                .ForMember(d => d.Tasktype, opt => opt.Ignore())
                .ForMember(d => d.Assignedworker, opt => opt.Ignore());

            CreateMap<UpdateCompanyTaskDto, CompanyTask>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Createdat, opt => opt.Ignore())
                .ForMember(d => d.Updatedat, opt => opt.Ignore())
                .ForMember(d => d.Company, opt => opt.Ignore())
                .ForMember(d => d.Tasktype, opt => opt.Ignore())
                .ForMember(d => d.Assignedworker, opt => opt.Ignore());

            // ========================================
            // AuditLog Mappings
            // ========================================
            CreateMap<Auditlog, AuditLogDto>()
                .ForMember(d => d.WorkerName,
                    opt => opt.MapFrom(s => s.Worker != null
                        ? $"{s.Worker.Firstname} {s.Worker.Lastname}"
                        : string.Empty));

            CreateMap<CreateAuditLogDto, Auditlog>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Createdat, opt => opt.Ignore())
                .ForMember(d => d.Worker, opt => opt.Ignore());
        }
    }
}