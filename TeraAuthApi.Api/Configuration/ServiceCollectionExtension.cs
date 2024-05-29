using FluentValidation;
using TeraAuthApi.Application.Common;
using TeraAuthApi.Application.Repository.Interfaces;
using TeraAuthApi.Application.Service;
using TeraAuthApi.Application.Service.Implementations;
using TeraAuthApi.Application.Service.Interfaces;
using TeraAuthApi.Application.Settings;
using TeraAuthApi.Application.UnitOfWork;
using TeraAuthApi.Infrastructure.Repository;
using TeraAuthApi.Infrastructure.UnitOfWork;

namespace TeraAuthApi.Api.Configuration;

internal static class ServiceCollectionExtension
{
    public static void AddServices(this IServiceCollection services)
    {
        //AutoMapper
        services.AddAutoMapper(typeof(IApplicationLayerAssemblyMarker).Assembly);
        //FluentValidation
        services.AddValidatorsFromAssemblyContaining<IApplicationLayerAssemblyMarker>();
        
        //Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IRoleService, RoleService>();
        
        //Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        
        //UnitOfWork
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();

    }
    public static void AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(typeof(JwtSettings).Name));
        services.Configure<SmtpSettings>(configuration.GetSection(typeof(SmtpSettings).Name));
    }
}