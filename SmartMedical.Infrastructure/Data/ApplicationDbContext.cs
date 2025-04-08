using Microsoft.EntityFrameworkCore;
using SmartMedical.Core.Entities.Auth;
using SmartMedical.Core.Entities.Users;
using SmartMedical.Core.Entities.HealthRecords;
using SmartMedical.Core.Entities.Medications;
using SmartMedical.Core.Entities.Appointments;
using SmartMedical.Core.Entities.AIAssistant;

namespace SmartMedical.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Auth schema
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        // Users schema
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<EmergencyContact> EmergencyContacts { get; set; }
        public DbSet<HealthcareProvider> HealthcareProviders { get; set; }
        public DbSet<UserProvider> UserProviders { get; set; }

        // Health Records schema
        public DbSet<Condition> Conditions { get; set; }
        public DbSet<Allergy> Allergies { get; set; }
        public DbSet<Immunization> Immunizations { get; set; }
        public DbSet<VitalStat> VitalStats { get; set; }

        // Medications schema
        public DbSet<Medication> Medications { get; set; }
        public DbSet<MedicationSchedule> MedicationSchedules { get; set; }
        public DbSet<MedicationDose> MedicationDoses { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }

        // Appointments schema
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentReminder> AppointmentReminders { get; set; }

        // AI Assistant schema
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<CachedPrompt> CachedPrompts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure schemas
            modelBuilder.HasDefaultSchema("public");
            
            modelBuilder.Entity<User>().ToTable("users", "auth");
            modelBuilder.Entity<Role>().ToTable("roles", "auth");
            modelBuilder.Entity<UserRole>().ToTable("user_roles", "auth");
            modelBuilder.Entity<RefreshToken>().ToTable("refresh_tokens", "auth");

            modelBuilder.Entity<Profile>().ToTable("profiles", "users");
            modelBuilder.Entity<Address>().ToTable("addresses", "users");
            modelBuilder.Entity<EmergencyContact>().ToTable("emergency_contacts", "users");
            modelBuilder.Entity<HealthcareProvider>().ToTable("healthcare_providers", "users");
            modelBuilder.Entity<UserProvider>().ToTable("user_providers", "users");

            modelBuilder.Entity<Condition>().ToTable("conditions", "health_records");
            modelBuilder.Entity<Allergy>().ToTable("allergies", "health_records");
            modelBuilder.Entity<Immunization>().ToTable("immunizations", "health_records");
            modelBuilder.Entity<VitalStat>().ToTable("vital_stats", "health_records");

            modelBuilder.Entity<Medication>().ToTable("medications", "medications");
            modelBuilder.Entity<MedicationSchedule>().ToTable("medication_schedules", "medications");
            modelBuilder.Entity<MedicationDose>().ToTable("medication_doses", "medications");
            modelBuilder.Entity<Prescription>().ToTable("prescriptions", "medications");

            modelBuilder.Entity<Appointment>().ToTable("appointments", "appointments");
            modelBuilder.Entity<AppointmentReminder>().ToTable("appointment_reminders", "appointments");

            modelBuilder.Entity<Conversation>().ToTable("conversations", "ai_assistant");
            modelBuilder.Entity<Message>().ToTable("messages", "ai_assistant");
            modelBuilder.Entity<CachedPrompt>().ToTable("cached_prompts", "ai_assistant");

            // Configure relationships
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserProvider>()
                .HasKey(up => new { up.UserId, up.ProviderId });
        }
    }
}
