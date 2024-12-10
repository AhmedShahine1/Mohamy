using AutoMapper;
using Mohamy.Core.DTO.AuthViewModel;
using Mohamy.Core.DTO.AuthViewModel.FilesModel;
using Mohamy.Core.DTO.AuthViewModel.RegisterModel;
using Mohamy.Core.DTO.AuthViewModel.RoleModel;
using Mohamy.Core.DTO.ConsultingViewModel;
using Mohamy.Core.Entity.ApplicationData;
using Mohamy.Core.Entity.ConsultingData;
using Mohamy.Core.Entity.Files;

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
            // Mapping for ApplicationUser <-> RegisterAdmin
            CreateMap<ApplicationUser, RegisterAdmin>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ReverseMap()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.PhoneNumber));

            //--------------------------------------------------------------------------------------------------------
            // Mapping for ApplicationUser <-> RegisterLawyer
            CreateMap<ApplicationUser, RegisterLawyer>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                //.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                //.ForMember(dest => dest.YearsExperience, opt => opt.MapFrom(src => src.yearsExperience))
                //.ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                //.ForMember(dest => dest.AcademicSpecialization, opt => opt.MapFrom(src => src.academicSpecialization))
                //.ForMember(dest => dest.CostConsulting, opt => opt.MapFrom(src => src.CostConsulting))
                //.ForMember(dest => dest.Education, opt => opt.MapFrom(src => src.Education))
                .ReverseMap()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.PhoneNumber));

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
            ////--------------------------------------------------------------------------------------------------------
            //// Mapping for Experience <-> ExperienceDTO
            //CreateMap<Experience, ExperienceDTO>()
            //    .ForMember(dest => dest.SubConsultings, opt => opt.Ignore()) // Populate this separately if needed
            //    .ReverseMap();

            ////--------------------------------------------------------------------------------------------------------
            //// Mapping for Consulting <-> ConsultingDTO
            //CreateMap<Consulting, ConsultingDTO>()
            //    .ForMember(dest => dest.Files, opt => opt.Ignore()) // Handle file mapping separately
            //    .ForMember(dest => dest.StatusConsultingEnum, opt => opt.MapFrom(src => src.statusConsulting))
            //    .ReverseMap();

            ////--------------------------------------------------------------------------------------------------------
            //// Mapping for RequestConsultingService <-> RequestConsultingDTO
            //CreateMap<RequestConsulting, RequestConsultingDTO>()
            //    .ForMember(dest => dest.StatusRequestConsultingEnum, opt => opt.MapFrom(src => src.statusRequestConsulting))
            //    .ReverseMap();
        }
    }
}
