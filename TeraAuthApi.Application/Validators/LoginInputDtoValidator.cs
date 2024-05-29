using FluentValidation;
using TeraAuthApi.Application.DTOs.Request;

namespace TeraAuthApi.Application.Validators;

public class LoginInputDtoValidator : AbstractValidator<LoginInputDto>
{
    public LoginInputDtoValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}