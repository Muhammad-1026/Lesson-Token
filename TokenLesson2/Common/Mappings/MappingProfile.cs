using AutoMapper;
using TokenLesson2.Dtos.Request;
using TokenLesson2.Dtos.Response;
using TokenLesson2.Models.User;

namespace TokenLesson2.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<CreateUserDto, User>();
        CreateMap<UpdateUserDto, User>();
        //CreateMap<RefreshToken, TokenDto>();
    }
}
