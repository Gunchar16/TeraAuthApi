using FluentValidation;
using TeraAuthApi.Application.DTOs.Request;

namespace TeraAuthApi.Application.Validators;

public class RegisterUserInputDtoValidator : AbstractValidator<RegisterUserInputDto>
{
    public RegisterUserInputDtoValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MinimumLength(3);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}