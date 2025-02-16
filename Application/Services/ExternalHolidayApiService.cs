using System.Text.Json;
using System.Text.RegularExpressions;
using Core.Exceptions;
using Core.Interfaces;
using Core.ViewModels;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class ExternalHolidayApiService : IExternalHolidayApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public ExternalHolidayApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }
    
    public async Task<List<ExternalHolidayViewModel>> GetHolidaysAsync(string countryCode, int startYear, int endYear)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var baseUrl = _configuration["ExternalHolidayApi:BaseUrl"];
        var holidayFOrDateRangeEndpoint = _configuration["ExternalHolidayApi:getHolidaysForDateRange"];
        var externalApiUrl = $"{baseUrl}/{holidayFOrDateRangeEndpoint}?fromDate={startYear}-01-01&toDate={endYear}-12-31&country={countryCode}&holidayType=all";
        var response = await httpClient.GetAsync(externalApiUrl);
        var responseContent = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            var holidays = JsonSerializer.Deserialize<List<ExternalHolidayViewModel>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return holidays;
        }
        if(response.StatusCode == System.Net.HttpStatusCode.InternalServerError 
           && Regex.IsMatch(responseContent, _configuration["ExternalHolidayApi:DatesForCountryNotSupportedPattern"]))
        {
            throw new DatesNotSupportedException($"Dates {startYear} - {endYear} for country {countryCode} are not supported.");
        }
        if(response.StatusCode == System.Net.HttpStatusCode.NotFound 
           && Regex.IsMatch(responseContent, _configuration["ExternalHolidayApi:CountryNotSupportedPattern"]))
        {
            throw new DatesNotSupportedException($"Country {countryCode} is not supported.");
        }
        throw new Exception($"Unexpected response status code {response.StatusCode}.");
    }
}