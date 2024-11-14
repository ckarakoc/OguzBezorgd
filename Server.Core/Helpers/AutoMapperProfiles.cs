using AutoMapper;
using Server.Core.DTOs;
using Server.Core.Entities;

namespace Server.Core.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<User, UserDto>();
    }
}