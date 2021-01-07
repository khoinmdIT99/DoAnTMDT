using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Infrastructure.Database.Entities;

namespace Domain.Shop.Entities
{
    public class DiemTichLuy:BaseEntity
    {
        [Key]
        public string Id { get; set; }
        [ForeignKey("Khachhang")]
        public string IdKhachHang { get; set; }
        public DateTime ThoiGian { get; set; }
        public double Diem { get; set; }
        [ForeignKey("Hoadon")]
        public string IdHoaDon { get; set; }

        public virtual Cart Hoadon { get; set; }
        public virtual Customer Khachhang { get; set; }
    }
}
