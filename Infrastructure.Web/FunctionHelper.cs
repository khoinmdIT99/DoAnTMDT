using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Web
{
    public class SelectListModel
    {
        public string ItemValue { get; set; }
        public string ItemText { get; set; }
    }
    public class FunctionHelper
    {
        public static List<object> PayStatus()
        {
            var list = new List<object> {
                new  { ItemValue = "0", ItemText = "-- Lựa chọn --"},
                new  { ItemValue = "1", ItemText = "Đã thanh toán"},
                new  { ItemValue = "2", ItemText = "Chưa thanh toán"}
            };
            return list;
        }
        public static List<SelectListModel> TypePay()
        {
            var list = new List<SelectListModel> {
                new SelectListModel { ItemValue = "1", ItemText = "Thanh toán qua Momo"},
                new SelectListModel { ItemValue = "2", ItemText = "Thanh toán qua Paypal"},
                new SelectListModel { ItemValue = "3", ItemText = "Thanh toán qua Ngân hàng"},
                new SelectListModel { ItemValue = "4", ItemText = "Thanh toán khi nhận hàng"},
                new SelectListModel { ItemValue = "5", ItemText = "Thanh toán tại cửa hàng"}
            };
            return list;
        }
        public static List<SelectListModel> TypeShip()
        {
            var list = new List<SelectListModel> {
                new SelectListModel { ItemValue = "1", ItemText = "Qua shop nhận hàng"},
                new SelectListModel { ItemValue = "2", ItemText = "Giao hàng tận nơi"},
            };
            return list;
        }
    }
}
