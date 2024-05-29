using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TeraAuthApi.Application.DTOs;
using TeraAuthApi.Application.DTOs.Request;
using TeraAuthApi.Application.DTOs.Response;
using TeraAuthApi.Application.Service.Interfaces;
using TeraAuthApi.Application.Settings;
using TeraAuthApi.Application.UnitOfWork;
using TeraAuthApi.Application.Utilities;
using TeraAuthApi.Application.Wrapper;
using TeraAuthApi.Domain.Entities;

namespace TeraAuthApi.Application.Service.Implementations;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AuthService> _logger;
    private readonly JwtSettings _jwtSettings;
    
    public AuthService(
        IUnitOfWork unitOfWork, 
        ILogger<AuthService> logger, 
        IOptions<JwtSettings> jwtSettings)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<ApiServiceResponse<AuthResponseDto>> RegisterUserAsync(RegisterUserInputDto registerUserInputDto,
        CancellationToken cancellationToken = default)
    {
        var existingUser = await _unitOfWork.UserRepository.GetByUsernameAsync(registerUserInputDto.Username, cancellationToken: cancellationToken);
        if (existingUser is not null)
            return ApiServiceResponse<AuthResponseDto>.FailureResponse("Username already exists");

        existingUser = await _unitOfWork.UserRepository.GetByEmailAsync(registerUserInputDto.Email, cancellationToken: cancellationToken);
        if (existingUser is not null)
            return ApiServiceResponse<AuthResponseDto>.FailureResponse("Email already exists");

        var user = new User
        {
            Username = registerUserInputDto.Username,
            Email = registerUserInputDto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerUserInputDto.Password),
            CreatedAt = DateTime.Now
        };

        var createdUser = await _unitOfWork.UserRepository.AddAsync(user, cancellationToken: cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);

        _logger.LogInformation("User registered successfully. UserId: {UserId}", createdUser.Id);
        var generatedTokens = await GenerateTokens(user);

        return ApiServiceResponse<AuthResponseDto>.SuccessResponse(generatedTokens);
    }

    public async Task<ApiServiceResponse<AuthResponseDto>> LoginAsync(LoginInputDto loginInputDto,
        CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.UserRepository.GetByUsernameAsync(loginInputDto.Username, cancellationToken: cancellationToken);
        if (user is null)
            return ApiServiceResponse<AuthResponseDto>.FailureResponse("Invalid username or password");

        if (!BCrypt.Net.BCrypt.Verify(loginInputDto.Password, user.PasswordHash))
            return ApiServiceResponse<AuthResponseDto>.FailureResponse("Invalid username or password");

        var token = await GenerateTokens(user);

        _logger.LogInformation("User logged in successfully. UserId: {UserId}", user.Id);
        return ApiServiceResponse<AuthResponseDto>.SuccessResponse(token, "User logged in successfully");
    }

    public async Task<ApiServiceResponse<AuthResponseDto>> RefreshTokenAsync(RefreshTokenInputDto refreshTokenInputDto,
        CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.UserRepository.GetByRefreshTokenAsync(refreshTokenInputDto.Token, cancellationToken: cancellationToken);
        if (user is null)
            return ApiServiceResponse<AuthResponseDto>.FailureResponse("Invalid refresh token");

        var refreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshTokenInputDto.Token);
        if (refreshToken is null || refreshToken.Invalidated || refreshToken.ExpirationDate < DateTime.UtcNow)
            return ApiServiceResponse<AuthResponseDto>.FailureResponse("Refresh token is invalid or expired");

        var newTokens = await GenerateTokens(user);

        refreshToken.InvalidateToken();
        await _unitOfWork.RefreshTokenRepository.UpdateAsync(refreshToken, cancellationToken: cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);
        return ApiServiceResponse<AuthResponseDto>.SuccessResponse(newTokens, "Refresh token renewed successfully");
    }

    private async Task<AuthResponseDto> GenerateTokens(User user)
    {
        var jwtToken = JwtUtility.GenerateJwtToken(user, _jwtSettings);
        var refreshToken = CreateRefreshTokenEntity(user, GenerateRefreshToken());

        await _unitOfWork.RefreshTokenRepository.SaveRefreshTokenAsync(refreshToken);

        return new AuthResponseDto(jwtToken, refreshToken.Token);
    }

    private RefreshToken CreateRefreshTokenEntity(User user, string refreshToken)
    {
        return new RefreshToken
        {
            Token = refreshToken,
            JwtId = Guid.NewGuid().ToString(),
            CreatedDate = DateTime.Now,
            ExpirationDate = DateTime.Now.AddDays(7),
            Invalidated = false,
            UserId = user.Id
        };
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    
}