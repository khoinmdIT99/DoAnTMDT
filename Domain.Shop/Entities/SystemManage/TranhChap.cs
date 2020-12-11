using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Shop.Entities.SystemManage
{
    public class TranhChap
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaTranhChap { get; set; }
        [ForeignKey("DonDatHang")]
        public string MaDDH { get; set; }
        public string LienHe { get; set; }
        public string NoiDung { get; set; }
        public DateTime ThoiGian { get; set; }
        public bool TrangThai { get; set; }
        public virtual Cart DonDatHang { get; set; }
    }
}