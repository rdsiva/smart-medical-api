using Microsoft.Extensions.DependencyInjection;
using SmartMedical.Business.Interfaces;
using SmartMedical.Business.Services;
using SmartMedical.Core.Interfaces;
using SmartMedical.Infrastructure.Repositories;

namespace SmartMedical.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            // Register repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IMedicationRepository, MedicationRepository>();
            services.AddScoped<IConversationRepository, ConversationRepository>();
            services.AddScoped<IHealthRecordsRepository, HealthRecordsRepository>();
            services.AddScoped<IInsuranceRepository, InsuranceRepository>();
            services.AddScoped<ILabResultsRepository, LabResultsRepository>();

            // Register services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IMedicationService, MedicationService>();
            services.AddScoped<IConversationService, ConversationService>();
            services.AddScoped<IHealthRecordsService, HealthRecordsService>();
            services.AddScoped<IInsuranceService, InsuranceService>();
            services.AddScoped<ILabResultsService, LabResultsService>();

            return services;
        }
    }
}
