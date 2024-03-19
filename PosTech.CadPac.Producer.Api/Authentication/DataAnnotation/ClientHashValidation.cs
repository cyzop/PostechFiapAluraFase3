using PosTech.CadPac.Domain.Services;
using System.ComponentModel.DataAnnotations;

namespace PosTech.CadPac.Producer.Api.Authentication.DataAnnotation
{
    public class ClientHashValidation : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var config = validationContext.GetService(typeof(IConfiguration)) as IConfiguration;
            var tokenService = validationContext.GetService(typeof(ITokenService)) as ITokenService;

            string internalSecret = tokenService.GenerateDynamicSecret();

            if (internalSecret.Equals(value?.ToString(), StringComparison.OrdinalIgnoreCase))
                return ValidationResult.Success;
            else
                return new ValidationResult("Invalid authentication parameters!");
        }
    }
}
