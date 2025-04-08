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
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Auth entities
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        // User entities
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<EmergencyContact> EmergencyContacts { get; set; }
        public DbSet<HealthcareProvider> HealthcareProviders { get; set; }
        public DbSet<UserProvider> UserProviders { get; set; }

        // Health record entities
        public DbSet<Condition> Conditions { get; set; }
        public DbSet<Allergy> Allergies { get; set; }
        public DbSet<Immunization> Immunizations { get; set; }
        public DbSet<VitalStat> VitalStats { get; set; }

        // Medication entities
        public DbSet<Medication> Medications { get; set; }
        public DbSet<MedicationSchedule> MedicationSchedules { get; set; }
        public DbSet<MedicationDose> MedicationDoses { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }

        // Appointment entities
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentReminder> AppointmentReminders { get; set; }

        // AI Assistant entities
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<CachedPrompt> CachedPrompts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and constraints
            
            // Auth configurations
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId);

            // User configurations
            modelBuilder.Entity<Profile>()
                .HasOne(p => p.User)
                .WithOne()
                .HasForeignKey<Profile>(p => p.UserId);

            modelBuilder.Entity<Address>()
                .HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<EmergencyContact>()
                .HasOne(ec => ec.User)
                .WithMany(u => u.EmergencyContacts)
                .HasForeignKey(ec => ec.UserId);

            modelBuilder.Entity<UserProvider>()
                .HasKey(up => new { up.UserId, up.ProviderId });

            modelBuilder.Entity<UserProvider>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserProviders)
                .HasForeignKey(up => up.UserId);

            modelBuilder.Entity<UserProvider>()
                .HasOne(up => up.Provider)
                .WithMany(p => p.UserProviders)
                .HasForeignKey(up => up.ProviderId);

            // Health record configurations
            modelBuilder.Entity<Condition>()
                .HasOne(c => c.User)
                .WithMany(u => u.Conditions)
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Allergy>()
                .HasOne(a => a.User)
                .WithMany(u => u.Allergies)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<Immunization>()
                .HasOne(i => i.User)
                .WithMany(u => u.Immunizations)
                .HasForeignKey(i => i.UserId);

            modelBuilder.Entity<VitalStat>()
                .HasOne(vs => vs.User)
                .WithMany(u => u.VitalStats)
                .HasForeignKey(vs => vs.UserId);

            // Medication configurations
            modelBuilder.Entity<Medication>()
                .HasOne(m => m.User)
                .WithMany(u => u.Medications)
                .HasForeignKey(m => m.UserId);

            modelBuilder.Entity<MedicationSchedule>()
                .HasOne(ms => ms.Medication)
                .WithMany(m => m.Schedules)
                .HasForeignKey(ms => ms.MedicationId);

            modelBuilder.Entity<MedicationDose>()
                .HasOne(md => md.Schedule)
                .WithMany(ms => ms.Doses)
                .HasForeignKey(md => md.ScheduleId);

            modelBuilder.Entity<Prescription>()
                .HasOne(p => p.Medication)
                .WithMany(m => m.Prescriptions)
                .HasForeignKey(p => p.MedicationId);

            // Appointment configurations
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.User)
                .WithMany(u => u.Appointments)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Provider)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.ProviderId);

            modelBuilder.Entity<AppointmentReminder>()
                .HasOne(ar => ar.Appointment)
                .WithMany(a => a.Reminders)
                .HasForeignKey(ar => ar.AppointmentId);

            // AI Assistant configurations
            modelBuilder.Entity<Conversation>()
                .HasOne(c => c.User)
                .WithMany(u => u.Conversations)
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Conversation)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ConversationId);
        }
    }
}
