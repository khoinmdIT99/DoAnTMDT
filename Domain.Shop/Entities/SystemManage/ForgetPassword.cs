using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Shop.Entities.SystemManage
{
    /// <summary>
    /// Quên mật khẩu
    /// Mã kích hoạt có giá trị trong 24 giờ
    /// </summary>
    [Table("ForgetPassword")]
    public class ForgetPassword
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string AccountId { get; set; }
        //Thời gian yêu cầu khôi phục mật khẩu
        public DateTime RequestTime { get; set; }
        //Mã xác nhận
        [StringLength(32)]
        public string ActiveCode { get; set; }
        //Mật khẩu tạm (đã mã hóa)
        public string TemporaryPassword { get; set; }
        /// <summary>
        /// Trạng thái
        /// 0: Vừa gửi yêu cầu khôi phục mật khẩu
        /// 1: Đã khôi phục mật khẩu thành công
        /// 2: Khôi phục mật khẩu không thành công
        /// 3: Đã hết hạn
        /// 4: Gửi email xác nhận yêu cầu khôi phục mật khẩu không thành công
        /// </summary>
        public int Status { get; set; }
        //Thời gian xác nhận mật khẩu mới thành công
        public DateTime? ActiveTime { get; set; }
        [StringLength(20)]
        public string RequestIp { get; set; }

        public string Describe()
        {
            return "{ AccountId : \"" + AccountId + "\", RequestTime : \"" + RequestTime + "\" }";
        }
    }
}
