using AutoMapper;
using Domain.Shop.Dto.Chat;
using Domain.Shop.Entities.SystemManage;

namespace Web.Mappings
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<Room, RoomViewModel>();
            CreateMap<RoomViewModel, Room>();
        }
    }
}
