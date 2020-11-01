using Domain.Application.Dto.Configuration;
using Domain.Shop.IRepositories;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shop.Repositories
{
    public class MailerRepository : IMailerRepository
    {
        public MailerRepository()
        {

        }
        public void SendEmail(string content, string ToEmail, string subject, string Title, MailSettingViewModel mailSetting)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(mailSetting.SmtpUsername);
                    mail.To.Add(ToEmail);
                    mail.Subject = subject;
                    mail.Body = Title + content;

                    using (SmtpClient smtp = new SmtpClient(mailSetting.SmtpServer, mailSetting.SmtpPort.Value))
                    {
                        smtp.Credentials = new NetworkCredential(mailSetting.SmtpUsername, mailSetting.SmtpPassword);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

    }
}
