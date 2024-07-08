namespace CurrencyConverter.Application.UnitTests.Services;

using AutoMapper;
using DTOs;
using Interfaces.Infrastructure;
using Interfaces.Persistence;
using CurrencyConverter.Application.Services;
using Domain.Models;
using Moq;

public class DataUploaderServiceTests
{
    private readonly Mock<IRateEntryRepository> _rateEntryRepository = new();
    private readonly Mock<IDataFetch> _dataFetch = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly DataUploaderService _dataUploaderService;

    public DataUploaderServiceTests()
    {
        _dataUploaderService = new DataUploaderService(_rateEntryRepository.Object, _dataFetch.Object, _mapper.Object);
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionWhenRateEntryRepositoryIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new DataUploaderService(null!, _dataFetch.Object, _mapper.Object));
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionWhenDataFetchIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new DataUploaderService(_rateEntryRepository.Object, null!, _mapper.Object));
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionWhenMapperIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new DataUploaderService(_rateEntryRepository.Object, _dataFetch.Object, null!));
    }

    [Fact]
    public async Task UploadCurrencyRatesAsyncShouldPerformAddOperationWhenFetchedDataExists()
    {
        _dataFetch.Setup(x => x.FetchDataAsync()).ReturnsAsync(new RateEntryDto());
        _mapper.Setup(x => x.Map<RateEntry>(It.IsAny<RateEntryDto>())).Returns(new RateEntry());
        _rateEntryRepository.Setup(x => x.AddAsync(It.IsAny<RateEntry>()));

        await _dataUploaderService.UploadCurrencyRatesAsync();

        _dataFetch.Verify(x => x.FetchDataAsync(), Times.Once);
        _mapper.Verify(x => x.Map<RateEntry>(It.IsAny<RateEntryDto>()), Times.Once);
        _rateEntryRepository.Verify(x => x.AddAsync(It.IsAny<RateEntry>()), Times.Once);
    }

    [Fact]
    public async Task UploadCurrencyRatesAsyncShouldThrowExceptionWhenFails()
    {
        _dataFetch.Setup(x => x.FetchDataAsync()).ThrowsAsync(new Exception());

        await Assert.ThrowsAsync<Exception>(() => _dataUploaderService.UploadCurrencyRatesAsync());
    }
}