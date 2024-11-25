using ALMS.API.Data.Models;
using ALMS.API.DTOs.YSubscription;
using AutoMapper;

namespace ALMS.API.Mappings
{
    public class YSubscriptionMapping : Profile
    {
        public YSubscriptionMapping()
        {
            CreateMap<CreateYSubscriptionDto, YSubscription>();
            CreateMap<UpdateYSubscriptionDto, YSubscription>();
            CreateMap<YSubscription, YSubscriptionDto>();
        }
    }
}
