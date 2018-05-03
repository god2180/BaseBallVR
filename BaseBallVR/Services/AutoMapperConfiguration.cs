using AutoMapper;
using BaseBallVR.Models;

namespace BaseBallVR.Servicese
{
    public class AutoMapperConfiguration { 
        public static void Initialize()
        {
            Mapper.Initialize((config) =>
            {
                config.CreateMap<User, UserViewModel>();
                config.CreateMap<UserViewModel, User>();

                config.CreateMap<Member, MemberViewModel>();
                config.CreateMap<MemberViewModel, Member>();

            });
        }
    }
}