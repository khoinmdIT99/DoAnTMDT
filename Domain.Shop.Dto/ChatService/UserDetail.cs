using System.Collections.Generic;

namespace Domain.Shop.Dto.ChatService
{
    public class UserDetail
    {
        public string ConnectionId { get; set; }
        public string UserName { get; set; }
        public string IsAdmin { get; set; }
        public List<MessageDetail> Message { get; set; }

    }
}
