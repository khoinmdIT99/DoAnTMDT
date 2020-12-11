namespace Web.nganluonAPI
{
    public class nganluongInfo
    {
        public static readonly string Merchant_id = "49767"; // mã Merchant
        public static readonly string Merchant_password = "7060744154ce7596e835129c2b0c4d69";  //Merchant password
        public static readonly string Receiver_email = "nguyenxuanchinh104@gmail.com";// email nhan tien
        public static readonly string UrlNganLuong = "https://sandbox.nganluong.vn:8088/nl35/checkout.api.nganluong.post.php";
        // dường dẫn khi thanh tán thành công
        public static readonly string return_url = "http://www.quanaonn.somee.com/confirm-orderPaymentOnline";
        // dường dẫn khi thanh tán thất bại
        public static readonly string cancel_url = "http://www.quanaonn.somee.com/cancel-order";

    }
}