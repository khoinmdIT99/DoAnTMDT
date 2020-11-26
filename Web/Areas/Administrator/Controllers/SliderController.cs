using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Shop.Dto;
using Domain.Shop.Dto.Slider;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Infrastructure.Common;
using Infrastructure.Web;
using Infrastructure.Web.HelperTool;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [BaseAuthorization]
    public class SliderController : BaseController
    {
        private readonly ISliderRepository _iSliderRepository;

        private readonly IWebHostEnvironment _iWebHostEnvironment;

        public SliderController(IWebHostEnvironment iWebHostEnvironment, ISliderRepository iSliderRepository)
        {
            this._iWebHostEnvironment = iWebHostEnvironment;
            this._iSliderRepository = iSliderRepository;
        }

        public async Task<List<SliderViewModel>> GetLisTask()
        {
            var myClients = await _iSliderRepository.All.Select(w => new SliderViewModel()
            {
                Id = w.Id,
                PhotoName = w.PhotoName,
                Status = w.Status

            }).ToListAsync();
            return myClients;
        }
        public async Task<IActionResult> Index()
        {
            return View(await GetLisTask());
        }
        public async Task<JsonResult> GetClients()
        {
            var clients = await GetLisTask();
            List<SliderViewModel> cls = new List<SliderViewModel>();
            string renderedImg = string.Empty;
            foreach (var c in clients)
            {
                cls.Add(c);
            }
            return Json(new { data = cls });
        }

        public IActionResult CreateSlider(string id)
        {
            var cl = id == null ? new SliderViewModel() : _iSliderRepository.GetDataByIdAsync(id).Result;
            return View(cl);
        }

        private async Task<string> UploadedFile(SliderViewModel model)
        {
            string uniqueFileName = null;

            if (model.ProfileImage != null)
            {
                string uploadsFolder = Path.Combine(_iWebHostEnvironment.WebRootPath, "images_slider");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfileImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                await using var fileStream = new FileStream(filePath, FileMode.Create);
                await model.ProfileImage.CopyToAsync(fileStream);
            }
            return uniqueFileName;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddShop(SliderViewModel slider)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string uniqueFileName = UploadedFile(slider).Result;
                    if (slider.Id == null)
                    {
                        var data = new Slider();
                        PropertyCopy.Copy(slider, data);
                        data.Id = Guid.NewGuid().ToString();
                        if (uniqueFileName != null)
                        {
                            data.PhotoName = "images_slider/" + uniqueFileName;
                        }

                        await _iSliderRepository.AddAsync(data);
                        _iSliderRepository.Save(RequestContext);
                    }
                    else
                    {
                        var data = await _iSliderRepository.All.FirstOrDefaultAsync(p => p.Id == slider.Id);
                        if (uniqueFileName != null)
                        {
                            slider.PhotoName = "images_slider/" + uniqueFileName;
                        }
                        if (uniqueFileName == null)
                        {
                            slider.PhotoName = data.PhotoName;
                        }
                        PropertyCopy.Copy(slider, data);

                        _iSliderRepository.Update(data);
                        _iSliderRepository.Save(RequestContext);
                    }
                    var myClients = await GetLisTask();
                    return Json(new
                    {
                        isValid = true,
                        html = Helper
                            .RenderRazorViewToString(this, "_ViewListSlider", myClients)
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Json(new
                {
                    isValid = false,
                    html = Helper
                        .RenderRazorViewToString(this, "CreateSlider", slider)
                });
            }
            return Json(new
            {
                isValid = false,
                html = Helper
                    .RenderRazorViewToString(this, "_ViewListSlider", slider)
            });
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(string id)
        {
            var data = await _iSliderRepository.All.FirstOrDefaultAsync(p => p.Id == id);
            return JsonDateTime(data);
        }
        [HttpPost]
        public async Task<JsonResult> DeleteSlider(string id)
        {
            var myClientsError = await GetLisTask();
            try
            {

                if (String.IsNullOrEmpty(id))
                {
                    return Json(new
                    {
                        isDeleted = false,
                        html = Helper
                            .RenderRazorViewToString(this, "_ViewListSlider", myClientsError)
                    });
                }

                var data = _iSliderRepository.Get(id);

                if (data == null)
                {
                    return Json(new
                    {
                        isDeleted = false,
                        html = Helper
                            .RenderRazorViewToString(this, "_ViewListSlider", myClientsError)
                    });
                }
                _iSliderRepository.Delete(data);
                _iSliderRepository.Save(RequestContext);
                var myClients = await GetLisTask();
                return Json(new
                {
                    isDeleted = true,
                    html = Helper
                        .RenderRazorViewToString(this, "_ViewListSlider", myClients)
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(new
                {
                    isDeleted = false,
                    html = Helper
                        .RenderRazorViewToString(this, "_ViewListSlider", myClientsError)
                });
            }
        }

    }
}
