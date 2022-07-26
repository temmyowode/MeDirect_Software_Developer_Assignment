
using AutoMapper;
using CurrencyXchange.Data.Enitity;
using CurrencyXchange.Data.Model;

namespace CurrencyXchange.Data.AutoMapper
{
    public class MyMappingProfiles : Profile
    {
        public MyMappingProfiles()
        {
            CreateMap<TransactionViewModel, TransactionHistory>().ReverseMap();
        }
    }
}

