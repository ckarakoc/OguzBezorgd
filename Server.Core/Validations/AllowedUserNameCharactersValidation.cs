using System.ComponentModel.DataAnnotations;

namespace Server.Core.Validations;

public class AllowedUserNameCharactersValidation : ValidationAttribute
{
    public AllowedUserNameCharactersValidation()
    {
        ErrorMessage = "Username must contain only alphanumeric characters or the following characters: - . _ +.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string strValue && strValue.All(x => char.IsLetterOrDigit(x) || x == '-' || x == '.' || x == '_' || x == '+'))
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}