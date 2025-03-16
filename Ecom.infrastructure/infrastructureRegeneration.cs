using Ecom.Core.Interfaces;
using Ecom.infrastructure.Data;
using Ecom.infrastructure.Repositories;
using Ecom.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class InfrastructureRegeneration
{
    public static IServiceCollection InfrastructureConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        // تسجيل الـ Generic Repository
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        // تسجيل الـ UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // تسجيل DbContext مع SQL Server
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        return services;
    }
}
