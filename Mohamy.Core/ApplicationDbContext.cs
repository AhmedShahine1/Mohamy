using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
        //----------------------------------------------------------------------------------
        public virtual DbSet<Paths> Paths { get; set; }
        public virtual DbSet<Images> Images { get; set; }
        //----------------------------------------------------------------------------------
        public virtual DbSet<Specialties> Specialties { get; set; }
        public virtual DbSet<lawyerLicense> LawyerLicenses { get; set; }
        public virtual DbSet<graduationCertificate> graduationCertificate { get; set; }
        public virtual DbSet<mainConsulting> MainConsultings { get; set; }
        public virtual DbSet<subConsulting> SubConsultings { get; set; }
        //----------------------------------------------------------------------------------
        public virtual DbSet<Consulting> Consultings { get; set; }
        public virtual DbSet<RequestConsulting> RequestConsultings { get; set; }
        //----------------------------------------------------------------------------------
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Chat> Chats { get; set; }
        //----------------------------------------------------------------------------------
        public virtual DbSet<Evaluation> Evaluations { get; set; }

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

            // Configure ApplicationUser
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                // One-to-One relationship with Images (Profile)
                entity.HasOne(u => u.Profile)
                    .WithMany() // No reverse navigation
                    .HasForeignKey(u => u.ProfileId)
                    .OnDelete(DeleteBehavior.Restrict);

                // One-to-One relationship with lawyerLicense
                entity.HasOne(u => u.lawyerLicense)
                    .WithOne(l => l.Lawyer)
                    .HasForeignKey<ApplicationUser>(u => u.lawyerLicenseId)
                    .OnDelete(DeleteBehavior.Cascade);

                // One-to-Many relationship with graduationCertificates
                entity.HasMany(u => u.graduationCertificates)
                    .WithOne(gc => gc.Lawyer)
                    .HasForeignKey(gc => gc.LawyerId)
                    .OnDelete(DeleteBehavior.Cascade);

                // One-to-Many relationship with Experiences
                entity.HasMany(u => u.Experiences)
                    .WithOne(e => e.Lawyer)
                    .HasForeignKey(e => e.LawyerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure graduationCertificate
            modelBuilder.Entity<graduationCertificate>(entity =>
            {
                entity.HasOne(gc => gc.Lawyer)
                    .WithMany(l => l.graduationCertificates)
                    .HasForeignKey(gc => gc.LawyerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure lawyerLicense
            modelBuilder.Entity<lawyerLicense>(entity =>
            {
                entity.HasOne(l => l.Lawyer)
                    .WithOne(lu => lu.lawyerLicense)
                    .HasForeignKey<lawyerLicense>(l => l.LawyerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            // Configure Specialties
            modelBuilder.Entity<Specialties>()
            .HasKey(ur => new { ur.LawyerId, ur.subConsultingId });

            // Configure Experience
            modelBuilder.Entity<Experience>(entity =>
            {
                entity.HasOne(e => e.Lawyer)
                    .WithMany(l => l.Experiences)
                    .HasForeignKey(e => e.LawyerId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.subConsulting)
                    .WithMany()
                    .HasForeignKey(e => e.subConsultingId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

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
