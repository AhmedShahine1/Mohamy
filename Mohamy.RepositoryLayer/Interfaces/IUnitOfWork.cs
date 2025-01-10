using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mohamy.Core.DTO.AuthViewModel.RequesrLog;
using Mohamy.Core.Entity.ApplicationData;
using Mohamy.Core.Entity.ChatData;
using Mohamy.Core.Entity.ConsultingData;
using Mohamy.Core.Entity.Files;
using Mohamy.Core.Entity.LawyerData;
using Mohamy.Core.Entity.Notification;
using Mohamy.Core.Entity.Others;

namespace Mohamy.RepositoryLayer.Interfaces;

public interface IUnitOfWork : IDisposable
{
    public IBaseRepository<RequestResponseLog> RequestResponseLogRepository { get; }
    public IBaseRepository<ApplicationUser> UserRepository { get; }
    public IBaseRepository<ApplicationRole> RoleRepository { get;}
    public IBaseRepository<IdentityUserRole<string>> UserRoleRepository { get;}
    public IBaseRepository<Paths> PathsRepository { get; }
    public IBaseRepository<Images> ImagesRepository { get; }
    public IBaseRepository<subConsulting> SubConsultingRepository { get; }
    public IBaseRepository<mainConsulting> MainConsultingRepository { get; }
    public IBaseRepository<Experience> ExperienceRepository { get; }
    public IBaseRepository<Consulting> ConsultingRepository { get; }
    public IBaseRepository<RequestConsulting> RequestConsultingRepository { get; }
    public IBaseRepository<Notification> NotificationRepository { get; }
    public IBaseRepository<Chat> ChatRepository { get; }
    public IBaseRepository<Evaluation> EvaluationRepository { get; }
    public IBaseRepository<City> CityRepository { get; }
    public IBaseRepository<lawyerLicense> lawyerLicenseRepository { get; }
    public IBaseRepository<graduationCertificate> graduationCertificateRepository { get; }
    public IBaseRepository<Specialties> SpecialtiesRepository { get; }
    public IBaseRepository<IgnoredConsultation> IgnoredConsultationsRepository { get; }
    public IBaseRepository<Profession> ProfessionsRepository { get; }

    //-----------------------------------------------------------------------------------
    int SaveChanges();

    Task<int> SaveChangesAsync();
}