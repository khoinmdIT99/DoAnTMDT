"use strict";
$(document).ready(function () {
    $('#list-suppliers').DataTable(
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
                "url": "/Administrator/Supplier/GetSuppliers/",
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
                { "data": "name" },
                { "data": "icn" },
                { "data": "phone" },
                { "data": "email" },
                { "data": "description" },
                {
                    "data": "money", "render": function (data) {
                        if (data !== 0) {
                            return '<div style = "text-align:center;" class="icon-status"></div>';
                        }
                        else
                            return '<div style = "text-align:center;" class="icon-status icon-status-highlight">0</div>';
                    }
                },
                {
                    "data": "id", "render": function (data) {
                        data = "'" + data + "'";
                        return '<div class="actions-buttons"><a onclick="openInPopUpEdit1(' +
                            data +
                            ')" class="btn btn-outline-info">Sửa</a>&nbsp;<a onclick="showPopUpDelete1(' +
                            data +
                            ')" class="btn btn-outline-info text-danger trigger-btn"><i class="fa fa-trash"></i></a></div>';
                    }
                }
            ]

        });
});