using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Web.AppConfig
{
    public class AppConfiguration
    {
        private readonly string _connectionString;
        private readonly string _SmtpClientAddressGmail;
        private readonly string _SmtpClientAddressVnUmail;

        private readonly string _SmtpClientPortGmail;
        private readonly string _SmtpClientPortVnUmail;
        public AppConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);

            var root = configurationBuilder.Build();
            _SmtpClientAddressGmail = root.GetSection("SMTP").GetSection("SmtpClientAddress_Gmail").Value;
            _SmtpClientAddressVnUmail = root.GetSection("SMTP").GetSection("SmtpClientAddress_VNUmail").Value;
            _SmtpClientPortGmail = root.GetSection("SMTP").GetSection("SmtpClientPost_Gmail").Value;
            _SmtpClientPortVnUmail = root.GetSection("SMTP").GetSection("SmtpClientPost_VNUmail").Value;

        }
        public string ConnectionString => _connectionString;
        public string SmtpClientAddressGmail => _SmtpClientAddressGmail;
        public string SmtpClientAddressVnUmail => _SmtpClientAddressVnUmail;

        public string SmtpClientPortGmail => _SmtpClientPortGmail;
        public string SmtpClientPortVnUmail => _SmtpClientPortVnUmail;
    }
}
