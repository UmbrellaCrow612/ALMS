using ALMS.API.Data.Models;
using ALMS.API.DTOs.Users;
using AutoMapper;

namespace ALMS.API.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<CreateUserDto, ApplicationUser>();
            CreateMap<UpdateUserDto, ApplicationUser>();
            CreateMap<ApplicationUser, UserDto>();
        }
    }
}
