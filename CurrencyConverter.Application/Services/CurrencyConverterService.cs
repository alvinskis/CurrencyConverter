using CurrencyConverter.Application.Exceptions;

namespace CurrencyConverter.Application.Services;

using DTOs;
using AutoMapper;
using Interfaces.Persistence;
using Interfaces.Services;

public class CurrencyConverterService : ICurrencyConverterService
{
    private readonly IRateEntryRepository _rateEntryRepository;
    private readonly ICurrencyRateRepository _currencyRateRepository;
    private readonly IMapper _mapper;

    public CurrencyConverterService(IRateEntryRepository rateEntryRepository,
        ICurrencyRateRepository currencyRateRepository, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(rateEntryRepository);
        ArgumentNullException.ThrowIfNull(currencyRateRepository);
        ArgumentNullException.ThrowIfNull(mapper);

        _rateEntryRepository = rateEntryRepository;
        _currencyRateRepository = currencyRateRepository;
        _mapper = mapper;
    }

    public async Task<RateEntryDto> GetLatestCurrencyRates()
    {
        try
        {
            var rateEntry = await _rateEntryRepository.GetLatestRecordAsync();
            if (rateEntry is null) throw new CurrencyConverterException("Latest rate entry data is missing");

            var currencyRates = await _currencyRateRepository.GetAllByRateEntryIdAsync(rateEntry.Id);
            if (!currencyRates.Any())
                throw new CurrencyConverterException("Currency rates for the latest entry data are missing");

            return _mapper.Map<RateEntryDto>(rateEntry);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}