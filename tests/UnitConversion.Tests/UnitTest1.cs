
using UnitConversion.Api.Models;
using UnitConversion.Api.Services;
using Xunit;

namespace UnitConversion.Tests;

public class ConversionServiceTests
{
    private readonly ConversionService _service = new();

    [Fact]
    public void Convert_MetersToFeet_ReturnsCorrectResult()
    {
        var request = new ConversionRequest
        {
            Value = 10m,
            FromUnit = "meter",
            ToUnit = "foot"
        };

        var result = _service.Convert(request);

        Assert.Equal(32.80839895m, result.ConvertedValue);
        Assert.Equal("length", result.Category);
    }

    [Fact]
    public void Convert_KilometersToMeters_ReturnsCorrectResult()
    {
        var request = new ConversionRequest
        {
            Value = 1m,
            FromUnit = "km",
            ToUnit = "meter"
        };

        var result = _service.Convert(request);

        Assert.Equal(1000m, result.ConvertedValue);
    }

    [Fact]
    public void Convert_KilogramsToPounds_ReturnsCorrectResult()
    {
        var request = new ConversionRequest
        {
            Value = 5m,
            FromUnit = "kg",
            ToUnit = "lb"
        };

        var result = _service.Convert(request);

        Assert.Equal(11.02311311m, result.ConvertedValue);
        Assert.Equal("weight", result.Category);
    }

    [Fact]
    public void Convert_CelsiusToFahrenheit_ReturnsCorrectResult()
    {
        var request = new ConversionRequest
        {
            Value = 100m,
            FromUnit = "celsius",
            ToUnit = "fahrenheit"
        };

        var result = _service.Convert(request);

        Assert.Equal(212m, result.ConvertedValue);
        Assert.Equal("temperature", result.Category);
    }

    [Fact]
    public void Convert_FahrenheitToCelsius_ReturnsCorrectResult()
    {
        var request = new ConversionRequest
        {
            Value = 32m,
            FromUnit = "fahrenheit",
            ToUnit = "celsius"
        };

        var result = _service.Convert(request);

        Assert.Equal(0m, result.ConvertedValue);
    }

    [Fact]
    public void Convert_UnsupportedUnit_ThrowsException()
    {
        var request = new ConversionRequest
        {
            Value = 10m,
            FromUnit = "unknown",
            ToUnit = "meter"
        };

        var exception = Assert.Throws<ConversionException>(
            () => _service.Convert(request));

        Assert.Equal("UNSUPPORTED_UNIT", exception.Code);
    }

    [Fact]
    public void Convert_IncompatibleCategories_ThrowsException()
    {
        var request = new ConversionRequest
        {
            Value = 10m,
            FromUnit = "meter",
            ToUnit = "kg"
        };

        var exception = Assert.Throws<ConversionException>(
            () => _service.Convert(request));

        Assert.Equal("INCOMPATIBLE_UNITS", exception.Code);
    }

    [Fact]
    public void Convert_BelowAbsoluteZero_ThrowsException()
    {
        var request = new ConversionRequest
        {
            Value = -274m,
            FromUnit = "celsius",
            ToUnit = "fahrenheit"
        };

        var exception = Assert.Throws<ConversionException>(
            () => _service.Convert(request));

        Assert.Equal("INVALID_TEMPERATURE", exception.Code);
    }
}
