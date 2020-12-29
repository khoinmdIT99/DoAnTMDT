using System;

namespace Domain.Shop.Entities.SystemManage
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public Customer FromUser { get; set; }
        public int ToRoomId { get; set; }
        public Room ToRoom { get; set; }
    }
}
