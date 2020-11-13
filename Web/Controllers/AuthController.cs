using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Domain.Application.Dto.Login;
using Domain.Application.Entities;
using Domain.Common.Security;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Infrastructure.Common;
using Infrastructure.Database.Models;
using Infrastructure.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Web.Controllers
{
    [Route("Auth")]
    public class AuthController : Controller
    {
        const string SessionName = "_Name";
        const string SessionId = "_Id";
        private readonly ILogger<HomeController> _logger;
        public MD5 Md5;
        public static EnCryptography Encrypt;
        private readonly IAccountRepository _accountRepository;

        public AuthController(ILogger<HomeController> logger, IAccountRepository accountRepository)
        {
            _logger = logger;
            _accountRepository = accountRepository;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}
        //[Route("demo2/{fullName}")]
        //public IActionResult Demo2(string fullName)
        //{
        //    return new JsonResult("Hello " + fullName);
        //}
        //[Route("Login/{email}")]
        //public IActionResult Login(string email)
        //{
        //    return new JsonResult("Hello " + email);
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
                        string passwordHashed = EncryptionHelper.GetHash(user.Password + loginUser.Salt);
                        checkpass = loginUser.Password == passwordHashed;
                        if (checkpass)
                        {
                            message = "Đăng nhập thành công";
                            HttpContext.Session.SetString(SessionName, loginUser.Email);
                            HttpContext.Session.SetString(SessionId, loginUser.Id);
                        }
                        else
                        {
                            message = passwordHashed;
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
                // ignored
            }
            return new JsonResult(message);
        }

        [HttpPost]
        //[Route("AddUser/{thongTinDangKy}")]
        public IActionResult AddUser([FromBody] UserItem item)
        {
            string message;

            var checkRegister = _accountRepository.All.Count(u => u.Email == item.email);
            if (checkRegister == 0)
            {
                string hashpass;
                //using (Md5 = MD5.Create())
                //{
                //    Encrypt = new EnCryptography();
                //    hashpass = Encrypt.GetMd5Hash(Md5, item.password);
                //}
                string salt = EncryptionHelper.GetSalt();
                Customer customer = new Customer()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = item.email,
                    Password = EncryptionHelper.GetHash(item.password + salt),
                    FullName = item.fullName,
                    CreateAt = DateTime.Now,
                    Salt = salt
                };

                _accountRepository.Add(customer);
                _accountRepository.Save();
                HttpContext.Session.SetString(SessionName, customer.Email);
                HttpContext.Session.SetString(SessionId, customer.Id);
                message = "Đăng ký thành công";
            }
            else
            {
                message = "Tài khoản này đã được đăng ký";
            }

            return Json(message);
        }
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(SessionName);
            HttpContext.Session.Remove(SessionId);
            HttpContext.Session.Clear();
            return new JsonResult("Đăng xuất thành công");
        }
    }
    public class UserItem
    {
        public string fullName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }
}
