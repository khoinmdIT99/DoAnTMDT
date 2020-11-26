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

function openInPopUpEdit1(id) {
    showInPopup1('/Administrator/Supplier/CreateSupplier/' + id);
}

function showPopUpDelete1(id) {
    //document.getElementById("btnDelete").setAttribute("onclick", "`('/Administrator/Slider/DeleteSlider/" + id + "')");
    $('#delete-modal1 #Supplier_Id').val(id);
    $('#delete-modal1').modal('show');
}

function deleteSupplier() {

    var id = $('#delete-modal1 #Supplier_Id').val();
    alert(id);
    $.ajax({
        type: 'POST',
        url: "/Administrator/Supplier/DeleteSupplier/" + id,
        success: function (res) {
            if (res.isDeleted) {
                $('#view-list-Supplier').html(res.html);
                $('#delete-modal1').modal('hide');
                toastr.warning("Deleted successfuly !");
            }
            else {
                $('#delete-modal1').modal('hide');
                toastr.info("Supplier not deleted");
            }
        },
        error: function (err) {
            $('#delete-modal1').modal('hide');
            toastr.error("Error occured");
        }
    });

}

function showInPopup1(url, title) {
    //alert(url);
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal1 .modal-body').html(res);
            $('#form-modal1 .modal-title').html(title);
            $('#form-modal1').modal('show');
        }
    });
}

function JqueryAjaxPost1(form) {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#view-list-Supplier').html(res.html);
                    $('#form-modal1 .modal-body').html('');
                    $('#form-modal1 .modal-title').html('');
                    $('.modal-content').modal('hide');
                    $('#form-modal1').modal('hide');
                    toastr.success("Supplier submitted successfuly", "Submit Supplier");
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

function onReset1() {
    document.getElementById("MyForm1").reset();
}

function getLastPayedPeriod(idPaymentOnGoing) {
    $.ajax({
        type: 'GET',
        url: '/Client/GetLastPayedPeriodByIdPayment/' + idPaymentOnGoing,
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
        url: '/Client/isPaymentOK/' + idPaymentOnGoing,
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

function getPaymentStatus(IdPaymentOnGoing) {
    $.ajax({
        type: 'GET',
        url: '/Client/isPaymentOK/' + IdPaymentOnGoing,
        success: function (res) {
            console.log("From getPaymentStatus isOk = " + res.isPaymentOk);
            return '<div class="icon-status"><i class="fa fa-thumbs-up fa-lg text-success"></i></div>';
        },
        error: function (err) {
            console.log("From getPaymentStatus isOk = false");
            return '<div class="icon-status"><i class="fa fa-thumbs-down fa-lg text-danger"></i></div>';
        }
    });
}


function pay(id) {
    $.ajax({
        type: 'POST',
        url: '/Client/Pay/' + id,
        data: id,
        success: function (res) {
            toastr.success("Payment is done successfuly");
            showPaymentAsOk();
            $('#view-list-clients').html(res.html);
            getLastPayedPeriod(res.IdPayment);
            $('#IdPaymentOnGoing').val(res.IdPayment);

        },
        error: function (err) {
            toastr.error("Payment failed");
        }
    });
}