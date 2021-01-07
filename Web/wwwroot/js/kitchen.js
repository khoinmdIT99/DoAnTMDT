"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/restauranthub").build();

connection.on("NewOrder", function (mathongbao, noidung, thoigian) {
    alert("Có đơn");
    var div = document.createElement('div');
    div.className = 'panel panel-default';
    div.id = mathongbao;
    div.innerHTML = '<div class="panel-body">' +
        '<div class="pull-left col-sm-6">' +
        '<div class="com-sm-12"><p>Đơn:' + noidung + '</p></div>' +
        '<div class="com-sm-12"><p>Thời gian:' + thoigian + '</p></div>' +
        '</div>' +
        '<div class="pull-right col-sm-6">' +
        '<div class="col-sm-12">' +
        '<form class="btn-group" role="group">' +
        '<a href=""~/trangchuquanly/choxuly.html" value="Xem đơn" id="begin+' + mathongbao + '" class="btn btn-default" />' +
        '</form> </div>' +
        '</div>' +
        '</div>';
    document.getElementById("orderlist").insertBefore(div, document.getElementById("orderlist").childNodes[0]);
});


connection.start().catch(function (err) {
    return console.error(err.toString());
});






