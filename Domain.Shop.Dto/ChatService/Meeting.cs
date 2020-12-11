using System;
using System.Collections.Generic;

namespace Domain.Shop.Dto.ChatService
{
    public class Meeting
    {

        public string Id { set; get; }
        public List<MessageDetail> Messages { set; get; }
        public DateTime StartTime { set; get; }

    }
}
