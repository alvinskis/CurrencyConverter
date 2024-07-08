namespace CurrencyConverter.Infrastructure.XmlDataFetchService;

using System.Globalization;
using CurrencyConverter.Application.Interfaces.Infrastructure;
using Constants;
using Configs;
using Application.DTOs;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Extensions.Options;

public class XmlDataFetchService : IDataFetch
{
    private readonly FetchDataOptions _fetchDataOptions;

    public XmlDataFetchService(IOptions<FetchDataOptions> fetchDataOptions)
    {
        ArgumentNullException.ThrowIfNull(fetchDataOptions);

        _fetchDataOptions = fetchDataOptions.Value;
    }

    public async Task<RateEntryDto> FetchDataAsync()
    {
        var currencyData = new RateEntryDto();
        var currencyRates = new List<CurrencyRateDto>();
        var xmlReaderSettings = new XmlReaderSettings
        {
            IgnoreWhitespace = true,
            Async = true
        };

        try
        {
            var xmlDocument = XDocument.Load(_fetchDataOptions.DataUrl!);
            using var reader = XmlReader.Create(new StringReader(xmlDocument.ToString()), xmlReaderSettings);

            while (await reader.ReadAsync())
                if (reader.NodeType is XmlNodeType.Element && reader.Name == InfrastructureConstants.XmlElement)
                {
                    if (reader.HasAttributes &&
                        !string.IsNullOrWhiteSpace(reader.GetAttribute(InfrastructureConstants.XmlTimeAttribute)) &&
                        DateOnly.TryParse(reader.GetAttribute(InfrastructureConstants.XmlTimeAttribute),
                            new CultureInfo("en-US"),
                            out var parsedDate))
                        currencyData.Date = parsedDate;

                    var currency = reader.GetAttribute(InfrastructureConstants.XmlCurrencyAttribute);
                    var conversionRate = reader.GetAttribute(InfrastructureConstants.XmlRateAttribute);

                    if (reader.HasAttributes && !string.IsNullOrWhiteSpace(currency) &&
                        !string.IsNullOrWhiteSpace(conversionRate))
                        currencyRates.Add(new CurrencyRateDto
                        {
                            Currency = currency,
                            ConversionRate = double.Parse(conversionRate)
                        });
                }

            currencyData.CurrencyRates = currencyRates;

            return currencyData;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}