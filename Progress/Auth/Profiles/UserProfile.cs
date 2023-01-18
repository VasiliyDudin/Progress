using AutoMapper;
using Entity.Models;
using View.Models.In;
using View.Models.Out;

namespace Auth.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<OutUserView, User>().ReverseMap();
        CreateMap<InUserView, User>();
    }
}