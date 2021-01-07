using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Domain.Common.Security;
using Domain.Shop.Dto;
using Domain.Shop.Dto.Cart;
using Domain.Shop.Dto.CartProduct;
using Domain.Shop.Dto.Order;
using Domain.Shop.Dto.ShoppingCart;
using Domain.Shop.Entities;
using Domain.Shop.Entities.SystemManage;
using Domain.Shop.IRepositories;
using Infrastructure.Web;
using Infrastructure.Web.Onepay;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Web.Hubs;
using Web.Models;
using Web.MomoAPI;
using Web.PaypalHelpers;
using Web.Services;

namespace Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IShoppingCartRepository _shoppingCart;
        readonly IServiceProvider _services;
        private readonly IAccountRepository _accountRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProvinceRepository _iProvinceRepository;
        private readonly IDictrictRepository _iDictrictRepository;
        private readonly ITranhChapRepository _iTranhChapRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISystemInformationRepository _systemInformationRepository;
        private readonly IOptions<PaypalApiSetting> paypalSettings;
        private readonly IActionContextAccessor _accessor;
        private readonly IConfiguration _configuration;
        private readonly IUtils _utils;
        private readonly IPhanQuyenRepository _phanQuyenRepository;
        private readonly IQuyenRepository _quyenRepository;
        private readonly IVnPayLibrary _vnPayLibrary;
        private readonly IThongBaoRepository _thongBaoRepository;
        private IHubContext<RestaurantHub> _hubContext;
        const string SessionId = "_Id";

        public OrderController(IProductRepository productRepository, IShoppingCartRepository shoppingCart, IServiceProvider services, 
            IAccountRepository accountRepository, ICartRepository cartRepository, IDictrictRepository iDictrictRepository, IProvinceRepository iProvinceRepository, ITranhChapRepository iTranhChapRepository, IWebHostEnvironment webHostEnvironment, ISystemInformationRepository systemInformationRepository, IOptions<PaypalApiSetting> paypalSettings, IConfiguration configuration, IUtils utils, IVnPayLibrary vnPayLibrary, IActionContextAccessor accessor, IPhanQuyenRepository phanQuyenRepository, IQuyenRepository quyenRepository, IThongBaoRepository thongBaoRepository, IHubContext<RestaurantHub> hubContext)
        {
            this._productRepository = productRepository;
            this._shoppingCart = shoppingCart;
            this._services = services;
            this._accountRepository = accountRepository;
            this._cartRepository = cartRepository;
            this._iDictrictRepository = iDictrictRepository;
            this._iProvinceRepository = iProvinceRepository;
            _iTranhChapRepository = iTranhChapRepository;
            _webHostEnvironment = webHostEnvironment;
            _systemInformationRepository = systemInformationRepository;
            this.paypalSettings = paypalSettings;
            this._configuration = configuration;
            _utils = utils;
            _vnPayLibrary = vnPayLibrary;
            _accessor = accessor;
            _phanQuyenRepository = phanQuyenRepository;
            _quyenRepository = quyenRepository;
            _thongBaoRepository = thongBaoRepository;
            _hubContext = hubContext;
        }
        [AllowAnonymous]
        public IActionResult CheckCurrency()
        {
            HttpRequest cookie = _services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Request;
            string cartId = cookie.Cookies["cardId"];
            var tienviet = double.Parse(_shoppingCart.GetShoppingCartTotalPrice(cartId).ToString());
            var tienusd = 23.129;
            return Content(CurrencyHelper.VNDTOUSD(tienviet, tienusd).ToString());
        }
        [AllowAnonymous]
        public IActionResult OrderManager()
        {
            var customer = _accountRepository.GetCustomerViewModel(HttpContext.Session.GetString(SessionId));
            if (customer != null)
            {
                IEnumerable<CartViewModel> model = _cartRepository.GetCartViewModels().ToList().Where(x =>x.CustomerId == customer.Id).ToList();
                foreach (var item in model)
                {
                    item.Customer = _accountRepository.GetCustomerViewModel(item.CustomerId);
                    item.Customer.District = _iDictrictRepository.GetDictrictViewModel(_accountRepository.GetCustomerViewModel(item.CustomerId).District).Name;
                    item.Customer.Province = _iProvinceRepository.GetProvinceViewModel(_accountRepository.GetCustomerViewModel(item.CustomerId).Province).Name;
                }
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ToCao([FromBody]TranhChapViewModel model)
        {
            try
            {
                var tranhchap = new TranhChap
                {
                    MaDDH = model.MaDDH,
                    NoiDung = model.typeData + model.txtOthers,
                    ThoiGian = DateTime.Now,
                    LienHe = model.txtContact
                };
                await _iTranhChapRepository.AddAsync(tranhchap);
                await _iTranhChapRepository.SaveAsync();
                return Json("Bạn đã gửi thành công !");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult TabThongTinTaiKhoan()
        {
            HttpRequest cookie = _services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Request;
            if (cookie != null)
            {
                string cartId = cookie.Cookies["cardId"];
                List<CartProductViewModel> cartProductViewModels = new List<CartProductViewModel>();

                foreach (var item in _shoppingCart.GetCartProducts(cartId))
                {
                    var cartProductViewModel = new CartProductViewModel()
                    {
                        Id = item.Id,
                        CartId = item.CartId,
                        Cart = _cartRepository.GetCartViewModel(item.CartId),
                        ProductId = item.ProductId,
                        Product = _productRepository.GetProductViewModelById(item.ProductId),
                        Price = item.Price,
                        PriceType = item.PriceType,
                        Quantity = item.Quantity,
                        Total = item.Total
                    };
                    cartProductViewModels.Add(cartProductViewModel);

                }
                var cart = new ShoppingCart()
                {
                    Id = cartId,
                    cartProducts = cartProductViewModels,
                    Total = _shoppingCart.GetShoppingCartTotal(cartId)
                };
                //get customer
                string customerId = SecurityManager.getUserId(cookie.Cookies[SecurityManager._securityToken]);

                var customer = _accountRepository.GetCustomerViewModel(HttpContext.Session.GetString(SessionId));
                if (HttpContext.Session.GetString(SessionId) == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                if (customer == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                var model = new OrderViewModel()
                {
                    CustomerId = customer.Id,
                    FullName = customer.FirstName + " " + customer.LastName,
                    Address = customer.Address,
                    District = customer.District,
                    Province = customer.Province,
                    PhoneNo = customer.PhoneNo,
                    Email = customer.Email,
                    ShoppingCart = cart
                };
                ViewBag.province = model.Province == null ? new SelectList(_iProvinceRepository.All, "Id", "Name") : new SelectList(_iProvinceRepository.All, "Id", "Name", model.Province);
                ViewBag.district = model.District == null ? new SelectList(_iDictrictRepository.All, "Id", "Name") : new SelectList(_iDictrictRepository.All, "Id", "Name", model.District);
                return PartialView("_PartialThongTinTaiKhoan",model);
            }
            return PartialView("_PartialThongTinTaiKhoan");
        }
        [AllowAnonymous]
        public async Task<IActionResult> CheckLogin()
        {
            var customer =
                await _accountRepository.All.FirstOrDefaultAsync(x => x.Id == HttpContext.Session.GetString(SessionId));
            if (customer != null )
            {
                if (customer.TinhTrang.Equals("Chưa kích hoạt") || customer.TinhTrang == null)
                {
                    return Json(new { success = false, message = "Vui lòng kích hoạt Email tài khoản để mua hàng" });
                }
                return Json(new { success = true ,message= customer.TinhTrang });
            }
            return Json(new { success = false, message = "Vui lòng đăng nhập để mua hàng" });
        }
        [AllowAnonymous]
        [Route("DonHang.html", Name = "DonHang")]
        public IActionResult HomeOrder()
        {
            try
            {
                HttpRequest cookie = _services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Request;
                if (cookie != null)
                {
                    string cartId = cookie.Cookies["cardId"];
                    List<CartProductViewModel> cartProductViewModels = new List<CartProductViewModel>();

                    foreach (var item in _shoppingCart.GetCartProducts(cartId))
                    {
                        var cartProductViewModel = new CartProductViewModel()
                        {
                            Id = item.Id,
                            CartId = item.CartId,
                            Cart = _cartRepository.GetCartViewModel(item.CartId),
                            ProductId = item.ProductId,
                            Product = _productRepository.GetProductViewModelById(item.ProductId),
                            Price = item.Price,
                            PriceType = item.PriceType,
                            Quantity = item.Quantity,
                            Total = item.Total
                        };
                        cartProductViewModels.Add(cartProductViewModel);

                    }
                    var cart = new ShoppingCart()
                    {
                        Id = cartId,
                        cartProducts = cartProductViewModels,
                        Total = _shoppingCart.GetShoppingCartTotal(cartId),
                        TotalPrice = _shoppingCart.GetShoppingCartTotalPrice(cartId)
                    };
                    ////get customer
                    //string customerId = SecurityManager.getUserId(cookie.Cookies[SecurityManager._securityToken]);

                    var customer = _accountRepository.GetCustomerViewModel(HttpContext.Session.GetString(SessionId));
                    if (HttpContext.Session.GetString(SessionId) == null)
                    {
                        return RedirectToAction("Index", "Home", new { thongbao = "Vui lòng đăng nhập để thanh toán" });
                    }

                    if (customer == null)
                    {
                        return RedirectToAction("Index", "Home", new { thongbao = "Vui lòng đăng nhập để thanh toán" });
                    }
                    var model = new OrderViewModel()
                    {
                        CustomerId = customer.Id,
                        FullName = customer.FirstName +" "+customer.LastName,
                        Address = customer.Address,
                        District = customer.District,
                        Province = customer.Province,
                        PhoneNo = customer.PhoneNo,
                        Email = customer.Email,
                        ShoppingCart = cart
                    };
                    ViewBag.PayTypes = GetPayList();
                    ViewBag.ShipTypes = GetShipList();
                    ViewBag.province = model.Province == null ? new SelectList(_iProvinceRepository.All, "Id", "Name") : new SelectList(_iProvinceRepository.All, "Id", "Name", model.Province);
                    ViewBag.district = model.District == null ? new SelectList(_iDictrictRepository.All, "Id", "Name") : new SelectList(_iDictrictRepository.All, "Id", "Name",model.District);
                    return View(model);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return View();
                throw;
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddCart(OrderViewModel model)
        {
            HttpRequest cookie = _services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Request;
            string cartId = cookie.Cookies["cardId"];
            try
            {
                ViewBag.PayTypes = GetPayList();
                ViewBag.ShipTypes = GetShipList();
                var customerpresent = _accountRepository.GetCustomerViewModel(HttpContext.Session.GetString(SessionId));
                var getmaquyen =
                    await _phanQuyenRepository.All.FirstOrDefaultAsync(x => x.MaTaiKhoan == customerpresent.Id);
                var idmaquyen = getmaquyen.MaQuyen;
                var tenquyen = await _quyenRepository.All.FirstOrDefaultAsync(x => x.MaQuyen == idmaquyen);
                long amountafter;
                if (tenquyen.GiamGia > 0)
                {
                    amountafter =
                        (long) (_shoppingCart.GetShoppingCartTotalPrice(cartId) -
                                Math.Round((decimal) (tenquyen.GiamGia *
                                                      _shoppingCart.GetShoppingCartTotalPrice(cartId))) / 100);
                }
                else
                {
                    amountafter = _shoppingCart.GetShoppingCartTotalPrice(cartId);
                }

                if (model.PaymentMethod == 1)
                {

                    //request params need to request to MoMo system
                    string endpoint = momoInfo.endpoint;
                    string partnerCode = momoInfo.partnerCode;
                    string accessKey = momoInfo.accessKey;
                    string serectkey = momoInfo.serectkey;
                    string orderInfo = momoInfo.orderInfo;
                    string returnUrl = momoInfo.returnUrl;
                    string notifyurl = momoInfo.notifyurl;

                    string amount = amountafter.ToString();
                    string orderid = Guid.NewGuid().ToString();
                    string requestId = Guid.NewGuid().ToString();
                    string extraData = "";

                    //Before sign HMAC SHA256 signature
                    string rawHash = "partnerCode=" +
                                     partnerCode + "&accessKey=" +
                                     accessKey + "&requestId=" +
                                     requestId + "&amount=" +
                                     amount + "&orderId=" +
                                     orderid + "&orderInfo=" +
                                     orderInfo + "&returnUrl=" +
                                     returnUrl + "&notifyUrl=" +
                                     notifyurl + "&extraData=" +
                                     extraData;


                    MoMoSecurity crypto = new MoMoSecurity();
                    //sign signature SHA256
                    string signature = crypto.signSHA256(rawHash, serectkey);

                    //build body json request
                    JObject message = new JObject
                    {
                        {"partnerCode", partnerCode},
                        {"accessKey", accessKey},
                        {"requestId", requestId},
                        {"amount", amount},
                        {"orderId", orderid},
                        {"orderInfo", orderInfo},
                        {"returnUrl", returnUrl},
                        {"notifyUrl", notifyurl},
                        {"extraData", extraData},
                        {"requestType", "captureMoMoWallet"},
                        {"signature", signature}

                    };
                    string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
                    JObject jmessage = JObject.Parse(responseFromMomo);
                    string payURL = jmessage.GetValue("payUrl").ToString();
                    HttpContext.Session.Set("Order", model);
                    return Redirect(payURL);
                }
                else if (model.PaymentMethod == 2)
                {
                    var paypalAPI = new PaypalAPI(_configuration);
                    var tienviet = double.Parse(amountafter.ToString());
                    var tienusd = 23129;
                    var total = Math.Round(CurrencyHelper.VNDTOUSD(tienviet, tienusd), 1,
                        MidpointRounding.AwayFromZero);
                    HttpContext.Session.Set("Order", model);
                    String url = await paypalAPI.GetRedirectUrlToPaypal(total, "USD");
                    return Redirect(url);
                }
                else if (model.PaymentMethod == 3)
                {
                    string vnp_Url =
                        _configuration.GetSection("VNPayInfo").GetSection("vnp_Url").Value; //URL thanh toan cua VNPAY 
                    string vnp_TmnCode =
                        _configuration.GetSection("VNPayInfo").GetSection("vnp_TmnCode").Value; //Ma website
                    string vnp_HashSecret =
                        _configuration.GetSection("VNPayInfo").GetSection("vnp_HashSecret").Value; //Chuoi bi mat
                    if (string.IsNullOrEmpty(vnp_TmnCode) || string.IsNullOrEmpty(vnp_HashSecret))
                    {
                        return Json(
                            "Vui lòng cấu hình các tham số: vnp_TmnCode,vnp_HashSecret trong file appsetting.json");
                    }

                    var hostname = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
                    _vnPayLibrary.AddRequestData("vnp_Version", "2.0.0");
                    _vnPayLibrary.AddRequestData("vnp_Command", "pay");
                    _vnPayLibrary.AddRequestData("vnp_TmnCode", vnp_TmnCode);
                    _vnPayLibrary.AddRequestData("vnp_Amount",
                        (amountafter * 100).ToString());
                    _vnPayLibrary.AddRequestData("vnp_BankCode", "");
                    _vnPayLibrary.AddRequestData("vnp_CreateDate",
                        _cartRepository.Get(cartId).CreateAt?.ToString("yyyyMMddHHmmss"));
                    _vnPayLibrary.AddRequestData("vnp_CurrCode", "VND");
                    _vnPayLibrary.AddRequestData("vnp_IpAddr", _utils.GetIpAddress());
                    _vnPayLibrary.AddRequestData("vnp_Locale", "vn");
                    _vnPayLibrary.AddRequestData("vnp_OrderInfo", model.Comment);
                    _vnPayLibrary.AddRequestData("vnp_OrderType", "130001"); //default value: other
                    _vnPayLibrary.AddRequestData("vnp_ReturnUrl", $"{hostname}/order/ket-qua-vnpay");
                    _vnPayLibrary.AddRequestData("vnp_TxnRef", cartId);
                    string paymentUrl = _vnPayLibrary.CreateRequestUrl(vnp_Url, vnp_HashSecret);

                    return Redirect(paymentUrl);
                }
                else if (model.PaymentMethod == 4)
                {
                    HttpContext.Session.Set("Order", model);
                    return RedirectToAction("OnePay");
                }
                else if (model.PaymentMethod == 5)
                {
                    HttpContext.Session.Set("Order", model);
                    return RedirectToAction("OnePayvn");
                }
                else
                {
                    List<CartProductViewModel> cartProductViewModels = new List<CartProductViewModel>();

                    foreach (var item in _shoppingCart.GetCartProducts(cartId))
                    {
                        var cartProductViewModel = new CartProductViewModel()
                        {
                            Id = item.Id,
                            CartId = item.CartId,
                            Cart = _cartRepository.GetCartViewModel(item.CartId),
                            ProductId = item.ProductId,
                            Product = _productRepository.GetProductViewModelById(item.ProductId),
                            Price = item.Price,
                            PriceType = item.PriceType,
                            Quantity = item.Quantity,
                            Total = item.Total
                        };
                        cartProductViewModels.Add(cartProductViewModel);

                    }

                    var cart = new ShoppingCart()
                    {
                        Id = cartId,
                        cartProducts = cartProductViewModels,
                        Total = _shoppingCart.GetShoppingCartTotal(cartId),
                        TotalPrice = _shoppingCart.GetShoppingCartTotalPrice(cartId)
                    };
                    var customer = _accountRepository.GetCustomerViewModel(HttpContext.Session.GetString(SessionId));
                    var orderViewModel = new OrderViewModel()
                    {
                        CustomerId = customer.Id,
                        FullName = customer.FirstName + " " + customer.LastName,
                        Address = customer.Address,
                        District = customer.District,
                        Province = customer.Province,
                        PhoneNo = customer.PhoneNo,
                        Email = customer.Email,
                        ShoppingCart = cart,
                        Status = "Chưa xử lý"
                    };
                    if (tenquyen.GiamGia > 0)
                    {
                        cart.TotalPrice =
                            (long)(cart.TotalPrice - Math.Round((decimal)(tenquyen.GiamGia * cart.TotalPrice)) / 100);
                    }
                    ViewBag.GiaTienCu = cart.TotalPrice;
                    ViewBag.LoaiTaiKhoan = tenquyen.TenQuyen;
                    ViewBag.GiamGia = tenquyen.GiamGia;
                    model.Status = "Chưa xử lý";
                    var kq = await SaveOrder(model);
                    ViewBag.KQ =
                        !kq
                            ? "Hệ thống gặp lỗi tuy nhiên đơn hàng của bạn vẫn được nhận"
                            : "Đơn hàng của bạn đã đặt thành công.Hệ thống sẽ liên hệ với bạn sớm";
                    _shoppingCart.ClearCart(cookie.Cookies["cardId"]);
                    HttpContext.Session.Remove("AddProducts");
                    return View(orderViewModel);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> Onepay()
        {
            HttpRequest cookie = _services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Request;
            string cartId = cookie.Cookies["cardId"];
            List<CartProductViewModel> cartProductViewModels = new List<CartProductViewModel>();

            foreach (var item in _shoppingCart.GetCartProducts(cartId))
            {
                var cartProductViewModel = new CartProductViewModel()
                {
                    Id = item.Id,
                    CartId = item.CartId,
                    Cart = _cartRepository.GetCartViewModel(item.CartId),
                    ProductId = item.ProductId,
                    Product = _productRepository.GetProductViewModelById(item.ProductId),
                    Price = item.Price,
                    PriceType = item.PriceType,
                    Quantity = item.Quantity,
                    Total = item.Total
                };
                cartProductViewModels.Add(cartProductViewModel);

            }
            var cart = new ShoppingCart()
            {
                Id = cartId,
                cartProducts = cartProductViewModels,
                Total = _shoppingCart.GetShoppingCartTotal(cartId),
                TotalPrice = _shoppingCart.GetShoppingCartTotalPrice(cartId)
            };
            var customerpresent = _accountRepository.GetCustomerViewModel(HttpContext.Session.GetString(SessionId));
            var getmaquyen =
                await _phanQuyenRepository.All.FirstOrDefaultAsync(x => x.MaTaiKhoan == customerpresent.Id);
            var idmaquyen = getmaquyen.MaQuyen;
            var tenquyen = await _quyenRepository.All.FirstOrDefaultAsync(x => x.MaQuyen == idmaquyen);
            long amountafter;
            if (tenquyen.GiamGia > 0)
            {
                amountafter =
                    (long)(cart.TotalPrice -
                           Math.Round((decimal)(tenquyen.GiamGia *
                                                cart.TotalPrice)) / 100);
            }
            else
            {
                amountafter = cart.TotalPrice;
            }
            var ip = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();
            string url = RedirectOnepay(RandomString(), (amountafter * 100).ToString(), ip);
            return Redirect(url);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Onepayvn()
        {
            HttpRequest cookie = _services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Request;
            string cartId = cookie.Cookies["cardId"];
            List<CartProductViewModel> cartProductViewModels = new List<CartProductViewModel>();

            foreach (var item in _shoppingCart.GetCartProducts(cartId))
            {
                var cartProductViewModel = new CartProductViewModel()
                {
                    Id = item.Id,
                    CartId = item.CartId,
                    Cart = _cartRepository.GetCartViewModel(item.CartId),
                    ProductId = item.ProductId,
                    Product = _productRepository.GetProductViewModelById(item.ProductId),
                    Price = item.Price,
                    PriceType = item.PriceType,
                    Quantity = item.Quantity,
                    Total = item.Total
                };
                cartProductViewModels.Add(cartProductViewModel);

            }
            var cart = new ShoppingCart()
            {
                Id = cartId,
                cartProducts = cartProductViewModels,
                Total = _shoppingCart.GetShoppingCartTotal(cartId),
                TotalPrice = _shoppingCart.GetShoppingCartTotalPrice(cartId)
            };
            var customerpresent = _accountRepository.GetCustomerViewModel(HttpContext.Session.GetString(SessionId));
            var getmaquyen =
                await _phanQuyenRepository.All.FirstOrDefaultAsync(x => x.MaTaiKhoan == customerpresent.Id);
            var idmaquyen = getmaquyen.MaQuyen;
            var tenquyen = await _quyenRepository.All.FirstOrDefaultAsync(x => x.MaQuyen == idmaquyen);
            long amountafter;
            if (tenquyen.GiamGia > 0)
            {
                amountafter =
                    (long)(cart.TotalPrice -
                           Math.Round((decimal)(tenquyen.GiamGia *
                                                cart.TotalPrice)) / 100);
            }
            else
            {
                amountafter = cart.TotalPrice;
            }
            var ip = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();
            string url = RedirectOnepayvn(RandomString(), (amountafter * 100).ToString(), ip);
            return Redirect(url);
        }
        /// <summary>
        /// Sinh ky tu ngau nhien
        /// </summary>
        private string RandomString()
        {
            var str = new StringBuilder();
            var random = new Random();
            for (int i = 0; i <= 5; i++)
            {
                var c = Convert.ToChar(Convert.ToInt32(random.Next(65, 68)));
                str.Append(c);
            }
            return str.ToString().ToLower();
        }
        public string RedirectOnepayvn(string codeInvoice, string amount, string ip)
        {
            // Khoi tao lop thu vien
            VPCRequest conn = new VPCRequest(OnepayPropertyNoiDia.URL_ONEPAY_TEST);
            conn.SetSecureSecret(OnepayPropertyNoiDia.HASH_CODE);

            // Gan cac thong so de truyen sang cong thanh toan onepay
            conn.AddDigitalOrderField("AgainLink", OnepayPropertyNoiDia.AGAIN_LINK);
            conn.AddDigitalOrderField("Title", "Nội thất sài thành");
            conn.AddDigitalOrderField("vpc_Locale", OnepayPropertyNoiDia.PAYGATE_LANGUAGE);
            conn.AddDigitalOrderField("vpc_Version", OnepayPropertyNoiDia.VERSION);
            conn.AddDigitalOrderField("vpc_Command", OnepayPropertyNoiDia.COMMAND);
            conn.AddDigitalOrderField("vpc_Merchant", OnepayPropertyNoiDia.MERCHANT_ID);
            conn.AddDigitalOrderField("vpc_AccessCode", OnepayPropertyNoiDia.ACCESS_CODE);
            conn.AddDigitalOrderField("vpc_MerchTxnRef", RandomString());
            conn.AddDigitalOrderField("vpc_Currency", "VND");
            conn.AddDigitalOrderField("vpc_OrderInfo", codeInvoice);
            conn.AddDigitalOrderField("vpc_Amount", amount);
            conn.AddDigitalOrderField("vpc_ReturnURL", Url.Action("OnepayResponsevn", "Order", null, Request.Scheme, null));

            // Thong tin them ve khach hang. De trong neu khong co thong tin
            conn.AddDigitalOrderField("vpc_SHIP_Street01", "");
            conn.AddDigitalOrderField("vpc_SHIP_Provice", "");
            conn.AddDigitalOrderField("vpc_SHIP_City", "");
            conn.AddDigitalOrderField("vpc_SHIP_Country", "");
            conn.AddDigitalOrderField("vpc_Customer_Phone", "");
            conn.AddDigitalOrderField("vpc_Customer_Email", "");
            conn.AddDigitalOrderField("vpc_Customer_Id", "");
            conn.AddDigitalOrderField("vpc_TicketNo", ip);

            string url = conn.Create3PartyQueryString();
            return url;
        }
        /// <summary>
        /// Redirect den onepay
        /// </summary>
        public string RedirectOnepay(string codeInvoice, string amount, string ip)
        {
            // Khoi tao lop thu vien
            VPCRequest conn = new VPCRequest(OnepayProperty.URL_ONEPAY_TEST);
            conn.SetSecureSecret(OnepayProperty.HASH_CODE);

            // Gan cac thong so de truyen sang cong thanh toan onepay
            conn.AddDigitalOrderField("AgainLink", OnepayProperty.AGAIN_LINK);
            conn.AddDigitalOrderField("Title", "Nội thất sài thành");
            conn.AddDigitalOrderField("vpc_Locale", OnepayProperty.PAYGATE_LANGUAGE);
            conn.AddDigitalOrderField("vpc_Version", OnepayProperty.VERSION);
            conn.AddDigitalOrderField("vpc_Command", OnepayProperty.COMMAND);
            conn.AddDigitalOrderField("vpc_Merchant", OnepayProperty.MERCHANT_ID);
            conn.AddDigitalOrderField("vpc_AccessCode", OnepayProperty.ACCESS_CODE);
            conn.AddDigitalOrderField("vpc_MerchTxnRef", RandomString());
            conn.AddDigitalOrderField("vpc_OrderInfo", codeInvoice);
            conn.AddDigitalOrderField("vpc_Amount", amount);
            conn.AddDigitalOrderField("vpc_ReturnURL", Url.Action("OnepayResponse", "Order", null, Request.Scheme, null));

            // Thong tin them ve khach hang. De trong neu khong co thong tin
            conn.AddDigitalOrderField("vpc_SHIP_Street01", "");
            conn.AddDigitalOrderField("vpc_SHIP_Provice", "");
            conn.AddDigitalOrderField("vpc_SHIP_City", "");
            conn.AddDigitalOrderField("vpc_SHIP_Country", "");
            conn.AddDigitalOrderField("vpc_Customer_Phone", "");
            conn.AddDigitalOrderField("vpc_Customer_Email", "");
            conn.AddDigitalOrderField("vpc_Customer_Id", "");
            conn.AddDigitalOrderField("vpc_TicketNo", ip);

            string url = conn.Create3PartyQueryString();
            return url;
        }
        [AllowAnonymous]
        public async Task<IActionResult> OnepayResponse()
        {
            string hashvalidateResult = "";

            // Khoi tao lop thu vien
            VPCRequest conn = new VPCRequest(OnepayProperty.URL_ONEPAY_TEST);
            conn.SetSecureSecret(OnepayProperty.HASH_CODE);

            // Xu ly tham so tra ve va du lieu ma hoa
            string a = "https://" + Request.Host.ToString() + "/Order/Onepay" + Request.QueryString.ToString();
            hashvalidateResult = conn.Process3PartyResponse(HttpUtility.ParseQueryString(new Uri(a).Query));

            // Lay tham so tra ve tu cong thanh toan
            string vpc_TxnResponseCode = conn.GetResultField("vpc_TxnResponseCode");
            string amount = conn.GetResultField("vpc_Amount");
            string localed = conn.GetResultField("vpc_Locale");
            string command = conn.GetResultField("vpc_Command");
            string version = conn.GetResultField("vpc_Version");
            string cardType = conn.GetResultField("vpc_Card");
            string orderInfo = conn.GetResultField("vpc_OrderInfo");
            string merchantID = conn.GetResultField("vpc_Merchant");
            string authorizeID = conn.GetResultField("vpc_AuthorizeId");
            string merchTxnRef = conn.GetResultField("vpc_MerchTxnRef");
            string transactionNo = conn.GetResultField("vpc_TransactionNo");
            string acqResponseCode = conn.GetResultField("vpc_AcqResponseCode");
            string txnResponseCode = vpc_TxnResponseCode;
            string message = conn.GetResultField("vpc_Message");

            // Kiem tra 2 tham so tra ve quan trong nhat
            if (hashvalidateResult == "CORRECTED" && txnResponseCode.Trim() == "0")
            {

                var session = HttpContext.Session.Get<OrderViewModel>("Order");
                session.Status = "Chưa xử lý";
                await SaveOrder(session);
                HttpRequest cookie = _services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Request;
                string cartId = cookie.Cookies["cardId"];
                List<CartProductViewModel> cartProductViewModels = new List<CartProductViewModel>();

                foreach (var item in _shoppingCart.GetCartProducts(cartId))
                {
                    var cartProductViewModel = new CartProductViewModel()
                    {
                        Id = item.Id,
                        CartId = item.CartId,
                        Cart = _cartRepository.GetCartViewModel(item.CartId),
                        ProductId = item.ProductId,
                        Product = _productRepository.GetProductViewModelById(item.ProductId),
                        Price = item.Price,
                        PriceType = item.PriceType,
                        Quantity = item.Quantity,
                        Total = item.Total
                    };
                    cartProductViewModels.Add(cartProductViewModel);

                }
                var cart = new ShoppingCart()
                {
                    Id = cartId,
                    cartProducts = cartProductViewModels,
                    Total = _shoppingCart.GetShoppingCartTotal(cartId),
                    TotalPrice = _shoppingCart.GetShoppingCartTotalPrice(cartId)
                };
                var customer = _accountRepository.GetCustomerViewModel(HttpContext.Session.GetString(SessionId));
                var getmaquyen = await _phanQuyenRepository.All.FirstOrDefaultAsync(x => x.MaTaiKhoan == customer.Id);
                var idmaquyen = getmaquyen.MaQuyen;
                var tenquyen = await _quyenRepository.All.FirstOrDefaultAsync(x => x.MaQuyen == idmaquyen);
                var orderViewModel = new OrderViewModel()
                {
                    CustomerId = customer.Id,
                    FullName = customer.FirstName + " " + customer.LastName,
                    Address = customer.Address,
                    District = customer.District,
                    Province = customer.Province,
                    PhoneNo = customer.PhoneNo,
                    Email = customer.Email,
                    ShoppingCart = cart,
                    Status = "Chưa xử lý"
                };
                ViewBag.GiaTienCu = cart.TotalPrice;
                ViewBag.LoaiTaiKhoan = tenquyen.TenQuyen;
                ViewBag.GiamGia = tenquyen.GiamGia;
                if (tenquyen.GiamGia > 0)
                {
                    cart.TotalPrice =
                        (long)(cart.TotalPrice - Math.Round((decimal)(tenquyen.GiamGia * cart.TotalPrice)) / 100);
                }
                _shoppingCart.ClearCart(cookie.Cookies["cardId"]);
                HttpContext.Session.Remove("AddProducts");
                HttpContext.Session.Remove("Order");
                ViewBag.TinhTrang = "Chưa xử lý";
                return View("PaySuccess", orderViewModel);
                //return View("PaySuccess");
            }
            else
            {
                return RedirectToAction("HomeOrder", "Order");
                //return Content("PayUnSuccess");
            }
        }
        [AllowAnonymous]
        public async Task<IActionResult> OnepayResponsevn()
        {
            string hashvalidateResult = "";

            // Khoi tao lop thu vien
            VPCRequest conn = new VPCRequest(OnepayPropertyNoiDia.URL_ONEPAY_TEST);
            conn.SetSecureSecret(OnepayPropertyNoiDia.HASH_CODE);

            // Xu ly tham so tra ve va du lieu ma hoa
            string a = "https://" + Request.Host.ToString() + "/Onepay/Onepayvn" + Request.QueryString.ToString();
            hashvalidateResult = conn.Process3PartyResponse(HttpUtility.ParseQueryString(new Uri(a).Query));

            // Lay tham so tra ve tu cong thanh toan
            string vpc_TxnResponseCode = conn.GetResultField("vpc_TxnResponseCode");
            string amount = conn.GetResultField("vpc_Amount");
            string localed = conn.GetResultField("vpc_Locale");
            string command = conn.GetResultField("vpc_Command");
            string version = conn.GetResultField("vpc_Version");
            string cardType = conn.GetResultField("vpc_Card");
            string orderInfo = conn.GetResultField("vpc_OrderInfo");
            string merchantID = conn.GetResultField("vpc_Merchant");
            string authorizeID = conn.GetResultField("vpc_AuthorizeId");
            string merchTxnRef = conn.GetResultField("vpc_MerchTxnRef");
            string transactionNo = conn.GetResultField("vpc_TransactionNo");
            string acqResponseCode = conn.GetResultField("vpc_AcqResponseCode");
            string txnResponseCode = vpc_TxnResponseCode;
            string message = conn.GetResultField("vpc_Message");

            // Kiem tra 2 tham so tra ve quan trong nhat
            if (hashvalidateResult == "CORRECTED" && txnResponseCode.Trim() == "0")
            {

                var session = HttpContext.Session.Get<OrderViewModel>("Order");
                session.Status = "Chưa xử lý";
                await SaveOrder(session);
                HttpRequest cookie = _services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Request;
                string cartId = cookie.Cookies["cardId"];
                List<CartProductViewModel> cartProductViewModels = new List<CartProductViewModel>();

                foreach (var item in _shoppingCart.GetCartProducts(cartId))
                {
                    var cartProductViewModel = new CartProductViewModel()
                    {
                        Id = item.Id,
                        CartId = item.CartId,
                        Cart = _cartRepository.GetCartViewModel(item.CartId),
                        ProductId = item.ProductId,
                        Product = _productRepository.GetProductViewModelById(item.ProductId),
                        Price = item.Price,
                        PriceType = item.PriceType,
                        Quantity = item.Quantity,
                        Total = item.Total
                    };
                    cartProductViewModels.Add(cartProductViewModel);

                }
                var cart = new ShoppingCart()
                {
                    Id = cartId,
                    cartProducts = cartProductViewModels,
                    Total = _shoppingCart.GetShoppingCartTotal(cartId),
                    TotalPrice = _shoppingCart.GetShoppingCartTotalPrice(cartId)
                };
                var customer = _accountRepository.GetCustomerViewModel(HttpContext.Session.GetString(SessionId));
                var getmaquyen = await _phanQuyenRepository.All.FirstOrDefaultAsync(x => x.MaTaiKhoan == customer.Id);
                var idmaquyen = getmaquyen.MaQuyen;
                var tenquyen = await _quyenRepository.All.FirstOrDefaultAsync(x => x.MaQuyen == idmaquyen);
                var orderViewModel = new OrderViewModel()
                {
                    CustomerId = customer.Id,
                    FullName = customer.FirstName + " " + customer.LastName,
                    Address = customer.Address,
                    District = customer.District,
                    Province = customer.Province,
                    PhoneNo = customer.PhoneNo,
                    Email = customer.Email,
                    ShoppingCart = cart,
                    Status = "Chưa xử lý"
                };
                ViewBag.GiaTienCu = cart.TotalPrice;
                ViewBag.LoaiTaiKhoan = tenquyen.TenQuyen;
                ViewBag.GiamGia = tenquyen.GiamGia;
                if (tenquyen.GiamGia > 0)
                {
                    cart.TotalPrice =
                        (long)(cart.TotalPrice - Math.Round((decimal)(tenquyen.GiamGia * cart.TotalPrice)) / 100);
                }
                _shoppingCart.ClearCart(cookie.Cookies["cardId"]);
                HttpContext.Session.Remove("AddProducts");
                HttpContext.Session.Remove("Order");
                ViewBag.TinhTrang = "Chưa xử lý";
                return View("PaySuccess", orderViewModel);
                //return View("PaySuccess");
            }
            else if (hashvalidateResult == "INVALIDATED" && txnResponseCode.Trim() == "0")
            {
                return Content("PayPending");
            }
            else
            {
                return Content("PayUnSuccess");
            }
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("ket-qua-vnpay")]
        public async Task<IActionResult> KetQuaVnPay()
        {
            {
                string vnp_HashSecret = _configuration.GetSection("VNPayInfo").GetSection("vnp_HashSecret").Value; //Chuoi bi mat
                string vnp_Amount = Request.Query["vnp_Amount"];
                string vnp_BankCode = Request.Query["vnp_BankCode"];
                string vnp_BankTranNo = Request.Query["vnp_BankTranNo"];
                string vnp_CardType = Request.Query["vnp_CardType"];
                string vnp_OrderInfo = Request.Query["vnp_OrderInfo"];
                string vnp_PayDate = Request.Query["vnp_PayDate"];
                string vnp_ResponseCode = Request.Query["vnp_ResponseCode"];
                string vnp_TmnCode = Request.Query["vnp_TmnCode"];
                string vnp_TransactionNo = Request.Query["vnp_TransactionNo"];
                string vnp_TxnRef = Request.Query["vnp_TxnRef"];
                string vnp_SecureHashType = Request.Query["vnp_SecureHashType"];
                string vnp_SecureHash = Request.Query["vnp_SecureHash"];

                _vnPayLibrary.AddResponseData("vnp_Amount", vnp_Amount);
                _vnPayLibrary.AddResponseData("vnp_BankCode", vnp_BankCode);
                _vnPayLibrary.AddResponseData("vnp_BankTranNo", vnp_BankTranNo);
                _vnPayLibrary.AddResponseData("vnp_CardType", vnp_CardType);
                _vnPayLibrary.AddResponseData("vnp_OrderInfo", vnp_OrderInfo);
                _vnPayLibrary.AddResponseData("vnp_PayDate", vnp_PayDate);
                _vnPayLibrary.AddResponseData("vnp_ResponseCode", vnp_ResponseCode);
                _vnPayLibrary.AddResponseData("vnp_TmnCode", vnp_TmnCode);
                _vnPayLibrary.AddResponseData("vnp_TransactionNo", vnp_TransactionNo);
                _vnPayLibrary.AddResponseData("vnp_TxnRef", vnp_TxnRef);
                _vnPayLibrary.AddResponseData("vnp_SecureHashType", vnp_SecureHashType);
                _vnPayLibrary.AddResponseData("vnp_SecureHash", vnp_SecureHash);

                long orderId = Convert.ToInt64(_vnPayLibrary.GetResponseData("vnp_TxnRef"));
                bool checkSignature = _vnPayLibrary.ValidateSignature(vnp_SecureHash, vnp_HashSecret);

                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        var session = HttpContext.Session.Get<OrderViewModel>("Order");
                        session.Status = "Chờ lấy hàng";
                        await SaveOrder(session);
                        HttpRequest cookie = _services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Request;
                        string cartId = cookie.Cookies["cardId"];
                        List<CartProductViewModel> cartProductViewModels = new List<CartProductViewModel>();

                        foreach (var item in _shoppingCart.GetCartProducts(cartId))
                        {
                            var cartProductViewModel = new CartProductViewModel()
                            {
                                Id = item.Id,
                                CartId = item.CartId,
                                Cart = _cartRepository.GetCartViewModel(item.CartId),
                                ProductId = item.ProductId,
                                Product = _productRepository.GetProductViewModelById(item.ProductId),
                                Price = item.Price,
                                PriceType = item.PriceType,
                                Quantity = item.Quantity,
                                Total = item.Total
                            };
                            cartProductViewModels.Add(cartProductViewModel);

                        }
                        var cart = new ShoppingCart()
                        {
                            Id = cartId,
                            cartProducts = cartProductViewModels,
                            Total = _shoppingCart.GetShoppingCartTotal(cartId),
                            TotalPrice = _shoppingCart.GetShoppingCartTotalPrice(cartId)
                        };
                        var customer = _accountRepository.GetCustomerViewModel(HttpContext.Session.GetString(SessionId));
                        var getmaquyen = await _phanQuyenRepository.All.FirstOrDefaultAsync(x => x.MaTaiKhoan == customer.Id);
                        var idmaquyen = getmaquyen.MaQuyen;
                        var tenquyen = await _quyenRepository.All.FirstOrDefaultAsync(x => x.MaQuyen == idmaquyen);
                        var orderViewModel = new OrderViewModel()
                        {
                            CustomerId = customer.Id,
                            FullName = customer.FirstName + " " + customer.LastName,
                            Address = customer.Address,
                            District = customer.District,
                            Province = customer.Province,
                            PhoneNo = customer.PhoneNo,
                            Email = customer.Email,
                            ShoppingCart = cart,
                            Status = "Chưa xử lý"
                        };
                        ViewBag.GiaTienCu = cart.TotalPrice;
                        ViewBag.LoaiTaiKhoan = tenquyen.TenQuyen;
                        ViewBag.GiamGia = tenquyen.GiamGia;
                        if (tenquyen.GiamGia > 0)
                        {
                            cart.TotalPrice =
                                (long)(cart.TotalPrice - Math.Round((decimal)(tenquyen.GiamGia * cart.TotalPrice)) / 100);
                        }
                        _shoppingCart.ClearCart(cookie.Cookies["cardId"]);
                        HttpContext.Session.Remove("AddProducts");
                        HttpContext.Session.Remove("Order");
                        ViewBag.TinhTrang = "Chưa xử lý";
                        return View(orderViewModel);

                    }
                    else
                    {
                        ViewBag.KetQua = "Thanh toán không thành công. Có lỗi xảy ra trong quá trình xử lý.";
                        return RedirectToAction(nameof(HomeOrder));
                    }
                }
                else
                {
                    ViewBag.KetQua = "Thanh toán không thành công. Có lỗi xảy ra trong quá trình xử lý.";
                    return RedirectToAction(nameof(HomeOrder));
                }
            }
            //SessionHelper.Set(HttpContext.Session, "cart", "");
            //return RedirectToAction(nameof(HomeOrder));
        }
        [AllowAnonymous]
        public async Task<IActionResult> Done(string amount, string errorCode)
        {
            
            if (errorCode == "0")
            {

                var session = HttpContext.Session.Get<OrderViewModel>("Order");
                session.Status = "Chưa xử lý";
                await SaveOrder(session);
                HttpRequest cookie = _services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Request;
                string cartId = cookie.Cookies["cardId"];
                List<CartProductViewModel> cartProductViewModels = new List<CartProductViewModel>();

                foreach (var item in _shoppingCart.GetCartProducts(cartId))
                {
                    var cartProductViewModel = new CartProductViewModel()
                    {
                        Id = item.Id,
                        CartId = item.CartId,
                        Cart = _cartRepository.GetCartViewModel(item.CartId),
                        ProductId = item.ProductId,
                        Product = _productRepository.GetProductViewModelById(item.ProductId),
                        Price = item.Price,
                        PriceType = item.PriceType,
                        Quantity = item.Quantity,
                        Total = item.Total
                    };
                    cartProductViewModels.Add(cartProductViewModel);

                }
                var cart = new ShoppingCart()
                {
                    Id = cartId,
                    cartProducts = cartProductViewModels,
                    Total = _shoppingCart.GetShoppingCartTotal(cartId),
                    TotalPrice = _shoppingCart.GetShoppingCartTotalPrice(cartId)
                };
                var customer = _accountRepository.GetCustomerViewModel(HttpContext.Session.GetString(SessionId));
                var getmaquyen = await _phanQuyenRepository.All.FirstOrDefaultAsync(x => x.MaTaiKhoan == customer.Id);
                var idmaquyen = getmaquyen.MaQuyen;
                var tenquyen = await _quyenRepository.All.FirstOrDefaultAsync(x => x.MaQuyen == idmaquyen);
                var orderViewModel = new OrderViewModel()
                {
                    CustomerId = customer.Id,
                    FullName = customer.FirstName + " " + customer.LastName,
                    Address = customer.Address,
                    District = customer.District,
                    Province = customer.Province,
                    PhoneNo = customer.PhoneNo,
                    Email = customer.Email,
                    ShoppingCart = cart,
                    Status = "Chưa xử lý"
                };
                ViewBag.GiaTienCu = cart.TotalPrice;
                ViewBag.LoaiTaiKhoan = tenquyen.TenQuyen;
                ViewBag.GiamGia = tenquyen.GiamGia;
                if (tenquyen.GiamGia > 0)
                {
                    cart.TotalPrice =
                        (long)(cart.TotalPrice - Math.Round((decimal)(tenquyen.GiamGia * cart.TotalPrice)) / 100);
                }
                _shoppingCart.ClearCart(cookie.Cookies["cardId"]);
                HttpContext.Session.Remove("AddProducts");
                HttpContext.Session.Remove("Order");
                ViewBag.TinhTrang = "Chưa xử lý";
                return View(orderViewModel);
            }
            return RedirectToAction(nameof(HomeOrder));
        }

        [AllowAnonymous]
        public async Task<IActionResult> success([FromQuery(Name = "paymentId")] string paymentId, [FromQuery(Name = "PayerID")] string PayerID)
        {
            var paypalAPI = new PaypalAPI(_configuration);
            var result = await paypalAPI.ExecutePayment(paymentId, PayerID);
            ViewBag.data = result;
            var session = HttpContext.Session.Get<OrderViewModel>("Order");
            session.Status = "Chưa xử lý";
            await SaveOrder(session);
            HttpRequest cookie = _services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Request;
            string cartId = cookie.Cookies["cardId"];
            List<CartProductViewModel> cartProductViewModels = new List<CartProductViewModel>();

            foreach (var item in _shoppingCart.GetCartProducts(cartId))
            {
                var cartProductViewModel = new CartProductViewModel()
                {
                    Id = item.Id,
                    CartId = item.CartId,
                    Cart = _cartRepository.GetCartViewModel(item.CartId),
                    ProductId = item.ProductId,
                    Product = _productRepository.GetProductViewModelById(item.ProductId),
                    Price = item.Price,
                    PriceType = item.PriceType,
                    Quantity = item.Quantity,
                    Total = item.Total
                };
                cartProductViewModels.Add(cartProductViewModel);

            }
            var cart = new ShoppingCart()
            {
                Id = cartId,
                cartProducts = cartProductViewModels,
                Total = _shoppingCart.GetShoppingCartTotal(cartId),
                TotalPrice = _shoppingCart.GetShoppingCartTotalPrice(cartId)
            };
            var customer = _accountRepository.GetCustomerViewModel(HttpContext.Session.GetString(SessionId));
            var getmaquyen = await _phanQuyenRepository.All.FirstOrDefaultAsync(x => x.MaTaiKhoan == customer.Id);
            var idmaquyen = getmaquyen.MaQuyen;
            var tenquyen = await _quyenRepository.All.FirstOrDefaultAsync(x => x.MaQuyen == idmaquyen);
            var orderViewModel = new OrderViewModel()
            {
                CustomerId = customer.Id,
                FullName = customer.FirstName + " " + customer.LastName,
                Address = customer.Address,
                District = customer.District,
                Province = customer.Province,
                PhoneNo = customer.PhoneNo,
                Email = customer.Email,
                ShoppingCart = cart,
                Status = "Chưa xử lý"
            };
            ViewBag.GiaTienCu = cart.TotalPrice;
            ViewBag.LoaiTaiKhoan = tenquyen.TenQuyen;
            ViewBag.GiamGia = tenquyen.GiamGia;
            if (tenquyen.GiamGia > 0)
            {
                cart.TotalPrice =
                    (long)(cart.TotalPrice - Math.Round((decimal)(tenquyen.GiamGia * cart.TotalPrice)) / 100);
            }
            _shoppingCart.ClearCart(cookie.Cookies["cardId"]);
            HttpContext.Session.Remove("AddProducts");
            HttpContext.Session.Remove("Order");
            ViewBag.TinhTrang = "Chưa xử lý";
            return View(orderViewModel);
        }
        [AllowAnonymous]
        public IActionResult cancel([FromQuery(Name = "token")] string token)
        {
            return RedirectToAction(nameof(HomeOrder));
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("api/[controller]/CapNhatNguoiDung")]
        public async Task<IActionResult> CapNhatNguoiDung([FromBody] Customer data)
        {
            var message ="";
            if (data is null)
            {
                message = "Vui lòng điền đầy đủ thông tin bên trên";
                return Ok(message);
            }
            var nd = await _accountRepository.All.FirstOrDefaultAsync(x => x.Id == data.Id);
            if (nd == null)
            {
                message = "Người dùng không tồn tại";
                return Ok(message);
            }
            nd.FullName = data.FullName;
            nd.Email = data.Email;
            nd.LastUpdateAt = DateTime.Now;
            nd.Address = data.Address;
            nd.District = data.District;
            nd.PhoneNo = data.PhoneNo;
            nd.Province = data.Province;
            try
            {
                _accountRepository.UpdateAsync(nd);
                await _accountRepository.SaveAsync();
                message = "Cập nhật thành công";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return Ok(message);
        }
        private List<SelectListModel> GetPayList()
        {
            return FunctionHelper.TypePay();
        }
        private List<SelectListModel> GetShipList()
        {
            return FunctionHelper.TypeShip();
        }
        [AllowAnonymous]
        public async Task<bool> SaveOrder(OrderViewModel model)
        {
            var systemInfo = _systemInformationRepository.All.FirstOrDefault();
            HttpRequest cookie = _services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Request;
            var customer = _accountRepository.GetCustomerViewModel(HttpContext.Session.GetString(SessionId));
            var getmaquyen = await _phanQuyenRepository.All.FirstOrDefaultAsync(x => x.MaTaiKhoan == customer.Id);
            var idmaquyen = getmaquyen.MaQuyen;
            var tenquyen = await _quyenRepository.All.FirstOrDefaultAsync(x => x.MaQuyen == idmaquyen);
            if (cookie != null)
            {
                string cartId = cookie.Cookies["cardId"];
                Cart cart = _cartRepository.Get(cartId);
                cart.CreateAt = DateTime.UtcNow;
                cart.CustomerId = customer.Id;
                cart.Total += _shoppingCart.GetShoppingCartTotal(cartId);
                cart.Totalprice += _shoppingCart.GetShoppingCartTotalPrice(cartId);
                if (tenquyen.GiamGia > 0)
                {
                    cart.Totalprice =
                        (long)(cart.Totalprice - Math.Round((decimal)(tenquyen.GiamGia * cart.Totalprice)) / 100);
                }
                cart.PaymentMethod = model.PaymentMethod;
                cart.ShippingMethod = model.ShippingMethod;
                cart.Comments = model.Comment;
                cart.Status = model.Status;
                cart.TinhTrangDanhGiaCustomer = "Chưa đánh giá";
                _cartRepository.UpdateAsync(cart);
                var thongbaoorder = new ThongBao
                {
                    MaTaiKhoan = customer.Id,
                    MaDdh = cart.Id,
                    NoiDung = $"Khách hàng {customer.FirstName} đặt hàng",
                    ThoiGian = DateTime.Now
                };
                await _thongBaoRepository.AddAsync(thongbaoorder);
                await _thongBaoRepository.SaveAsync();
                await _cartRepository.SaveAsync();
                await _hubContext.Clients.All.SendAsync("NewOrder", thongbaoorder.MaThongBao, thongbaoorder.NoiDung,thongbaoorder.ThoiGian);
                var detailCart = _shoppingCart.All.Where(x => x.CartId == cartId).ToList();
                foreach (var item in detailCart)
                {
                    var sp = _productRepository.All.ToList().Single(n => n.Id == item.ProductId);
                    sp.BasketCount -= item.Quantity;
                    sp.BuyCount += item.Quantity;
                    item.TinhTrangChiTiet = "Chưa xử lý";
                    item.DiemCustomerDanhGia = 0;
                    _shoppingCart.UpdateAsync(item);
                    await _shoppingCart.SaveAsync();
                }

                string tblData = "";
                ViewBag.TinhTrang = "Chưa xử lý";
                ViewBag.cart = _shoppingCart.All.AsNoTracking().Include(x => x.Product).Where(x => x.CartId == cartId).ToList();
                ViewBag.name = model.FullName;
                ViewBag.email = model.Email;
                ViewBag.address = $"{model.Address} {_iDictrictRepository.Get(model.District).Name} {_iProvinceRepository.Get(model.Province).Name}";
                ViewBag.code = model.Comment;
                ViewBag.phone = model.PhoneNo;
                ViewBag.PaymenMethod = GetPayList().FirstOrDefault(x => x.ItemValue.Equals(model.PaymentMethod.ToString()))?.ItemText;
                ViewBag.ShippingMethod = GetShipList().FirstOrDefault(x => x.ItemValue.Equals(model.ShippingMethod.ToString()))?.ItemText;
                var webRoot = _webHostEnvironment.WebRootPath;
                var file = Path.Combine(webRoot, "orderBill.html");
                string content = System.IO.File.ReadAllText(file);
                content = content.Replace("{{username}}", model.FullName);
                content = content.Replace("{{customername}}", model.FullName);
                content = content.Replace("{{phone}}", model.PhoneNo);
                content = content.Replace("{{shiptoaddress}}", $"{model.Address} {_iDictrictRepository.Get(model.District).Name} {_iProvinceRepository.Get(model.Province).Name}");
                content = content.Replace("{{email}}", model.Email);
                long totalPrice = 0;
                foreach (var item in _shoppingCart.All.AsNoTracking().Include(x => x.Product).Where(x => x.CartId == cartId).ToList())
                {
                    long p = item.Price.GetValueOrDefault() * item.Quantity;
                    totalPrice += p;
                    tblData += "<tr><td>" + item.Product.ProductName + "</td><td>" + item.Quantity + "</td><td>" +
                               $"{p:0,0} VNĐ" + "</td></tr>";
                }
                content = content.Replace("{{tblData}}", tblData);
                content = content.Replace("{{price}}", string.Format("{0:0,0 VNĐ}", totalPrice));
                PdfGenerator generator = new PdfGenerator();
                var pdf = generator.CreatePdf(model, _shoppingCart.All.AsNoTracking().Include(x => x.Product).Where(x => x.CartId == cartId).ToList(), systemInfo.SMTPEmail);
                bool result2 = await SendEmail.SendAsync(systemInfo.SMTPName, systemInfo.SMTPEmail, systemInfo.SMTPPassword, model.Email, "Hóa đơn thanh toán từ Furnitrue House", content, pdf);
                return result2;
            }
            else
            {
                await _cartRepository.SaveAsync();
                return false;
            }
        }
    }
}
