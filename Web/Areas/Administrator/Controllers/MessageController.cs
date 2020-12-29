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
    public class MessageController : BaseController
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public MessageController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
