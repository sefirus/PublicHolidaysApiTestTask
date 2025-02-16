using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/")]
public class HolidayScheduleController : ControllerBase
{
    private readonly ICountryService _service;

    public HolidayScheduleController(ICountryService externalHolidayApiService)
    {
        _service = externalHolidayApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _service.GetAllCountriesAsync());
    }
}