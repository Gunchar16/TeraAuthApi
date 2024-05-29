using FluentValidation;
using TeraAuthApi.Application.DTOs.Request;

namespace TeraAuthApi.Application.Validators;

public class RefreshTokenInputDtoValidator : AbstractValidator<RefreshTokenInputDto>
{
    public RefreshTokenInputDtoValidator()
    {
        RuleFor(x => x.Token).NotEmpty();
    }
}