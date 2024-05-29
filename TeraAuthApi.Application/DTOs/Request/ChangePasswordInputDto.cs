using System.ComponentModel.DataAnnotations;

namespace TeraAuthApi.Application.DTOs.Request;

public record ChangePasswordInputDto(string CurrentPassword, string NewPassword);