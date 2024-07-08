namespace CurrencyConverter.Infrastructure.UnitTests.XmlDataFetchService;

using Configs;
using Microsoft.Extensions.Options;
using Moq;

public class XmlDataFetchServiceTests
{
    private readonly Mock<IOptions<FetchDataOptions>> _fetchDataOptions = new();

    [Fact]
    public void ShouldThrowArgumentNullExceptionWhenFetchDataOptionsIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new Infrastructure.XmlDataFetchService.XmlDataFetchService(_fetchDataOptions.Object));
    }

    [Fact]
    public async Task FetchDataAsyncShouldThrowFileNotFoundExceptionWhenInvalidUrl()
    {
        var fetchDataOptionsMock = new Mock<IOptions<FetchDataOptions>>();
        fetchDataOptionsMock.Setup(x => x.Value).Returns(new FetchDataOptions
        {
            DataUrl = "InvalidUrl",
            FrequencyInHours = 24
        });
        var xmlDataFetchService =
            new Infrastructure.XmlDataFetchService.XmlDataFetchService(fetchDataOptionsMock.Object);

        await Assert.ThrowsAsync<FileNotFoundException>(() => xmlDataFetchService.FetchDataAsync());
    }

    [Fact]
    public async Task FetchDataAsyncShouldReturnDataWhenValidUrl()
    {
        var fetchDataOptionsMock = new Mock<IOptions<FetchDataOptions>>();
        fetchDataOptionsMock.Setup(x => x.Value).Returns(new FetchDataOptions
        {
            DataUrl = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml",
            FrequencyInHours = 24
        });
        var xmlDataFetchService =
            new Infrastructure.XmlDataFetchService.XmlDataFetchService(fetchDataOptionsMock.Object);

        var result = await xmlDataFetchService.FetchDataAsync();

        Assert.NotNull(result);
        Assert.NotEmpty(result.CurrencyRates);
    }
}