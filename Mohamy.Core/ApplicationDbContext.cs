using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mohamy.Core.DTO.AuthViewModel.RequesrLog;
using Mohamy.Core.Entity.ApplicationData;
using Mohamy.Core.Entity.ChatData;
using Mohamy.Core.Entity.ConsultingData;
using Mohamy.Core.Entity.Files;
using Mohamy.Core.Entity.LawyerData;
using Mohamy.Core.Entity.Notification;
using Mohamy.Core.Entity.Others;
using Mohamy.Core.Helpers;

namespace Mohamy.Core
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public ApplicationDbContext()
        {
        }
        public DbSet<RequestResponseLog> RequestResponseLogs { get; set; }
        //----------------------------------------------------------------------------------
        public virtual DbSet<Paths> Paths { get; set; }
        public virtual DbSet<Images> Images { get; set; }
        //----------------------------------------------------------------------------------
        public virtual DbSet<Specialties> Specialties { get; set; }
        public virtual DbSet<Experience> Experiences { get; set; }
        public virtual DbSet<Profession> Professions { get; set; }
        public virtual DbSet<lawyerLicense> LawyerLicenses { get; set; }
        public virtual DbSet<graduationCertificate> graduationCertificate { get; set; }
        public virtual DbSet<mainConsulting> MainConsultings { get; set; }
        public virtual DbSet<subConsulting> SubConsultings { get; set; }
        //----------------------------------------------------------------------------------
        public virtual DbSet<Consulting> Consultings { get; set; }
        public virtual DbSet<RequestConsulting> RequestConsultings { get; set; }
        public virtual DbSet<IgnoredConsultation> IgnoredConsultations { get; set; }
        //----------------------------------------------------------------------------------
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Chat> Chats { get; set; }
        //----------------------------------------------------------------------------------
        public virtual DbSet<Evaluation> Evaluations { get; set; }

        public virtual DbSet<City> Cities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                     "Server=148.66.156.172,1433;Database=Mohamy;User Id=Mohamy;Password=Ahmed@123;Encrypt=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.Entity<ApplicationUser>().ToTable("Users", "dbo");
            modelBuilder.Entity<ApplicationRole>().ToTable("Role", "dbo");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRole", "dbo");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim", "dbo");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin", "dbo");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "dbo");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "dbo");
            modelBuilder.Entity<RequestResponseLog>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.RequestUrl).IsRequired().HasMaxLength(2048);
                entity.Property(e => e.HttpMethod).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Timestamp).IsRequired();
            });
            // Configure ApplicationUser
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                // One-to-One relationship with Images (Profile)
                entity.HasOne(u => u.Profile)
                    .WithMany() // No reverse navigation
                    .HasForeignKey(u => u.ProfileId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Experience to SubConsulting relationship
            modelBuilder.Entity<Experience>()
                .HasOne(e => e.subConsulting)
                .WithMany()
                .HasForeignKey(e => e.subConsultingId)
                .OnDelete(DeleteBehavior.Restrict); // Use Restrict, Cascade, or SetNull as needed.

            // Specialties to SubConsulting relationship
            modelBuilder.Entity<Specialties>()
                .HasOne(s => s.mainConsulting)
                .WithMany()
                .HasForeignKey(s => s.mainConsultingId)
                .OnDelete(DeleteBehavior.Restrict);

            // Specialties to SubConsulting relationship
            modelBuilder.Entity<IgnoredConsultation>()
                .HasOne(s => s.consulting)
                .WithMany()
                .HasForeignKey(s => s.consultingId)
                .OnDelete(DeleteBehavior.Restrict);

            // LawyerLicense to ApplicationUser relationship
            modelBuilder.Entity<lawyerLicense>()
                .HasOne(l => l.Lawyer)
                .WithMany()
                .HasForeignKey(l => l.LawyerId)
                .OnDelete(DeleteBehavior.Cascade);

            // GraduationCertificate to ApplicationUser relationship
            modelBuilder.Entity<graduationCertificate>()
                .HasOne(g => g.Lawyer)
                .WithMany(a => a.graduationCertificates)
                .HasForeignKey(g => g.LawyerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure mainConsulting
            modelBuilder.Entity<mainConsulting>(entity =>
            {
                // One-to-One relationship with Images (Icon)
                entity.HasOne(mc => mc.Icon)
                    .WithMany() // No reverse navigation
                    .HasForeignKey(mc => mc.iconId)
                    .OnDelete(DeleteBehavior.Restrict);

                // One-to-Many relationship with subConsulting
                entity.HasMany(mc => mc.SubConsultings)
                    .WithOne(sc => sc.MainConsulting)
                    .HasForeignKey(sc => sc.MainConsultingId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure subConsulting
            modelBuilder.Entity<subConsulting>(entity =>
            {
                // One-to-One relationship with Images (Icon)
                entity.HasOne(sc => sc.Icon)
                    .WithMany() // No reverse navigation
                    .HasForeignKey(sc => sc.iconId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Required field for MainConsulting
                entity.HasOne(sc => sc.MainConsulting)
                    .WithMany(mc => mc.SubConsultings)
                    .HasForeignKey(sc => sc.MainConsultingId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Paths and Images
            modelBuilder.Entity<Paths>(entity =>
            {
                entity.HasMany(p => p.Images)
                    .WithOne(i => i.path)
                    .HasForeignKey(i => i.pathId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Images>(entity =>
            {
                entity.HasOne(i => i.path)
                    .WithMany(p => p.Images)
                    .HasForeignKey(i => i.pathId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<RequestConsulting>(entity =>
            {
                // Configure Lawyer relationship (One-to-Many)
                entity.HasOne(rc => rc.Lawyer)
                      .WithMany(l => l.RequestConsultings)
                      .HasForeignKey(rc => rc.LawyerId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .IsRequired();

                // Configure Consulting relationship (One-to-Many)
                entity.HasOne(rc => rc.Consulting)
                      .WithMany(c => c.RequestConsultings)
                      .HasForeignKey(rc => rc.ConsultingId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired();

                // Configure PriceService column
                entity.Property(rc => rc.PriceService)
                      .HasColumnType("decimal(18,2)");

                // Configure StatusRequestConsulting default value
                entity.Property(rc => rc.statusRequestConsulting)
                      .HasDefaultValue(statusRequestConsulting.Waiting);
            });

            modelBuilder.Entity<Evaluation>()
           .HasOne(e => e.Evaluator)
           .WithMany()
           .HasForeignKey(e => e.EvaluatorId)
           .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Evaluation>()
                .HasOne(e => e.Evaluated)
                .WithMany(u => u.EvaluationsReceived)
                .HasForeignKey(e => e.EvaluatedId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Chat>(entity =>
            {
                // Configure Sender relationship
                entity.HasOne(c => c.Sender)
                      .WithMany()
                      .HasForeignKey(c => c.SenderId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Configure Receiver relationship
                entity.HasOne(c => c.Receiver)
                      .WithMany()
                      .HasForeignKey(c => c.ReceiverId)
                      .OnDelete(DeleteBehavior.Restrict);

            });

        }
    }
}
