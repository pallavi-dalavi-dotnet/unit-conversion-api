
using UnitConversion.Api.Models;

namespace UnitConversion.Api.Services;

public class ConversionService : IConversionService
{
    private const decimal Precision = 8;

    private sealed record UnitInfo(string Category, decimal FactorToBase);

    private static readonly Dictionary<string, UnitInfo> Units =
        new(StringComparer.OrdinalIgnoreCase)
        {
            // Length: base unit = meter
            ["meter"] = new("length", 1m),
            ["m"] = new("length", 1m),
            ["meters"] = new("length", 1m),
            ["kilometer"] = new("length", 1000m),
            ["km"] = new("length", 1000m),
            ["centimeter"] = new("length", 0.01m),
            ["cm"] = new("length", 0.01m),
            ["millimeter"] = new("length", 0.001m),
            ["mm"] = new("length", 0.001m),
            ["foot"] = new("length", 0.3048m),
            ["feet"] = new("length", 0.3048m),
            ["ft"] = new("length", 0.3048m),
            ["inch"] = new("length", 0.0254m),
            ["inches"] = new("length", 0.0254m),
            ["in"] = new("length", 0.0254m),
            ["mile"] = new("length", 1609.344m),
            ["miles"] = new("length", 1609.344m),
            ["mi"] = new("length", 1609.344m),

            // Mass: base unit = kilogram
            ["kilogram"] = new("weight", 1m),
            ["kg"] = new("weight", 1m),
            ["gram"] = new("weight", 0.001m),
            ["g"] = new("weight", 0.001m),
            ["milligram"] = new("weight", 0.000001m),
            ["mg"] = new("weight", 0.000001m),
            ["pound"] = new("weight", 0.45359237m),
            ["lb"] = new("weight", 0.45359237m),
            ["lbs"] = new("weight", 0.45359237m),
            ["ounce"] = new("weight", 0.028349523125m),
            ["oz"] = new("weight", 0.028349523125m),
            ["tonne"] = new("weight", 1000m),
            ["metric-tonne"] = new("weight", 1000m)
        };

    public ConversionResponse Convert(ConversionRequest request)
    {
        string from = request.FromUnit.Trim();
        string to = request.ToUnit.Trim();

        if (string.IsNullOrWhiteSpace(from) ||
            string.IsNullOrWhiteSpace(to))
        {
            throw new ConversionException(
                "INVALID_UNIT",
                "FromUnit and ToUnit are required.");
        }

        string normalizedFrom = Normalize(from);
        string normalizedTo = Normalize(to);

        bool isTemperature =
            IsTemperature(normalizedFrom) &&
            IsTemperature(normalizedTo);

        string category;

        decimal result;

        if (isTemperature)
        {
            category = "temperature";
            result = ConvertTemperature(
                request.Value, normalizedFrom, normalizedTo);
        }
        else
        {
            if (!Units.TryGetValue(normalizedFrom, out UnitInfo? source))
            {
                throw new ConversionException(
                    "UNSUPPORTED_UNIT",
                    $"Unit '{from}' is not supported.");
            }

            if (!Units.TryGetValue(normalizedTo, out UnitInfo? target))
            {
                throw new ConversionException(
                    "UNSUPPORTED_UNIT",
                    $"Unit '{to}' is not supported.");
            }

            if (source.Category != target.Category)
            {
                throw new ConversionException(
                    "INCOMPATIBLE_UNITS",
                    "Units must belong to the same category.");
            }

            category = source.Category;

            result = request.Value *
                     source.FactorToBase /
                     target.FactorToBase;
        }

        result = Math.Round(
            result, (int)Precision, MidpointRounding.AwayFromZero);

        return new ConversionResponse(
            request.Value, from, to, result, category);
    }

    private static string Normalize(string unit)
    {
        string value = unit.Trim().ToLowerInvariant();

        return value switch
        {
            "c" or "°c" or "celsius" => "celsius",
            "f" or "°f" or "fahrenheit" => "fahrenheit",
            "k" or "kelvin" => "kelvin",
            _ => value
        };
    }

    private static bool IsTemperature(string unit)
    {
        return unit is "celsius" or "fahrenheit" or "kelvin";
    }

    private static decimal ConvertTemperature(
        decimal value, string from, string to)
    {
        decimal celsius = from switch
        {
            "celsius" => value,
            "fahrenheit" => (value - 32m) * 5m / 9m,
            "kelvin" => value - 273.15m,
            _ => throw new ConversionException(
                "UNSUPPORTED_UNIT", "Unsupported temperature unit.")
        };

        if (celsius < -273.15m)
        {
            throw new ConversionException(
                "INVALID_TEMPERATURE",
                "Temperature cannot be below absolute zero.");
        }

        return to switch
        {
            "celsius" => celsius,
            "fahrenheit" => celsius * 9m / 5m + 32m,
            "kelvin" => celsius + 273.15m,
            _ => throw new ConversionException(
                "UNSUPPORTED_UNIT", "Unsupported temperature unit.")
        };
    }

    public object GetSupportedUnits()
    {
        return new
        {
            length = new[]
            {
                "meter", "kilometer", "centimeter",
                "millimeter", "foot", "inch", "mile"
            },
            temperature = new[]
            {
                "celsius", "fahrenheit", "kelvin"
            },
            weight = new[]
            {
                "kilogram", "gram", "milligram",
                "pound", "ounce", "tonne"
            }
        };
    }
}
