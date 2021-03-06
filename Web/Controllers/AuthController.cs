﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Domain.Application.Dto.Login;
using Domain.Application.Entities;
using Domain.Common.Security;
using Domain.Shop.Entities;
using Domain.Shop.Entities.SystemManage;
using Domain.Shop.IRepositories;
using Infrastructure.Common;
using Infrastructure.Database.Models;
using Infrastructure.Web;
using Infrastructure.Web.HelperTool;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Web.Controllers
{
    [Route("Auth")]
    public class AuthController : Controller
    {
        public static IConfiguration AConfiguration;
        const string SessionName = "_Name";
        const string SessionId = "_Id";
        const string SessionIdQuyen = "_IdQuyen";
        private readonly ILogger<HomeController> _logger;
        public MD5 Md5;
        public static EnCryptography Encrypt;
        private readonly IAccountRepository _accountRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IForgetPasswordRepository _forgetPasswordRepository;
        private readonly ISystemInformationRepository _systemInformationRepository;
        private readonly IPhanQuyenRepository _phanQuyenRepository;
        private readonly IQuyenRepository _quyenRepository;
        private readonly IXacMinhRepository _xacMinhRepository;
        public IConfiguration Configuration { get; }

        public AuthController(ILogger<HomeController> logger, IAccountRepository accountRepository, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IForgetPasswordRepository forgetPasswordRepository, ISystemInformationRepository systemInformationRepository, IPhanQuyenRepository phanQuyenRepository, IQuyenRepository quyenRepository, IXacMinhRepository xacMinhRepository)
        {
            _logger = logger;
            _accountRepository = accountRepository;
            Configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _forgetPasswordRepository = forgetPasswordRepository;
            _systemInformationRepository = systemInformationRepository;
            _phanQuyenRepository = phanQuyenRepository;
            _quyenRepository = quyenRepository;
            _xacMinhRepository = xacMinhRepository;
        }
        //private Uri RedirectUri
        //{
        //    get
        //    {
        //        var uriBuilder = new UriBuilder(Request.GetEncodedUrl())
        //        {
        //            Query = null, Fragment = null, Path = Url.Action("FacebookCallback")
        //        };
        //        return uriBuilder.Uri;
        //    }
        //}
        //[HttpGet]
        //[Route("LoginFaceBook")]
        //public IActionResult LoginFaceBook()
        //{

        //    var fb = new FacebookClient();
        //    var loginUrl = fb.GetLoginUrl(new
        //    {
        //        client_id = Configuration.GetSection("Facebook:FACEBOOK_APP_ID").Value,
        //        client_secret = Configuration.GetSection("Facebook:FACEBOOK_APP_SECRET").Value,
        //        redirect_uri = RedirectUri.AbsoluteUri,
        //        reponse_type = "code",
        //        scope = new[] { "email" }
        //    });
        //    return Redirect(loginUrl.AbsoluteUri);
        //}
        //public async Task<IActionResult> FacebookCallback(string code)
        //{
        //    var fb = new FacebookClient();
        //    dynamic result = fb.Post("oauth/access_token", new
        //    {
        //        client_id = Configuration.GetSection("Facebook:FACEBOOK_APP_ID").Value,
        //        client_secret = Configuration.GetSection("Facebook:FACEBOOK_APP_SECRET").Value,
        //        redirect_uri = RedirectUri.AbsoluteUri,
        //        code = code
        //    });
        //    var accessToken = result.access_token;
        //    if (!string.IsNullOrEmpty(accessToken))
        //    {
        //        fb.AccessToken = accessToken;
        //        dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
        //        string email = me.email;
        //        string userName = me.email;
        //        string firstname = me.first_name;
        //        string middlename = me.middle_name;
        //        string lastname = me.last_name;
        //        var loginUser = await _accountRepository.All.FirstOrDefaultAsync(u => u.Email == email);
        //        if (loginUser != null)
        //        {
        //            var phanquyenId = _phanQuyenRepository.All
        //                .FirstOrDefaultAsync(x => x.MaTaiKhoan.Equals(loginUser.Id)).Result.MaQuyen;
        //            HttpContext.Session.SetInt32(SessionIdQuyen, phanquyenId);
        //            HttpContext.Session.SetString(SessionName, loginUser.Email);
        //            HttpContext.Session.SetString(SessionId, loginUser.Id);
        //        }
        //        else
        //        {
        //            Customer customer = new Customer()
        //            {
        //                Id = Guid.NewGuid().ToString(),
        //                Email = userName,
        //                FullName = middlename + lastname + firstname,
        //                CreateAt = DateTime.Now,
        //                Password = StringHelper.stringToSHA512("1"),
        //                TinhTrang = "Không khoá",
        //                Avatar = "avatar1.png"
        //            };
        //            _accountRepository.Add(customer);
        //            await _accountRepository.SaveAsync();
        //            var maquyen = _quyenRepository.All.FirstOrDefaultAsync(x => x.TenQuyen.Equals("Khách hàng")).GetAwaiter().GetResult().MaQuyen;
        //            PhanQuyen phanQuyen = new PhanQuyen()
        //            {
        //                MaQuyen = maquyen,
        //                MaTaiKhoan = customer.Id
        //            };
        //            await _phanQuyenRepository.AddAsync(phanQuyen);
        //            await _phanQuyenRepository.SaveAsync();
        //            HttpContext.Session.SetInt32(SessionIdQuyen, maquyen);
        //            HttpContext.Session.SetString(SessionName, customer.Email);
        //            HttpContext.Session.SetString(SessionId, customer.Id);

        //        }
        //        return RedirectToAction("Index", "Home");
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //}
        [HttpPost]
        [Route("Login/{userStr}")]
        public async Task<IActionResult> Login(string userStr)
        {
            string message = "There was an error";
            try
            {
                Customer user = JsonConvert.DeserializeObject<Customer>(userStr);
                var loginUser = await _accountRepository.All.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (loginUser == null)
                {
                    message = "Không thể tìm thấy email";
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        var checkpass = false;
                        //using (Md5 = MD5.Create())
                        //{
                        //    Encrypt = new EnCryptography();
                        //    checkpass = Encrypt.VerifyMd5Hash(Md5, user.Password, loginUser.Password);
                        //}
                        //string passwordHashed = EncryptionHelper.GetHash(user.Password + loginUser.Salt);
                        string passwordHashed = StringHelper.stringToSHA512(StringHelper.KillChars(user.Password)).ToLower();

                        checkpass = loginUser.Password.ToLower() == passwordHashed;
                        if (checkpass)
                        {
                            var phanquyenId = _phanQuyenRepository.All
                                .FirstOrDefaultAsync(x => x.MaTaiKhoan.Equals(loginUser.Id)).Result.MaQuyen;
                            if (phanquyenId == 2)
                            {
                                message = "Đăng nhập quyền nhân viên bán hàng thành công";
                            }
                            else if(phanquyenId == 3)
                            {
                                message = "Đăng nhập thành công";
                            }
                            HttpContext.Session.SetInt32(SessionIdQuyen, phanquyenId);
                            HttpContext.Session.SetString(SessionName, loginUser.Email);
                            HttpContext.Session.SetString(SessionId, loginUser.Id);

                        }
                        else
                        {
                            message = "Mật khẩu bị sai";
                        }
                    }
                    else
                    {
                        message = "Thông tin nhập sai";
                    }
                }
            }
            catch
            {
                message = "Hệ thống đang gặp lỗi";
            }
            return new JsonResult(message);
        }
        const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqestuvwxyz0123456789";
        [HttpPost]
        //[Route("AddUser/{thongTinDangKy}")]
        public async Task<IActionResult> AddUser([FromBody] UserItem item)
        {
            string message;

            var checkRegister = _accountRepository.All.Count(u => u.Email == item.email);
            if (checkRegister == 0)
            {
                //string hashpass;
                //using (Md5 = MD5.Create())
                //{
                //    Encrypt = new EnCryptography();
                //    hashpass = Encrypt.GetMd5Hash(Md5, item.password);
                //}
                //string salt = EncryptionHelper.GetSalt();
                //Password = EncryptionHelper.GetHash(item.password + salt),
                var avatars = new string[] {"avatar1.png", "avatar2.png", "avatar3.png", "avatar4.png"};
                var index = int.Parse(item.avatar);
                if (index < 0 || index > avatars.Count() - 1)
                    index = 0;

                var avatarName = avatars[index];
                Customer customer = new Customer()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = item.email,
                    Password = StringHelper.stringToSHA512(item.password),
                    FullName = item.fullName,
                    CreateAt = DateTime.Now,
                    Avatar = avatarName,
                    TinhTrang = "Chưa kích hoạt"
                };
                _accountRepository.Add(customer);
                await _accountRepository.SaveAsync();
                var quyen = _quyenRepository.All.FirstOrDefault(x => x.TenQuyen.Contains("Khách hàng"));
                if (quyen != null)
                {
                    var maquyen = quyen.MaQuyen;
                    PhanQuyen phanQuyen = new PhanQuyen()
                    {
                        MaQuyen = maquyen,
                        MaTaiKhoan = customer.Id
                    };
                    await _phanQuyenRepository.AddAsync(phanQuyen);
                    await _phanQuyenRepository.SaveAsync();
                    HttpContext.Session.SetInt32(SessionIdQuyen, maquyen);
                    HttpContext.Session.SetString(SessionName, customer.Email);
                    HttpContext.Session.SetString(SessionId, customer.Id);
                    try
                    {
                        Random random = new Random();
                        var code = new string(Enumerable.Repeat(Chars, 9)
                            .Select(s => s[random.Next(s.Length)]).ToArray());
                        var taikhoan = await _accountRepository.All.FirstOrDefaultAsync(x => x.Email.Contains(item.email));
                        XacMinh xm = new XacMinh
                        {
                            Code = code,
                            Id_User = taikhoan.Id,
                            Timer = DateTime.Now
                        };
                        await _xacMinhRepository.AddAsync(xm);
                        await _xacMinhRepository.SaveAsync();
                        string kichhoat = "Để kích hoạt tài khoản, vui lòng nhấn vào link phía dưới: \n";
                        var local = HttpContext.Request.Host;
                        kichhoat += "https://" + local + "/TaiKhoan/Activate?key=" + taikhoan.Id + "&code=" + code;
                        var client = new SmtpClient
                        {
                            Host = "smtp.gmail.com",
                            Port = 587,
                            EnableSsl = true,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential("khoitedu99@gmail.com", "irts mecn xoyz pngm")
                        };

                        using (var messagee = new MailMessage("khoitedu99@gmail.com", taikhoan.Email)
                        {
                            Subject = "Email kích hoạt tài khoản",
                            Body = kichhoat,
                            Priority = MailPriority.High,
                            BodyEncoding = Encoding.UTF8
                        })
                        {
                            await client.SendMailAsync(messagee);
                        }
                        message = "Đăng ký thành công. Vui lòng kích hoạt email để mua sản phẩm";
                    }
                    catch (Exception)
                    {
                        message = "Hệ thống đang gặp lỗi";
                    }

                    
                }
                else
                {
                    message = "Hệ thống đang gặp lỗi";
                }

            }
            else
            {
                message = "Tài khoản này đã được đăng ký";
            }

            return Json(message);
        }

        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.Remove(SessionName);
            HttpContext.Session.Remove(SessionId);
            HttpContext.Session.Remove(SessionIdQuyen);
            HttpContext.Session.Clear();
            return new JsonResult("Đăng xuất thành công");
        }
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            string message;
            var checkRegister = _accountRepository.All.Count(u => u.Email == email);
            var checkRegisterNull = _accountRepository.All.FirstOrDefault(u => u.Email == email);
            if (checkRegister != 0 && checkRegisterNull != null)
            {
                string activeCode = StringHelper.CreateRandomString(32);
                activeCode = StringHelper.StringToMd5(activeCode).ToLower();
                var temporaryPassword = StringHelper.CreateRandomString(8);

                ForgetPassword forgetPassword = new ForgetPassword
                {
                    AccountId = checkRegisterNull.Id,
                    ActiveCode = activeCode,
                    RequestTime = DateTime.Now,
                    TemporaryPassword = Base64Hepler.Base64Encode(StringHelper.KillChars(temporaryPassword).ToLower()),
                    Status = 0,
                    RequestIp = Request.HttpContext.Connection.RemoteIpAddress.ToString()
                };
                await _forgetPasswordRepository.AddAsync(forgetPassword);
                await _forgetPasswordRepository.SaveAsync();
                var systemInfo = _systemInformationRepository.All.FirstOrDefault();

                if (systemInfo != null)
                {
                    string domainName = _httpContextAccessor.HttpContext.Request.Host.Value;
                    StringBuilder body = new StringBuilder();
                    body.Append("Kính gửi " + StringHelper.KillChars(checkRegisterNull.Email) + ",<br /><br />");
                    body.Append("Quí vị đã yêu cầu khôi phục mật khẩu trên website " + systemInfo.SiteName + "!<br />");
                    body.Append("Mật khẩu mới của quí vị là: " + temporaryPassword);
                    body.Append("<br />Quí vị vui lòng bấm ");
                    body.Append("<a href=\"" + "https://" + domainName + "/Auth/xac-nhan-khoi-phuc-mat-khau/" + activeCode + "\" target=\"_blank\">vào đây</a>");
                    body.Append(" để xác thực việc quên mật khẩu. <br />");
                    body.Append(" Yêu cầu của quí vị chỉ có hiệu lực trong 24 giờ. <br />");
                    body.Append("<br /><br />Vô cùng xin lỗi nếu email này làm phiền quí vị!<br /><br />");
                    body.Append("<br />Kính thư, <br /><br />");
                    body.Append(systemInfo.SiteName + "<br />");
                    body.Append("Phát triển bởi Công ty Thiết kế Nội thất<br />");
                    body.Append("Webmaster: support@huflit.edu.vn");


                    bool result2 = await SendEmail.SendAsync(systemInfo.SMTPName, systemInfo.SMTPEmail, systemInfo.SMTPPassword, email, "Xác nhận khôi phục mật khẩu", body.ToString());
                    if (!result2)
                    {//Gửi email không thành công
                        var item = _forgetPasswordRepository.All.FirstOrDefault(x => x.AccountId == forgetPassword.AccountId);
                        if (item != null)
                        {
                            item.Status = 4;
                            _forgetPasswordRepository.UpdateAsync(item);
                        }
                        await _forgetPasswordRepository.SaveAsync();
                        message = "Đã xảy ra lỗi khi thực hiện yêu cầu của bạn! (không gửi được email)";
                    }
                    else
                    {
                        message = "Yêu cầu khôi phục mật khẩu của bạn đã được chấp nhận. Vui lòng kiểm tra email và làm theo hướng dẫn.";
                    }
                }
                else
                {
                    message = "Lỗi không xác định! Vui lòng thử lại!";
                }
            }
            else
            {
                message = "Địa chỉ email không tồn tại!";
            }
            return Json(message);
        }
        /// <summary>
        /// Xác nhận yêu cầu mật khẩu mới
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Route("xac-nhan-khoi-phuc-mat-khau/{code?}")]
        public async Task<IActionResult> ConfirmPassword(string code)
        {
            string message;
            try
            {
                var forgetPassword = await _forgetPasswordRepository.All.FirstOrDefaultAsync(o => o.ActiveCode == code);
                if (forgetPassword == null)
                {
                    message = "Yêu cầu của bạn không chính xác. Vui lòng kiểm tra lại!";
                }
                else
                {
                    if (forgetPassword.Status != 0)
                    {
                        message = "Yêu cầu của bạn không chính xác. Vui lòng kiểm tra lại!";
                    }
                    else
                    {
                        DateTime requestTime = forgetPassword.RequestTime;
                        TimeSpan timespan = DateTime.Now - requestTime;
                        double hours = timespan.TotalMinutes;
                        if (hours > 10)
                        {
                            var item = _forgetPasswordRepository.All.FirstOrDefault(x => x.AccountId == forgetPassword.AccountId);
                            if (item != null)
                            {
                                item.Status = 3;
                                _forgetPasswordRepository.UpdateAsync(item);
                                await _forgetPasswordRepository.SaveAsync();
                            }
                            await _forgetPasswordRepository.SaveAsync();
                            message = "Yêu cầu khôi phục mật khẩu của bạn đã hết thời hạn!";
                        }
                        else
                        {
                            var account = await _accountRepository.All.FirstOrDefaultAsync(x => x.Id == forgetPassword.AccountId.ToString());
                            var item1 = _forgetPasswordRepository.All.FirstOrDefault(x => x.AccountId == forgetPassword.AccountId);
                            if (item1 != null)
                            {
                                account.Password = StringHelper.stringToSHA512(Base64Hepler.Base64Decode(item1.TemporaryPassword));
                                _accountRepository.UpdateAsync(account);
                                await _forgetPasswordRepository.SaveAsync();
                                try
                                {
                                    item1.Status = 1;
                                    item1.ActiveTime = DateTime.Now;
                                    _forgetPasswordRepository.UpdateAsync(item1);
                                    await _forgetPasswordRepository.SaveAsync();
                                    message =
                                        "Yêu cầu của bạn đã được xử lý thành công. Bạn đã có thể dùng mật khẩu mới để truy cập!";
                                    //Có cần gửi email thông báo là đổi mật khẩu thành công hay không?
                                }
                                catch
                                {
                                    var item2 = _forgetPasswordRepository.All.FirstOrDefault(x =>
                                        x.AccountId == forgetPassword.AccountId);
                                    if (item2 != null)
                                    {
                                        item2.Status = 2;
                                        _forgetPasswordRepository.UpdateAsync(item2);
                                        await _forgetPasswordRepository.SaveAsync();
                                    }

                                    message = "Yêu cầu của bạn đã được xử lý không thành công. Vui lòng thử lại!";
                                }
                            }
                            else
                            {
                                message = "Lỗi khi kích hoạt mật khẩu";
                            }
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                message = "Lỗi khi kích hoạt mật khẩu: " + ex.Message;
            }
            ViewBag.Url = "https://" + _httpContextAccessor.HttpContext.Request.Host.Value;
            ViewBag.Message = message;
            return View();
        }
    }
    public class UserItem
    {
        public string fullName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string avatar { get; set; }
    }
}
