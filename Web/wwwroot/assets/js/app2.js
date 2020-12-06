"use strict";
$(document).ready(function () {
    $('#list-imports').DataTable(
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
                "url": "/Administrator/ImportBill/GetImports/",
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
                { "data": 'idImport' },
                {
                    "data": "totalValue", "render": function (data) {

                        return data.toLocaleString('it-IT', { style: 'currency', currency: 'VND' });
                    }
                },
                { "data": "amount" },
                { "data": "idSupplier" },
                //{ "data": "dateCreated" },
                {
                    "data": "dateCreated", "render": function (data) {

                        return data;
                    }
                },
                //{
                //    "data": "tienNo", "render": function (data) {
                //        return '<div style = "text-align:center;" class="icon-status icon-status-highlight">' + data + '</div>';
                //    }
                //},
                {
                    "data": "idImport", "render": function (data) {
                        var id = data;
                        data = "'" + data + "'";
                        return '<div class="actions-buttons"><a onclick="openInPopUpEdit2(' +
                            data +
                            ')" class="btn btn-outline-info">Sửa</a>&nbsp;<a onclick="showPopUpDelete2(' +
                            data +
                            ')" class="btn btn-outline-info text-danger trigger-btn"><i class="fa fa-trash"></i></a>&nbsp;<a onclick="viewdetail(' +
                            data +
                            ')" class="btn btn-outline-info btnDetail">Detail</a></div>';
                    }
                }
            ]

        });
});