using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Application.Entities;
using Domain.Shop.Dto.Materials;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Infrastructure.Common;
using Infrastructure.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [BaseAuthorization]
    public class MaterialController : BaseController
    {
        private readonly IMaterialRepository _materialRepository;
      
        public MaterialController(IMaterialRepository materialRepository)
        {
     
            _materialRepository = materialRepository;
         
        }
        public IActionResult Index()
        {
            return View(_materialRepository.GetMaterialViewModels());
        }
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
	
		public IActionResult Create(MaterialViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{      
					_materialRepository.Add(new Material()
					{
						Id = Guid.NewGuid().ToString(),
						MaterialName = model.MaterialName,
						Note = model.Note
					});			
					_materialRepository.Save(RequestContext);
				}
				catch (Exception )
				{
					
					return View();
				}
				return RedirectToAction("Index");
			}
			return View();
		} 
		public IActionResult Update(string id)
        {
			var model = _materialRepository.GetMaterialViewModelById(id);
			if(model == null)
            {
				return View();
            }
            else
            {
				return View(model);
            }
        }
		[HttpPost]
		public IActionResult Update(MaterialViewModel model)
		{
            if (ModelState.IsValid)
            {
				try
				{
					var material = _materialRepository.GetMaterialById(model.Id);
					PropertyCopy.Copy(model, material);
					_materialRepository.Update(material);
					_materialRepository.Save(RequestContext);
				}
				catch (Exception )
				{
					return View();
				}
				return RedirectToAction("Index");
			}
			return View();
		}
		[HttpPost]
		public bool Delete(string id)
        {
			try
			{
				var model = _materialRepository.GetMaterialById(id);
				_materialRepository.Delete(model);
				_materialRepository.Save(RequestContext);	

				return true;
			}
			catch (Exception )
			{
				return false;
			}
			
        }
	}
}
