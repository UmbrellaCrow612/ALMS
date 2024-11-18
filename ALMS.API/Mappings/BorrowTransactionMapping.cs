using ALMS.API.Data.Models;
using ALMS.API.DTOs.BorrowTransaction;
using AutoMapper;

namespace ALMS.API.Mappings
{
    public class BorrowTransactionMapping : Profile
    {
        public BorrowTransactionMapping()
        {
            CreateMap<BorrowTransaction, BorrowTransactionDto>();
        }
    }
}
