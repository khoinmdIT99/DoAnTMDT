using Domain.Application.Dto.Configuration;
using Domain.Application.IRepositories;
using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Web
{
	public class ConfigurationCache
	{
        readonly ISiteSettingRepository _siteSettingRepository;
        readonly IMailSettingRepository _mailSettingRepository;
        readonly ISSOSettingRepository _ssoSettingRepository;
        readonly ICacheBase _cache;
		private string _ConfigurationKey = "_PageConfiguration";
		public ConfigurationCache(
			ISiteSettingRepository siteSettingRepository,
			IMailSettingRepository mailSettingRepository,
			ISSOSettingRepository ssoSettingRepository,
			ICacheBase cache
			)
		{
			this._siteSettingRepository = siteSettingRepository;
			this._mailSettingRepository = mailSettingRepository;
			this._ssoSettingRepository = ssoSettingRepository;
			this._cache = cache;
		}
		public ConfigurationViewModel GetConfiguration()
		{
			return _cache.Get<ConfigurationViewModel>(_ConfigurationKey);
		}

		public void SetConfiguration()
		{
			var siteSetting = _siteSettingRepository.Single();
			var mailSetting = _mailSettingRepository.Single();
			var ssoSetting = _ssoSettingRepository.Single();

			var configuration = new ConfigurationViewModel()
			{
				MailSetting = new MailSettingViewModel(),
				SSOSetting = new SSOSettingViewModel(),
				SiteSetting = new SiteSettingViewModel()
			};

			if (siteSetting != null)
			{
				PropertyCopy.Copy(siteSetting, configuration.SiteSetting);
			}
			if (mailSetting != null)
			{
				PropertyCopy.Copy(mailSetting, configuration.MailSetting);
			}
			if (ssoSetting != null)
			{
				PropertyCopy.Copy(ssoSetting, configuration.SSOSetting);
			}
			SetConfiguration(configuration);
		}

		public void SetConfiguration(ConfigurationViewModel configuration)
		{
			_cache.Set<ConfigurationViewModel>(_ConfigurationKey, configuration);
		}
	}
}
