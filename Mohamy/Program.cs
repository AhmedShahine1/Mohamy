using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Mohamy.BusinessLayer.Hubs;
using Mohamy.Core;
using Mohamy.Extensions;
using Mohamy.Middleware;
using System.Globalization;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MohamyFireBase.json"));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSignalR();
builder.Services.AddContextServices(builder.Configuration);
builder.Services.AddMemoryCache();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Cookie configuration
builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromHours(1); // Set cookie timeout to 1 hour
    options.SlidingExpiration = true;              // Reset cookie lifetime with each request
    options.LoginPath = "/Auth/Login";
    options.LogoutPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/AccessDenied";
});
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100 MB
});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin() // Specify the allowed origin
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Add Swagger documentation
builder.Services.AddSwaggerDocumentation();

var app = builder.Build();
// Exception page only for development mode
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    // Ensure you have proper exception handling for production
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Global exception logging middleware
app.Use(async (context, next) =>
{
    try
    {
        await next.Invoke();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Unhandled Exception: {ex.Message}, StackTrace: {ex.StackTrace}");
        throw;
    }
});

// Exception middleware to handle exceptions centrally
app.UseMiddleware<ExceptionMiddleware>();

// Swagger and static files
app.UseSwaggerDocumentation();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapStaticAssets();

app.UseRouting();

// Ensure authentication is used before authorization
app.UseAuthentication();
app.UseAuthorization();

// Apply CORS settings
app.UseCors(); // Apply the CORS policy

// Add custom middlewares
app.UseApplicationMiddleware();
app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.MapHub<ChatHub>("/chathub");
app.MapHub<NotificationHub>("/notificationhub");

// Define endpoints
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Lawyer}/{action=Index}/{id?}");
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Auth}/{action=Login}/{id?}");
    //endpoints.MapHub<ChatHub>("/chathub");
});

// Start the application
app.Run();
