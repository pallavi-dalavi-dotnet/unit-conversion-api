
namespace UnitConversion.Api.Models;

public record ConversionResponse(
    decimal OriginalValue,
    string FromUnit,
    string ToUnit,
    decimal ConvertedValue,
    string Category
);
