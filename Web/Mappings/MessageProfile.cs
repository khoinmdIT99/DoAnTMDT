using AutoMapper;
using Chat.Web.Helpers;
using Domain.Shop.Dto.Chat;
using Domain.Shop.Entities.SystemManage;

namespace Web.Mappings
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<Message, MessageViewModel>()
                .ForMember(dst => dst.From, opt => opt.MapFrom(x => x.FromUser.Email))
                .ForMember(dst => dst.To, opt => opt.MapFrom(x => x.ToRoom.Name))
                .ForMember(dst => dst.Avatar, opt => opt.MapFrom(x => x.FromUser.Avatar))
                .ForMember(dst => dst.Content, opt => opt.MapFrom(x => BasicEmojis.ParseEmojis(x.Content)))
                .ForMember(dst => dst.Timestamp, opt => opt.MapFrom(x => x.Timestamp));
            CreateMap<MessageViewModel, Message>();
        }
    }
}
