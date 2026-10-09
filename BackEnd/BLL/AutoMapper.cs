using AutoMapper;
using BLL.DTO;
using DAL.Entity;

namespace BLL
{

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Entity -> DTO
            CreateMap<UserEntity, UserDto>();
            CreateMap<VacancyEntity, VacancyDto>()
                .ForMember(
                    dest => dest.CompanyName,
                    opt => opt.MapFrom(src => src.Company != null
                        ? src.Company.Name
                        : string.Empty));
            CreateMap<ResumeEntity, ResumeDto>();
            CreateMap<CompanyEntity, CompanyDto>();
            CreateMap<FavoriteVacancyEntity, FavoriteVacancyDto>();
            CreateMap<JobApplicationEntity, JobApplicationDto>()
                .ForMember(
                    dest => dest.VacancyTitle,
                    opt => opt.MapFrom(src => src.Vacancy != null ? src.Vacancy.Title : string.Empty))
                .ForMember(
                    dest => dest.CompanyName,
                    opt => opt.MapFrom(src => src.Vacancy != null && src.Vacancy.Company != null ? src.Vacancy.Company.Name : string.Empty))
                .ForMember(
                    dest => dest.Location,
                    opt => opt.MapFrom(src => src.Vacancy != null ? src.Vacancy.Location : string.Empty))
                .ForMember(
                    dest => dest.ApplicantName,
                    opt => opt.MapFrom(src => src.User != null ? $"{src.User.FirstName} {src.User.LastName}".Trim() : string.Empty))
                .ForMember(
                    dest => dest.ApplicantEmail,
                    opt => opt.MapFrom(src => src.User != null ? src.User.Email : string.Empty))
                .ForMember(
                    dest => dest.ApplicantPhone,
                    opt => opt.MapFrom(src => src.User != null ? src.User.PhoneNumber : null))
                .ForMember(
                    dest => dest.ResumeTitle,
                    opt => opt.MapFrom(src => src.Resume != null ? src.Resume.Title : null));

            // DTO -> Entity
            // Don't map PasswordHash from DTOs (they shouldn't carry password hashes)
            CreateMap<UserDto, UserEntity>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<VacancyDto, VacancyEntity>();
            CreateMap<CompanyDto, CompanyEntity>();

        }
    }
}
