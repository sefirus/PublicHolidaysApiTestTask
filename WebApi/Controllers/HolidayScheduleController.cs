using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/")]
public class HolidayScheduleController : ControllerBase
{
    private readonly ICountryService _service;
    private readonly IHolidayScheduleService _holidayScheduleService;

    public HolidayScheduleController(ICountryService externalHolidayApiService, IHolidayScheduleService holidayScheduleService)
    {
        _service = externalHolidayApiService;
        _holidayScheduleService = holidayScheduleService;
    }

    [HttpGet("countries")]
    public async Task<IActionResult> Get()
    {
        var countries = await _service.GetAllCountries();
        return Ok(countries);
    }
    
    [HttpGet()]
    public async Task<IActionResult> Get2()
    {
        await _holidayScheduleService.ProcessHolidayChunkAsync("ua", 2020);
        return Ok();
    }
}