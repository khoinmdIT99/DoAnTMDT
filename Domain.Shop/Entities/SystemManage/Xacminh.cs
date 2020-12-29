using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Shop.Entities.SystemManage
{
    public class XacMinh
    {
        [Key] public int Id { get; set; }
        [Required] public string Code { get; set; }
        public DateTime Timer { get; set; }
        public string Id_User { get; set; }
    }
}
