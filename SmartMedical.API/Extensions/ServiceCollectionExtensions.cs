using Microsoft.Extensions.DependencyInjection;
using SmartMedical.Business.Interfaces;
using SmartMedical.Business.Services;
using SmartMedical.Core.Interfaces;
using SmartMedical.Infrastructure.Repositories;

namespace SmartMedical.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
            
            return services;
        }
        
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProfileService, ProfileService>();
            
            return services;
        }
    }
}
