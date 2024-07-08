namespace CurrencyConverter.Application.Services;

using Interfaces.Infrastructure;
using AutoMapper;
using Interfaces.Persistence;
using Application.Interfaces.Services;
using Domain.Models;

public class DataUploaderService : IDataUploaderService
{
    private readonly IRateEntryRepository _rateEntryRepository;
    private readonly IDataFetch _dataFetch;
    private readonly IMapper _mapper;

    public DataUploaderService(IRateEntryRepository rateEntryRepository, IDataFetch dataFetch, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(rateEntryRepository);
        ArgumentNullException.ThrowIfNull(dataFetch);
        ArgumentNullException.ThrowIfNull(mapper);

        _rateEntryRepository = rateEntryRepository;
        _dataFetch = dataFetch;
        _mapper = mapper;
    }

    public async Task UploadCurrencyRatesAsync()
    {
        try
        {
            var rateEntryDto = await _dataFetch.FetchDataAsync();
            var rateEntry = _mapper.Map<RateEntry>(rateEntryDto);
            await _rateEntryRepository.AddAsync(rateEntry);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}