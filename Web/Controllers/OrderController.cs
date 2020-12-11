using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Web.Models;
using Web.MomoAPI;
using Web.PaypalHelpers;

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
        private readonly IConfiguration _configuration;
        private readonly IUtils _utils;
        private readonly IVnPayLibrary _vnPayLibrary;
        const string SessionId = "_Id";

        public OrderController(IProductRepository productRepository, IShoppingCartRepository shoppingCart, IServiceProvider services, 
            IAccountRepository accountRepository, ICartRepository cartRepository, IDictrictRepository iDictrictRepository, IProvinceRepository iProvinceRepository, ITranhChapRepository iTranhChapRepository, IWebHostEnvironment webHostEnvironment, ISystemInformationRepository systemInformationRepository, IOptions<PaypalApiSetting> paypalSettings, IConfiguration configuration, IUtils utils, IVnPayLibrary vnPayLibrary)
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
            if(customer != null)
                return Json(new { success = true });
            return Json(new { success = false, message = "Vui lòng đăng nhập để mua hàng" });
        }
        [AllowAnonymous]
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

                string amount = "25000000";
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
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

                };
                    string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
                    JObject jmessage = JObject.Parse(responseFromMomo);
                    string payURL = jmessage.GetValue("payUrl").ToString();
                    HttpContext.Session.Set("Order", model);
                    return Redirect(jmessage.GetValue("payUrl").ToString());
                }
                else if (model.PaymentMethod == 2)
                {
                    var paypalAPI = new PaypalAPI(_configuration);
                    var tienviet = double.Parse(_shoppingCart.GetShoppingCartTotalPrice(cartId).ToString());
                    var tienusd = 23129;
                    var total = Math.Round(CurrencyHelper.VNDTOUSD(tienviet, tienusd),1 , MidpointRounding.AwayFromZero);
                    HttpContext.Session.Set("Order", model);
                    String url = await paypalAPI.GetRedirectUrlToPaypal(total, "USD");
                    return Redirect(url);
                }
                else if (model.PaymentMethod == 3)
                {
                    string vnp_Url = _configuration.GetSection("VNPayInfo").GetSection("vnp_Url").Value; //URL thanh toan cua VNPAY 
                    string vnp_TmnCode = _configuration.GetSection("VNPayInfo").GetSection("vnp_TmnCode").Value; //Ma website
                    string vnp_HashSecret = _configuration.GetSection("VNPayInfo").GetSection("vnp_HashSecret").Value; //Chuoi bi mat
                    if (string.IsNullOrEmpty(vnp_TmnCode) || string.IsNullOrEmpty(vnp_HashSecret))
                    {
                        return Json("Vui lòng cấu hình các tham số: vnp_TmnCode,vnp_HashSecret trong file appsetting.json");
                    }
                    var hostname = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
                    _vnPayLibrary.AddRequestData("vnp_Version", "2.0.0");
                    _vnPayLibrary.AddRequestData("vnp_Command", "pay");
                    _vnPayLibrary.AddRequestData("vnp_TmnCode", vnp_TmnCode);
                    _vnPayLibrary.AddRequestData("vnp_Amount", (_shoppingCart.GetShoppingCartTotalPrice(cartId) * 100).ToString());
                    _vnPayLibrary.AddRequestData("vnp_BankCode", "");
                    _vnPayLibrary.AddRequestData("vnp_CreateDate", _cartRepository.Get(cartId).CreateAt?.ToString("yyyyMMddHHmmss"));
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
                        Total = _shoppingCart.GetShoppingCartTotal(cartId)
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
                        Status = 0
                    };
                    var kq = await SaveOrder(model);
                    ViewBag.KQ = !kq ? "Hệ thống gặp lỗi tuy nhiên đơn hàng của bạn vẫn được nhận" : "Đơn hàng của bạn đã đặt thành công.Hệ thống sẽ liên hệ với bạn sớm";
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
                            Status = 1
                        };
                        if (cookie != null) _shoppingCart.ClearCart(cookie.Cookies["cardId"]);
                        HttpContext.Session.Remove("AddProducts");
                        HttpContext.Session.Remove("Order");
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
            return RedirectToAction(nameof(HomeOrder));
        }
        [AllowAnonymous]
        public async Task<IActionResult> Done(string amount, string errorCode)
        {
            
            if (errorCode == "0")
            {

                var session = HttpContext.Session.Get<OrderViewModel>("Order");
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
                    Status = 1
                };
                if (cookie != null) _shoppingCart.ClearCart(cookie.Cookies["cardId"]);
                HttpContext.Session.Remove("AddProducts");
                HttpContext.Session.Remove("Order");
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
                Status = 1
            };
            _shoppingCart.ClearCart(cookie.Cookies["cardId"]);
            HttpContext.Session.Remove("AddProducts");
            HttpContext.Session.Remove("Order");
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
            if (cookie != null)
            {
                string cartId = cookie.Cookies["cardId"];
                Cart cart = _cartRepository.Get(cartId);
                cart.CreateAt = DateTime.UtcNow;
                cart.CustomerId = customer.Id;
                cart.Total += _shoppingCart.GetShoppingCartTotal(cartId);
                cart.Totalprice += _shoppingCart.GetShoppingCartTotalPrice(cartId);
                cart.PaymentMethod = model.PaymentMethod;
                cart.ShippingMethod = model.ShippingMethod;
                cart.Comments = model.Comment;
                _cartRepository.UpdateAsync(cart);
                await _cartRepository.SaveAsync();
                var detailCart = _shoppingCart.All.Where(x => x.CartId == cartId).ToList();
                foreach (var item in detailCart)
                {
                    var sp = _productRepository.All.ToList().Single(n => n.Id == item.ProductId);
                    sp.BasketCount -= item.Quantity;
                    sp.BuyCount += item.Quantity;
                }
                string tblData = "";
                ViewBag.cart = _shoppingCart.All.AsNoTracking().Include(x => x.Product).Where(x => x.CartId == cartId).ToList();
                ViewBag.name = model.FullName;
                ViewBag.email = model.Email;
                ViewBag.address = $"{model.Address} {model.District} {model.Province}";
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
                bool result2 = await SendEmail.SendAsync(systemInfo.SMTPName, systemInfo.SMTPEmail, systemInfo.SMTPPassword, model.Email, "Hóa đơn thanh toán từ Furnitrue House", content);
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
