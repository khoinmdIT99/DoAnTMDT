using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Common.Security;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Infrastructure.Web;
using Infrastructure.Web.HelperTool;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [BaseAuthorization]
    public class ImportProductController : BaseController
    {
        private readonly ISupplierRepository _iSupplierRepository;
        private readonly IImportRepository _importRepository;
        private readonly IImportDetailRepository _importDetailRepository;
        private readonly IProductRepository _iProductRepository;
        private readonly IServiceProvider _services;
        public ImportProductController(ISupplierRepository iSupplierRepository, IImportRepository importRepository, IImportDetailRepository importDetailRepository, IProductRepository iProductRepository, IServiceProvider services)
        {
            _iSupplierRepository = iSupplierRepository;
            _importRepository = importRepository;
            _importDetailRepository = importDetailRepository;
            _iProductRepository = iProductRepository;
            _services = services;
        }

        public ActionResult Index()
        {
            ViewBag.MaNCC = _iSupplierRepository.All;
            ViewBag.ListSanPham = _iProductRepository.All;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NhapHang(ImportBill model, IEnumerable<ImportBillDetail> lstModel)
        {
            HttpRequest cookie = _services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Request;
            string customerId = SecurityManager.getUserId(cookie.Cookies[SecurityManager._securityToken]);
            model.Amount = 0;
            model.TotalValue = 0;
            ViewBag.MaNCC = _iSupplierRepository.All;
            ViewBag.ListSanPham = _iProductRepository.All;
            // Kiểm tra dữ liệu đầu vào bằng javascript hay bên metadata đều được
            // Phải ktra để khớp với kiểu dữ liệu của database
            await _importRepository.AddAsync(model);
            await _importRepository.SaveAsync(RequestContext);
            //Gán đã xóa = false
            // SaveChanges lần đầu để  sinh ra mã phiếu nhập gán cho lstChiTietPhieuNhap
            var importBillDetails = lstModel.ToList();
            foreach (var item in importBillDetails)
            {
                // Cập nhật số lượng tồn
                // vì sản phẩm trong lstModel chắc chắn có nên k tạo new SanPham
                var sp = _iProductRepository.All.ToList().Single(n => n.Id == item.IdProduct);
                sp.BasketCount += item.Amount.GetValueOrDefault();
                // Gán mã phiếu nhập cho từng chi tiết phiếu nhập
                item.IdImport = model.IdImport;
            }

            foreach (var item in importBillDetails)
            {
                await _importDetailRepository.AddAsync(item);
            }
            await _importDetailRepository.SaveAsync(RequestContext);
            model.RefreshTotalValue();
            model.StaffId = customerId;
            _importRepository.UpdateAsync(model);
            await _importRepository.SaveAsync(RequestContext);
            TempData["messages"] = "Nhập hàng thành công";
            return RedirectToAction("Index");
        }

    }
}