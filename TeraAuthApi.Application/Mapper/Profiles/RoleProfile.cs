using AutoMapper;
using TeraAuthApi.Application.DTOs;
using TeraAuthApi.Domain.Entities;

namespace TeraAuthApi.Application.Mapper.Profiles;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<Role, RoleDto>().ReverseMap();
    }
}