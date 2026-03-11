using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Infrastructure.Jobs
{

        public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                var httpContext = context.GetHttpContext();

                return httpContext.User.Identity?.IsAuthenticated == true
                    && httpContext.User.IsInRole("Admin");
            }
        }
    
}
