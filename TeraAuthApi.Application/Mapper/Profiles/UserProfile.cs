using AutoMapper;
using TeraAuthApi.Application.DTOs;
using TeraAuthApi.Domain.Entities;

namespace TeraAuthApi.Application.Mapper.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom((src, dest, destMember, context) =>
            {
                if (src.UserRoles is not null && src.UserRoles.Any())
                {
                    return src.UserRoles.FirstOrDefault()?.Role?.Name;
                }
                return string.Empty;
            })).ReverseMap();
        
        CreateMap<List<User>, List<UserDto>>()
            .ConvertUsing((src, dest, context) =>
            {
                return src.Select(user => context.Mapper.Map<UserDto>(user)).ToList();
            });
    }
}