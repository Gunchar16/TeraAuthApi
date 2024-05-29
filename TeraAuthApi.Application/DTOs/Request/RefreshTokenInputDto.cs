using System.ComponentModel.DataAnnotations;

namespace TeraAuthApi.Application.DTOs.Request;

public record RefreshTokenInputDto(string Token);