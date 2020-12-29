using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Shop.Entities.SystemManage
{
    [Table("Quyen")]
    public class Quyen
    {
        [Key]
        [Display(Name = "Mã quyền")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaQuyen { get; set; }

        [Display(Name = "Tên quyền")]
        [Required(ErrorMessage = "Hãy nhập tên quyền")]
        [MaxLength(256)]
        public string TenQuyen { get; set; }
        public virtual ICollection<PhanQuyen> PhanQuyens { get; set; }
    }
}