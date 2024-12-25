using Microsoft.AspNetCore.Identity;
using Mohamy.Core;
using Mohamy.Core.DTO.AuthViewModel.RequesrLog;
using Mohamy.Core.Entity.ApplicationData;
using Mohamy.Core.Entity.ChatData;
using Mohamy.Core.Entity.ConsultingData;
using Mohamy.Core.Entity.Files;
using Mohamy.Core.Entity.LawyerData;
using Mohamy.Core.Entity.Notification;
using Mohamy.Core.Entity.Others;
using Mohamy.RepositoryLayer.Interfaces;

namespace Mohamy.RepositoryLayer.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IBaseRepository<RequestResponseLog> RequestResponseLogRepository { get; set; }
    public IBaseRepository<ApplicationUser> UserRepository { get; set; }
    public IBaseRepository<ApplicationRole> RoleRepository { get; set; }
    public IBaseRepository<IdentityUserRole<string>> UserRoleRepository { get; set; }

    public IBaseRepository<Paths> PathsRepository { get; set; }
    public IBaseRepository<Images> ImagesRepository { get; set; }

    public IBaseRepository<subConsulting> SubConsultingRepository { get; set; }
    public IBaseRepository<mainConsulting> MainConsultingRepository { get; set; }
    public IBaseRepository<Consulting> ConsultingRepository { get; set; }
    public IBaseRepository<RequestConsulting> RequestConsultingRepository { get; set; }
    public IBaseRepository<Notification> NotificationRepository { get; set; }
    public IBaseRepository<Chat> ChatRepository { get; set; }
    public IBaseRepository<Evaluation> EvaluationRepository { get; set; }
    public IBaseRepository<City> CityRepository { get; set; }

    public IBaseRepository<lawyerLicense> lawyerLicenseRepository { get; set; }
    public IBaseRepository<graduationCertificate> graduationCertificateRepository { get; set; }
    public IBaseRepository<Experience> ExperienceRepository { get; set; }
    public IBaseRepository<Specialties> SpecialtiesRepository { get; set; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        RequestResponseLogRepository = new BaseRepository<RequestResponseLog>(context);
        UserRepository = new BaseRepository<ApplicationUser>(context);
        RoleRepository = new BaseRepository<ApplicationRole>(context);
        UserRoleRepository = new BaseRepository<IdentityUserRole<string>>(context);
        PathsRepository = new BaseRepository<Paths>(context);
        ImagesRepository = new BaseRepository<Images>(context);
        SubConsultingRepository = new BaseRepository<subConsulting>(context);
        MainConsultingRepository = new BaseRepository<mainConsulting>(context);
        ConsultingRepository = new BaseRepository<Consulting>(context);
        RequestConsultingRepository = new BaseRepository<RequestConsulting>(context);
        NotificationRepository = new BaseRepository<Notification>(context);
        ChatRepository = new BaseRepository<Chat>(context);
        EvaluationRepository = new BaseRepository<Evaluation>(context);
        CityRepository = new BaseRepository<City>(context);
        ExperienceRepository = new BaseRepository<Experience>(context);
        lawyerLicenseRepository = new BaseRepository<lawyerLicense>(context);
        graduationCertificateRepository = new BaseRepository<graduationCertificate>(context);
        SpecialtiesRepository = new BaseRepository<Specialties>(context);
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}