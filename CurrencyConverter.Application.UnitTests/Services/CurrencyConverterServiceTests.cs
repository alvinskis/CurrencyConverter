using CurrencyConverter.Application.Exceptions;

namespace CurrencyConverter.Application.UnitTests.Services;

using AutoMapper;
using DTOs;
using Interfaces.Persistence;
using CurrencyConverter.Application.Services;
using Domain.Models;
using Moq;

public class CurrencyConverterServiceTests
{
    private readonly Mock<IRateEntryRepository> _rateEntryRepository = new();
    private readonly Mock<ICurrencyRateRepository> _currencyRateRepository = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly CurrencyConverterService _currencyConverterService;

    public CurrencyConverterServiceTests()
    {
        _currencyConverterService = new CurrencyConverterService(_rateEntryRepository.Object,
            _currencyRateRepository.Object, _mapper.Object);
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionWhenRateEntryRepositoryIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new CurrencyConverterService(null!, _currencyRateRepository.Object, _mapper.Object));
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionWhenCurrencyRateRepositoryIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new CurrencyConverterService(_rateEntryRepository.Object, null!, _mapper.Object));
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionWhenMapperIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new CurrencyConverterService(_rateEntryRepository.Object, _currencyRateRepository.Object, null!));
    }

    [Fact]
    public async Task GetLatestCurrencyRatesShouldReturnRateEntryDtoWhenDataExists()
    {
        var rateEntry = new RateEntry
        {
            Date = new DateOnly(2024, 01, 01),
            CurrencyRates = new List<CurrencyRate>
            {
                new()
                {
                    Currency = "USD",
                    ConversionRate = 1.1
                }
            }
        };
        var expectedRateEntryDto = new RateEntryDto
        {
            Date = new DateOnly(2024, 01, 01),
            CurrencyRates = new List<CurrencyRateDto>
            {
                new()
                {
                    Currency = "USD",
                    ConversionRate = 1.1
                }
            }
        };
        _rateEntryRepository.Setup(x => x.GetLatestRecordAsync()).ReturnsAsync(rateEntry);
        _currencyRateRepository.Setup(x => x.GetAllByRateEntryIdAsync(It.IsAny<int>()))
            .ReturnsAsync(rateEntry.CurrencyRates);
        _mapper.Setup(x => x.Map<RateEntryDto>(It.IsAny<RateEntry>())).Returns(expectedRateEntryDto);

        var result = await _currencyConverterService.GetLatestCurrencyRates();

        Assert.NotNull(result);
        Assert.Equal(expectedRateEntryDto.Date, result.Date);
        Assert.Equal(expectedRateEntryDto.CurrencyRates, result.CurrencyRates);
    }

    [Fact]
    public async Task GetLatestCurrencyRatesShouldThrowCurrencyConverterExceptionWhenRateEntryMissing()
    {
        _rateEntryRepository.Setup(x => x.GetLatestRecordAsync()).ReturnsAsync((RateEntry)null!);

        var exception =
            await Assert.ThrowsAsync<CurrencyConverterException>(() =>
                _currencyConverterService.GetLatestCurrencyRates());
        Assert.Equal("Latest rate entry data is missing", exception.Message);
    }

    [Fact]
    public async Task GetLatestCurrencyRatesShouldThrowCurrencyConverterExceptionWhenDataCurrencyRatesMissing()
    {
        _rateEntryRepository.Setup(x => x.GetLatestRecordAsync()).ReturnsAsync(new RateEntry());
        _currencyRateRepository.Setup(x => x.GetAllByRateEntryIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new List<CurrencyRate>());

        var exception =
            await Assert.ThrowsAsync<CurrencyConverterException>(() =>
                _currencyConverterService.GetLatestCurrencyRates());
        Assert.Equal("Currency rates for the latest entry data are missing", exception.Message);
    }
}