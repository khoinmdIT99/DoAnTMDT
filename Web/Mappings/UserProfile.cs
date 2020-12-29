using AutoMapper;
using Domain.Shop.Dto.Chat;
using Domain.Shop.Entities;

namespace Web.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Customer, UserViewModel>()
                .ForMember(dst => dst.Email, opt => opt.MapFrom(x => x.Email));
            CreateMap<UserViewModel, Customer>();
        }
    }
}
