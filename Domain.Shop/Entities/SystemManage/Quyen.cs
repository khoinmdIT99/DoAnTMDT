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
        [Required(ErrorMessage = "Nhập điểm tích lũy cần có!")]
        [RegularExpression("\\d+", ErrorMessage = "Điểm phải là một số nguyên dương")]
        public double Diem { get; set; }
        [Required(ErrorMessage = "Nhập giá giảm!")]
        [Range(0, 100, ErrorMessage = "Giảm giá không được nhỏ hơn 1% và vượt quá 100%")]
        public int GiamGia { get; set; }
        public string GhiChu { get; set; }
        public virtual ICollection<PhanQuyen> PhanQuyens { get; set; }
    }
}