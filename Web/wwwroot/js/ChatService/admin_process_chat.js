$(function () {
    //Init function

    // Declare a proxy to reference the hub. 
    setScreen(true);
    /*var chatHub = $.connection.chatHub;

    registerClientMethods(chatHub);

    // Start Hub
    $.connection.hub.start().done(function () {

        registerEvents(chatHub);

    });*/
    var base_url = $("#base_url").val();
    var base_url_admin = $("#base_url_admin").val();
    var connection = new signalR.HubConnectionBuilder()
        .withUrl(base_url + "chatHub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    
    connection.start().then(function () {
        console.log("OK");
        registerClientMethods(connection);
        registerEvents(connection);
    }).catch(err => console.error(err.toString()));

});
var registerEvents = function (connection) {
    var name = $('#username-chat').val();
    //chatHub.server.adminConnect(name);
    connection.send("adminConnect", name);
   // process_click_send_message(chatHub);
    process_select_user();
};
var registerClientMethods = function (connection) {
    /*chatHub.client.onAdminConnected = function (id, username, allGuestUsers,boxchats) {
        $('#list-guest-chat').html('');
        $('#tab-content-chat').html(boxchats);
        for (var i = 0; i < allGuestUsers.length; i++) {
            addUser(chatHub, allGuestUsers[i].ConnectionId, allGuestUsers[i].UserName);
            process_click_send_message(chatHub, allGuestUsers[i].ConnectionId);
        }
        //console.log("giá 2123:"+"abc");
        setScreen(false);
    };*/
    connection.on("onAdminConnected",function (id, username, allGuestUsers,boxchats) {
        $('#list-guest-chat').html('');
        $('#tab-content-chat').html(boxchats);
        for (var i = 0; i < allGuestUsers.length; i++) {
            addUser(connection, allGuestUsers[i].ConnectionId, allGuestUsers[i].UserName);
            process_click_send_message(chatHub, allGuestUsers[i].ConnectionId);
        }
        //console.log("giá 2123:"+"abc");
        setScreen(false);
    });
    /*chatHub.client.redirectUrl = function (url) {
        window.location = url;
    };*/
    connection.on("redirectUrl", function (url) {
        window.location = url;
    });
    /*chatHub.client.processReplaceUserAdmin = function (url, message) {
        alert(message);
        redirect_url(url);
    };*/
    connection.on("processReplaceUserAdmin", function (url, message) {
        alert(message);
        redirect_url(base_url_admin + url);
    });
    /*chatHub.client.onUserDisconnected = function (id,name) {
        $('#ele-'+id).remove();
    };*/
    connection.on("onUserDisconnected", function (id, name) {
        $('#ele-' + id).remove();
    });
    /*chatHub.client.onAddGuestInAdmin = function (id, name) {
        addUser(chatHub, id, name);
        addBoxChat(chatHub, id, name);
    };*/
    connection.on("onAddGuestInAdmin", onAddGuestInAdmin = function (id, name) {
        addUser(connection, id, name);
        addBoxChat(connection, id, name);
    });
    /*chatHub.client.onAdminReceivedMessage = function (id, name, message, align,object_last_info) {
        appendMessage(name, message, id, align);
        received_status(id);
        if (object_last_info != null) {
            append_delay_info(object_last_info.countunread, object_last_info.lastmessage);
            console.log(object_last_info);
        }
       
    };*/
    connection.on("onAdminReceivedMessage", function (id, name, message, align, object_last_info) {
        appendMessage(name, message, id, align);
        received_status(id);
        if (object_last_info != null) {
            append_delay_info(object_last_info.countunread, object_last_info.lastmessage);
            console.log(object_last_info);
        }

    });
    /*chatHub.client.onLoadMessage = function (list_message) {
        console.log(list_message);
    };*/
    connection.on("onLoadMessage", function (list_message) {
        console.log(list_message);
    });
};
var addUser = function (connection, id, name) {
    var str_ele = "";
    str_ele += '<li class="" id="ele-' + id + '">' +
                    '<a href="#ct-' + id + '" class="clearfix user-chat" data-iduser="' + id + '" data-toggle="pill">' +
                        '<img src="https://bootdey.com/img/Content/user_1.jpg" alt="" class="img-circle">' +
                        '<div class="friend-name">' +
        '<strong>' + name + '</strong>' +
                        '</div>' +
                        '<div class="last-message text-muted" id="content-last-ms-' + id + '"></div>' +
                        '<small class="time text-muted" id="time-last-ms-'+id+'"></small>'+
                        '<small class="chat-alert label label-danger" id="count-delay-ms-' + id + '"></small>' +
                    '</a>' +
                '</li>';
    $('#list-guest-chat').append(str_ele);
    process_select_user();
    
};
var appendMessage = function (name, message, id, class_align) {
    var content_message = '<li class="'+class_align+' clearfix">'+
                            '<span class="chat-img pull-'+class_align+'">'+
                                '<img src="https://bootdey.com/img/Content/user_3.jpg" alt="User Avatar">'+
                            '</span>'+
                            '<div class="chat-body clearfix">'+
                                '<div class="header">'+
                                    '<strong class="primary-font">'+name+'</strong>'+
                                   /* '<small class="pull-right text-muted"><i class="fa fa-clock-o"></i> 12 mins ago</small>'+*/
                                '</div>'+
                                '<p>'+
                                    message+
                                '</p>'+
                            '</div>'+
                        '</li>';
    $('#chat-' + id).append(content_message);

    var height = $('#ct-' + id).find('.chat-message')[0].scrollHeight;
    $('#ct-' + id).scrollTop(height);
    console.log("Height chat height: "+height);
    

};
var addBoxChat = function (connection,id, name) {
    var group_action = '<div class="chat-box bg-white">'+
            '<div class="input-group">'+
                '<input class="form-control border no-shadow no-rounded" id="textbox-message-'+id+'" placeholder="Type your message here">'+
                '<span class="input-group-btn">'+
                    '<button class="btn btn-success no-rounded" id="btn-send-message-'+id+'" data-iduser="'+id+'" type="button">Send</button>'+
                '</span>'+
            '</div><!-- /input-group -->'+
        '</div>';
    var boxchat = ' <div id="ct-' + id + '" class="col-md-8 bg-white tab-pane fade" style="max-height:400px;overflow-y:scroll">'
    + '<div class="chat-message">'
    + '<ul class="chat" id="chat-' + id + '"></ul>'
    +group_action
    + '</div>';

    $('#tab-content-chat').append(boxchat);
    process_click_send_message(connection,id);

};

var redirect_url = function (url) {
    window.location = url;
};
var setScreen = function (boolean_value) {
    //  $('body').prop("disabled", boolean_value);
    //alert("abc");
    if(boolean_value)
        $('#myModal').modal({ backdrop: "static" });
    else 
        $('#myModal').modal("hide");
};
var process_click_send_message = function (connection,id) {
    // if ('#btn-send-message') { }
    console.log(id+"- register");
    $('#btn-send-message-'+id).click(function () {
        alert('abc');
        var userid = $('#guest-selected').val();
        var message = $('#textbox-message-'+id).val();
        if ($.trim(message) != '') {
            console.log(userid + " Message: " + message);
            sending_status(id,"");
            //chatHub.server.adminSendMessage(userid, message);
            connection.send("adminSendMessage", userid, message);
        }
    });

};
var process_select_user = function () {
    $('.user-chat').click(function () {
        var toiduser = $(this).data('iduser');
        $('#guest-selected').val(toiduser);
    });
};
var sending_status = function (id, name) {
    $('#textbox-message-' + id).attr("readonly", true);
    $('#btn-send-message-' + id).attr("disabled", true);
    $('#textbox-message-' + id).val("Sending...");
}
var received_status = function (id) {
    $('#textbox-message-' + id).attr("readonly", false);
    $('#btn-send-message-' + id).attr("disabled",false);
    $('#textbox-message-' + id).val("");
}
var append_delay_info = function (count_ms_delay, last_ms) {
    if (count_ms_delay > 0 && last_ms != null) {
        $('#content-last-ms-' + last_ms.ConnectionId).html(last_ms.Message);
        $('#content-last-ms-' + last_ms.ConnectionId).html(last_ms.Message);
        $('#count-delay-ms-' + last_ms.ConnectionId).html(count_ms_delay);
    }
    else if (count_ms_delay <= 0 && last_ms != null) {
        $('#content-last-ms-' + last_ms.ConnectionId).html(last_ms.Message);
        $('#content-last-ms-' + last_ms.ConnectionId).html(last_ms.Message);
        $('#count-delay-ms-' + last_ms.ConnectionId).html("");
    }

}

