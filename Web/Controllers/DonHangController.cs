using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers
{
    public class DonHangController : Controller
    {
        const string SessionName = "_Name";
        const string SessionId = "_Id";
        const string SessionIdQuyen = "_IdQuyen";
        private readonly ICartRepository _cartRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IDiemTichLuyRepository _diemTichLuyRepository;

        public DonHangController(ICartRepository cartRepository, IShoppingCartRepository shoppingCartRepository, ICustomerRepository customerRepository, IDiemTichLuyRepository diemTichLuyRepository)
        {
            _cartRepository = cartRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _customerRepository = customerRepository;
            _diemTichLuyRepository = diemTichLuyRepository;
        }

        public IActionResult Index(string thongbao)
        {
            //Thông báo
            if (thongbao != null)
            {
                ViewBag.ThongBao = thongbao;
            }
            return RedirectToAction("ChoXuLy");
        }
        
        public async Task<IActionResult> Test()
        {
            List<CartProduct> chitiet = await _shoppingCartRepository.All.Where(c => c.TinhTrangChiTiet == "Chưa xử lý")
                .Include(c => c.Cart)
                .Include(c => c.Cart.Customer)
                .Include(x => x.Product)
                .ToListAsync();
            return Json(chitiet.First().Product.ProductName);
        }
        [Route("trangchuquanly/choxuly.html", Name = "ChoXuLy")]
        public async Task<IActionResult> ChoXuLy()
        {
            string id = HttpContext.Session.GetString(SessionId);
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<CartProduct> chitiet = await _shoppingCartRepository.All.Where(c => c.TinhTrangChiTiet == "Chưa xử lý")
                .Include(c => c.Cart)
                .Include(c => c.Cart.Customer)
                .Include(x => x.Product)
                .Include(x => x.Product.ProductImages)
                .ToListAsync();
            ViewBag.ChiTiet = chitiet;
            var tenmer = await _shoppingCartRepository.All.Where(s => s.TinhTrangChiTiet == "Chưa xử lý")
                .Include(c => c.Cart)
                .Include(c => c.Cart.Customer)
                .Include(x => x.Product)
                .Include(x => x.Product.ProductImages)
                .Select(s => s.CartId)
                .Distinct()
                 .ToListAsync();
            List<Cart> list =await _cartRepository.All.Where(sp => tenmer.Contains(sp.Id)).Include(sp => sp.Customer).ToListAsync();
            return View(list);
        }
        //chuyển qua để xử lý hàng
        public async Task<IActionResult> XuLy(string iddonhang)
        {
            string id = HttpContext.Session.GetString(SessionId);
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var customer = await _customerRepository.All.FirstOrDefaultAsync(x => x.Id == id);
            List<CartProduct> donhang = await _shoppingCartRepository.All
                .Where(s => s.CartId == iddonhang)
                .Include(c => c.Cart)
                .Include(c => c.Cart.Customer)
                .Include(x => x.Product).Include(x => x.Product.ProductImages).ToListAsync();
            foreach (var item in donhang)
            {
                item.TinhTrangChiTiet = "Chờ lấy hàng";
                _shoppingCartRepository.UpdateAsync(item);
            }
            await _shoppingCartRepository.SaveAsync();
            return RedirectToAction("ChoLayHang");
        }
        public async Task<IActionResult> Search_XL(string search)
        {
            List<Cart> list = await _cartRepository.All.Where(s => s.Id.ToString().Contains(search))
                .Include(s => s.Customer).ToListAsync();
            return View("XuLy", list);
        }
        [Route("trangchuquanly/cholayhang.html", Name = "ChoLayHang")]
        public async Task<IActionResult> ChoLayHang()
        {
            string id = HttpContext.Session.GetString(SessionId);
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var customer = await _customerRepository.All.FirstOrDefaultAsync(x => x.Id == id);
            List<CartProduct> chitiet = await _shoppingCartRepository.All.Where(c => c.TinhTrangChiTiet == "Chờ lấy hàng")
                .Include(c => c.Cart)
                .Include(c => c.Cart.Customer).Include(x => x.Product).Include(x => x.Product.ProductImages)
                .ToListAsync();
            ViewBag.ChiTiet = chitiet;
            var tenmer = await _shoppingCartRepository.All.Where(s => s.TinhTrangChiTiet == "Chờ lấy hàng")
                .Include(c => c.Cart)
                .Include(c => c.Cart.Customer).Include(x => x.Product).Include(x => x.Product.ProductImages)
                .Select(s => s.CartId)
                .Distinct()
                .ToListAsync();
            List<Cart> list = await _cartRepository.All.Where(sp => tenmer.Contains(sp.Id)).Include(sp => sp.Customer).ToListAsync();
            return View(list);
        }
        public async Task<IActionResult> Search_LH(string search)
        {
            List<Cart> list = await _cartRepository.All.Where(s => s.Id.ToString().Contains(search))
                .Include(s => s.Customer).ToListAsync();
            return View("ChoLayHang", list);
        }
        //cap nhật giao hàng

        public async Task<IActionResult> GiaoHang(string iddonhang)
        {
            string id = HttpContext.Session.GetString(SessionId);
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var customer = await _customerRepository.All.FirstOrDefaultAsync(x => x.Id == id);
            List<CartProduct> donhang = await _shoppingCartRepository.All
                .Where(s => s.CartId == iddonhang)
                .Include(c => c.Cart)
                .Include(c => c.Cart.Customer).Include(x => x.Product).Include(x => x.Product.ProductImages).ToListAsync();
            foreach (var item in donhang)
            {
                item.TinhTrangChiTiet = "Đang giao";
                _shoppingCartRepository.UpdateAsync(item);
            }
            await _shoppingCartRepository.SaveAsync();
            return RedirectToAction("DangGiao");
        }
        public async Task<IActionResult> Search_DG(string search)
        {
            List<Cart> list = await _cartRepository.All.Where(s => s.Id.ToString().Contains(search))
                .Include(s => s.Customer).ToListAsync();
            return View("DangGiao", list);
        }
        [Route("trangchuquanly/danggiao.html", Name = "DangGiao")]
        public async Task<IActionResult> DangGiao()
        {
            string id = HttpContext.Session.GetString(SessionId);
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var customer = await _customerRepository.All.FirstOrDefaultAsync(x => x.Id == id);
            List<CartProduct> chitiet =await _shoppingCartRepository.All.Where(c => c.TinhTrangChiTiet == "Đang giao")
                .Include(c => c.Cart)
                .Include(c => c.Cart.Customer).Include(x => x.Product).Include(x => x.Product.ProductImages)
                .ToListAsync();
            ViewBag.ChiTiet = chitiet;
            var tenmer = await _shoppingCartRepository.All.Where(s => s.TinhTrangChiTiet == "Đang giao")
                .Include(c => c.Cart)
                .Include(c => c.Cart.Customer).Include(x => x.Product).Include(x => x.Product.ProductImages)
                .Select(s => s.CartId)
                .Distinct()
                .ToListAsync();
            List<Cart> list = await _cartRepository.All.Where(sp => tenmer.Contains(sp.Id))
                .Include(sp => sp.Customer).ToListAsync();
            return View(list);
        }
        public async Task<IActionResult> CapNhat(string iddonhang)
        {
            string id = HttpContext.Session.GetString(SessionId);
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var customer = await _customerRepository.All.FirstOrDefaultAsync(x => x.Id == id);
            List<CartProduct> donhang = await _shoppingCartRepository.All
                .Where(s => s.CartId == iddonhang)
                .Include(c => c.Cart)
                .Include(c => c.Cart.Customer).Include(x => x.Product).Include(x => x.Product.ProductImages).ToListAsync();
            foreach (var item in donhang)
            {
                item.TinhTrangChiTiet = "Đã xử lý";
                item.NgayGiao = DateTime.Now;
                _shoppingCartRepository.UpdateAsync(item);
            }
            await _shoppingCartRepository.SaveAsync();
            var cart = await _cartRepository.All.Include(x=> x.Customer).SingleOrDefaultAsync(x => x.Id == iddonhang);
            cart.Status = "Đã xử lý";
            _cartRepository.UpdateAsync(cart);
            await _cartRepository.SaveAsync();
            var diemtichluy = new DiemTichLuy
            {
                Id = Guid.NewGuid().ToString(),
                Diem = Math.Round(a: (double) (cart.Totalprice / 1000000)),
                IdKhachHang = cart.Customer.Id,
                IdHoaDon = cart.Id,
                ThoiGian = DateTime.Now
            };
            await _diemTichLuyRepository.AddAsync(diemtichluy);
            await _diemTichLuyRepository.SaveAsync();
            return RedirectToAction("DaGiao");
        }
        [Route("trangchuquanly/dagiao.html", Name = "DaGiao")]
        public async Task<IActionResult> DaGiao()
        {
            string id = HttpContext.Session.GetString(SessionId);
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var customer = await _customerRepository.All.FirstOrDefaultAsync(x => x.Id == id);
            List<CartProduct> chitiet = await _shoppingCartRepository.All.Where(c => c.TinhTrangChiTiet == "Đã xử lý")
                .Include(c => c.Cart)
                .Include(c => c.Cart.Customer).Include(x => x.Product).Include(x => x.Product.ProductImages)
                .ToListAsync();
            ViewBag.ChiTiet = chitiet;
            var tenmer = await _shoppingCartRepository.All.Where(s => s.TinhTrangChiTiet == "Đã xử lý")
                .Include(c => c.Cart)
                .Include(c => c.Cart.Customer).Include(x => x.Product).Include(x => x.Product.ProductImages)
                .Select(s => s.CartId)
                .Distinct()
                .ToListAsync();
            List<Cart> list = await _cartRepository.All.Where(sp => tenmer.Contains(sp.Id))
                .Include(sp => sp.Customer).ToListAsync();
            return View(list);
        }
        public async Task<IActionResult> Search_G(string search)
        {
            List<Cart> list = await _cartRepository.All.Where(s => s.Id.ToString().Contains(search))
                .Include(s => s.Customer).ToListAsync();
            return View("DaGiao", list);
        }
        [Route("trangchuquanly/dahuy.html", Name = "DaHuy")]
        public async Task<IActionResult> DaHuy()
        {
            string id = HttpContext.Session.GetString(SessionId);
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var customer = await _customerRepository.All.FirstOrDefaultAsync(x => x.Id == id);
            var dh = await _cartRepository.All.Where(s => s.Status == "Đã huỷ").Select(s => s.Id).ToListAsync();
            List<CartProduct> ct = await _shoppingCartRepository.All
                .Where(s => dh.Contains(s.CartId))
                .Include(c => c.Cart)
                .Include(c => c.Cart.Customer).Include(x => x.Product).Include(x => x.Product.ProductImages)
                .ToListAsync();
            ViewBag.ChiTiet = ct;
            var tenmer = await _shoppingCartRepository.All
                .Include(c => c.Cart)
                .Include(c => c.Cart.Customer).Include(x => x.Product).Include(x => x.Product.ProductImages)
                .Select(s => s.CartId)
                .Distinct()
                .ToListAsync();
            List<Cart> list = await _cartRepository.All
                .Where(sp => tenmer.Contains(sp.Id)&& sp.Status  =="Đã huỷ")
                .Include(sp => sp.Customer).ToListAsync();
            return View(list);
        }
        public async Task<IActionResult> Search_DH(string search)
        {
            List<Cart> list = await _cartRepository.All.Where(s => s.Id.ToString().Contains(search))
                .Include(s => s.Customer).ToListAsync();
            return View("DaHuy", list);
        }
        [Route("trangchuquanly/chitiethoadon {id}.html", Name = "GetChiTiet")]
        public async Task<IActionResult> GetChiTiet(string id)
        {
            string iddangnhap = HttpContext.Session.GetString(SessionId);
            if (iddangnhap == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var customer = await _customerRepository.All.FirstOrDefaultAsync(x => x.Id == iddangnhap);
            List<CartProduct> list = await _shoppingCartRepository.All
                .Where(s => s.CartId == id)
                .Include(c => c.Cart)
                .Include(c => c.Cart.Customer).Include(x => x.Product)
                .Include(x => x.Product.ProductImages).ToListAsync();

            var iddon = await _cartRepository.All.Where(s => s.Id == id).Select(s => s.Id).SingleOrDefaultAsync();
            ViewBag.Id = iddon;
            var ten = await _cartRepository.All.Where(s => s.Id == id).Include(s => s.Customer).Select(s => s.Customer.FullName).SingleOrDefaultAsync();
            ViewBag.HoTen = ten;
            double tongtien = _shoppingCartRepository.All.Where(s => s.CartId == id).Sum(s => (s.Quantity * s.Price)).GetValueOrDefault();
            ViewBag.TongTien = tongtien;

            Cart donhang = await _cartRepository.All.Where(dh => dh.Id == id).SingleOrDefaultAsync();
            ViewBag.DonHang = donhang;

            var ngay = await _shoppingCartRepository.All
                .Where(s => s.CartId == id)
                .Include(c => c.Cart)
                .Include(c => c.Cart.Customer).Include(x => x.Product).Include(x => x.Product.ProductImages)
                .Select(s => s.NgayGiao).FirstOrDefaultAsync();
            ViewBag.NgayGiao = ngay;

            //double diemdanhgia = ctx.ChiTietDonHang.Where(dh => dh.IdDonHang == Guid.Parse(id) && dh.IdSizeSanPhamNavigation.IdSanPhamNavigation.IdTaiKhoanNavigation.TenDangNhap == tendangnhap).Select(dh => dh.DiemMerchantDanhGia).FirstOrDefault() ?? 0;
            //ViewBag.DiemDanhGia=diemdanhgia;
            bool danhgia = 0 > 0 ? false : true;
            ViewBag.DanhGia = danhgia;

            var tinhtrang = await _shoppingCartRepository.All.Where(s => s.CartId == id)
                .Include(c => c.Cart)
                .Include(c => c.Cart.Customer).Include(x => x.Product).Include(x => x.Product.ProductImages)
                                              .Select(s => s.TinhTrangChiTiet)
                                              .FirstOrDefaultAsync();
            ViewBag.TinhTrang = tinhtrang;
            var don = await _shoppingCartRepository.All.Where(s => s.CartId == id)
                .Include(c => c.Cart)
                .Include(c => c.Cart.Customer).Include(x => x.Product).Include(x => x.Product.ProductImages)
                .Select(s => $"{s.Cart.Customer.Address} - {s.Cart.Customer.District} - {s.Cart.Customer.Province}")
                .SingleOrDefaultAsync();
            ViewBag.DiaChi = don;
            var cus = await _shoppingCartRepository.All.Where(s => s.CartId == id)
                .Include(c => c.Cart)
                .Include(c => c.Cart.Customer).Include(x => x.Product).Include(x => x.Product.ProductImages)
                .Select(s => s.Cart.Customer.Email)
                .SingleOrDefaultAsync();
            ViewBag.Ten = cus;
            return View(list);
        }

        //public IActionResult MerchantDanhGia(string iddonhang, int radio_check)
        //{
        //    List<ChiTietDonHang> listchitietdonhang = new List<ChiTietDonHang>();
        //    listchitietdonhang = ctx.ChiTietDonHang.Where(c => c.IdDonHang == Guid.Parse(iddonhang) && c.IdSizeSanPhamNavigation.IdSanPhamNavigation.IdTaiKhoanNavigation.TenDangNhap == HttpContext.Session.GetString("TenDangNhap"))
        //                                               .Include(c => c.IdSizeSanPhamNavigation)
        //                                               .Include(c => c.IdSizeSanPhamNavigation.IdSanPhamNavigation)
        //                                               .Include(c => c.IdSizeSanPhamNavigation.IdSanPhamNavigation.IdTaiKhoanNavigation)
        //                                               .ToList();
        //    foreach (var item in listchitietdonhang)
        //    {
        //        item.DiemMerchantDanhGia = radio_check;
        //    }
        //    ctx.SaveChanges();

        //    //Đánh giá
        //    TaiKhoanBUS taikhoanbus = new TaiKhoanBUS();
        //    TaiKhoan taikhoan = taikhoanbus.CheckTaiKhoan(HttpContext.Session.GetString("TenDangNhap"));
        //    DonHang donhang = ctx.DonHang.Where(dh => dh.Id == Guid.Parse(iddonhang)).SingleOrDefault();

        //    DanhGia danhgia = new DanhGia();
        //    danhgia.Id = Guid.Parse(Guid.NewGuid().ToString().ToUpper());
        //    danhgia.IdTaiKhoanDanhGia = taikhoan.Id;
        //    danhgia.IdTaiKhoanDuocDanhGia = donhang.IdTaiKhoan;
        //    danhgia.Diem = radio_check;
        //    ctx.DanhGia.Add(danhgia);
        //    ctx.SaveChanges();

        //    //Return
        //    return RedirectToAction("DaGiao");
        //}
    }
}