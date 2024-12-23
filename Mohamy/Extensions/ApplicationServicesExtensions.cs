using AutoMapper;
using Mohamy.BusinessLayer.AutoMapper;
using Mohamy.BusinessLayer.Hubs;
using Mohamy.BusinessLayer.Interfaces;
using Mohamy.BusinessLayer.Services;
using Mohamy.RepositoryLayer.Interfaces;
using Mohamy.RepositoryLayer.Repositories;

namespace Mohamy.Extensions;

public static class ApplicationServicesExtensions
{
    // interfaces sevices [IAccountService, IPhotoHandling  ]
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddHttpContextAccessor();
        services.AddDistributedMemoryCache(); // Add this line to configure the distributed cache

        // Session Service
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromHours(1);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });
        // Configure ChatService with Firebase JSON file path
        //services.AddScoped<IChatService,ChatService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IAccountService, AccountService>();
        services.AddScoped<IConsultingService, ConsultingService>();
        services.AddTransient<IMainConsultingService, MainConsultingService>();
        services.AddTransient<ISubConsultingService, SubConsultingService>();
        services.AddTransient<IRequestConsultingService, RequestConsultingService>();
        services.AddTransient<IEvaluationService, EvaluationService>();
        services.AddTransient<IFileHandling, FileHandling>();
        services.AddTransient<IChatService, ChatService>();
        services.AddTransient<INotificationService, NotificationService>();
        services.AddSignalR();
        services.AddHttpClient();
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddSignalR();
        return services;
    }

    public static IApplicationBuilder UseApplicationMiddleware(this IApplicationBuilder app)
    {
        app.UseSession();
        return app;
    }
}
