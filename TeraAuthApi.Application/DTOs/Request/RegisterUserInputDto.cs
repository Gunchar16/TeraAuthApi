using System.ComponentModel.DataAnnotations;

namespace TeraAuthApi.Application.DTOs.Request;

public record RegisterUserInputDto(string Username, string Email, string Password);
