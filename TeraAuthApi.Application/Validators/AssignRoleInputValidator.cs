using FluentValidation;
using TeraAuthApi.Application.DTOs.Request;

namespace TeraAuthApi.Application.Validators;

public class AssignRoleInputValidator : AbstractValidator<AssignRoleInputDto>
{
    public AssignRoleInputValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.RoleId).NotEmpty();
    }
}