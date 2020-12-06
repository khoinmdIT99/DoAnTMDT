

var importbillController = {
    init: function () {
        importbillController.registerEvent();
    },

    registerEvent: function () {


        $('.btnDetail').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            alert("abc");
            var id = $(this).data('id');
            importbillController.loadDetail(id);
        });

        //$('#btnSearch').off('click').on('click', function () {
        //    importbillController.loadData(true);
        //});

        //$('#txtSearch').off('keypress').on('keypress', function (e) {
        //    if (e.which == 13) {
        //        importbillController.loadData(true);
        //    }
        //});
    
    },
    loadDetail: function (id) {
        $.ajax({
            url: '/Administrator/ImportBill/GetImportById',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                var data = response.data;
                var html = '';
                var template = $('#data-template-detailimport').html();
                $.each(data, function (i, item) {
                    html += Mustache.render(template, {
                        IDDetailImport: item.IdDetailImport,
                        IDProduct: item.IdProduct,
                        NameProduct: item.NameProduct,
                        Price: item.Price,
                        Amount: item.Amount
                    });
                });
                $('#tblDataDetailImport').html(html);
            },
            error: function (err) {
                console.log(err);
            }
        });
    }
}
importbillController.init();


