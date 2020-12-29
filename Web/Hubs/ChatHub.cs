using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Common.Security;
using Domain.Shop.Dto.ChatService;
using Domain.Shop.Entities.SystemManage;
using Domain.Shop.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.SignalR;
using Web.Common;

namespace Web.Hubs
{
    public class ChatHub : Hub
    {
        static List<UserDetail> ConnectedUsers = new List<UserDetail>();
        static List<MessageDetail> CurrentMessage = new List<MessageDetail>();
        static List<Meeting> ListMeeting = new List<Meeting>();
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChatHub(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var item = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                ConnectedUsers.Remove(item);
                //Process Save Meeting in File or DB.
                /*{
                 * 
                 * 
                 *  }*/
                //ListMeeting.Remove();
                var meeting = ListMeeting.Where(x => x.Id == item.ConnectionId).SingleOrDefault();
                if (meeting != null)
                {
                    ListMeeting.Remove(meeting);
                }
                var id = Context.ConnectionId;
                await Clients.All.SendAsync("onUserDisconnected", id, item.UserName);
            }
            await base.OnDisconnectedAsync(exception);
        }
        /*public override Task OnReconnected()
        {

            return base.OnReconnected();
        }*/
        public void AdminSend()
        {

        }
        public void AdminReceived()
        {
        }
        public void ClientSend()
        {

        }
        public void ClientReceived()
        {

        }
        public void Connect(string userName)
        {
            var id = Context.ConnectionId;
            if (ConnectedUsers.Count(x => x.ConnectionId == id) == 0)
            {
                ConnectedUsers.Add(new UserDetail { ConnectionId = id, UserName = userName });
                // send to caller

                // send to all except caller client
                // Clients.AllExcept(id).onNewUserConnected(id, userName);


            }
        }
        public void AdminConnect(string userName)
        {
            var id = Context.ConnectionId;
            if (ConnectedUsers.Count(x => x.ConnectionId == id) == 0)
            {
                if (ConnectedUsers.Count(x => x.IsAdmin == "1" && x.UserName == userName) == 0)
                {
                    var chatboxs = renderBoxchats(ListMeeting);
                    ConnectedUsers.Add(new UserDetail { ConnectionId = id, UserName = userName, IsAdmin = "1" });
                    Clients.Caller.SendAsync("onAdminConnected", id, userName, ConnectedUsers.Where(x => x.ConnectionId != id).ToList(), chatboxs)/*.(id, userName, ConnectedUsers.Where(x => x.ConnectionId != id).ToList(),chatboxs)*/;
                }
                else
                {
                    Clients.Caller.SendAsync("processReplaceUserAdmin", "admin/quanly", "Tài khoản này đang đăng nhập...")/*.processReplaceUserAdmin("/admin/quanly","Tài khoản này đang đăng nhập...")*/;
                }

                // send to caller
                // Clients.Caller.onConnected(id, userName, ConnectedUsers, CurrentMessage);

                // send to all except caller client
                // Clients.AllExcept(id).onNewUserConnected(id, userName);

            }
            Clients.Client(id).SendAsync("onLoadMessage", ListMeeting)/*.onLoadMessage(ListMeeting)*/;
        }
        public void CheckAdminOnline()
        {
            var id = Context.ConnectionId;
            var count = ConnectedUsers.Count(x => x.IsAdmin == "1");
            if (count > 0)
            {
                Clients.Client(id).SendAsync("adminIsOnline")/*.()*/;
            }

        }
        public bool GuestConnect(string userName)
        {
            try
            {
                var id = Context.ConnectionId;
                if (ConnectedUsers.Count(x => x.ConnectionId == id) == 0)
                {
                    ConnectedUsers.Add(new UserDetail { ConnectionId = id, UserName = userName, IsAdmin = "0" });
                    ListMeeting.Add(new Meeting { Id = id, StartTime = DateTime.Now, Messages = new List<MessageDetail>() });
                    Clients.Caller.SendAsync("guestOnConnected", id, userName)/*.guestOnConnected()*/;
                    var arr_id = ConnectedUsers.Where(x => x.IsAdmin == "0").Select(x => x.ConnectionId).ToArray();
                    Clients.AllExcept(arr_id).SendAsync("onAddGuestInAdmin", id, userName)/*.onAddGuestInAdmin(id, userName)*/;
                    //Clients.AllExcept()
                    // Clients.Caller.

                    // send to caller
                    // Clients.Caller.onConnected(id, userName, ConnectedUsers, CurrentMessage);
                    // Clients.
                    // send to all except caller client
                    // Clients.AllExcept(id).onNewUserConnected(id, userName);

                }
                return true;
            }
            catch
            {
                return false;
            }

        }
        public void SendPrivateMessage(string toUserId, string message)
        {

            string fromUserId = Context.ConnectionId;

            var toUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == toUserId);
            var fromUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == fromUserId);

            if (toUser != null && fromUser != null)
            {
                // send to 
                Clients.Client(toUserId).SendAsync("sendPrivateMessage", fromUserId, fromUser.UserName, message)/*.sendPrivateMessage(fromUserId, fromUser.UserName, message)*/;

                // send to caller user
                Clients.Caller.SendAsync("sendPrivateMessage", toUserId, fromUser.UserName, message)/*.sendPrivateMessage(toUserId, fromUser.UserName, message)*/;
            }

        }
        public void AdminSendMessage(string toUserId, string message)
        {
            string fromAdminId = Context.ConnectionId;
            var toUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == toUserId);
            var fromAdminUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == fromAdminId);
            if (toUser != null && fromAdminUser != null)
            {
                var meeting = ListMeeting.SingleOrDefault(x => x.Id == toUserId);

                if (meeting != null)
                {
                    meeting.Messages = SetStatusAllMessageInMeeting(meeting, 1);
                    var message_ = new MessageDetail { ConnectionId = toUserId, UserName = toUser.UserName, IsAdmin = 1, Status = 0, CreateDate = DateTime.Now, Message = message };
                    var messages_ = meeting.Messages;
                    messages_.Add(message_);
                    meeting.Messages = messages_;
                }
                var last_message = meeting.Messages.Where(x => x.Status == 0).OrderByDescending(x => x.CreateDate).FirstOrDefault();

                Clients.Client(toUserId).SendAsync("onGuestReceivedMessage", fromAdminId, fromAdminUser.UserName, message, "left")/*.onGuestReceivedMessage(fromAdminId, fromAdminUser.UserName, message, "left");*/ ;
                Clients.Caller.SendAsync("onAdminReceivedMessage", toUserId, fromAdminUser.UserName, message, "right", new { lastmessage = last_message, countunread = 0 })/*.onAdminReceivedMessage(toUserId, fromAdminUser.UserName, message, "right", new { lastmessage = last_message, countunread = 0 })*/;
            }

        }
        public void GuestSendMessage(string toAdminId, string message)
        {
            var admin = ConnectedUsers.FirstOrDefault(x => x.IsAdmin == "1");
            if (admin != null)
            {
                toAdminId = ConnectedUsers.FirstOrDefault(x => x.IsAdmin == "1").ConnectionId;
            }
            else return;
            string fromUserId = Context.ConnectionId;
            var toAdmin = admin;
            var fromUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == fromUserId);
            if (toAdmin != null && fromUser != null)
            {
                var meeting = ListMeeting.SingleOrDefault(x => x.Id == fromUserId);
                if (meeting != null)
                {
                    // meeting.Messages = SetStatusAllMessageInMeeting(meeting,1);
                    var message_ = new MessageDetail { ConnectionId = fromUserId, UserName = fromUser.UserName, IsAdmin = 0, Status = 0, CreateDate = DateTime.Now, Message = message };
                    var messages_ = meeting.Messages;
                    messages_.Add(message_);
                    // meeting.Messages.Add(message_);
                    meeting.Messages = messages_;
                }
                var last_message = meeting.Messages.Where(x => x.Status == 0).OrderByDescending(x => x.CreateDate).FirstOrDefault();
                var count_message_unread = meeting.Messages.Count(x => x.Status == 0);
                Clients.Client(toAdminId).SendAsync("onAdminReceivedMessage", fromUserId, fromUser.UserName, message, "left", new { lastmessage = last_message, countunread = count_message_unread })/*.onAdminReceivedMessage(fromUserId, fromUser.UserName, message, "left", new { lastmessage = last_message, countunread = count_message_unread })*/;
                Clients.Caller.SendAsync("onGuestReceivedMessage", toAdminId, fromUser.UserName, message, "right")/*.onGuestReceivedMessage(toAdminId,fromUser.UserName,message,"right")*/;
            }
        }

        private void AddMessageinCache(string userName, string message, string ConnectionId, int IsAdmin, int Status)
        {
            CurrentMessage.Add(new MessageDetail { UserName = userName, Message = message, CreateDate = DateTime.Now, ConnectionId = ConnectionId, IsAdmin = IsAdmin, Status = Status });

            if (CurrentMessage.Count > 100)
                CurrentMessage.RemoveAt(0);
        }
        private void AddMessageinMeetings(string username, string message, string connectionid, int isadmin, int status)
        {
            var meeting = ListMeeting.Where(x => x.Id == connectionid).SingleOrDefault();
            if (meeting != null)
            {
                meeting.Messages.Add(new MessageDetail
                {
                    UserName = username,
                    Message = message,
                    CreateDate = DateTime.Now,
                    ConnectionId = Context.ConnectionId,
                    IsAdmin = isadmin,
                    Status = status
                });
            }
        }
        private string renderBoxchats(List<Meeting> meetings)
        {
            var boxchats = "";
            try
            {
                foreach (var meeting in meetings)
                {
                    boxchats += renderBoxchat(meeting);
                }
                return boxchats;
            }
            catch (Exception ex)
            {

                return ex.Message;
            }

        }
        private string renderBoxchat(Meeting meeting)
        {
            //var content = "";

            // var meeting = "abc";
            if (meeting != null)
            {
                var group_action = "<div class=\"chat-box bg-white\">" +
           "<div class=\"input-group\">" +
               "<input class=\"form-control border no-shadow no-rounded\" id=\"textbox-message-" + meeting.Id + "\" placeholder=\"Type your message here\">" +
               "<span class=\"input-group-btn\">" +
                   "<button class=\"btn btn-success no-rounded\" id=\"btn-send-message-" + meeting.Id + "\" data-iduser=\"" + meeting.Id + "\" type=\"button\">Send</button>" +
               "</span>" +
           "</div><!-- /input-group -->" +
       "</div>";
                var messages = "";
                foreach (var message in meeting.Messages)
                {
                    var class_align = message.IsAdmin == 1 ? "right" : "left";
                    var name = message.UserName;
                    //var messages = "";
                    messages += "<li class=\"" + class_align + " clearfix\">" +
                            "<span class=\"chat-img pull-" + class_align + "\">" +
                                "<img src=\"https://bootdey.com/img/Content/user_3.jpg \" alt=\"User Avatar\">" +
                            "</span>" +
                            "<div class=\"chat-body clearfix\">" +
                                "<div class=\"header\">" +
                                    "<strong class=\"primary-font\">" + name + "</strong>" +
                                /* '<small class="pull-right text-muted"><i class="fa fa-clock-o"></i> 12 mins ago</small>'+*/
                                "</div>" +
                                "<p>" +
                                    message.Message +
                                "</p>" +
                            "</div>" +
                        "</li>";

                }

                var boxchat = " <div id=\"ct-" + meeting.Id + "\" class=\"col-md-8 bg-white tab-pane fade\" style=\"max-height:400px;overflow-y:scroll\">"
           + "<div class=\"chat-message\">"
           + "<ul class=\"chat\" id=\"chat-" + meeting.Id + "\">"
           + messages +
           "</ul>"
           + group_action
           + "</div>";
                return boxchat;

            }
            return "";

        }
        public List<MessageDetail> SetStatusAllMessageInMeeting(Meeting meeting, int status)
        {
            try
            {
                var messages_ = meeting.Messages.Where(x => x.Status != status).ToList();
                if (messages_.Count > 0)
                {
                    foreach (var m in messages_)
                    {
                        m.Status = status;
                    }
                }
                return messages_;

            }
            catch
            {
                return null;
            }

        }


    }
}