"use strict";
var initDataTables = {
    init: function () {
        var table = $('#list-clients').DataTable(
            {
                dom: 'Blfrtip',
                language: {
                    infoEmpty: "Hiển thị 0 bản ghi",
                    emptyTable: "Không có dữ liệu",
                    lengthMenu: "Hiển thị mỗi _MENU_ kết quả",
                    infoFiltered: "(lọc từ _MAX_ total bản ghi)",
                    paginate: {
                        next: '&#8594;', // or '→'
                        previous: '&#8592;' // or '←'
                    },
                    search: 'Tìm kiếm',
                    info: "Hiển thị _START_ - _END_ trong _TOTAL_ kết quả"
                },
                "lengthChange": true,
                "info": false,
                "processing": true,
                "searching": true,
                "lengthMenu": [[5, 10, 15, -1], [5, 10, 15, "All"]],
                "pagingType": "full_numbers",
                "ajax": {
                    "url": "/Administrator/Slider/GetClients/",
                    "type": "GET",
                    "datatype": "json"
                },
                "buttons": [
                    {
                        extend: 'excel',
                        className: 'btn btn-success',
                        text: 'Xuất file Excel ',
                        title: 'Danh sách tin tức',
                        exportOptions: {
                            columns: 'th:not(:last-child)'
                        },
                        filename: 'Tin tức'
                    },
                    {
                        extend: 'print',
                        className: 'btn btn-info',
                        text: 'In',
                        title: 'Tin tức',
                        exportOptions: {
                            columns: 'th:not(:last-child)'
                        },
                        customize: function (win) {
                            $(win.document.body).find('table')
                                .addClass('compact')
                                .css('font-size', 'inherit');
                        }
                    }
                ],
                "order": [[0, 'desc']],
                "columns": [
                    { "data": "id" },
                    { "data": "photoName" },
                    {
                        "data": "status", "render": function (data) {
                            if (data === true) {
                                return '<div class="icon-status"><i class="fa fa-check-square fa-lg text-success"></i></div>';
                            }
                            else
                                return '<div class="icon-status icon-status-highlight"><i class="fa fa-thumbs-down fa-lg text-danger"></i></div>';
                        }
                    },
                    {
                        "data": "id", "render": function (data) {
                            data = "'" + data + "'";
                            return '<div class="actions-buttons"><a onclick="openInPopUpEdit(' +
                                data +
                                ')" class="btn btn-outline-info">Sửa</a>&nbsp;<a onclick="showPopUpDelete(' +
                                data +
                                ')" class="btn btn-outline-info text-danger trigger-btn"><i class="fa fa-trash"></i></a></div>';
                        }
                    }
                ]

            });

    },
    refresh: function () {
        var table = $('#list-clients').DataTable();
        table.destroy();
        this.init();
    }
};
var initSignalR = function () {
    var connection = new signalR.HubConnectionBuilder().withUrl("/NotifyHub").build();
    connection.on("ReceiveNotification", function (message, type) {
        //Type
        //1 = Add
        //2 = Update
        //3 = Delete
        console.log(message);
        initDataTables.refresh();

    });
    connection.onclose(function () {
        setTimeout(function () {
            console.log("Reconnecting in 3 seconds...");
            connection.start().catch(function (err) {
                alert("An error occured. Cannot connect to web socket server");
                console.log(err);
            });

        }, 2000);
    });
    connection.start().catch(function (err) {
        alert("An error occured. Cannot connect to web socket server");
        console.log(err);
    });
};
$(document).ready(function () {
    initDataTables.init();
    initSignalR();
});
