
using Microsoft.AspNetCore.Mvc;
using UnitConversion.Api.Models;
using UnitConversion.Api.Services;

namespace UnitConversion.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConversionsController : ControllerBase
{
    private readonly IConversionService _conversionService;

    public ConversionsController(IConversionService conversionService)
    {
        _conversionService = conversionService;
    }

    [HttpPost]
    public ActionResult<ConversionResponse> Convert(
        [FromBody] ConversionRequest request)
    {
        try
        {
            return Ok(_conversionService.Convert(request));
        }
        catch (ConversionException ex)
        {
            return BadRequest(new ApiError(ex.Code, ex.Message));
        }
        catch (OverflowException)
        {
            return BadRequest(new ApiError(
                "VALUE_OUT_OF_RANGE",
                "The supplied value is outside the supported range."));
        }
    }

    [HttpGet("units")]
    public ActionResult<object> GetSupportedUnits()
    {
        return Ok(_conversionService.GetSupportedUnits());
    }
}
