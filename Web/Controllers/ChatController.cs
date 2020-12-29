using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class ChatController : Controller
    {
        const string SessionId = "_Id";
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString(SessionId) == null)
            {
                return RedirectToAction("Index", "Home", new { thongbao = "Vui lòng đăng nhập để chat" });
            }
            return View();
        }
    }
}
