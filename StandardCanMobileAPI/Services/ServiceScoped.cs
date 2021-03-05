using Microsoft.Extensions.DependencyInjection;
using StandardCanMobileAPI.Services.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardCanMobileAPI.Services
{
    public static class ServiceScoped
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<INotiService, NotiService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ITimeInOutService, TimeInOutService>();
            services.AddScoped<ISettingService, SettingService>();
            services.AddScoped<IBenefitService, BenefitService>();
            services.AddScoped<IPayrollService, PayrollService>();
            services.AddScoped<IMeetingService, MeetingService>();
            services.AddScoped<ITraningService, TraningService>();
            services.AddScoped<ISystemLogService, SystemLogService>();
            services.AddScoped<ICalendarHolidayService, CalendarHolidayService>();
        }
    }
}
