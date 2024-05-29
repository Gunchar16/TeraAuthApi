using FluentValidation;
using TeraAuthApi.Application.DTOs.Request;

namespace TeraAuthApi.Application.Validators;

public class ResetPasswordInputDtoValidator : AbstractValidator<ResetPasswordInputDto>
{
    public ResetPasswordInputDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}