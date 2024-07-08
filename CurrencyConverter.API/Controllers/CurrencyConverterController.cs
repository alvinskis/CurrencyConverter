namespace CurrencyConverter.API.Controllers;

using System.Net;
using Application.DTOs;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class CurrencyConverterController : ControllerBase
{
    private readonly ICurrencyConverterService _currencyConverterService;

    public CurrencyConverterController(ICurrencyConverterService currencyConverterService)
    {
        ArgumentNullException.ThrowIfNull(currencyConverterService);
        _currencyConverterService = currencyConverterService;
    }

    [HttpGet]
    public async Task<ActionResult<RateEntryDto>> GetAsync()
    {
        try
        {
            var rateEntryDto = await _currencyConverterService.GetLatestCurrencyRates();
            if (!rateEntryDto.CurrencyRates.Any()) return NotFound("Currency data was not found");
            return Ok(rateEntryDto);
        }
        catch (Exception e)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
        }
    }
}