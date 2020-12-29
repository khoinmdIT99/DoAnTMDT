using System.Collections.Generic;

namespace Domain.Shop.Entities.SystemManage
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Customer Admin { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
