using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Shop.Entities.DanhMuc
{
    public class Mail
    {
        [Key]
        public long Id { get; set; }

        [Display(Name = "Mail người gửi")]
        public string MailNguoiGui { get; set; }

        [Display(Name = "Mail người gửi")]
        public string MailNguoiNhan { get; set; }
        [Display(Name = "Subject")]
        public string Subject { get; set; }
        [Display(Name = "Nội dung")]
        public string Body { get; set; }

        public string Description()
        {
            return "Gửi mail cho " + MailNguoiNhan + "\"" + " tiêu đề : " + Subject + "\"" + " với nội dung " + Body;
        }
    }
}
