using System.Text.Json.Serialization;
using Mohamy.BusinessLayer.Interfaces;
using Mohamy.BusinessLayer.Services;
using Mohamy.Core;
using Mohamy.RepositoryLayer.Interfaces;
using Mohamy.RepositoryLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Mohamy.Extensions;

public static class ContextServicesExtensions
{
    public static IServiceCollection AddContextServices(this IServiceCollection services, IConfiguration config)
    {

        //- context && json services
        services.AddDbContextPool<ApplicationDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
        services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        services.AddControllersWithViews();
        services.AddScoped<IRequestResponseService, RequestResponseService>();
        // IBaseRepository && IUnitOfWork Service
        //services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>)); // only Repository
        services.AddTransient<IUnitOfWork, UnitOfWork>(); // Repository and UnitOfWork
        //services.AddHostedService<ConsultingStatusCheckerService>();
        //services.AddHostedService<ChatBackgroundService>();

        return services;
    }

}
