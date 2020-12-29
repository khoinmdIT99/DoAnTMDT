using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Infrastructure.Web.HelperTool;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers
{
    public class QuanLyTaiKhoanController : Controller
    {
        const string SessionName = "_Name";
        const string SessionId = "_Id";
        const string SessionIdQuyen = "_IdQuyen";
        private readonly ICustomerRepository _customerRepository;

        public QuanLyTaiKhoanController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IActionResult> ThongTinTaiKhoan()
        {
            string sessionval = HttpContext.Session.GetString(SessionId);
            if (sessionval == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var taikhoan = !string.IsNullOrEmpty(sessionval)
                ? await _customerRepository.All.Where(x => x.Id.Equals(sessionval)).SingleOrDefaultAsync()
                : null;
            return View(taikhoan);
        }
        public async Task<IActionResult> DoiMatKhau()
        {
            string sessionval = HttpContext.Session.GetString(SessionId);
            if (sessionval == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var taikhoan = !string.IsNullOrEmpty(sessionval)
                ? await _customerRepository.All.Where(x => x.Id.Equals(sessionval)).SingleOrDefaultAsync()
                : null;
            return View(taikhoan);
        }

        public async Task<string> SuaPassword(string id, string matkhaucu, string matkhaumoi)
        {
            string thongbao = "";
            var taikhoan = await _customerRepository.All.SingleOrDefaultAsync(tk => tk.Id == id);
            string passwordHashed = StringHelper.stringToSHA512(StringHelper.KillChars(matkhaucu)).ToLower();
            if (taikhoan != null)
            {
                var checkpass = taikhoan.Password.ToLower() == passwordHashed;
                if (!checkpass)
                {
                    taikhoan.Password = StringHelper.stringToSHA512(matkhaumoi);
                    _customerRepository.UpdateAsync(taikhoan);
                    await _customerRepository.SaveAsync();
                    thongbao = "Sửa thành công";
                }
                else
                {
                    thongbao = "Mật khẩu cũ không trùng khớp";
                }
            }
            else
            {
                thongbao = "Tài khoản không tồn tại";
            }
            return thongbao;
        }

        public async Task<string> EditThongTin(string id, string sdt)
        {
            string thongbao = "";
            var taikhoan = await _customerRepository.All.SingleOrDefaultAsync(tk => tk.Id == id);
            if (taikhoan != null)
            {
                taikhoan.PhoneNo = sdt;
                _customerRepository.UpdateAsync(taikhoan);
                await _customerRepository.SaveAsync();
                thongbao = "Sửa thành công";
            }
            else
            {
                thongbao = "Tài khoản không tồn tại";
            }
            return thongbao;
        }
    }
}