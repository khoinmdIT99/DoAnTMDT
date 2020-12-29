using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Shop.Entities.SystemManage
{
    public class DanhGia
    {
        public string Id { get; set; }
        [ForeignKey("IdTaiKhoanDanhGiaNavigation")]
        public string IdTaiKhoanDanhGia { get; set; }
        public double? Diem { get; set; }
        public Customer IdTaiKhoanDanhGiaNavigation { get; set; }
    }
}
