using System.ComponentModel.DataAnnotations;

namespace Plogging.Core.Attributes;

public class FutureDate : ValidationAttribute
{
    public DateTime Date { get; }

    public string GetErrorMessage() =>
        $"Date must be past the current date";

    protected override ValidationResult? IsValid(
        object? value, ValidationContext validationContext)
    {
        var dateField = ((DateTime)value);

        if (dateField < DateTime.Now)
        {
            return new ValidationResult(GetErrorMessage());
        }

        return ValidationResult.Success;
    }
}
