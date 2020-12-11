using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Shop.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.SignalR;
using Web.Common;

namespace Web.Hubs
{
    public static class SignalrExtensions
    {
        public static HttpContext GetHttpContext(this HubCallerContext context) =>
            context
                ?.Features
                .Select(x => x.Value as IHttpContextFeature)
                .FirstOrDefault(x => x != null)
                ?.HttpContext;

        public static T GetQueryParameterValue<T>(this IQueryCollection httpQuery, string queryParameterName) =>
            httpQuery.TryGetValue(queryParameterName, out var value) && value.Any()
                ? (T)Convert.ChangeType(value.FirstOrDefault(), typeof(T))
                : default;
    }
    public class ChatDetailHub
    {
        public static ChatHub instance = null;
        private static readonly ConnectionMapping<string> Connections =
            new ConnectionMapping<string>();
        private static int _check = 0;
        private readonly ICartRepository _iCartRepository;
        private readonly ITinNhanRepository _iTinNhanRepository;
        private readonly IProductRepository _iProductRepository;
        //public void SendChatMessage(string who, string senderName, string message, string maDdh)
        //{
        //    var httpContext = Context.GetHttpContext();
        //    var query = httpContext.Request.Query;
        //    string uid = query.GetQueryParameterValue<string>("uid");
        //    TinNhan tn = new TinNhan {MaTaiKhoan = uid, ThoiGian = DateTime.Now, NoiDung = message, MaDdh = maDdh};
        //    if (CheckUid(uid, maDdh))
        //        tn.BuyerSeen = true;
        //    else
        //        tn.SellerSeen = true;
        //    _iTinNhanRepository.AddAsync(tn);
        //    _iTinNhanRepository.SaveAsync();
        //    foreach (var connectionId in Connections.GetConnections(uid))
        //    {
        //        Clients.Client(connectionId).SendAsync(senderName, message, int.Parse(uid), CheckInRoom(uid));

        //    }

        //    foreach (var connectionId in Connections.GetConnections(who))
        //    {

        //        Clients.Client(connectionId).SendAsync(senderName, message, int.Parse(uid), CheckInRoom(who));
        //    }
        //}
        //public void CheckSeenNotification(string id)
        //{
        //    var httpContext = Context.GetHttpContext();
        //    var query = httpContext.Request.Query;
        //    string uid = query.GetQueryParameterValue<string>("uid");
        //    var maTk = uid;
        //    int dem = 0;
        //    var seller = db.SanPhams.Where(a => a.MaTaiKhoan == maTk).ToList();
        //    var buyer = db.DonDatHangs.Where(a => a.MaTaiKhoan == maTk).ToList();
        //    if (seller.Count > 0)
        //    {
        //        foreach (var item in seller)
        //        {
        //            var ddh = db.DonDatHangs.Where(a => a.MaSP == item.MaSP).Distinct().ToList();
        //            if (ddh.Count > 0)
        //            {
        //                foreach (var item2 in ddh)
        //                {
        //                    var tinnhan = db.TinNhans.Where(a => a.MaDDH == item2.MaDDH && a.SellerSeen != true).FirstOrDefault();
        //                    if (tinnhan != null)
        //                    {
        //                        dem++;
        //                        continue;
        //                    }
        //                    var thongbao = db.ThongBaos.Where(a => a.MaDDH == item2.MaDDH && a.SellerSeen != true).ToList();
        //                    foreach (var item3 in thongbao)
        //                    {
        //                        if (item3 != null)
        //                        {
        //                            dem++;
        //                            continue;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    if (buyer.Count > 0)
        //    {
        //        foreach (var item in buyer)
        //        {
        //            var tinnhan = db.TinNhans.Where(a => a.MaDDH == item.MaDDH && a.BuyerSeen != true).FirstOrDefault();
        //            if (tinnhan != null)
        //            {
        //                dem++;
        //                continue;
        //            }
        //            var thongbao = db.ThongBaos.Where(a => a.MaDDH == item.MaDDH && a.BuyerSeen != true).ToList();
        //            foreach (var item3 in thongbao)
        //            {

        //                if (item3 != null)
        //                {
        //                    dem++;
        //                    continue;
        //                }
        //            }
        //        }
        //    }
        //    foreach (var connectionId in Connections.GetConnections(id))
        //    {
        //        Clients.Client(connectionId).addNotification(dem, _check);
        //    }
        //}
        //private bool CheckUid(string maTk, string madonHang)
        //{
        //    var taikhoan = _iCartRepository.All.FirstOrDefault(a => a.Id == madonHang && a.CustomerId == maTk);
        //    if (taikhoan != null)
        //        return true;
        //    return false;
        //}
        //private int CheckInRoom(string id)
        //{
        //    var maTk = id;
        //    int dem = 0;
        //    var seller = db.SanPhams.Where(a => a.MaTaiKhoan == maTk).ToList();
        //    var buyer = db.DonDatHangs.Where(a => a.MaTaiKhoan == maTk).ToList();
        //    if (seller.Count > 0)
        //    {
        //        foreach (var item in seller)
        //        {
        //            var ddh = db.DonDatHangs.Where(a => a.MaSP == item.MaSP).Distinct().ToList();
        //            if (ddh.Count > 0)
        //            {
        //                foreach (var item2 in ddh)
        //                {
        //                    var tinnhan = db.TinNhans.Where(a => a.MaDDH == item2.MaDDH && a.SellerSeen != true).FirstOrDefault();

        //                    if (tinnhan != null)
        //                    {
        //                        dem++;
        //                        continue;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    if (buyer.Count > 0)
        //    {
        //        foreach (var item in buyer)
        //        {
        //            var tinnhan = db.TinNhans.Where(a => a.MaDDH == item.MaDDH && a.BuyerSeen != true).FirstOrDefault();
        //            if (tinnhan != null)
        //            {
        //                dem++;
        //                continue;
        //            }
        //        }
        //    }
        //    return dem;
        //}
        //public void CheckSeen(string id)
        //{
        //    string uid = Context.QueryString["uid"];
        //    var MaTK = int.Parse(id);
        //    int dem = 0;
        //    var seller = db.SanPhams.Where(a => a.MaTaiKhoan == MaTK).ToList();
        //    var buyer = db.DonDatHangs.Where(a => a.MaTaiKhoan == MaTK).ToList();
        //    if (seller.Count > 0)
        //    {
        //        foreach (var item in seller)
        //        {
        //            var ddh = db.DonDatHangs.Where(a => a.MaSP == item.MaSP).Distinct().ToList();
        //            if (ddh.Count > 0)
        //            {
        //                foreach (var item2 in ddh)
        //                {
        //                    var tinnhan = db.TinNhans.Where(a => a.MaDDH == item2.MaDDH && a.SellerSeen != true).FirstOrDefault();

        //                    if (tinnhan != null)
        //                    {
        //                        dem++;
        //                        continue;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    if (buyer.Count > 0)
        //    {
        //        foreach (var item in buyer)
        //        {
        //            var tinnhan = db.TinNhans.Where(a => a.MaDDH == item.MaDDH && a.BuyerSeen != true).FirstOrDefault();
        //            if (tinnhan != null)
        //            {
        //                dem++;
        //                continue;
        //            }
        //        }
        //    }
        //    foreach (var connectionId in Connections.GetConnections(uid))
        //    {
        //        Clients.Client(connectionId).addNotification(dem);
        //    }
        //}
        //public Task OnConnected()
        //{
        //    string name = Context.QueryString["uid"];
        //    //string name = Context.User.Identity.Name;
        //    if (instance == null)
        //        instance = this;
        //    Connections.Add(name, Context.ConnectionId);
        //    _check = 0;
        //    return base.OnConnectedAsync();
        //}

        //public Task OnDisconnected(bool stopCalled)
        //{
        //    string name = Context.QueryString["uid"];
        //    //string name = Context.User.Identity.Name;

        //    Connections.Remove(name, Context.ConnectionId);

        //    return base.OnDisconnectedAsync(stopCalled);
        //}

        //private Task OnReconnected()
        //{
        //    string name = Context.QueryString["uid"];
        //    //string name = Context.User.Identity.Name;

        //    if (!Connections.GetConnections(name).Contains(Context.ConnectionId))
        //    {
        //        Connections.Add(name, Context.ConnectionId);
        //    }

        //    return base.OnReconnected();
        //}
    }
}
