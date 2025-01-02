using AutoMapper;
using Mohamy.Core.DTO.AuthViewModel;
using Mohamy.Core.DTO.AuthViewModel.FilesModel;
using Mohamy.Core.DTO.AuthViewModel.LawyerDetailsModel;
using Mohamy.Core.DTO.AuthViewModel.RegisterModel;
using Mohamy.Core.DTO.AuthViewModel.RoleModel;
using Mohamy.Core.DTO.CityViewModel;
using Mohamy.Core.DTO.ConsultingViewModel;
using Mohamy.Core.Entity.ApplicationData;
using Mohamy.Core.Entity.ConsultingData;
using Mohamy.Core.Entity.Files;
using Mohamy.Core.Entity.LawyerData;
using Mohamy.Core.Entity.Others;

namespace Mohamy.BusinessLayer.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //--------------------------------------------------------------------------------------------------------
            // Mapping for RoleDTO <-> ApplicationRole
            CreateMap<RoleDTO, ApplicationRole>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.RoleName))
                .ForMember(dest => dest.ArName, opt => opt.MapFrom(src => src.RoleAr))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.RoleDescription))
                .ReverseMap();

            //--------------------------------------------------------------------------------------------------------
            // Mapping for Paths <-> PathsModel
            CreateMap<Paths, PathsModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ReverseMap();

            //--------------------------------------------------------------------------------------------------------
            // Mapping for City <-> CityDTO
            CreateMap<City, CityDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();

            //--------------------------------------------------------------------------------------------------------
            // Mapping for ApplicationUser <-> RegisterAdmin
            CreateMap<ApplicationUser, RegisterAdmin>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ReverseMap()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.PhoneNumber));

            //--------------------------------------------------------------------------------------------------------
            // Map RegisterLawyer DTO to ApplicationUser
            CreateMap<RegisterLawyer, ApplicationUser>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.RegistrationDate, opt => opt.Ignore()) // Set in the service
                .ForMember(dest => dest.professionalAccreditation, opt => opt.Ignore()) // Set in the service
                .ForMember(dest => dest.Profile, opt => opt.Ignore()) // Handle profile image separately
                .ForMember(dest => dest.Experiences, opt => opt.Ignore())
                .ForMember(dest => dest.lawyerLicense, opt => opt.Ignore())
                .ForMember(dest => dest.graduationCertificates, opt => opt.Ignore())
                .ForMember(dest => dest.Specialties, opt => opt.Ignore()) 
                .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore()); // Set in the service

            // Map GraduationCertificateDTO to graduationCertificate
            CreateMap<GraduationCertificateDTO, graduationCertificate>()
                .ForMember(dest => dest.Start, opt => opt.MapFrom(src => src.Start))
                .ForMember(dest => dest.End, opt => opt.MapFrom(src => src.End))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Collage, opt => opt.MapFrom(src => src.Collage))
                .ForMember(dest => dest.University, opt => opt.MapFrom(src => src.University))
                .ForMember(dest => dest.LawyerId, opt => opt.Ignore()); // Set in the service
            //--------------------------------------------------------------------------------------------------------
            // Mapping for ApplicationUser <-> RegisterCustomer
            CreateMap<ApplicationUser, RegisterCustomer>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ReverseMap()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.PhoneNumber));

            //--------------------------------------------------------------------------------------------------------
            // Mapping for ApplicationUser <-> RegisterSupportDeveloper
            CreateMap<ApplicationUser, RegisterSupportDeveloper>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ReverseMap()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.PhoneNumber));

            //--------------------------------------------------------------------------------------------------------
            // Mapping for ApplicationUser <-> AuthDTO
            CreateMap<ApplicationUser, AuthDTO>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                //.ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                //.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                //.ForMember(dest => dest.Education, opt => opt.MapFrom(src => src.Education))
                //.ForMember(dest => dest.academicSpecialization, opt => opt.MapFrom(src => src.academicSpecialization))
                //.ForMember(dest => dest.yearsExperience, opt => opt.MapFrom(src => src.yearsExperience))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.ProfileImage, opt => opt.Ignore()) // Manually handle file uploads
                .ForMember(dest => dest.ProfileImageId, opt => opt.MapFrom(src => src.ProfileId))
                //.ForMember(dest => dest.lawyerLicenseId, opt => opt.MapFrom(src => src.lawyerLicenseId))
                //.ForMember(dest => dest.graduationCertificateId, opt => opt.MapFrom(src => src.graduationCertificateId))
                //.ForMember(dest => dest.numberConsulting, opt => opt.MapFrom(src => src.LawyerConsultings.Count))
                .ReverseMap();

            //--------------------------------------------------------------------------------------------------------
            // Mapping for ApplicationUser <-> LawyerDTO
            CreateMap<ApplicationUser, LawyerDTO>()
                .ForMember(dest => dest.ConsultingPrice, opt => opt.MapFrom(src => src.PriceService))
                .ForMember(dest => dest.ProfileImage, opt => opt.Ignore())
                .ReverseMap();

            //--------------------------------------------------------------------------------------------------------
            // Mapping for MainConsulting <-> MainConsultingDTO
            CreateMap<MainConsultingDTO, mainConsulting>()
                .ForMember(dest => dest.Icon, opt => opt.Ignore()) // Handle file uploads separately
                .ForMember(dest => dest.SubConsultings, opt => opt.Ignore())
                .ReverseMap();
            //--------------------------------------------------------------------------------------------------------
            // Mapping for SubConsulting <-> SubConsultingDTO
            CreateMap<SubConsultingDTO, subConsulting>()
                .ForMember(dest => dest.MainConsultingId, opt => opt.MapFrom(src => src.MainConsultingId))
                .ForMember(dest => dest.Icon, opt => opt.Ignore()) // Handle file uploads separately
                .ReverseMap();

            //--------------------------------------------------------------------------------------------------------
            // Mapping for Consulting <-> ConsultingDTO
            CreateMap<Consulting, ConsultingDTO>()
                .ForMember(dest => dest.Files, opt => opt.Ignore()) // Handle file mapping separately
                .ForMember(dest => dest.StatusConsultingEnum, opt => opt.MapFrom(src => src.statusConsulting))
                .ForMember(dest => dest.LaywerId, opt => opt.MapFrom(src => src.LawyerId))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ReverseMap();

            //--------------------------------------------------------------------------------------------------------
            // Mapping for RequestConsultingService <-> RequestConsultingDTO
            CreateMap<RequestConsulting, RequestConsultingDTO>()
                .ForMember(dest => dest.StatusRequestConsultingEnum, opt => opt.MapFrom(src => src.statusRequestConsulting))
                .ReverseMap();
            //--------------------------------------------------------------------------------------------------------
            CreateMap<ApplicationUser, AuthDTO>()
           .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
           .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
           .ForMember(dest => dest.ProfileImage, opt => opt.Ignore()) // Manually handle file uploads
           .ForMember(dest => dest.ProfileImageId, opt => opt.MapFrom(src => src.ProfileId))
           .ForMember(dest => dest.lawyerLicenseId, opt => opt.MapFrom(src => src.lawyerLicense.Id))
           .ForMember(dest => dest.lawyerLicenseNumber, opt => opt.MapFrom(src => src.lawyerLicense.LicenseNumber))
           .ForMember(dest => dest.lawyerLicenseState, opt => opt.MapFrom(src => src.lawyerLicense.State))
           .ForMember(dest => dest.lawyerLicenseStart, opt => opt.MapFrom(src => src.lawyerLicense.Start))
           .ForMember(dest => dest.lawyerLicenseEnd, opt => opt.MapFrom(src => src.lawyerLicense.End))
           .ForMember(dest => dest.GraduationCertificates, opt => opt.MapFrom(src => src.graduationCertificates))
           .ForMember(dest => dest.Specialties, opt => opt.MapFrom(src => src.Specialties))
           .ForMember(dest => dest.Experiences, opt => opt.MapFrom(src => src.Experiences))
           .ReverseMap();
            //--------------------------------------------------------------------------------------------------------
            CreateMap<graduationCertificate, GraduationCertificateDTO>();
            //--------------------------------------------------------------------------------------------------------
            CreateMap<Specialties, SpecialtiesDTO>()
                .ForMember(dest => dest.subConsultingName, opt => opt.MapFrom(src => src.subConsulting.Name));
            //--------------------------------------------------------------------------------------------------------
            CreateMap<Experience, Core.DTO.AuthViewModel.LawyerDetailsModel.ExperienceDTO>()
                .ForMember(dest => dest.subConsultingName, opt => opt.MapFrom(src => src.subConsulting.Name));
        }
    }
}
