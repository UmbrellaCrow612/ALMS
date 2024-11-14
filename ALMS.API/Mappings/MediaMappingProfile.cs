using ALMS.API.Data.Models;
using ALMS.API.DTOs.Media;
using AutoMapper;

namespace ALMS.API.Mappings
{
    public class MediaMappingProfile : Profile
    {
        public MediaMappingProfile()
        {
            CreateMap<CreateMediaDto, Media>();
            CreateMap<UpdateMediaDto, Media>();
            CreateMap<Media, MediaDto>();
        }
    }
}
