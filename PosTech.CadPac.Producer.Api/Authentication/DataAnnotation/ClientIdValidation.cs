using System.ComponentModel.DataAnnotations;

namespace PosTech.CadPac.Producer.Api.Authentication.DataAnnotation
{
    public class ClientIdValidation : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var config = validationContext.GetService<AuthenticationCredentials>();

            var clientId = config.ClientId;

            if (value?.ToString()== clientId)
                return ValidationResult.Success;
            else
                return new ValidationResult("Invalid authentication parameters!");
        }
    }
}
