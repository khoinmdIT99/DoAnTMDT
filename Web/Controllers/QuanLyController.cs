using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Shop.Dto.Products;
using Domain.Shop.Entities;
using Domain.Shop.Entities.SystemManage;
using Domain.Shop.Enums;
using Domain.Shop.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Web.Controllers
{
    public class QuanLyController : Controller
    {
        const int PageSizeAll = 11;
        const int PageSize = 12;
        int _pageNumber = 1;
        const string SessionName = "_Name";
        const string SessionId = "_Id";
        const string SessionIdQuyen = "_IdQuyen";
        private readonly IProductRepository _productRepository;
        private readonly IMemoryCache _cache;
        private readonly ITranhChapRepository _tranhChapRepository;
        private readonly IDanhGiaRepository _danhGiaRepository;
        private readonly IProductReViewRepository _productReViewRepository;
        public QuanLyController(IProductRepository productRepository, IMemoryCache cache, ITranhChapRepository tranhChapRepository, IDanhGiaRepository danhGiaRepository, IProductReViewRepository productReViewRepository)
        {
            _productRepository = productRepository;
            _cache = cache;
            _tranhChapRepository = tranhChapRepository;
            _danhGiaRepository = danhGiaRepository;
            _productReViewRepository = productReViewRepository;
        }

        [Route("trangchuquanly.html", Name = "TrangChuQuanLy")]
        public IActionResult Index(string thongbao = null)
        {
            if (thongbao != null)
            {
                ViewBag.ThongBao = thongbao;
            }
            if (HttpContext.Session.GetString(SessionId) == null)
            {
                return RedirectToAction("Index", "Home", new { thongbao = "Vui lòng đăng nhập để thanh toán" });
            }
            if (HttpContext.Session.GetInt32(SessionIdQuyen) == 3)
            {
                return RedirectToAction("Index", "Home", new { thongbao = "Đường dẫn bị hư" });
            }
            return View();
        }
        public List<ProductViewModel> Get()
        {
            if (_cache.TryGetValue("CACHE_MASTER_PRODUCT", out List<ProductViewModel> cLstProd))
            {
                return cLstProd.Where(x => x.Actived == true && x.BasketCount > 0).ToList();
            }

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now + TimeSpan.FromHours(3),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(3),
                SlidingExpiration = TimeSpan.FromHours(3),
                Priority = CacheItemPriority.NeverRemove
            };
            IEnumerable<ProductViewModel> list = _productRepository.GetProductViewModels().ToList();
            foreach (var item in list)
            {
                item.PriceType = Enum.GetName(typeof(PriceType), int.Parse(item.PriceType));
            }
            _cache.Set("CACHE_MASTER_PRODUCT", list, options);
            var listactive = list.Where(x => x.Actived == true && x.BasketCount > 0).ToList();
            return listactive;
        }
        public List<ProductViewModel> Get(int pagenumber, int pagesize)
        {
            if (_cache.TryGetValue("CACHE_MASTER_PRODUCT", out List<ProductViewModel> cLstProd))
            {
                return cLstProd.Where(x => x.Actived == true && x.BasketCount > 0).Skip((pagenumber - 1) * pagesize)
                    .Take(pagesize)
                    .ToList();
            }

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now + TimeSpan.FromHours(3),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(3),
                SlidingExpiration = TimeSpan.FromHours(3),
                Priority = CacheItemPriority.NeverRemove
            };
            IEnumerable<ProductViewModel> list = _productRepository.GetProductViewModels().ToList();
            foreach (var item in list)
            {
                item.PriceType = Enum.GetName(typeof(PriceType), int.Parse(item.PriceType));
            }
            _cache.Set("CACHE_MASTER_PRODUCT", list, options);
            var listactive = list.Where(x => x.Actived == true && x.BasketCount > 0)
                .Skip((pagenumber - 1) * pagesize)
                .Take(pagesize)
                .ToList();
            return listactive;
        }
        public int TongTrang(List<ProductViewModel> list)
        {
            return ((list.Count / PageSize) + 1);
        }
        public async Task<IActionResult> ListSp(string thongbao, int? pagenumber)
        {
            //Thông báo
            if (thongbao != null)
            {
                ViewBag.ThongBaoList = thongbao;
            }

            _pageNumber = pagenumber ?? 1;
            List<ProductViewModel> list = Get(_pageNumber, PageSizeAll);
            List<ProductViewModel> tong = Get();

            ViewBag.TongTrang = TongTrang(tong);
            ViewBag.TrangHienTai = _pageNumber;
            ViewBag.TrangThai = "index";
            ViewBag.Date = DateTime.Now;
            return await Task.Run(() => View(list));
        }
        public List<ProductViewModel> Gethh()
        {
            if (_cache.TryGetValue("CACHE_MASTER_PRODUCT", out List<ProductViewModel> cLstProd))
            {
                return cLstProd.Where(x => x.Actived == true && x.BasketCount <= 0).ToList();
            }

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now + TimeSpan.FromHours(3),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(3),
                SlidingExpiration = TimeSpan.FromHours(3),
                Priority = CacheItemPriority.NeverRemove
            };
            IEnumerable<ProductViewModel> list = _productRepository.GetProductViewModels().ToList();
            foreach (var item in list)
            {
                item.PriceType = Enum.GetName(typeof(PriceType), int.Parse(item.PriceType));
            }
            _cache.Set("CACHE_MASTER_PRODUCT", list, options);
            var listactive = list.Where(x => x.Actived == true && x.BasketCount <= 0).ToList();
            return listactive;
        }
        public List<ProductViewModel> Gethh(int pagenumber, int pagesize)
        {
            if (_cache.TryGetValue("CACHE_MASTER_PRODUCT", out List<ProductViewModel> cLstProd))
            {
                return cLstProd.Where(x => x.Actived == true && x.BasketCount <= 0).Skip((pagenumber - 1) * pagesize)
                    .Take(pagesize)
                    .ToList();
            }

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now + TimeSpan.FromHours(3),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(3),
                SlidingExpiration = TimeSpan.FromHours(3),
                Priority = CacheItemPriority.NeverRemove
            };
            IEnumerable<ProductViewModel> list = _productRepository.GetProductViewModels().ToList();
            foreach (var item in list)
            {
                item.PriceType = Enum.GetName(typeof(PriceType), int.Parse(item.PriceType));
            }
            _cache.Set("CACHE_MASTER_PRODUCT", list, options);
            var listactive = list.Where(x => x.Actived == true && x.BasketCount <= 0)
                .Skip((pagenumber - 1) * pagesize)
                .Take(pagesize)
                .ToList();
            return listactive;
        }
        public async Task<IActionResult> HetHang(int? pagenumber)
        {
            _pageNumber = pagenumber ?? 1;
            List<ProductViewModel> list = Gethh(_pageNumber, PageSize);
            List<ProductViewModel> tong = Gethh();
            ViewBag.TongTrang = TongTrang(tong);
            ViewBag.TrangHienTai = _pageNumber;
            ViewBag.TrangThai = "index";
            ViewBag.Date = DateTime.Now;
            return await Task.Run(() => View(list));
        }
        public async Task<IActionResult> GetProductDetails(string id)
        {
            if (_cache.TryGetValue("CACHE_MASTER_PRODUCT", out List<ProductViewModel> cLstProd))
            {
                {
                    var modelCache = cLstProd.FirstOrDefault(x => x.Id == id);
                    return await Task.Run(() => PartialView("pSuaSanPham", modelCache));
                }
            }
            else
            {
                var model = _productRepository.GetProductViewModelById(id);
                return await Task.Run(() => PartialView("pSuaSanPham", model));
            }
        }
        public async Task<List<TranhChap>> Gettranhchap()
        {
            var list = await _tranhChapRepository.All
                .Include(x => x.DonDatHang)
                .Include(x => x.DonDatHang.Customer)
                .ToListAsync();
            return list;
        }
        public async Task<List<TranhChap>> Gettranhchap(int pagenumber, int pagesize)
        {
            var list = await _tranhChapRepository.All
                .Include(x => x.DonDatHang)
                .Include(x => x.DonDatHang.Customer)
                .ToListAsync();
            var listactive = list
                .Skip((pagenumber - 1) * pagesize)
                .Take(pagesize)
                .ToList();
            return listactive;
        }
        public int TongTrangTranhChap(List<TranhChap> list)
        {
            return ((list.Count / PageSize) + 1);
        }
        public async Task<IActionResult> ListTranhChap(string thongbao, int? pagenumber)
        {
            _pageNumber = pagenumber ?? 1;
            List<TranhChap> list = await Gettranhchap(_pageNumber, PageSize);
            List<TranhChap> tong = await Gettranhchap();
            ViewBag.TongTrang = TongTrangTranhChap(tong);
            ViewBag.TrangHienTai = _pageNumber;
            ViewBag.TrangThai = "index";
            return await Task.Run(() => View(list));
        }
        public async Task<List<DanhGia>> GetDanhGia()
        {
            var list = await _danhGiaRepository.All
                .Include(x => x.IdTaiKhoanDanhGiaNavigation)
                .ToListAsync();
            return list;
        }
        public async Task<List<DanhGia>> GetDanhGia(int pagenumber, int pagesize)
        {
            var list = await _danhGiaRepository.All
                .Include(x => x.IdTaiKhoanDanhGiaNavigation)
                .ToListAsync();
            var listactive = list
                .Skip((pagenumber - 1) * pagesize)
                .Take(pagesize)
                .ToList();
            return listactive;
        }
        public int TongTrangDanhGia(List<DanhGia> list)
        {
            return ((list.Count / PageSize) + 1);
        }
        public async Task<IActionResult> ListDanhGia(string thongbao, int? pagenumber)
        {
            _pageNumber = pagenumber ?? 1;
            List<DanhGia> list = await GetDanhGia(_pageNumber, PageSize);
            List<DanhGia> tong = await GetDanhGia();
            ViewBag.TongTrang = TongTrangDanhGia(tong);
            ViewBag.TrangHienTai = _pageNumber;
            ViewBag.TrangThai = "index";
            return await Task.Run(() => View(list));
        }
        public async Task<string> DoiTrangThaiSp(int id)
        {
            try
            {
                TranhChap sp = await _tranhChapRepository.All.Where(s => s.MaTranhChap == id).SingleOrDefaultAsync();
                sp.TrangThai = !sp.TrangThai;
                _tranhChapRepository.UpdateAsync(sp);
                await _tranhChapRepository.SaveAsync();
                return "Đổi trạng thái thành công";
            }
            catch
            {
                return "Lỗi thực hiện";
            }
        }
        public async Task<string> DoiTrangThaiReview(string id)
        {
            try
            {
                var sp = await _productReViewRepository.All.Where(s => s.Id == id).SingleOrDefaultAsync();
                sp.Approved = !sp.Approved;
                _productReViewRepository.UpdateAsync(sp);
                await _productReViewRepository.SaveAsync();
                return "Đổi trạng thái thành công";
            }
            catch
            {
                return "Lỗi thực hiện";
            }
        }
        public async Task<List<ProductReview>> GetReview()
        {
            var list = await _productReViewRepository.All
                .Include(x => x.Customer).Include(x => x.Product)
                .ToListAsync();
            return list;
        }
        public async Task<List<ProductReview>> GetReview(int pagenumber, int pagesize)
        {
            var list = await _productReViewRepository.All
                .Include(x => x.Customer).Include(x => x.Product)
                .ToListAsync();
            var listactive = list
                .Skip((pagenumber - 1) * pagesize)
                .Take(pagesize)
                .ToList();
            return listactive;
        }
        public int TongTrangReview(List<ProductReview> list)
        {
            return ((list.Count / PageSize) + 1);
        }
        public async Task<IActionResult> ListReview(string thongbao, int? pagenumber)
        {
            _pageNumber = pagenumber ?? 1;
            List<ProductReview> list = await GetReview(_pageNumber, PageSize);
            List<ProductReview> tong = await GetReview();
            ViewBag.TongTrang = TongTrangReview(tong);
            ViewBag.TrangHienTai = _pageNumber;
            ViewBag.TrangThai = "index";
            return await Task.Run(() => View(list));
        }
    }
}
