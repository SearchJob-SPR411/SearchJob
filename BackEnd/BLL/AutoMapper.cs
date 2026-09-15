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
            CreateMap<VacancyEntity, VacancyDto>();

            // DTO -> Entity
            // Don't map PasswordHash from DTOs (they shouldn't carry password hashes)
            CreateMap<UserDto, UserEntity>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<VacancyDto, VacancyEntity>();
        }
    }
}
