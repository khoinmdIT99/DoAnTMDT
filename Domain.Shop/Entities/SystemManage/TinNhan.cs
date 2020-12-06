using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Shop.Entities.SystemManage
{
    public class TinNhan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaTinNhan { get; set; }
        public DateTime ThoiGian { get; set; }
        public string NoiDung { get; set; }
        [ForeignKey("DonDatHang")]
        public string MaDdh { get; set; }
        [ForeignKey("TaiKhoan")]
        public string MaTaiKhoan { get; set; }
        public bool? SellerSeen { get; set; }
        public bool? BuyerSeen { get; set; }
        public virtual Cart DonDatHang { get; set; }
        public virtual Customer TaiKhoan { get; set; }
    }
}
