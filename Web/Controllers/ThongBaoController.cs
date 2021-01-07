using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Shop.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers
{
    public class ThongBaoController : Controller
    {
        private readonly IThongBaoRepository _iThongBaoRepository;

        public ThongBaoController(IThongBaoRepository iThongBaoRepository)
        {
            _iThongBaoRepository = iThongBaoRepository;
        }

        public async Task<IActionResult> Index()
        {
            var listthongbao = await _iThongBaoRepository.All.ToListAsync();
            return View(listthongbao);
        }
    }
}
