using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class AjaxContentController : Controller
    {
        public IActionResult CartInLayout()
        {
            return ViewComponent("CartInLayout");
        }
    }
}
