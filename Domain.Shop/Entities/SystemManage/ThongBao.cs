using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Shop.Entities.SystemManage
{
    public class ThongBao
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaThongBao { get; set; }
        public string MaDdh { get; set; }
        public string NoiDung { get; set; }
        public DateTime ThoiGian { get; set; }
        public string MaTaiKhoan { get; set; }
        public bool? SellerSeen { get; set; }
        public bool? BuyerSeen { get; set; }

    }
}