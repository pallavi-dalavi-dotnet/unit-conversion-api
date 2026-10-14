
namespace UnitConversion.Api.Services;

public class ConversionException : Exception
{
    public string Code { get; }

    public ConversionException(string code, string message)
        : base(message)
    {
        Code = code;
    }
}
