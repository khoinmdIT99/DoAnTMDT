// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var isOk;
toastr.options = {
    "closeButton": true,
    "debug": false,
    "newestOnTop": true,
    "progressBar": true,
    "positionClass": "toast-top-left",
    "preventDuplicates": false,
    "showDuration": "100",
    "hideDuration": "100",
    "timeOut": "3000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}

function openInPopUpEdit2(id) {
    showInPopup2('/Administrator/ImportBill/Pay/' + id);
}

function viewdetail(id) {
    $('#modalAddUpdate').modal('show');
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
                    IdDetailImport: item.idDetailImport,
                    NameProduct: item.nameProduct,
                    Price: item.price.toLocaleString('it-IT', { style: 'currency', currency: 'VND' }),
                    Amount: item.amount
                });
            });
            $('#tblDataDetailImport').html(html);
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function showPopUpDelete2(id) {
    //document.getElementById("btnDelete").setAttribute("onclick", "deleteClient('/Client/DeleteClient/" + id + "')");
    $('#delete-modal2 #ImportBill_Id').val(id);
    $('#delete-modal2').modal('show');
}

function deleteImport() {
    var id = $('#delete-modal2 #ImportBill_Id').val();
    $.ajax({
        type: 'POST',
        url: "/Administrator/ImportBill/DeleteImport/" + id,
        success: function (res) {
            if (res.isDeleted) {
                $('#view-list-import').html(res.html);
                $('#delete-modal2').modal('hide');
                toastr.warning("Import deleted successfuly !");
            }
            else {
                $('#delete-modal2').modal('hide');
                toastr.info("Import not deleted");
            }
        },
        error: function (err) {
            $('#delete-modal2').modal('hide');
            toastr.error("Error occured");
        }
    });
}

function showInPopup2(url, title) {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal2 .modal-body').html(res);
            $('#form-modal2 .modal-title').html(title);
            $('#form-modal2').modal('show');
        }
    });
}

function JqueryAjaxPost2(form) {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#view-list-import').html(res.html);
                    $('#form-modal2 .modal-body').html('');
                    $('#form-modal2 .modal-title').html('');
                    $('.modal-content').modal('hide');
                    $('#form-modal2').modal('hide');
                    toastr.success("Client submitted successfuly", "Submit client");
                }
                else {
                    toastr.warning("Form not valid");
                }
            },
            error: function (err) {
                console.log(err);
                alert("Error occured");
            }
        });
    }
    catch (e) {
        console.log(e);
    }

    // to prevent form submit event.
    return false;
}

function onReset() {
    document.getElementById("MyForm2").reset();
}

function getLastPayedPeriod(id) {
    $.ajax({
        type: 'GET',
        url: '/Administrator/ImportBill/GetLastPayedPeriodByIdPayment/' + id,
        success: function (res) {
            $("#lastPayedPeriod").html(res.lastPayedPeriod);
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function isPaymentOK(idPaymentOnGoing) {
    $.ajax({
        type: 'GET',
        url: '/Administrator/ImportBill/isPaymentOK/' + idPaymentOnGoing,
        success: function (res) {
            if (res.isPaymentOk) {
                showPaymentAsOk();
            }
            else {
                showPaymentAsNok();
            }
        }
    });
}

function showPaymentAsOk() {
    $(".payment-div-ok").show();
    $(".payment-div-nok").hide();
    $('#btnPay').prop('disabled', true);
}

function showPaymentAsNok() {
    $(".payment-div-ok").hide();
    $(".payment-div-nok").show();
    $('#btnPay').prop('disabled', false);
}

//function pay(id) {
//    $.ajax({
//        type: 'POST',
//        url: '/Client/Pay/' + id,
//        data: id,
//        success: function (res) {
//            toastr.success("Payment is done successfuly");
//            showPaymentAsOk();
//            $('#view-list-clients').html(res.html);
//            getLastPayedPeriod(res.IdPayment);
//            $('#IdPaymentOnGoing').val(res.IdPayment);

//        },
//        error: function (err) {
//            toastr.error("Payment failed");
//        }
//    });
//}