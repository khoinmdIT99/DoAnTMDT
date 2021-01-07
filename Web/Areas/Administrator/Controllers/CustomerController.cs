using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Application.Dto.Users;
using Domain.Shop.Dto.Customer;
using Domain.Shop.Entities.SystemManage;
using Domain.Shop.IRepositories;
using Infrastructure.Database.DynamicLinq;
using Infrastructure.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [BaseAuthorization]
    public class CustomerController : BaseController
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IDictrictRepository _dictrictRepository;
        private readonly IProvinceRepository _provinceRepository;
        private readonly IPhanQuyenRepository _phanQuyenRepository;
        private readonly IQuyenRepository _quyenRepository;
        private readonly IDiemTichLuyRepository _diemTichLuy;

        public CustomerController(IAccountRepository accountRepository, IDictrictRepository dictrictRepository, IProvinceRepository provinceRepository, IPhanQuyenRepository phanQuyenRepository, IQuyenRepository quyenRepository, IDiemTichLuyRepository diemTichLuy)
        {
            _accountRepository = accountRepository;
            _dictrictRepository = dictrictRepository;
            _provinceRepository = provinceRepository;
            _phanQuyenRepository = phanQuyenRepository;
            _quyenRepository = quyenRepository;
            _diemTichLuy = diemTichLuy;
        }

        public async Task<ActionResult> Index(string thongbao = null)
        {
            if (thongbao != null)
            {
                ViewBag.ThongBao = thongbao;
            }

            var phanquyenlist =await _phanQuyenRepository.All.ToListAsync();
            var groupedCustomerList = phanquyenlist.GroupBy(u => u.MaQuyen)
                .Select(grp => new { GroupID = grp.Key, CustomerList = grp.ToList() })
                .ToList();
            var listquyen = await _quyenRepository.All.ToListAsync();
            var users =await _accountRepository.All.ToListAsync();
            var listTk = new List<ThongKeKhachHang>();
            foreach (var group in groupedCustomerList)
            {
                var stuffToRemove = listquyen.SingleOrDefault(s => s.MaQuyen == group.GroupID);
                if (stuffToRemove != null)
                {
                    listquyen.Remove(stuffToRemove);
                }

                var listten = new List<string>();
                foreach (var cx in group.CustomerList)
                {
                    listten.Add(users.FirstOrDefault(x=>x.Id == cx.MaTaiKhoan)?.FullName);
                }

                var tk = new ThongKeKhachHang
                {
                    LoaiKhachHang = _quyenRepository.All.FirstOrDefaultAsync(x => x.MaQuyen == @group.GroupID)
                        .Result.TenQuyen,
                    SL = @group.CustomerList.Count,
                    TenKhachHang = listten
                };
                listTk.Add(tk);
            }
            ViewBag.groupList = listTk.Where(x => x.LoaiKhachHang.Contains("Khách")).ToList();
            ViewBag.listquyen = listquyen.Where(x => x.TenQuyen.Contains("Khách")).ToList();
            return View();
        }
        public async Task<ActionResult> QuétTask()
        {
            var phanquyen = await _phanQuyenRepository.All.ToListAsync();
            var users = _accountRepository.GetCustomerViewModel();
            foreach (var item in users)
            {
                item.TenLoaiKhachHang = _phanQuyenRepository.All.Include(x => x.Quyen)
                    .FirstOrDefault(x => x.MaTaiKhoan == item.Id)
                    ?.Quyen.TenQuyen;
            }
            users= users.Where(x => x.TenLoaiKhachHang.ToLower().Contains("Khách".ToLower())).ToList();
            // maquyen 
            foreach (var item in phanquyen)
            {
                var kh = users.FirstOrDefault(x => x.Id == item.MaTaiKhoan);
                var quyenkh = _quyenRepository.Get(item.MaQuyen);
                if (kh != null)
                {
                    var mucdiemtieptheochuasosanh = await _quyenRepository.All.Where(x => (kh.Point >= x.Diem && x.Diem >= 1)).ToListAsync();
                    var mucdiemtieptheo = mucdiemtieptheochuasosanh.OrderBy(quyen => Math.Abs(kh.Point - quyen.Diem)).First();
                    if (!mucdiemtieptheo.TenQuyen.Equals(quyenkh.TenQuyen))
                    {
                        _phanQuyenRepository.Delete(item);
                        item.MaQuyen = mucdiemtieptheo.MaQuyen;
                        item.MaTaiKhoan = kh.Id;
                        await _phanQuyenRepository.AddAsync(item);
                    }
                }
            }
            await _phanQuyenRepository.SaveAsync(RequestContext);
            string thongBao = "Đã nâng cấp tài khoản khách hàng thành công";
            return RedirectToAction("Index","Customer", new { thongbao = thongBao.ToUpper() });
        }
        [HttpPost]
        public async Task<ActionResult> GetDataCustomer([FromBody] DatatableRequest request)
        {
            if (request.columns != null)
            {
                foreach (var column in request.columns)
                {
                    if (column.search == null)
                        continue;
                    switch (column.name)
                    {
                        case "roles":
                            column.search.field = "roles.Select(r=>r.roleId)";
                            column.search.Operator = FilterOperator.Contains;
                            break;
                        default:
                            column.search.Operator = FilterOperator.Contains;
                            break;
                    }
                }
            }
            var users = _accountRepository.GetCustomerViewModel(request);
            var listdictrict = await _dictrictRepository.All.ToListAsync();
            var listprovince = await _provinceRepository.All.ToListAsync();
            var listdiemtichluy = await _diemTichLuy.All.ToListAsync();
            foreach (var item in users.data)
            {
                item.Address ??= "";
                item.District = listdictrict.FirstOrDefault(x => x.Id == item.District)?.Name ?? "";
                item.Province = listprovince.FirstOrDefault(x => x.Id == item.Province)?.Name ?? "";
                item.Point = listdiemtichluy.Where(x => x.IdKhachHang == item.Id).Sum(x => x.Diem);
                item.TenLoaiKhachHang = _phanQuyenRepository.All.Include(x => x.Quyen)
                    .FirstOrDefault(x => x.MaTaiKhoan == item.Id)
                    ?.Quyen.TenQuyen;
            }
            users.data = users.data.Where(x => x.TenLoaiKhachHang.ToLower().Contains("Khách".ToLower()));
            return await Task.Run(() => Json(users));
        }
        public ActionResult Test()
        {
            var users = _accountRepository.GetCustomerViewModel();
            foreach (var item in users)
            {
                item.TenLoaiKhachHang = _phanQuyenRepository.All.Include(x => x.Quyen)
                    .FirstOrDefault(x => x.MaTaiKhoan == item.Id)
                    ?.Quyen.TenQuyen;
            }
            return Json(users.Where(x => x.TenLoaiKhachHang.ToLower().Contains("Khách".ToLower())));
        }
        [HttpPost]
        public async Task<bool> Delete(string id)
        {
            try
            {
                var customer = _accountRepository.Get(id);
                _accountRepository.Delete(customer);
                await _accountRepository.SaveAsync(requestContext: RequestContext);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        //[HttpPost]
        //public async Task<ActionResult> Update(string id)
        //{
        //    var phanquyen = await _phanQuyenRepository.All.ToListAsync();
        //    // maquyen 
        //    foreach (var item in phanquyen)
        //    {
        //        var quyenkh = _quyenRepository.Get(item.MaQuyen);
        //        if (quyenkh.Diem >= 0)
        //        {
        //            var mucdiemtieptheochuasosanh = await _quyenRepository.All.Where(x => (quyenkh.Diem >= x.Diem)).ToListAsync();
        //            var mucdiemtieptheo = mucdiemtieptheochuasosanh.OrderBy(quyen => Math.Abs(quyenkh.Diem - quyen.Diem)).First();
        //            if (!mucdiemtieptheo.TenQuyen.Equals(quyenkh.TenQuyen))
        //            {
        //                item.MaQuyen = mucdiemtieptheo.MaQuyen;
        //                _phanQuyenRepository.UpdateAsync(item);
        //            }
        //        }
        //    }
        //    await _phanQuyenRepository.SaveAsync(RequestContext);
        //    ViewBag.ThongBao = "Đã nâng cấp tài khoản khách hàng thành công";
        //    return RedirectToAction("Index");
        //}
    }

    public class ThongKeKhachHang
    {
        public string LoaiKhachHang;
        public List<string> TenKhachHang;
        public int SL;
    }
}
