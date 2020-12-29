using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Shop.Entities.SystemManage
{
    [Table("PhanQuyen")]
    public class PhanQuyen
    {
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Quyen")]
        [Required]
        public int MaQuyen { get; set; }

        [Key]
        [Column(Order = 2)]
        [ForeignKey("TaiKhoan")]
        [Required]
        public string MaTaiKhoan { get; set; }

        public virtual Quyen Quyen { get; set; }
        public virtual Customer TaiKhoan { get; set; }
    }
}