using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class QuanLyController : Controller
    {
        const string SessionName = "_Name";
        const string SessionId = "_Id";
        const string SessionIdQuyen = "_IdQuyen";
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
    }
}
