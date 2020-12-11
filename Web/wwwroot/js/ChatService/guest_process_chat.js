$(function () {
        //Init function
   
        // Declare a proxy to reference the hub. 
        /*var chatHub = $.connection.chatHub;
        console.log(chatHub);
   
        registerClientMethods(chatHub);

        // Start Hub
        $.connection.hub.start().done(function () {
            registerEvents(chatHub);
            check_admin_online(chatHub);


        }).fail(function() {
            alert("Could not Connect!");
        });*/
        //
        //Declare new style
        //
        var base_url = $("#base_url").val(); 
        var connection = new signalR.HubConnectionBuilder()
            .withUrl(base_url+"chatHub")
            .configureLogging(signalR.LogLevel.Information)
            .build();

            

        connection.start().then(function () {
            console.log("OK");
            registerClientMethods(connection);
            registerEvents(connection);
            check_admin_online(connection);
        }).catch(err => console.error(err.toString()));
    
});

var registerClientMethods = function (connection) {
    /*chatHub.client.guestOnConnected = function (id,userName) {
        $('#register-content').hide();
        $('#content-messages').removeClass('hidden');
        $('#name-user-chat').html(userName);
        console.log(userName);
    };*/
    connection.on("guestOnConnected", function (id, userName) {
        $('#register-content').hide();
        $('#content-messages').removeClass('hidden');
        $('#name-user-chat').html(userName);
        console.log(userName);
    });
    /*chatHub.client.onGuestReceivedMessage = function (id, userName, message, align) {
        alert_message(id+userName+message+align);
        appendMessage(id, userName, message, align);
        received_status();
    };*/
    connection.on("onGuestReceivedMessage", function (id, userName, message, align) {
        alert_message(id + userName + message + align);
        appendMessage(id, userName, message, align);
        received_status();
    });
    /*chatHub.client.adminIsOnline = function () {
        readly_chat_status();
    };*/
    connection.on("adminIsOnline", function () {
        readly_chat_status();
    });
};
var registerEvents = function (connection) {
    $('#btn-chat-register').click(function () {
        var name = $('#btn-input-name-chat').val();
        if (name.length > 0) {
            connecting_status();
            //chatHub.server.guestConnect(name);
            connection.send("guestConnect", name);
        }
        else {
            alert("Bạn chưa nhập tên...");
        }

    });
    process_click_send_message(connection);
};
var appendMessage = function (id, name, message, align) {
    var content_message = '<li class="' + align + ' clearfix">' +
                                        '<span class="chat-img pull-' + align + '">' +
                                            '<img src="https://placehold.it/50/55C1E7/fff&text=U" alt="User Avatar" style="width:30px" class="img-circle" />' +
                                        '</span>' +
                                        '<div class="chat-body clearfix" style="margin-'+align+':35px">' +
                                            '<div class="header">' +
                                                '<strong class="primary-font" style="font-size:smaller">' + name + '</strong>' + /*<small class="pull-right text-muted">'+
                                                    <span class="glyphicon glyphicon-time"></span>14 mins ago
                                                </small>*/
                                            '</div>' +
                                            '<p style="font-size:smaller">' +
                                            message +
                                           '</p>' +
                                        '</div>' +
                                    '</li>';
    console.log(content_message);
    $('#content-chat').append(content_message);
    var height = $('#content-messages').find('#content-chat')[0].scrollHeight;
    $('#content-messages').find(".panel-body").scrollTop(height);
    console.log("height: "+height);

};
var alert_message = function (msg) {
    console.log(msg);
};
var process_click_send_message = function (connection) {
    $('#btn-chat-send').click(function () {
        var message = $('#btn-input-message').val();
        if ($.trim(message) != '') {
            sending_status();
            //chatHub.server.guestSendMessage("", message);
            //console.log("send " + message);
            connection.send("guestSendMessage","", message);
        }
    });
};
var sending_status = function () {
    $("#btn-input-message").attr("readonly", true);
    $('#btn-chat-send').attr("disabled",true);
    $("#btn-input-message").val("Sending...");
};
var received_status = function () {
    $("#btn-input-message").attr("readonly", false);
    $('#btn-chat-send').attr("disabled", false);
    $("#btn-input-message").val("");
};
var connecting_status = function () {
    $('#btn-input-name-chat').attr("readonly",true);
    $('#btn-input-name-chat').val("Connecting...");
};
var check_status_connect_ok = function (connection) {
    if (connection != null) {
        //if (chatHub.connection.lastError != null) {
           /* alert("Chưa kết nối được với Server.Vui lòng reload lại trang \n Chi tiết lỗi: "
                + chatHub.connection.lastError.message);*/
            //return false;
        //}
        return true;
    }
    else {
        /*alert("Không tìm thấy trạng thái kết nối chat");*/
        return false;
    }
};
var fail_connect_status = function (connection) {
    set_notifi_chatbox("Không thể kết nối để chat vui lòng reload trang", true);
    $('#title-heading-chat').hide();
    $('#btn-input-name-chat').attr("readonly", true);
    $('#btn-chat-register').attr("disabled", true);
};
var set_notifi_chatbox = function (message,isshow) {
    $('#notifi-message-heading').html(message);
    if (isshow) { $('#notifi-message-heading').show(); }
    else { $('#notifi-message-heading').hide(); }
};

var init_chat_app = function (connection) {
    $('#title-heading-chat').hide();
    set_notifi_chatbox("Chưa có nhân viên hỗ trợ", true);
    $('#btn-input-name-chat').attr("readonly", true);
    $('#btn-chat-register').attr("disabled", true);
};

var check_admin_online = function (connection) {
    init_chat_app(connection);
    //chatHub.server.checkAdminOnline();
    connection.send("checkAdminOnline");
};
var empty_admin_online = function () {
    set_notifi_chatbox("Chưa có nhân viên Online", true);
    $('#title-heading-chat').hide();
};
var readly_chat_status = function () {
    $('#title-heading-chat').show();
    $('#notifi-message-heading').hide();
    $('#btn-input-name-chat').attr("readonly", false);
    $('#btn-chat-register').attr("disabled", false);
}


