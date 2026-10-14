
using UnitConversion.Api.Models;

namespace UnitConversion.Api.Services;

public interface IConversionService
{
    ConversionResponse Convert(ConversionRequest request);

    object GetSupportedUnits();
}
