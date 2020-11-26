using Domain.Application.Entities;
using Infrastructure.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Domain.Shop.Entities
{
	[Table("CUSTOMERS")]
	public class Customer : BaseEntity
	{
		[Column("ID")]
		[Key]
		[MaxLength(50)]
		public string Id { get; set; }
		[Column("FULL_NAME")]
        [RegularExpression(@"^([\w\!\#$\%\&\'*\+\-\/\=\?\^`{\|\}\~]+\.)*[\w\!\#$\%\&\'‌​*\+\-\/\=\?\^`{\|\}\~]+@((((([a-zA-Z0-9]{1}[a-zA-Z0-9\-]{0,62}[a-zA-Z0-9]{1})|[‌​a-zA-Z])\.)+[a-zA-Z]{2,6})|(\d{1,3}\.){3}\d{1,3}(\:\d{1,5})?)$", ErrorMessage = "Địa chỉ E-mail không hợp lệ!")]
        [Display(Name = "Địa chỉ E-mail!")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ email!")]
        [StringLength(50, ErrorMessage = "Địa chỉ E-mail không được vượt quá 50 ký tự!")]
		public string FullName { get; set; }
		[Display(Name = "Mật khẩu")]
        // [StringLength(32, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 ký tự trở lên và ít hơn 32 ký tự!")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu!")]
		[Column("PASSWORD")]
		public string Password { get; set; }
        /// <summary>
        /// Mã salt ngẫu nhiên.
        /// </summary>
		public string Salt { get; set; }
		[MaxLength(255)]
		[Column("EMAIL")]
		public string Email { get; set; }
		[MaxLength(20)]
		[Column("PHONE_NO")]
		public string PhoneNo { get; set; }
		[Column("ADDRESS")]
		public string Address { get; set; }

		[Column("DISTRICT")]
		public string District { get; set; }
		[Column("PROVINCE")]
		public string Province { get; set; }
        public Cart Cart { get; set; }
		public virtual List<DiemTichLuy> ListDiemTichLuy { get; set; }

		public virtual ICollection<CustomerFeedback> CustomerFeedbacks { get; set; }
		public virtual ICollection<ProductReview> ProductReviews { get; set; }
        public int TongDiemTichLuy() => ListDiemTichLuy?.ToList().Select(d => d.Diem).Sum() ?? 0;
    }
}
