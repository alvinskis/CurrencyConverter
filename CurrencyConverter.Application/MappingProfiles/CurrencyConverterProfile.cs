using AutoMapper;
using CurrencyConverter.Application.DTOs;
using CurrencyConverter.Domain.Models;

namespace CurrencyConverter.Application.MappingProfiles;

public class CurrencyConverterProfile : Profile
{
    public CurrencyConverterProfile()
    {
        CreateMap<RateEntry, RateEntryDto>().ReverseMap();
        CreateMap<CurrencyRate, CurrencyRateDto>().ReverseMap();
    }
}