using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/")]
public class HolidayScheduleController : ControllerBase
{
    private readonly IExternalHolidayApiService _externalHolidayApiService;

    public HolidayScheduleController(IExternalHolidayApiService externalHolidayApiService)
    {
        _externalHolidayApiService = externalHolidayApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _externalHolidayApiService.GetCountries());
    }
}