using AutoMapper;
using AccountingSystem.Domain.Entities;
using AccountingSystem.Application.DTOs;

namespace AccountingSystem.Application.Mappings.Resolvers
{
    public class WorkerNamesResolver : IValueResolver<Reportinstance, ReportInstanceDetailDto, List<string>>
    {
        public List<string> Resolve(
            Reportinstance source,
            ReportInstanceDetailDto destination,
            List<string> destMember,
            ResolutionContext context)
        {
            var isAdminMode = context.Items.ContainsKey("IsAdminMode") && (bool)context.Items["IsAdminMode"];

            if (!isAdminMode)
            {
                return new List<string>();
            }

            if (source.Config?.Company?.Companyworkers == null)
                return new List<string>();

            return source.Config.Company.Companyworkers
                .Where(cw => cw.Worker != null && cw.Isactive == true)
                .Select(cw => $"{cw.Worker.Firstname} {cw.Worker.Lastname}")
                .OrderBy(name => name)
                .ToList();
        }
    }
}