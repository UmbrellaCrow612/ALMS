using ALMS.API.Data.Models;
using ALMS.API.DTOs.Reservation;
using AutoMapper;

namespace ALMS.API.Mappings
{
    public class ReservationMapping : Profile
    {
        public ReservationMapping()
        {
            CreateMap<CreateReservationDto, Reservation>();
            CreateMap<Reservation, ReservationDto>();
        }
    }
}
