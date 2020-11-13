using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Infrastructure.Database.Entities;

namespace Domain.Shop.Entities
{
    public class DiemTichLuy:BaseEntity
    {
        [Key]
        public string Id { get; set; }
        public int IdKhachHang { get; set; }
        public DateTime ThoiGian { get; set; }
        public int Diem { get; set; }
        public int IdHoaDon { get; set; }

        public virtual Cart Hoadon { get; set; }
        public virtual Customer Khachhang { get; set; }
    }
}
