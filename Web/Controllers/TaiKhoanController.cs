using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Domain.Shop.Dto;
using Domain.Shop.Dto.Cart;
using Domain.Shop.Entities;
using Domain.Shop.Entities.SystemManage;
using Domain.Shop.IRepositories;
using Infrastructure.Web;
using Infrastructure.Web.HelperTool;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductRepository _productRepository;
        private readonly IShoppingCartRepository _shoppingCart;
        private readonly IAccountRepository _accountRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProvinceRepository _iProvinceRepository;
        private readonly IDictrictRepository _iDictrictRepository;
        private readonly ITranhChapRepository _iTranhChapRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISystemInformationRepository _systemInformationRepository;
        private readonly ICategoryRepository _iCategoryRepository;
        private readonly IXacMinhRepository _xacMinhRepository;
        private readonly IDanhGiaRepository _danhGiaRepository;
        private readonly IQuyenRepository _quyenRepository;
        private readonly IPhanQuyenRepository _phanQuyenRepository;
        const string SessionName = "_Name";
        const string SessionId = "_Id";
        const string SessionIdQuyen = "_IdQuyen";

        public TaiKhoanController(IProductRepository productRepository, IShoppingCartRepository shoppingCart, IAccountRepository accountRepository, ICartRepository cartRepository, IProvinceRepository iProvinceRepository, IDictrictRepository iDictrictRepository, ITranhChapRepository iTranhChapRepository, IWebHostEnvironment webHostEnvironment, ISystemInformationRepository systemInformationRepository, ICategoryRepository iCategoryRepository, IXacMinhRepository xacMinhRepository, IDanhGiaRepository danhGiaRepository, IHttpContextAccessor httpContextAccessor, IQuyenRepository quyenRepository, IPhanQuyenRepository phanQuyenRepository)
        {
            _productRepository = productRepository;
            _shoppingCart = shoppingCart;
            _accountRepository = accountRepository;
            _cartRepository = cartRepository;
            _iProvinceRepository = iProvinceRepository;
            _iDictrictRepository = iDictrictRepository;
            _iTranhChapRepository = iTranhChapRepository;
            _webHostEnvironment = webHostEnvironment;
            _systemInformationRepository = systemInformationRepository;
            _iCategoryRepository = iCategoryRepository;
            _xacMinhRepository = xacMinhRepository;
            _danhGiaRepository = danhGiaRepository;
            _httpContextAccessor = httpContextAccessor;
            _quyenRepository = quyenRepository;
            _phanQuyenRepository = phanQuyenRepository;
        }
        [HttpGet]
        [Route("ThongTinTaiKhoan.html", Name = "ThongTinTaiKhoan")]
        public async Task<IActionResult> ThongTinTaiKhoan()
        {
            string sessionval = HttpContext.Session.GetString(SessionId);
            Customer taikhoan;
            taikhoan = !string.IsNullOrEmpty(sessionval) ? _accountRepository.All.FirstOrDefaultAsync(x => x.Id == sessionval).Result : null;
            var customerpresent = _accountRepository.GetCustomerViewModel(HttpContext.Session.GetString(SessionId));
            var getmaquyen =
                await _phanQuyenRepository.All.FirstOrDefaultAsync(x => x.MaTaiKhoan == customerpresent.Id);
            var idmaquyen = getmaquyen.MaQuyen;
            var tenquyen = await _quyenRepository.All.FirstOrDefaultAsync(x => x.MaQuyen == idmaquyen);
            ViewBag.LoaiTaiKhoan = tenquyen.TenQuyen;
            ViewBag.GiamGia = tenquyen.GiamGia;
            return View(taikhoan);
        }
        public string EditThongTin(string tendangnhap, string email, string sdt)
        {
            string sessionval = HttpContext.Session.GetString(SessionId);
            if (!string.IsNullOrEmpty(sessionval))
            {
                string thongbao = "";
                if (email == null)
                {
                    thongbao = "Không thể để trống mục Email";
                }
                else
                {
                    var taikhoan = _accountRepository.All.FirstOrDefault(x => x.Id == sessionval);
                    if (taikhoan != null)
                    {
                        taikhoan.FullName = tendangnhap;
                        taikhoan.Email = email;
                        taikhoan.PhoneNo = sdt;
                        _accountRepository.Update(taikhoan);
                        _accountRepository.Save();
                        thongbao = "Sửa thành công";
                    }
                    else
                    {
                        _accountRepository.Save();
                        thongbao = "Sửa thất bại";
                    }

                }
                return thongbao;
            }
            return "";
        }
        [HttpGet]
        [Route("DoiMatKhau.html", Name = "DoiMatKhau")]
        public IActionResult DoiMatKhau()
        {
            string sessionval = HttpContext.Session.GetString(SessionId);
            Customer taikhoan;
            taikhoan = !string.IsNullOrEmpty(sessionval) ? _accountRepository.All.FirstOrDefaultAsync(x => x.Id == sessionval).Result : null;
            List<Category> hang = _iCategoryRepository.All.OrderBy(x => x.HierarchyCode).ToList();
            ViewBag.Hang = hang;
            return View(taikhoan);
        }
        [HttpPost]
        public string EditPassword(string matkhaucu, string matkhaumoi)
        {
            string sessionval = HttpContext.Session.GetString(SessionId);
            if (!string.IsNullOrEmpty(sessionval))
            {
                var taikhoan = _accountRepository.All.FirstOrDefault(x => x.Id == sessionval);
                string thongbao = "";
                if (taikhoan != null)
                {
                    string passwordHashed = StringHelper.stringToSHA512(StringHelper.KillChars(matkhaucu)).ToLower();

                    var checkpass = taikhoan.Password.ToLower() == passwordHashed;
                    if (checkpass)
                    {
                        if (matkhaumoi.ToLower() == passwordHashed)
                        {
                            thongbao = "Mật khẩu đã được sử dụng";
                        }
                        else
                        {
                            taikhoan.Password = StringHelper.stringToSHA512(matkhaumoi);
                            _accountRepository.Update(taikhoan);
                            _accountRepository.Save();
                            thongbao = "Sửa thành công";
                        }
                    }
                    else
                    {
                        thongbao = matkhaucu.ToLower() + "---" + passwordHashed;
                    }
                }
                else
                {
                    thongbao = "Sửa thất bại";
                }
                return thongbao;
            }
            return "";
        }
        public IActionResult OrderManager()
        {
            var customer = _accountRepository.GetCustomerViewModel(HttpContext.Session.GetString(SessionId));
            if (customer != null)
            {
                IEnumerable<CartViewModel> model = _cartRepository.GetCartViewModels().ToList().Where(x => x.CustomerId == customer.Id).ToList();
                foreach (var item in model)
                {
                    item.Customer = _accountRepository.GetCustomerViewModel(item.CustomerId);
                    item.Customer.District = _iDictrictRepository.GetDictrictViewModel(_accountRepository.GetCustomerViewModel(item.CustomerId).District).Name;
                    item.Customer.Province = _iProvinceRepository.GetProvinceViewModel(_accountRepository.GetCustomerViewModel(item.CustomerId).Province).Name;
                }
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> ToCao([FromBody] TranhChapViewModel model)
        {
            try
            {
                var tranhchap = new TranhChap
                {
                    MaDDH = model.MaDDH,
                    NoiDung = model.typeData + model.txtOthers,
                    ThoiGian = DateTime.Now,
                    LienHe = model.txtContact
                };
                await _iTranhChapRepository.AddAsync(tranhchap);
                await _iTranhChapRepository.SaveAsync();
                return Json("Bạn đã gửi thành công !");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IActionResult> ChiTietDonMua(string id)
        {
            string sessionval = HttpContext.Session.GetString(SessionId);
            Customer taikhoan;
            Cart donhang;
            List<CartProduct> listchitietdonhang = new List<CartProduct>();
            if (!string.IsNullOrEmpty(sessionval))
            {
                taikhoan = await _accountRepository.All.SingleOrDefaultAsync(x => x.Id == sessionval);
                donhang = await _cartRepository.All.Include(x => x.Customer).SingleOrDefaultAsync(x => x.Id == id);
                var district = _iDictrictRepository.GetDictrictViewModel(_accountRepository.GetCustomerViewModel(taikhoan.Id).District).Name;
                var province = _iProvinceRepository.GetProvinceViewModel(_accountRepository.GetCustomerViewModel(taikhoan.Id).Province).Name;
                ViewBag.DiaChi = $"{taikhoan.Address} - {district} - {province}";
                listchitietdonhang = await _shoppingCart.All.Where(x => x.CartId == donhang.Id)
                    .Include(x => x.Cart)
                    .Include(x => x.Cart.Customer)
                    .Include(dh => dh.Product)
                    .Include(dh => dh.Product.ProductImages)
                    .ToListAsync();
            }
            else
            {
                taikhoan = null;
                donhang = null;
            }
            ViewBag.TaiKhoan = taikhoan;
            ViewBag.ChiTietDonHang = listchitietdonhang;
            ViewBag.PaymenMethod = GetPayList().FirstOrDefault(x => donhang != null && x.ItemValue.Equals(donhang.PaymentMethod.ToString()))?.ItemText;
            ViewBag.ShippingMethod = GetShipList().FirstOrDefault(x => donhang != null && x.ItemValue.Equals(donhang.ShippingMethod.ToString()))?.ItemText;
            //Kiểm tra có  cho huỷ hay không
            List<CartProduct> listhuydon = listchitietdonhang.Where(c => c.TinhTrangChiTiet != "Chưa xử lý").ToList();
            var huydonhang = listhuydon.Count == 0;
            ViewBag.HuyDonHang = huydonhang;
            return View(donhang);
        }
        [Route("DonMua{tinhtrang}.html", Name = "DonMuaKhachHang")]
        public IActionResult DonMua(string tinhtrang)
        {
            if (tinhtrang =="chuaxuly")
            {
                tinhtrang = "Chưa xử lý";
            }
            else
            {
                switch (tinhtrang)
                {
                    case "danggiao":
                        tinhtrang = "Đang giao";
                        break;
                    case "dangxuly":
                        tinhtrang = "Đang xử lý";
                        break;
                    case "daxuly":
                        tinhtrang = "Đã xử lý";
                        break;
                    case "dahuy":
                        tinhtrang = "Đã huỷ";
                        break;
                }
            }

            string host = _httpContextAccessor.HttpContext.Request.GetDisplayUrl();
            ViewBag.Host = host;
            string sessionval = HttpContext.Session.GetString(SessionId);
            List<Cart> listdonhang;
            var listchitietdonhang = new List<CartProduct>();
            Customer taikhoan;
            if (!string.IsNullOrEmpty(sessionval))
            {
                taikhoan = _accountRepository.All.FirstOrDefault(x => x.Id == sessionval);
                if (tinhtrang == "Đã xử lý")
                {
                    listdonhang = _cartRepository.All.Where(dh => dh.Customer.Id == taikhoan.Id && dh.Status == "Đã xử lý")
                                          .Include(dh => dh.Customer)
                                          .ToList();
                }
                else
                {
                    if (tinhtrang == "Chưa xử lý")
                    {
                        var listchitiet = _shoppingCart.All.Where(ct => ct.TinhTrangChiTiet != tinhtrang).ToList();
                        var listid = listchitiet.Select(ct => ct.CartId).ToList();
                        listdonhang = _cartRepository.All.Where(dh => !listid.Contains(dh.Id)).ToList();
                    }
                    else
                    {
                        var listchitiet = _shoppingCart.All.Where(ct => ct.TinhTrangChiTiet == tinhtrang).ToList();
                        var listid = listchitiet.Select(ct => ct.CartId).ToList();
                        listdonhang = _cartRepository.All.Where(dh => listid.Contains(dh.Id)).ToList();
                    }
                }
                listchitietdonhang = _shoppingCart.All.Where(dh => dh.TinhTrangChiTiet == tinhtrang && dh.Cart.Customer.Id == taikhoan.Id)
                    .Include(dh => dh.Cart)
                    .Include(dh => dh.Cart.Customer)
                    .Include(dh => dh.Product)
                    .Include(dh => dh.Product.ProductImages)
                    .ToList();
            }
            else
            {
                taikhoan = null;
                listdonhang = null;
            }
            ViewBag.TaiKhoan = taikhoan;
            ViewBag.ChiTietDonHang = listchitietdonhang;
            return View(listdonhang);
        }
        private List<SelectListModel> GetPayList()
        {
            return FunctionHelper.TypePay();
        }
        private List<SelectListModel> GetShipList()
        {
            return FunctionHelper.TypeShip();
        }
        public async Task HuyDonMua(string madonhang)
        {
            string sessionval = HttpContext.Session.GetString(SessionId);
            if (!string.IsNullOrEmpty(sessionval))
            {
                var donhang = _cartRepository.Get(madonhang);
                donhang.Status = "Chưa xử lý";
                List<CartProduct> chitietdonhang = await _shoppingCart.All.Where(dh => dh.CartId == madonhang)
                    .Include(dh => dh.Cart)
                    .ToListAsync();
                foreach (var item in chitietdonhang)
                {
                    item.TinhTrangChiTiet = "Đã huỷ";
                    _shoppingCart.Update(item);
                }
                await _shoppingCart.SaveAsync();
                await _cartRepository.SaveAsync();

            }
        }

        public async Task<JsonResult> CheckTaiKhoan(string tendangnhap)
        {
            var taikhoan = await _accountRepository.All.FirstOrDefaultAsync(x => x.FullName.Contains(tendangnhap));
            return Json(taikhoan);
        }
        public async Task<IActionResult> Activate(string key,string code)
        {
            string thongbao = "";
            var taikhoan = await _accountRepository.All.SingleOrDefaultAsync(x => x.Id == key);
            var checkCode = await _xacMinhRepository.All.FirstOrDefaultAsync(x => x.Code.Contains(code));
            if (taikhoan?.TinhTrang != "Chưa kích hoạt")
            {
                thongbao = "Tài khoản đã được kích hoạt";
            }
            else
            {
                if (checkCode != null)
                {
                    if (Sosanhngay(checkCode.Timer, DateTime.Now) >= 10)
                    {
                        _xacMinhRepository.Delete(checkCode);
                        await _xacMinhRepository.SaveAsync();
                        await HttpContext.SignOutAsync();
                        HttpContext.Session.Remove(SessionName);
                        HttpContext.Session.Remove(SessionId);
                        HttpContext.Session.Remove(SessionIdQuyen);
                        HttpContext.Session.Clear();
                        thongbao = "Quá thời gian xác nhận. Tài khoản của bạn đã bị xóa";
                    }
                    else
                    {
                        taikhoan.TinhTrang = "Không khoá";
                        _accountRepository.UpdateAsync(taikhoan);
                        _accountRepository.Save();
                        thongbao = "Kích hoạt thành công";
                    }
                }
                else
                {
                    thongbao = "Code đã sai.Vui lòng đăng ký hoặc đăng nhập lại";
                }
            }
            return RedirectToAction("Index", "Home", new { thongbao = thongbao });
        }
        public int Sosanhngay(DateTime d1, DateTime d2)
        {
            TimeSpan time = d1 - d2;
            int tongSoPhut = time.Minutes;
            return tongSoPhut;
        }
        public async Task<IActionResult> CustomerDanhGia(string iddonhang, int radio_check)
        {
            string sessionval = HttpContext.Session.GetString(SessionId);
            if (!string.IsNullOrEmpty(sessionval))
            {
                var customer = await _accountRepository.All.FirstOrDefaultAsync(x => x.Id == sessionval);
                List<CartProduct> listchitietdonhang;
                listchitietdonhang = await _shoppingCart.All.Where(c => c.CartId == iddonhang)
                    .Include(c => c.Cart)
                    .Include(c => c.Product)
                    .ToListAsync();
                foreach (var item in listchitietdonhang)
                {
                    item.DiemCustomerDanhGia = radio_check;
                    _shoppingCart.UpdateAsync(item);
                }
                await _shoppingCart.SaveAsync();
                DanhGia danhgia = new DanhGia
                {
                    Id = Guid.NewGuid().ToString().ToUpper(), IdTaiKhoanDanhGia = customer.Id, Diem = radio_check
                };
                await _danhGiaRepository.AddAsync(danhgia);
                await _danhGiaRepository.SaveAsync();
            }
            return RedirectToAction("ChiTietDonMua", new { id = iddonhang });
        }
    }
}