using Domain.Application.Dto.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shop.IRepositories
{
    public interface IMailerRepository
    {
        public void SendEmail(string content, string ToEmail, string subject, string Title, MailSettingViewModel mailSetting);
    }

}
