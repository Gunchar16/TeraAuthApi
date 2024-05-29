using System.Security.Cryptography;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TeraAuthApi.Application.DTOs;
using TeraAuthApi.Application.DTOs.Request;
using TeraAuthApi.Application.DTOs.Response;
using TeraAuthApi.Application.Service.Interfaces;
using TeraAuthApi.Application.UnitOfWork;
using TeraAuthApi.Application.Wrapper;
using TeraAuthApi.Domain.Entities;

namespace TeraAuthApi.Application.Service.Implementations;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UserService> _logger;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;

    public UserService(IUnitOfWork unitOfWork, ILogger<UserService> logger, IMapper mapper, IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
        _emailService = emailService;
    }

    public async Task<ApiServiceResponse<UserDto>> GetUserProfileAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.UserRepository.GetByIdWithRoleAsync(userId, cancellationToken: cancellationToken);
        if (user is null) 
            return ApiServiceResponse<UserDto>.FailureResponse("User not found");

        var userDto = _mapper.Map<UserDto>(user);
        return ApiServiceResponse<UserDto>.SuccessResponse(userDto);
    }

    public async Task<ApiServiceResponse<UserDto>> UpdateUserProfileAsync(Guid userId, UpdateUserInputDto updateUserInputDto,
        CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.UserRepository.GetByIdWithRoleAsync(userId, cancellationToken: cancellationToken);
        if (user is null)
            return ApiServiceResponse<UserDto>.FailureResponse("User not found");

        var isEmailOrUsernameTaken = await _unitOfWork.UserRepository.IsEmailOrUsernameTaken(userId, updateUserInputDto.Username, updateUserInputDto.Email,
            cancellationToken: cancellationToken);
        if (isEmailOrUsernameTaken)
            return ApiServiceResponse<UserDto>.FailureResponse("Username or email is already taken");

        user.Username = updateUserInputDto.Username;
        user.Email = updateUserInputDto.Email;
        user.UpdatedAt = DateTime.UtcNow;

        var updatedUser = await _unitOfWork.UserRepository.UpdateAsync(user, cancellationToken: cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);

        var userDto = _mapper.Map<UserDto>(updatedUser);
        return ApiServiceResponse<UserDto>.SuccessResponse(userDto, "User profile updated successfully");
    }

    public async Task<ApiServiceResponse<bool>> ChangePasswordAsync(Guid userId, ChangePasswordInputDto changePasswordInputDto,
        CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.UserRepository.GetByIdWithRoleAsync(userId, cancellationToken: cancellationToken);
        if (user is null)
            return ApiServiceResponse<bool>.FailureResponse("User not found");

        if (!BCrypt.Net.BCrypt.Verify(changePasswordInputDto.CurrentPassword, user.PasswordHash))
            return ApiServiceResponse<bool>.FailureResponse("Invalid current password");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordInputDto.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.UserRepository.UpdateAsync(user, cancellationToken: cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);

        return ApiServiceResponse<bool>.SuccessResponse(true, "Password changed successfully");
    }

    public async Task<ApiServiceResponse<bool>> ResetPasswordAsync(ResetPasswordInputDto resetPasswordInputDto,
        CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.UserRepository.GetByEmailAsync(resetPasswordInputDto.Email, cancellationToken: cancellationToken);
        if (user is null)
            return ApiServiceResponse<bool>.FailureResponse("User not found");

        var newPassword = GenerateRandomPassword();
        await UpdateUserPasswordAsync(user, newPassword, cancellationToken);

        var emailSent = await _emailService.SendNewPasswordEmailAsync(resetPasswordInputDto.Email, newPassword, cancellationToken: cancellationToken);
        if (!emailSent)
        {
            _logger.LogWarning("Failed to send new password email to user with email: {Email}", user.Email);
            return ApiServiceResponse<bool>.FailureResponse(
                "Password reset successfully, but failed to send email");
        }

        return ApiServiceResponse<bool>.SuccessResponse(true, "Password reset successfully");
    }

    private async Task UpdateUserPasswordAsync(User user, string newPassword, CancellationToken cancellationToken)
    {
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        user.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.UserRepository.UpdateAsync(user, cancellationToken: cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);
    }
    
    #region PasswordGeneration
    private string GenerateRandomPassword(int length = 12)
    {
        const string validChars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_";

        using (var rng = RandomNumberGenerator.Create())
        {
            var result = new char[length];
            var validCharCount = validChars.Length;

            for (int i = 0; i < length; i++)
            {
                var randomIndex = GetRandomNumber(rng, validCharCount);
                result[i] = validChars[randomIndex];
            }

            return new string(result);
        }
    }

    private int GetRandomNumber(RandomNumberGenerator rng, int maxValue)
    {
        if (maxValue <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxValue), "The maximum value must be a positive integer.");

        var data = new byte[4];
        rng.GetBytes(data);
        var randomNumber = BitConverter.ToUInt32(data, 0);

        return (int)(randomNumber % maxValue);
    }
    #endregion
}