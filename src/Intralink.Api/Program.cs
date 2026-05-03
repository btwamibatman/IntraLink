using Data;
using Data.Repositories;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using Application.Interfaces;
using Application.Services;
using Application.Users;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/app-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Starting API host");

    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddSwaggerGen();
    builder.Services.AddControllers();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IPasswordHasher<UserAccount>, PasswordHasher<UserAccount>>();
    builder.Services.AddProblemDetails();
    builder.Services.AddValidatorsFromAssemblyContaining<Program>();
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("client", policy =>
        {
            policy.WithOrigins(
                    "http://localhost:5173",
                    "http://127.0.0.1:5173")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            var logger = context.RequestServices
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger("GlobalExceptionHandler");

            var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
            var exception = exceptionFeature?.Error;

            if (exception is not null)
            {
                logger.LogError(
                    exception,
                    "Unhandled exception for {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);
            }

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Detail = "An unexpected error occurred. Check logs for details."
            });
        });
    });

    app.UseCors("client");
    app.UseHttpsRedirection();
    app.MapGet("/", () => "Server is running.");
    app.MapControllers();
    app.Run();
}
catch (HostAbortedException)
{
    // EF Core tools abort the host after discovering the DbContext at design time.
}
catch (Exception ex)
{
    Log.Fatal(ex, "API host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
