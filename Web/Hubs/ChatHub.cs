using System;
using System.Linq;
using System.Threading.Tasks;
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
    public class ChatHub : Hub
    {
        public static ChatHub instance = null;
        private static readonly ConnectionMapping<string> Connections =
            new ConnectionMapping<string>();
        private static int _check = 0;
        //public void SendChatMessage(string who, string senderName, string message, int MaDDH)
        //{
        //    var httpContext = Context.GetHttpContext();
        //    var query = httpContext.Request.Query;
        //    string uid = query.GetQueryParameterValue<string>("uid");
        //    //string name = Context.User.Identity.Name;

        //    string connectID = Context.ConnectionId;
        //    //Call the addNewMessageToPage method to update clients.
        //    //var matk = db.TaiKhoans.Where(a => a.TenTaiKhoan == name).Select(a => a.MaTaiKhoan).FirstOrDefault();

        //    TinNhan tn = new TinNhan();
        //    tn.MaTaiKhoan = int.Parse(uid);
        //    tn.ThoiGian = DateTime.Now;
        //    tn.NoiDung = message;
        //    tn.MaDDH = MaDDH;
        //    if (CheckUID(int.Parse(uid), MaDDH) == true)
        //        tn.BuyerSeen = true;
        //    else
        //        tn.SellerSeen = true;
        //    db.TinNhans.Add(tn);
        //    db.SaveChanges();

        //    foreach (var connectionId in Connections.GetConnections(uid))
        //    {
        //        Clients.Client(connectionId).addChatMessage(senderName, message, int.Parse(uid), CheckInRoom(uid));

        //    }

        //    foreach (var connectionId in Connections.GetConnections(who))
        //    {

        //        Clients.Client(connectionId).addChatMessage(senderName, message, int.Parse(uid), CheckInRoom(who));
        //    }
        //}
        //public void AcceptNotification(int MaDDH)
        //{
        //    string uid = Context.QueryString["uid"];
        //    string connectID = Context.ConnectionId;
        //    //Call the addNewMessageToPage method to update clients.
        //    var buyerID = db.DonDatHangs.Where(a => a.MaDDH == MaDDH).FirstOrDefault();
        //    var sellerID = (from d in db.DonDatHangs
        //                    join s in db.SanPhams on d.MaSP equals s.MaSP
        //                    join t in db.TaiKhoans on s.MaTaiKhoan equals t.MaTaiKhoan
        //                    where d.MaDDH == MaDDH
        //                    select t).FirstOrDefault();
        //    ThongBao thongBao = new ThongBao();
        //    thongBao.MaDDH = MaDDH;
        //    thongBao.MaTaiKhoan = sellerID.MaTaiKhoan;
        //    thongBao.NoiDung = "Đơn hàng của bạn đã được " + sellerID.TenTaiKhoan + " chấp nhận, vui lòng kiểm tra";
        //    thongBao.ThoiGian = DateTime.Now;
        //    thongBao.SellerSeen = true;
        //    db.ThongBaos.Add(thongBao);
        //    db.SaveChanges();
        //    _check = 2;
        //    foreach (var connectionId in Connections.GetConnections(buyerID.MaTaiKhoan.ToString()))
        //    {
        //        CheckSeenNotification(buyerID.MaTaiKhoan.ToString());
        //    }
        //}
        //public void DenyNotification(int MaDDH)
        //{
        //    string uid = Context.QueryString["uid"];
        //    string connectID = Context.ConnectionId;
        //    //Call the addNewMessageToPage method to update clients.
        //    var buyerID = db.DonDatHangs.Where(a => a.MaDDH == MaDDH).FirstOrDefault();
        //    var sellerID = (from d in db.DonDatHangs
        //                    join s in db.SanPhams on d.MaSP equals s.MaSP
        //                    join t in db.TaiKhoans on s.MaTaiKhoan equals t.MaTaiKhoan
        //                    where d.MaDDH == MaDDH
        //                    select t).FirstOrDefault();
        //    ThongBao thongBao = new ThongBao();
        //    thongBao.MaDDH = MaDDH;
        //    thongBao.MaTaiKhoan = sellerID.MaTaiKhoan;
        //    thongBao.NoiDung = "Đơn hàng của bạn đã bị " + sellerID.TenTaiKhoan + " từ chối chấp nhận, vui lòng kiểm tra";
        //    thongBao.ThoiGian = DateTime.Now;
        //    thongBao.SellerSeen = true;
        //    db.ThongBaos.Add(thongBao);
        //    db.SaveChanges();
        //    _check = 3;
        //    foreach (var connectionId in Connections.GetConnections(buyerID.MaTaiKhoan.ToString()))
        //    {
        //        CheckSeenNotification(buyerID.MaTaiKhoan.ToString());
        //    }
        //}
        //public void HoanThanhNotification(int MaDDH)
        //{
        //    string uid = Context.QueryString["uid"];
        //    string connectID = Context.ConnectionId;
        //    var donhang = db.DonDatHangs.Where(a => a.MaDDH == MaDDH).FirstOrDefault();
        //    //Call the addNewMessageToPage method to update clients.
        //    var id = int.Parse(uid);
        //    var matk = db.TaiKhoans.Where(a => a.MaTaiKhoan == id).FirstOrDefault();

        //    if (donhang.MaTaiKhoan == id)
        //    {
        //        ThongBao thongBao = new ThongBao();
        //        thongBao.MaDDH = MaDDH;
        //        thongBao.MaTaiKhoan = matk.MaTaiKhoan;
        //        thongBao.NoiDung = "Đon hàng của bạn đã được " + matk.TenTaiKhoan + " chấp nhận hoàn thành, vui lòng kiểm tra";
        //        thongBao.BuyerSeen = true;
        //        thongBao.ThoiGian = DateTime.Now;
        //        db.ThongBaos.Add(thongBao);
        //        db.SaveChanges();
        //        _check = 4;
        //        var sellerID = (from d in db.DonDatHangs
        //                        join s in db.SanPhams on d.MaSP equals s.MaSP
        //                        where d.MaDDH == MaDDH
        //                        select s.MaTaiKhoan).FirstOrDefault();
        //        foreach (var connectionId in Connections.GetConnections(sellerID.ToString()))
        //        {
        //            CheckSeenNotification(sellerID.ToString());
        //        }
        //    }
        //    else
        //    {
        //        ThongBao thongBao = new ThongBao();
        //        thongBao.MaDDH = MaDDH;
        //        thongBao.MaTaiKhoan = id;
        //        thongBao.NoiDung = "Đon hàng của bạn đã được quản trị viên chấp nhận hoàn thành, vui lòng kiểm tra";
        //        thongBao.ThoiGian = DateTime.Now;
        //        db.ThongBaos.Add(thongBao);
        //        db.SaveChanges();
        //        _check = 4;
        //        var sellerID = (from d in db.DonDatHangs
        //                        join s in db.SanPhams on d.MaSP equals s.MaSP
        //                        where d.MaDDH == MaDDH && d.MaSP == s.MaSP
        //                        select s.MaTaiKhoan).FirstOrDefault();
        //        foreach (var connectionId in Connections.GetConnections(donhang.MaTaiKhoan.ToString()))
        //        {
        //            CheckSeenNotification(donhang.MaTaiKhoan.ToString());
        //        }
        //        foreach (var connectionId in Connections.GetConnections(sellerID.ToString()))
        //        {
        //            CheckSeenNotification(sellerID.ToString());
        //        }
        //    }

        //}
        //public void ChuyenTienNotification(int MaDDH)
        //{
        //    string uid = Context.QueryString["uid"];
        //    string connectID = Context.ConnectionId;
        //    //Call the addNewMessageToPage method to update clients.
        //    var id = int.Parse(uid);
        //    var matk = db.TaiKhoans.Where(a => a.MaTaiKhoan == id).FirstOrDefault();
        //    ThongBao thongBao = new ThongBao();
        //    thongBao.MaDDH = MaDDH;
        //    thongBao.MaTaiKhoan = id;
        //    thongBao.NoiDung = "Đon hàng của bạn đã được quản trị viên chuyển tiền, vui lòng kiểm tra";
        //    thongBao.ThoiGian = DateTime.Now;
        //    thongBao.BuyerSeen = true;
        //    db.ThongBaos.Add(thongBao);
        //    db.SaveChanges();
        //    _check = 5;
        //    var sellerID = (from d in db.DonDatHangs
        //                    join s in db.SanPhams on d.MaSP equals s.MaSP
        //                    where d.MaDDH == MaDDH
        //                    select s.MaTaiKhoan).FirstOrDefault();
        //    foreach (var connectionId in Connections.GetConnections(sellerID.ToString()))
        //    {
        //        CheckSeenNotification(sellerID.ToString());
        //    }
        //}
        //public void OfferNotification(int MaDDH)
        //{
        //    string uid = Context.QueryString["uid"];
        //    string connectID = Context.ConnectionId;
        //    //Call the addNewMessageToPage method to update clients.
        //    var id = int.Parse(uid);
        //    var matk = db.TaiKhoans.Where(a => a.MaTaiKhoan == id).FirstOrDefault();
        //    ThongBao thongBao = new ThongBao();
        //    thongBao.MaDDH = MaDDH;
        //    thongBao.MaTaiKhoan = id;
        //    thongBao.NoiDung = "Bài đăng của bạn đã được " + matk.TenTaiKhoan + " mua vui lòng kiểm tra";
        //    thongBao.ThoiGian = DateTime.Now;
        //    thongBao.BuyerSeen = true;
        //    db.ThongBaos.Add(thongBao);
        //    db.SaveChanges();
        //    _check = 1;
        //    var sellerID = (from d in db.DonDatHangs
        //                    join s in db.SanPhams on d.MaSP equals s.MaSP
        //                    where d.MaDDH == MaDDH
        //                    select s.MaTaiKhoan).FirstOrDefault();
        //    foreach (var connectionId in Connections.GetConnections(sellerID.ToString()))
        //    {
        //        CheckSeenNotification(sellerID.ToString());
        //    }
        //}
        //public void CheckSeenNotification(string id)
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
        //        Clients.Client(connectionId).addNotification(dem,_check);
        //    }
        //}
        //private bool CheckUID(int MaTK, int madonHang)
        //{
        //    var taikhoan = db.DonDatHangs.Where(a => a.MaDDH == madonHang && a.MaTaiKhoan == MaTK).FirstOrDefault();
        //    if (taikhoan != null)
        //        return true;
        //    return false;
        //}
        //private int CheckInRoom(string id)
        //{
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
        //    return base.OnConnected();
        //}

        //public Task OnDisconnected(bool stopCalled)
        //{
        //    string name = Context.QueryString["uid"];
        //    //string name = Context.User.Identity.Name;

        //    Connections.Remove(name, Context.ConnectionId);

        //    return base.OnDisconnected(stopCalled);
        //}

        //public Task OnReconnected()
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