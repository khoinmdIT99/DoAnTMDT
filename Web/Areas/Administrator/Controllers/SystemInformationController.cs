using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Domain.Shop.Entities.SystemManage;
using Domain.Shop.IRepositories;
using Infrastructure.Web;
using Infrastructure.Web.HelperTool;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [BaseAuthorization]
    public class SystemInformationController : BaseController
    {
        private ISystemInformationRepository _informationRepository;

        public SystemInformationController(ISystemInformationRepository informationRepository)
        {
            this._informationRepository = informationRepository;
        }

        public async Task<ActionResult> Index()
        {
            SystemInformation systemInformation =
                (await _informationRepository.All.FirstOrDefaultAsync() ?? new SystemInformation()
                {
                    Id = 0,
                    SMTPPassword = ""
                });
            systemInformation.SMTPPassword = Base64Hepler.Base64Decode(systemInformation.SMTPPassword);
            return View(systemInformation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(SystemInformation model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var systemInformation = await _informationRepository.All.FirstOrDefaultAsync();
                    if (systemInformation == null)
                    {
                        systemInformation = new SystemInformation
                        {
                            SiteName = StringHelper.KillChars(model.SiteName),
                            Slogan = StringHelper.KillChars(model.Slogan),
                            Copyright = StringHelper.KillChars(model.Copyright),
                            Email = StringHelper.KillChars(model.Email),
                            CompanyName = StringHelper.KillChars(model.CompanyName),
                            Address = StringHelper.KillChars(model.Address),
                            HotLine = StringHelper.KillChars(model.HotLine),
                            PhoneNumber = StringHelper.KillChars(model.PhoneNumber),
                            WebsiteAddress = StringHelper.KillChars(model.WebsiteAddress),
                            FacebookPage = StringHelper.KillChars(model.FacebookPage),
                            FacebookAppId = StringHelper.KillChars(model.FacebookAppId),
                            TaxiFare = model.TaxiFare
                        };
                        await _informationRepository.AddAsync(systemInformation);
                        await _informationRepository.SaveAsync(RequestContext);
                    }
                    else
                    {
                        systemInformation.SiteName = StringHelper.KillChars(model.SiteName);
                        systemInformation.Slogan = StringHelper.KillChars(model.Slogan);

                        systemInformation.Copyright = StringHelper.KillChars(model.Copyright);
                        systemInformation.Email = StringHelper.KillChars(model.Email);

                        systemInformation.CompanyName = StringHelper.KillChars(model.CompanyName);
                        systemInformation.Address = StringHelper.KillChars(model.Address);
                        systemInformation.HotLine = StringHelper.KillChars(model.HotLine);
                        systemInformation.PhoneNumber = StringHelper.KillChars(model.PhoneNumber);
                        systemInformation.WebsiteAddress = StringHelper.KillChars(model.WebsiteAddress);
                        systemInformation.FacebookPage = StringHelper.KillChars(model.FacebookPage);
                        systemInformation.FacebookAppId = StringHelper.KillChars(model.FacebookAppId);
                        systemInformation.TaxiFare = model.TaxiFare;
                        _informationRepository.UpdateAsync(systemInformation);
                        await _informationRepository.SaveAsync(RequestContext);
                    }
                    ViewBag.Success = "Đã ghi nhận thành công!";

                    return View(systemInformation);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Đã xảy ra lỗi: " + ex.Message;
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Vui lòng nhập chính xác thông tin!");
                ViewBag.Error = "Vui lòng nhập chính xác thông tin!";
                return View(model);
            }
        }

        [HttpPost]
        public async Task<ActionResult> SMTP(SystemInformation model)
        {
            try
            {
                var systemInformation =
                    (await _informationRepository.All.FirstOrDefaultAsync());
                if (systemInformation == null)
                {
                    systemInformation = new SystemInformation
                    {
                        SMTPEmail = model.SMTPEmail,
                        SMTPName = model.SMTPName,
                        SMTPPassword = Base64Hepler.Base64Encode(model.SMTPPassword)
                    };
                    await _informationRepository.AddAsync(systemInformation);
                    await _informationRepository.SaveAsync(RequestContext);
                }
                else
                {
                    systemInformation.SMTPEmail = model.SMTPEmail;
                    systemInformation.SMTPName = model.SMTPName;
                    systemInformation.SMTPPassword = Base64Hepler.Base64Encode(model.SMTPPassword);
                    _informationRepository.UpdateAsync(systemInformation);
                    await _informationRepository.SaveAsync(RequestContext);
                }
                return Json(new {success = true});
            }
            catch (Exception ex)
            {
                string message = "Đã xảy ra lỗi: " + ex.Message;
                return Json(new {success = false, message = message});
            }
        }
    }
}
