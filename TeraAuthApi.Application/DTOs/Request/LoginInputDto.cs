using System.ComponentModel.DataAnnotations;

namespace TeraAuthApi.Application.DTOs.Request;

public record LoginInputDto(string Username, string Password);