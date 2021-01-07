using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper;
using Domain.Shop.IRepositories;
using Microsoft.EntityFrameworkCore;
using Web.Common;
namespace Web.Hubs
{
    public class RestaurantHub : Hub
    {
        private IThongBaoRepository thongBaoRepository;
        private ICustomerRepository customerRepository;

        public RestaurantHub(IThongBaoRepository thongBaoRepository, ICustomerRepository customerRepository)
        {
            this.thongBaoRepository = thongBaoRepository;
            this.customerRepository = customerRepository;
        }

        public async Task AddClient(int orderid)
        {
            var callerId = Context.ConnectionId;
            await Groups.AddToGroupAsync(callerId, orderid.ToString());
        }


        public async Task NewOrderMessage(int mathongbao , string noidung, DateTime thoigian)
        {
            var order = await thongBaoRepository.All.FirstOrDefaultAsync(o => 
                                            o.MaThongBao == mathongbao && o.NoiDung.ToLower().Contains(noidung.ToLower()));
            await Clients.All.SendAsync("NewOrder", order.MaThongBao,order.NoiDung,order.ThoiGian);

        }



    }
}
