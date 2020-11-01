using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Component
{
    public class AccountViewComponent : ViewComponent
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        const string SessionName = "_Name";
        const string SessionId = "_Id";
        public AccountViewComponent(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            ViewBag.Age = HttpContext.Session.GetInt32(SessionId);
            return View("Index");
        }
    }
}
