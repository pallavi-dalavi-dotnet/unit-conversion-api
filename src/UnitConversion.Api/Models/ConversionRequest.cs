
using System.ComponentModel.DataAnnotations;

namespace UnitConversion.Api.Models;

public class ConversionRequest
{
    public decimal Value { get; set; }

    [Required]
    [StringLength(40, MinimumLength = 1)]
    public string FromUnit { get; set; } = string.Empty;

    [Required]
    [StringLength(40, MinimumLength = 1)]
    public string ToUnit { get; set; } = string.Empty;
}
