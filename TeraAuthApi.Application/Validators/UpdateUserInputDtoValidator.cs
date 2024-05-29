using FluentValidation;
using TeraAuthApi.Application.DTOs.Request;

namespace TeraAuthApi.Application.Validators;

public class UpdateUserInputDtoValidator : AbstractValidator<UpdateUserInputDto>
{
    public UpdateUserInputDtoValidator()
    {
        RuleFor(x => x.Username)
            .MinimumLength(3)
            .When(x => !string.IsNullOrEmpty(x.Username));

        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.Email));
    }
}