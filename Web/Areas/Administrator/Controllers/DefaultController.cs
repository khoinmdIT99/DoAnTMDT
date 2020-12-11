using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Web.Hubs;

namespace Web.Areas.Administrator.Controllers
{
	[Area("Administrator")]
	[BaseAuthorization]
	public class DefaultController : BaseController
	{
        private readonly IHubContext<ChatHub> _hubContext;

        public DefaultController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public IActionResult Index()
		{
            HttpContext.Session.Remove("ProductCode");
			return View();
		}
	}
}
