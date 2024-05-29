using System.ComponentModel.DataAnnotations;

namespace TeraAuthApi.Application.DTOs.Response;

public record AuthResponseDto([Required] string Token, [Required] string RefreshToken);
