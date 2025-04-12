using Microsoft.Extensions.DependencyInjection;
using SmartMedical.Infrastructure;

namespace SmartMedical.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // Use the centralized dependency injection from Infrastructure
            services.AddInfrastructureServices();
            
            return services;
        }
        
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            // Business services are now registered in the Infrastructure.DependencyInjection class
            // This method is kept for backward compatibility
            return services;
        }
    }
}
