using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers
{
    public class DoanhThuController : Controller
    {
        const string SessionName = "_Name";
        const string SessionId = "_Id";
        const string SessionIdQuyen = "_IdQuyen";
        private readonly ICustomerRepository _customerRepository;
        private readonly IShoppingCartRepository _cartRepository;

        public DoanhThuController(ICustomerRepository customerRepository, IShoppingCartRepository cartRepository)
        {
            _customerRepository = customerRepository;
            _cartRepository = cartRepository;
        }

        public IActionResult Index()
        {
            string id = HttpContext.Session.GetString(SessionId);
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<CartProduct> listchitiet = _cartRepository.All
                .Where(c => c.TinhTrangChiTiet == "Đã xử lý")
                .Include(c => c.Cart)
                .Include(c => c.Cart.Customer)
                .ToList();

            double tongdoanhthu = listchitiet.Sum(c => c.Quantity * c.Price) ?? 0;
            ViewBag.TongDoanhThu = tongdoanhthu;
            return View();
        }

        public double GetTongDoanhThuTheoNgay(DateTime nbd, DateTime nkt)
        {
            string id = HttpContext.Session.GetString("SessionId");
            List<CartProduct> listchitiet = _cartRepository.All
                .Where(c => c.TinhTrangChiTiet == "Đã xử lý"  && c.NgayGiao >= nbd && c.NgayGiao <= nkt)
                .Include(c => c.Cart)
                .Include(c => c.Cart.Customer)
                .ToList();

            double tongdoanhthu = listchitiet.Sum(c => c.Quantity * c.Price) ?? 0;
            return tongdoanhthu;
        }
    }
}