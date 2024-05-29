using FluentValidation;
using TeraAuthApi.Application.DTOs.Request;

namespace TeraAuthApi.Application.Validators;

public class ChangePasswordInputDtoValidator : AbstractValidator<ChangePasswordInputDto>
{
    public ChangePasswordInputDtoValidator()
    {
        RuleFor(x => x.CurrentPassword).NotEmpty().MinimumLength(6);
        RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(6);
    }
}