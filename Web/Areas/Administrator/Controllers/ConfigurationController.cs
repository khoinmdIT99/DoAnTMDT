using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Web;
using Microsoft.AspNetCore.Mvc;
using Domain.Application.Dto.Configuration;
using Domain.Application.IRepositories;
using Infrastructure.Common;

namespace Web.Areas.Administrator.Controllers
{
	[Area("Administrator")]
	[BaseAuthorization]
	public class ConfigurationController : BaseController
	{
        readonly ISiteSettingRepository _siteSettingRepository;
        readonly IMailSettingRepository _mailSettingRepository;
        readonly ISSOSettingRepository _ssoSettingRepository;
        readonly ConfigurationCache _configuration;
		public ConfigurationController(
			ISiteSettingRepository siteSettingRepository,
			IMailSettingRepository mailSettingRepository,
			ISSOSettingRepository ssoSettingRepository,
			ConfigurationCache configuration
			)
		{
			this._siteSettingRepository = siteSettingRepository;
			this._mailSettingRepository = mailSettingRepository;
			this._ssoSettingRepository = ssoSettingRepository;
			this._configuration = configuration;
		}
		public IActionResult Index()
		{
			var model = _configuration.GetConfiguration();
			if (model == null)
			{
				_configuration.SetConfiguration();
				model = _configuration.GetConfiguration();
			}	

			return View(model);
		}

		[HttpPost]
		public IActionResult Update(SiteSettingViewModel siteSetting, MailSettingViewModel mailSetting, SSOSettingViewModel SSOSetting)
		{
			var currentSiteSetting = _siteSettingRepository.Single();
			var currentMailSetting = _mailSettingRepository.Single();
			var currentSsoSetting = _ssoSettingRepository.Single();
			if (currentSiteSetting == null)
			{
				currentSiteSetting = new Domain.Application.Entities.SiteSetting()
				{
					Id = Guid.NewGuid().ToString()
				};
				_siteSettingRepository.Add(currentSiteSetting);
			}
			else
				_siteSettingRepository.Update(currentSiteSetting);
			if (currentMailSetting == null)
			{
				currentMailSetting = new Domain.Application.Entities.MailSetting()
				{
					Id = Guid.NewGuid().ToString()
				};
				_mailSettingRepository.Add(currentMailSetting);
			}
			else
				_mailSettingRepository.Update(currentMailSetting);
			if (currentSsoSetting == null)
			{
				currentSsoSetting = new Domain.Application.Entities.SSOSetting()
				{
					Id = Guid.NewGuid().ToString()
				};
				_ssoSettingRepository.Add(currentSsoSetting);
			}
			else
				_ssoSettingRepository.Update(currentSsoSetting);
			PropertyCopy.Copy(siteSetting, currentSiteSetting);
			PropertyCopy.Copy(mailSetting, currentMailSetting);
			PropertyCopy.Copy(SSOSetting, currentSsoSetting);
			_siteSettingRepository.Save(RequestContext);
			_configuration.SetConfiguration();

			return RedirectToAction("Index");
		}
	}
}
