using System.ComponentModel.DataAnnotations;

namespace TeraAuthApi.Application.DTOs.Request;

public record ResetPasswordInputDto(string Email);