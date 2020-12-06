using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Domain.Shop.Dto.ImportBill;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Infrastructure.Common;
using Infrastructure.Web;
using Infrastructure.Web.HelperTool;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [BaseAuthorization]
    public class ImportBillController : BaseController
    {
        private readonly IImportRepository _importRepository;
        private readonly IImportDetailRepository _importDetailRepository;
        private readonly IProductRepository _productRepository;
        public ImportBillController(IImportRepository importRepository, IImportDetailRepository importDetailRepository, IProductRepository productRepository)
        {
            _importRepository = importRepository;
            _importDetailRepository = importDetailRepository;
            _productRepository = productRepository;
        }

        // GET: Admin/ImportBill
        public async Task<IActionResult> Index()
        {
            var listimport = await _importRepository.All.ToListAsync();
            return View(listimport);
        }

        public async Task<JsonResult> GetImportById(int id)
        {
            var getBill = await _importRepository.All.FirstOrDefaultAsync(p => p.IdSupplier == id);
            ViewBag.getBill = getBill;
            IQueryable<DetailImportModel> model = from a in _importDetailRepository.All
                join b in _productRepository.All
                    on a.IdProduct equals b.Id
                where a.IdImport == id
                select new DetailImportModel()
                {
                    IdDetailImport = a.IdDetailImport,
                    IdProduct = a.IdProduct,
                    NameProduct = b.ProductName,
                    Price = a.Price,
                    Amount = a.Amount
                };
            return Json(new
            {
                data = model,
                status = true
            });
        }

        public async Task<JsonResult> GetImports()
        {
            var listimport = await _importRepository.All.ToListAsync();
            List<ImportBill> cls = new List<ImportBill>();
            foreach (var c in listimport)
            {
                c.IsPaymentOk = _importRepository.IsPaymentOk(c.IdImport);
                cls.Add(c);
            }
            return Json(new { data = cls });
        }
        public IActionResult IsPaymentOk(int id)
        {
            bool isPOk = _importRepository.IsPaymentOk(id);
            return Json(new { isPaymentOk = isPOk });
        }
        [HttpPost]
        public async Task<JsonResult> DeleteImport(int id)
        {
            try
            {
                var listdetailId = _importDetailRepository.All.Where(x => x.IdImport == id).ToList();
                foreach (var i in listdetailId)
                {
                    _importDetailRepository.Delete(i);
                    var sp = _productRepository.All.ToList().Single(n => n.Id == i.IdProduct);
                    sp.BasketCount -= i.Amount.GetValueOrDefault();
                    _productRepository.UpdateAsync(sp);
                }

                await _productRepository.SaveAsync(requestContext: RequestContext);
                await _importDetailRepository.SaveAsync(RequestContext);
                _importRepository.Delete(id);
                await _importRepository.SaveAsync(RequestContext);
                return Json(new
                {
                    isDeleted = true,
                    html = Helper
                        .RenderRazorViewToString(this, "_ViewListImports", await _importRepository.All.ToListAsync())
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Json(new
                {
                    isDeleted = false,
                    html = Helper
                        .RenderRazorViewToString(this, "_ViewListImports", await _importRepository.All.ToListAsync())
                });
            }
        }
        public IActionResult Pay(int id)
        {
            var getBill = _importRepository.All.FirstOrDefault(p => p.IdImport == id);
            return View(getBill);
        }
        [HttpPost]
        public async Task<IActionResult> Pay(ImportBill importBill)
        {
            var getBill = await _importRepository.All.AsNoTracking().FirstOrDefaultAsync(p => p.IdImport == importBill.IdImport);
            if (getBill != null && importBill.Payment <= getBill.TienNo)
            {
                getBill.Payment += importBill.Payment;
                getBill.TienNo = getBill.TotalValue - getBill.Payment;
                if (getBill.EndDate.ToString(CultureInfo.InvariantCulture).Contains("0001"))
                {
                    getBill.EndDate = DateTime.Now;
                }
                else
                {
                    getBill.StartDate = getBill.EndDate;
                    getBill.EndDate = DateTime.Now;
                }
                _importRepository.UpdateAsync(getBill);
                await _importRepository.SaveAsync(RequestContext);
                return Json(new
                {
                    isValid = true,
                    html = Helper
                        .RenderRazorViewToString(this, "_ViewListImports", await _importRepository.All.ToListAsync())
                });
            }
            return Json(new
            {
                isValid = false,
                html = Helper
                    .RenderRazorViewToString(this, "_ViewListImports", await _importRepository.All.ToListAsync())
            });
        }
        public JsonResult GetLastPayedPeriodByIdPayment(int id)
        {
            string lastPayedPeriod = string.Empty;
            try
            {
                ImportBill payment = _importRepository.Get(id);
                if (payment != null)
                {
                    if(!payment.EndDate.ToString(CultureInfo.InvariantCulture).Contains("0001"))
                        lastPayedPeriod = "Từ "+payment.StartDate.ToString("dd/MM/yyyy") + " đến " + payment.EndDate.ToString("dd/MM/yyyy");
                    else
                    {
                        lastPayedPeriod = payment.StartDate.ToString("dd/MM/yyyy");
                    }

                    if (payment.Payment == payment.TotalValue)
                    {
                        lastPayedPeriod += " => Đã trả đủ";
                    }
                }
                return Json(new { lastPayedPeriod = lastPayedPeriod });
            }
            catch (Exception)
            {
                return Json(new { lastPayedPeriod = lastPayedPeriod });
            }
        }
    }
}