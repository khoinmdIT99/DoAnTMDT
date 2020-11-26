var BaseController = function () {

    this.initialize = function () {
        registerEvents();
    };
    function cartitemCount() {
        $.post("/Cart/ItemControl",
            function (result) {
                $("#itemCount").text(result);
            });
    }
    function priceitemCount() {
        $.post("/Cart/PriceControl",
            function(result) {
                $("#itemPrice").text(result);
            });
    }
    function updateProduct(productId, count) {

        var sData = { productId: productId, quantity: count };
        $.ajax({

            url: "/Cart/AddToCart",
            data: sData,
            type: "Post",
            dataType: "json",
            cache: false,
            success: function (result) {
                if (count <= 100) {
                    Swal.fire(
                        'Cập nhật thành công',
                        '',
                        'success'
                    );
                    cartitemCount();
                    priceitemCount();
                    loadHeaderCart();
                }
                else {
                    Swal.fire(
                        'Không được quá ' + 100 + ' sản phẩm',
                        '',
                        'error'
                    );
                }
            },
            error: function (jqXHR, textStatus) {
                alert("Một lỗi đã xảy ra.");
            }
        });
    }
    function XacNhan(code) {

        var sData = { code: code};
        $.ajax({

            url: "/Cart/AddToCart",
            data: sData,
            type: "Post",
            dataType: "json",
            cache: false,
            success: function (result) {
                if (count <= 100) {
                    Swal.fire(
                        'Cập nhật thành công',
                        '',
                        'success'
                    );
                    cartitemCount();
                    priceitemCount();
                    loadHeaderCart();
                }
                else {
                    Swal.fire(
                        'Không được quá ' + 100 + ' sản phẩm',
                        '',
                        'error'
                    );
                }
            },
            error: function (jqXHR, textStatus) {
                alert("Một lỗi đã xảy ra.");
            }
        });
    }
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
                        success: function (result) {
                            cartitemCount();
                            priceitemCount();
                            loadHeaderCart();
                            if (result.state === 1) {
                                Swal.fire(
                                    'Bạn đã thêm vào giỏ hàng !',
                                    '',
                                    'success'
                                );
                            }
                            else if (result.state === 2) {
                                Swal.fire({
                                    title: 'Bạn đã có sản phẩm này trong giỏ của bạn. Nhập số sản phẩm mới.',
                                    input: 'number',
                                    inputAttributes: {
                                        autocapitalize: 'off'
                                    },
                                    showCancelButton: true,
                                    confirmButtonText: 'Thêm vào giỏ',
                                    cancelButtonText: 'Huỷ bỏ',
                                    showLoaderOnConfirm: true,
                                    preConfirm: (newCount) => {

                                        updateProduct(id, newCount);
                                        cartitemCount();
                                        priceitemCount();
                                        loadHeaderCart();
                                    }
                                });
                            }
                            else {
                                Swal.fire(result.message, "", "error");
                                cartitemCount();
                                priceitemCount();
                                loadHeaderCart();
                            }
                            
                        },
                        error: function () {
                            alert("Có lỗi xảy ra, vui lòng thử lại sau!");
                        }
                    });
                }
            });
        });
        $('.aaf').on("click",
            function(e) {
                e.preventDefault();
                var bla = $(this).data("id");
                var sData = {
                    productId: bla
                };
                $.ajax({
                    url: "/Cart/ViewProduct",
                    data: sData,
                    type: "GET",
                    dataType: "json",
                    cache: false,
                    success: function (result) {
                        $("#labelId").text(result.id);
                        $("#labelCode").text(result.productCode);
                        $("#labelName").text(result.productName);
                        $("#labelDescription").text(result.description);
                        $("#labelPrice").text("$" + result.price);
                        $('#viewImg1').attr('href', "imageUpload/" +result.displayImages[0]);
                        $('#viewImg2').attr('src', "/imageUpload/"+result.displayImages[0]);
                        $('#labelCategory').text(result.categoryName);
                    },
                    error: function (jqXHR, textStatus) {
                        alert("Đã xảy ra lỗi.");
                    }
                });
                $("#myModal1").modal();
            });
        //$('.remove-cart').click(function (e) {
        //    e.preventDefault();
        //    var id = $(".remove-cart").data("id");
        //    $.ajax({
        //        url: '/Cart/RemoveFromCart',
        //        type: 'post',
        //        data: {
        //            productId: id
        //        },
        //        success: function (response) {
        //            Swal.fire(
        //                'Bạn đã xóa vật phẩm thành công !',
        //                '',
        //                'success'
        //            );
        //            cartitemCount();
        //            priceitemCount();
        //            loadHeaderCart();
        //        },
        //        error: function () {
        //            cartitemCount();
        //            priceitemCount();
        //            loadHeaderCart();
        //        }
        //    });
        //});
        $('body').on('click', '.remove-cart', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            alert("id");
            $.ajax({
                url: '/Cart/RemoveFromCart',
                type: 'post',
                data: {
                    productId: id
                },
                success: function (response) {
                    Swal.fire(
                        'Bạn đã xóa vật phẩm thành công !',
                        '',
                        'success'
                    );
                    cartitemCount();
                    priceitemCount();
                    loadHeaderCart();
                },
                error: function () {
                    cartitemCount();
                    priceitemCount();
                    loadHeaderCart();
                }
            });
        });
    }

    function loadHeaderCart() {
        $("#headerCart").load("/AjaxContent/CartInLayout", function (responseTxt, statusTxt, xhr) {
            cartitemCount();
            priceitemCount();
        });
    }
};

var baseObj = new BaseController();
baseObj.initialize();