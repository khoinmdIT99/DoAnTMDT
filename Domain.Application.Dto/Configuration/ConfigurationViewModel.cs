using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Application.Dto.Configuration
{
	public class ConfigurationViewModel
	{
		public SiteSettingViewModel SiteSetting { get; set; }
		public MailSettingViewModel MailSetting { get; set; }
		public SSOSettingViewModel SSOSetting { get; set; }
	}
}
