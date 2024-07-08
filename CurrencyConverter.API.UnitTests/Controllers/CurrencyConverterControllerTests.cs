namespace CurrencyConverter.API.UnitTests.Controllers;

using System.Net;
using CurrencyConverter.API.Controllers;
using Application.DTOs;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

public class CurrencyConverterControllerTests
{
    private readonly Mock<ICurrencyConverterService> _currencyConverterService = new();
    private readonly CurrencyConverterController _currencyConverterController;

    public CurrencyConverterControllerTests()
    {
        _currencyConverterController = new CurrencyConverterController(_currencyConverterService.Object);
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionWhenCurrencyConverterServiceIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new CurrencyConverterController(null!));
    }

    [Fact]
    public async Task GetAsyncShouldReturnOkWhenDataExists()
    {
        var rateEntryDto = new RateEntryDto
        {
            Date = new DateOnly(),
            CurrencyRates = new List<CurrencyRateDto>
            {
                new()
                {
                    Currency = "USD",
                    ConversionRate = 1.1
                }
            }
        };
        _currencyConverterService.Setup(x => x.GetLatestCurrencyRates()).ReturnsAsync(rateEntryDto);

        var result = await _currencyConverterController.GetAsync();

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetAsyncShouldReturnNotFoundWhenDataDoNotExists()
    {
        _currencyConverterService.Setup(x => x.GetLatestCurrencyRates()).ReturnsAsync(new RateEntryDto());

        var result = await _currencyConverterController.GetAsync();

        Assert.NotNull(result);
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetAsyncShouldReturnInternalServerErrorOnException()
    {
        _currencyConverterService.Setup(x => x.GetLatestCurrencyRates()).ThrowsAsync(new Exception());

        var result = await _currencyConverterController.GetAsync();

        Assert.NotNull(result);
        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
    }
}