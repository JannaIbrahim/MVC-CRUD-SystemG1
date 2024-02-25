using Demo.BLL;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.PL.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            ////services.AddTransient<IDepartmentRepository,DepartmentRepository>();
            //services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            ////services.AddSingleton<IDepartmentRepository, DepartmentRepository>();

            //services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            return services;
        }
    }
}
