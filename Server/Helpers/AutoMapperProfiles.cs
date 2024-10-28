using AutoMapper;
using Server.Dtos;
using Server.Entities;

namespace Server.Helpers;

public class AutoMapperProfiles : Profile
{
    protected AutoMapperProfiles()
    {
        CreateMap<User, UserDto>();
    }
}