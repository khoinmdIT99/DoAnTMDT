using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Application.Entities;
using Domain.Shop.Dto.ProductTypes;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Infrastructure.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [BaseAuthorization]
    public class ProductTypeController : BaseController
    {
        private readonly IProductTypeRepository _productTypeRepository;
        public ProductTypeController( IProductTypeRepository productTypeRepository)
        {
            _productTypeRepository = productTypeRepository;
        }
        public ActionResult Index()
        {
            return View(_productTypeRepository.GetProductTypeViewModels());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ProductTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _productTypeRepository.Add(new ProductType()
                    {
                        Id = Guid.NewGuid().ToString(),
                        TypeName = model.TypeName,
                        Description = model.Description
                    });
                    _productTypeRepository.Save(RequestContext);
                  
                    return RedirectToAction("Index");
                }
                catch (Exception )
                {         
                    return View();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public bool Delete(string id)
        {
            try
            {
                ProductType productType = _productTypeRepository.Get(id);
                _productTypeRepository.Delete(productType);
                _productTypeRepository.Save(RequestContext);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public ActionResult Update(string id)
        {
            var model = _productTypeRepository.GetProductTypeViewModel(id);
            if (model == null)
            {
                return View();
            }
            else
            {
                return View(model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(ProductTypeViewModel productType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ProductType d = _productTypeRepository.All.Where(s => s.Id == productType.Id).First();
                    d.TypeName = productType.TypeName;
                    _productTypeRepository.Save(RequestContext);   
                }
                catch (Exception )
                {
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
