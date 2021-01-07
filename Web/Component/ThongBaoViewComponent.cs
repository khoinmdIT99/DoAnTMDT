using System.Linq;
using System.Threading.Tasks;
using Domain.Shop.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Component
{
    public class ThongBaoViewComponent:ViewComponent
    {
        private readonly IThongBaoRepository _thongBaoRepository;

        public ThongBaoViewComponent(IThongBaoRepository thongBaoRepository)
        {
            _thongBaoRepository = thongBaoRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listthongbao = await _thongBaoRepository.All.ToListAsync();
            return View(listthongbao.ToList().OrderByDescending(x => x.ThoiGian));
        }
    }
}
