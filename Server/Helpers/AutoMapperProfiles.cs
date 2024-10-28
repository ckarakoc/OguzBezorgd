using AutoMapper;
using Server.DTOs;
using Server.Entities;

namespace Server.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<User, UserDto>();
    }
}