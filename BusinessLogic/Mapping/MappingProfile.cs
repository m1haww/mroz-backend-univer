using AutoMapper;
using Domain.DTOs;
using Domain.Entities.App;
using Domain.Entities.User;

namespace BusinessLogic.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User
        CreateMap<User, UserDto>();

        // App
        CreateMap<App, AppDto>();
        CreateMap<CreateAppDto, App>();
        CreateMap<UpdateAppDto, App>();

        // AppUser
        CreateMap<AppUser, AppUserDto>();
        CreateMap<CreateAppUserDto, AppUser>();
    }
}
