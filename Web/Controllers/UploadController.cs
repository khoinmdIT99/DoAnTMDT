using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Shop.Dto.Chat;
using Domain.Shop.Entities.SystemManage;
using Domain.Shop.IRepositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Web.Hubs;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly int FileSizeLimit;
        private readonly string[] AllowedExtensions;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        private readonly IHubContext<ChatDetailHub> _hubContext;
        private readonly IAccountRepository _accountRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMessageRepository _messageRepository;
        const string SessionName = "_Name";

        public UploadController(
            IMapper mapper,
            IWebHostEnvironment environment,
            IHubContext<ChatDetailHub> hubContext,
            IConfiguration configruation, IAccountRepository accountRepository, IRoomRepository roomRepository, IMessageRepository messageRepository)
        {
            _mapper = mapper;
            _environment = environment;
            _hubContext = hubContext;
            _accountRepository = accountRepository;
            _roomRepository = roomRepository;
            _messageRepository = messageRepository;

            FileSizeLimit = configruation.GetSection("FileUpload").GetValue<int>("FileSizeLimit");
            AllowedExtensions = configruation.GetSection("FileUpload").GetValue<string>("AllowedExtensions").Split(",");
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload([FromForm] UploadViewModel uploadViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!Validate(uploadViewModel.File))
                {
                    return BadRequest("Validation failed!");
                }

                var fileName = DateTime.Now.ToString("yyyymmddMMss") + "_" + Path.GetFileName(uploadViewModel.File.FileName);
                var folderPath = Path.Combine(_environment.WebRootPath, "uploads");
                var filePath = Path.Combine(folderPath, fileName);
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await uploadViewModel.File.CopyToAsync(fileStream);
                }

                var user = await _accountRepository.All.FirstOrDefaultAsync(u => u.Email == HttpContext.Session.GetString(SessionName));
                var room = await _roomRepository.All.FirstOrDefaultAsync(r => r.Id == uploadViewModel.RoomId);
                if (user == null || room == null)
                    return NotFound();

                string htmlImage = string.Format(
                    "<a href=\"/uploads/{0}\" target=\"_blank\">" +
                    "<img src=\"/uploads/{0}\" class=\"post-image\">" +
                    "</a>", fileName);

                var message = new Message()
                {
                    Content = Regex.Replace(htmlImage, @"(?i)<(?!img|a|/a|/img).*?>", string.Empty),
                    Timestamp = DateTime.Now,
                    FromUser = user,
                    ToRoom = room
                };

                await _messageRepository.AddAsync(message);
                await _messageRepository.SaveAsync();

                // Send image-message to group
                var messageViewModel = _mapper.Map<Message, MessageViewModel>(message);
                await _hubContext.Clients.Group(room.Name).SendAsync("newMessage", messageViewModel);

                return Ok();
            }

            return BadRequest();
        }

        private bool Validate(IFormFile file)
        {
            if (file.Length > FileSizeLimit)
                return false;

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(extension) || !AllowedExtensions.Any(s => s.Contains(extension)))
                return false;

            return true;
        }
    }
}
