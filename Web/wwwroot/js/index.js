var BaseController = function () {

    this.initialize = function () {
        registerEvents();
    };

    function registerEvents() {
        $('body').on('click', '.add-to-cart', function (e) {
            e.preventDefault();
            Swal.fire({
                title: 'Thêm sản phẩm vào giỏ hàng?',
                text: '',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#28A745',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Vâng',
                cancelButtonText: 'Huỷ bỏ'
            }).then((result) => {
                if (result.value) {
                    var id = $(this).data('id');
                    $.ajax({
                        url: '/Cart/AddToCart',
                        type: 'Post',
                        data: {
                            productId: id,
                            quantity: 1
                        },
                        success: function (response) {
                            Swal.fire(
                                'Bạn đã thêm vào giỏ hàng !',
                                '',
                                'success'
                            );
                            loadHeaderCart();
                        },
                        error: function () {
                            alert("Có lỗi xảy ra, vui lòng thử lại sau!");
                        }
                    });
                }
            });
        });

        $('body').on('click', '.remove-cart', function (e) {
            appcore.startLoading();
            e.preventDefault();
            var id = $(this).data('id');
            $.ajax({
                url: '/Cart/RemoveFromCart',
                type: 'post',
                data: {
                    productId: id
                },
                success: function (response) {
                    appcore.notify(Message.removeItemSuccess, Notify.success);
                    loadHeaderCart();
                },
                error: function () {
                    appcore.stopLoading();
                }
            });
        });
    }

    function loadHeaderCart() {
        $("#headerCart").load("/AjaxContent/CartInLayout", function (responseTxt, statusTxt, xhr) {
            appcore.stopLoading();
        });
    }
};

var baseObj = new BaseController();
baseObj.initialize();