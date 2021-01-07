using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Shop.Dto.Chat;
using Domain.Shop.Entities;
using Domain.Shop.Entities.SystemManage;
using Domain.Shop.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;
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

    public class ChatDetailHub : Hub
    {
        public readonly static List<UserViewModel> _Connections = new List<UserViewModel>();
        public readonly static List<RoomViewModel> _Rooms = new List<RoomViewModel>();
        private readonly static Dictionary<string, string> _ConnectionsMap = new Dictionary<string, string>();
        private readonly IMapper _mapper;
        private readonly IRoomRepository _iRoomRepository;
        private readonly IMessageRepository _iMessageRepository;
        private readonly IAccountRepository _iAccountRepository;
        private IThongBaoRepository thongBaoRepository;
        private ICustomerRepository customerRepository;
        const string SessionName = "_Name";

        public ChatDetailHub(IMapper mapper, IRoomRepository iRoomRepository, IMessageRepository iMessageRepository, IAccountRepository iAccountRepository, IThongBaoRepository thongBaoRepository, ICustomerRepository customerRepository)
        {
            _mapper = mapper;
            _iRoomRepository = iRoomRepository;
            _iMessageRepository = iMessageRepository;
            _iAccountRepository = iAccountRepository;
            this.thongBaoRepository = thongBaoRepository;
            this.customerRepository = customerRepository;
        }

        public async Task SendPrivate(string receiverName, string message)
        {
            if (_ConnectionsMap.TryGetValue(receiverName, out string userId))
            {
                var sender = _Connections.First(u => u.Email == IdentityName);

                if (!string.IsNullOrEmpty(message.Trim()))
                {
                    var messageViewModel = new MessageViewModel()
                    {
                        Content = Regex.Replace(message, @"(?i)<(?!img|a|/a|/img).*?>", string.Empty),
                        From = sender.FullName,
                        Avatar = sender.Avatar,
                        To = "",
                        Timestamp = DateTime.Now.ToLongTimeString()
                    };
                    await Clients.Client(userId).SendAsync("newMessage", messageViewModel);
                    await Clients.Caller.SendAsync("newMessage", messageViewModel);
                }
            }
        }

        public async Task SendToRoom(string roomName, string message)
        {
            try
            {
                var user = _iAccountRepository.All.FirstOrDefault(u => u.Email == IdentityName);
                var room = _iRoomRepository.All.FirstOrDefault(r => r.Name == roomName);

                if (!string.IsNullOrEmpty(message.Trim()))
                {
                    var msg = new Message()
                    {
                        Content = Regex.Replace(message, @"(?i)<(?!img|a|/a|/img).*?>", string.Empty),
                        FromUser = user,
                        ToRoom = room,
                        Timestamp = DateTime.Now
                    };
                    await _iMessageRepository.AddAsync(msg);
                    await _iMessageRepository.SaveAsync();
                    var messageViewModel = _mapper.Map<Message, MessageViewModel>(msg);
                    await Clients.Group(roomName).SendAsync("newMessage", messageViewModel);
                }
            }
            catch (Exception)
            {
                await Clients.Caller.SendAsync("onError", "Message not send! Message should be 1-500 characters.");
            }
        }

        public async Task Join(string roomName)
        {
            try
            {
                var user = _Connections.FirstOrDefault(u => u.Email == IdentityName);
                if (user != null && user.CurrentRoom != roomName)
                {
                    if (!string.IsNullOrEmpty(user.CurrentRoom))
                        await Clients.OthersInGroup(user.CurrentRoom).SendAsync("removeUser", user);

                    await Leave(user.CurrentRoom);
                    await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                    user.CurrentRoom = roomName;
                    await Clients.OthersInGroup(roomName).SendAsync("addUser", user);
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("onError", "You failed to join the chat room!" + ex.Message);
            }
        }

        public async Task Leave(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task CreateRoom(string roomName)
        {
            try
            {
                Match match = Regex.Match(roomName, @"^\w+( \w+)*$");
                if (!match.Success)
                {
                    await Clients.Caller.SendAsync("onError", "Invalid room name!\nRoom name must contain only letters and numbers.");
                }
                else if (roomName.Length < 5 || roomName.Length > 100)
                {
                    await Clients.Caller.SendAsync("onError", "Room name must be between 5-100 characters!");
                }
                else if (_iRoomRepository.All.Any(r => r.Name == roomName))
                {
                    await Clients.Caller.SendAsync("onError", "Another chat room with this name exists");
                }
                else
                {
                    var user = await _iAccountRepository.All.FirstOrDefaultAsync(u => u.Email == IdentityName);
                    var room = new Room()
                    {
                        Name = roomName,
                        Admin = user
                    };
                    await _iRoomRepository.AddAsync(room);
                    await _iRoomRepository.SaveAsync();
                    if (room != null)
                    {
                        var roomViewModel = _mapper.Map<Room, RoomViewModel>(room);
                        _Rooms.Add(roomViewModel);
                        await Clients.All.SendAsync("addChatRoom", roomViewModel);
                    }
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("onError", "Couldn't create chat room: " + ex.Message);
            }
        }

        public async Task DeleteRoom(string roomName)
        {
            try
            {
                var room = _iRoomRepository.All
                    .Include(r => r.Admin).
                    FirstOrDefault(r => r.Name == roomName && r.Admin.Email == IdentityName);
                _iRoomRepository.Delete(room);
                await _iRoomRepository.SaveAsync();

                var roomViewModel = _Rooms.First(r => r.Name == roomName);
                _Rooms.Remove(roomViewModel);

                await Clients.Group(roomName).SendAsync("onRoomDeleted",
                    $"Room {roomName} has been deleted.\nYou are now moved to the Lobby!");

                await Clients.All.SendAsync("removeChatRoom", roomViewModel);
            }
            catch (Exception)
            {
                await Clients.Caller.SendAsync("onError", "Can't delete this chat room. Only owner can delete this room.");
            }
        }

        public IEnumerable<RoomViewModel> GetRooms()
        {
            if (_Rooms.Count == 0)
            {
                foreach (var room in _iRoomRepository.All)
                {
                    var roomViewModel = _mapper.Map<Room, RoomViewModel>(room);
                    _Rooms.Add(roomViewModel);
                }
            }
            return _Rooms.ToList();
        }

        public IEnumerable<UserViewModel> GetUsers(string roomName)
        {
            return _Connections.Where(u => u.CurrentRoom == roomName).ToList();
        }

        public IEnumerable<MessageViewModel> GetMessageHistory(string roomName)
        {
            var messageHistory = _iMessageRepository.All.Where(m => m.ToRoom.Name == roomName)
                    .Include(m => m.FromUser)
                    .Include(m => m.ToRoom)
                    .OrderByDescending(m => m.Timestamp)
                    .Take(20)
                    .AsEnumerable()
                    .Reverse()
                    .ToList();

            return _mapper.Map<IEnumerable<Message>, IEnumerable<MessageViewModel>>(messageHistory);
        }

        public override Task OnConnectedAsync()
        {
            try
            {
                var user = _iAccountRepository.All.FirstOrDefault(u => u.Email == IdentityName);
                var userViewModel = _mapper.Map<Customer, UserViewModel>(user);
                userViewModel.Device = GetDevice();
                userViewModel.CurrentRoom = "";

                if (_Connections.All(u => u.Email != IdentityName))
                {
                    _Connections.Add(userViewModel);
                    _ConnectionsMap.Add(IdentityName, Context.ConnectionId);
                }

                if (user != null) Clients.Caller.SendAsync("getProfileInfo", user.Email, user.Avatar);
                else
                {
                    Clients.Caller.SendAsync("onError", "OnConnected:" + "Không tìm thấy account");
                }
            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("onError", "OnConnected:" + ex.Message);
            }
            return base.OnConnectedAsync();
        }

        public async Task AddClient(int orderid)
        {
            var callerId = Context.ConnectionId;
            await Groups.AddToGroupAsync(callerId, orderid.ToString());
        }


        public async Task NewOrderMessage(int mathongbao, string noidung, DateTime thoigian)
        {
            var order = await thongBaoRepository.All.FirstOrDefaultAsync(o =>
                o.MaThongBao == mathongbao && o.NoiDung.ToLower().Contains(noidung.ToLower()));
            await Clients.All.SendAsync("NewOrder", order.MaThongBao, order.NoiDung, order.ThoiGian);

        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                var user = _Connections.First(u => u.Email == IdentityName);
                _Connections.Remove(user);

                Clients.OthersInGroup(user.CurrentRoom).SendAsync("removeUser", user);

                _ConnectionsMap.Remove(user.Email);
            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("onError", "OnDisconnected: " + ex.Message);
            }

            return base.OnDisconnectedAsync(exception);
        }

        private string IdentityName => Context.GetHttpContext().Session.GetString(SessionName);

        private string GetDevice()
        {
            var device = Context.GetHttpContext().Request.Headers["Device"].ToString();
            if (!string.IsNullOrEmpty(device) && (device.Equals("Desktop") || device.Equals("Mobile")))
                return device;

            return "Web";
        }
    }
}
