using ALMS.API.Data.Models;
using ALMS.API.DTOs.Media;
using ALMS.API.DTOs.Subscription;
using AutoMapper;

namespace ALMS.API.Mappings
{
    public class SubscriptionMappingProfile : Profile
    {
        public SubscriptionMappingProfile()
        {
            CreateMap<UpdateSubscriptionDto, Subscription>();
            CreateMap<CreateStripeProductDto, StripeProductEntity>();
            CreateMap<UpdateStripeProductDto, StripeProductEntity>();
        }
    }
}
