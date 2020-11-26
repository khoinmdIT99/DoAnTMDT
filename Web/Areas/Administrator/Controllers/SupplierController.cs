using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Infrastructure.Common;
using Infrastructure.Web;
using Infrastructure.Web.HelperTool;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [BaseAuthorization]
    public class SupplierController : BaseController
    {
        private readonly ISupplierRepository _iSupplierRepository;

        public SupplierController(ISupplierRepository iSupplierRepository)
        {
            this._iSupplierRepository = iSupplierRepository;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("ICN") != null)
            {
                HttpContext.Session.Remove("ICN");
            }
            var mySuppliers = await _iSupplierRepository.All.ToListAsync();
            return View(mySuppliers);
        }

        public IActionResult CreateSupplier(int id)
        {
            var cl = id == 0 ? new Supplier() : _iSupplierRepository.All.FirstOrDefault(x =>x.Id == id);
            if (HttpContext.Session.GetString("ICN") == null)
            {
                var productCode = ProductCode.GenerateRandomNo().ToString();
                HttpContext.Session.SetString("ICN", productCode);
                ViewBag.ICN = HttpContext.Session.GetString("ICN");
            }

            return View(cl);
        }

        [HttpPost]
        public IActionResult CreateSupplier(Supplier supplier)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (supplier.Id == 0)
                    {
                        _iSupplierRepository.Add(supplier);
                        _iSupplierRepository.Save(RequestContext);
                        HttpContext.Session.Remove("ICN");
                    }
                    else
                    {
                        var data = _iSupplierRepository.All.AsNoTracking().ToList().FirstOrDefault(p => p.Id == supplier.Id);
                        PropertyCopy.Copy(supplier, data);
                        _iSupplierRepository.Update(supplier);
                        _iSupplierRepository.Save(RequestContext);

                    }
                    var suppliers = _iSupplierRepository.All.ToList();
                    List<Supplier> cls = new List<Supplier>();
                    foreach (var c in suppliers)
                    {
                        cls.Add(c);
                    }
                    return Json(new
                    {
                        isValid = true,
                        html = Helper
                        .RenderRazorViewToString(this, "_ViewListSuppliers", cls)
                    });
                }
                return Json(new
                {
                    isValid = false,
                    html = Helper
                        .RenderRazorViewToString(this, "CreateSupplier", supplier)
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Json(new
                {
                    isValid = false,
                    html = Helper
                    .RenderRazorViewToString(this, "_ViewListSuppliers", _iSupplierRepository.All.ToList())
                });
            }
        }

        public async Task<Supplier> GetSupplierById(int id)
        {
            return await _iSupplierRepository.All.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<JsonResult> GetSuppliers()
        {
            var suppliers = await _iSupplierRepository.All.ToListAsync();
            List<Supplier> cls = new List<Supplier>();
            foreach (var c in suppliers)
            {
                cls.Add(c);
            }
            return Json(new { data = cls });
        }

        [HttpPost]
        public JsonResult DeleteSupplier(int id)
        {
            try
            {
                var data = _iSupplierRepository.Get(id);
                _iSupplierRepository.Delete(data);
                _iSupplierRepository.Save(RequestContext);
                return Json(new
                {
                    isDeleted = true,
                    html = Helper
                        .RenderRazorViewToString(this, "_ViewListSuppliers", _iSupplierRepository.All.ToList())
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Json(new
                {
                    isDeleted = false,
                    html = Helper
                        .RenderRazorViewToString(this, "_ViewListSuppliers", _iSupplierRepository.All.ToList())
                });
            }
        }
    }
}
