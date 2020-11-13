$(document).ready(function(){

	function destroy_owl(el) {
		el.data('owlCarousel').destroy();
	}

	function brands_tab(){
		if ($(".brands").find('a[data-toggle="tab"]').length > 0) {
			if ($(".brands .brand-right li").hasClass("active")) {
				$(".brands .brand-left li").find('a[data-toggle="tab"]').each(function(){
					$(this).on('show.bs.tab' , function(){
						$(".brand-right").find("li").removeClass("active");

					});
				});
			}

			if ($(".brands .brand-left li").hasClass("active")) {
				$(".brands .brand-right li").find('a[data-toggle="tab"]').each(function(){
					$(this).on('show.bs.tab' , function(){
						$(".brand-left").find("li").removeClass("active");

					});
				});
			}

			if ($(window).width() < 768 && $(window).width() > 479) {
				var height_center = $('.brand-center').find('.tab-content').height();
				var height_img = $('.brand-left').find('img').height();
				var ul = $('.brand-left').find('ul.nav');
				var title = $('.brand-left').find('ul.nav').find('.brand-title-box');
				title.height(height_center - height_img * 3 - 2);
			}
		}
	}

	brands_tab();

	if ($('header .cart').length >0) {
        $('.cart').hover(function() {
            $(this).toggleClass("open");
        });
    }

	function hover_menu_dropdown(dropdown){
		if(dropdown.length > 0){
			dropdown.find("ul li").each(function(){
				if ($(this).find("ul li").length > 0) {
					$(this).hover(function(){
						$(this).toggleClass("open");
					});
				}
			}); 
		}
	}

	hover_menu_dropdown($('.header-language'));
	hover_menu_dropdown($('.header-user'));
	hover_menu_dropdown($('.cart'));

	function deal_center_slider(el){
		el.owlCarousel2({
			loop:true,
			margin:0,
			nav:true,
			items:1,
			dots: false,
			navText : ["<i class='fa fa-angle-left'></i>","<i class='fa fa-angle-right'></i>"],
			responsive : {
				0 : {
					nav: false,
				},
				479 : {
					nav: true,
				},
			}
		});
	}

	function deal_slider_tab(){
		if ($('.deals-center-slider').length > 0 && $('.deals-left a[data-toggle="tab"]').length > 0) {
			deal_center_slider($('#living-room .deals-center-slider'));
			$('.deals-left a[data-toggle="tab"]').each(function(){
				var $this = $(this);
				var $class  = ($this.attr('href'));
				var $slider = $($class).find('.deals-center-slider');

				$this.on('shown.bs.tab', function () {
					deal_center_slider($($slider));
				}).on('hide.bs.tab', function () {
					destroy_owl($($slider));
				});
			});
		}
	}

	deal_slider_tab();

	if($('.table-countdown').length > 0){
		$('.table-countdown').each(function(){
			var date = $(this).data('countdown');
			$(this).countdown(date, function(event) {
				$(this).html(event.strftime(''
					+ '<div class="countdown-box"> '
					+ '<span class="time">%D</span> '
					+ '<span class="text">DAYS</span> '
					+ '</div> '
					+ '<div class="countdown-box"> '
					+ '<span class="time">%H</span> '
					+ '<span class="text">HOURS</span> '
					+ '</div> '
					+ '<div class="countdown-box"> '
					+ '<span class="time">%M</span> '
					+ '<span class="text">MINS</span> '
					+ '</div> '
					+ '<div class="countdown-box"> '
					+ '<span class="time">%S</span> '
					+ '<span class="text">SECS</span> '
					+ '</div> '
					));
			});
		});
	}

	if ($('.layout-1 .search-form-btn').length > 0 && $(window).width() < 991 && $(window).width() > 768) {
		var btn = $('.search-form-btn');
		var form = $('.search-form').find("form");
		btn.click(function(){
			form.stop().slideToggle();
			btn.find("i").toggleClass("fa-search").toggleClass("fa-times");
		});
		$(window).click(function(e){
			if($(form).has(e.target).length === 0 && !$(form).is(e.target) && $(btn).has(e.target).length === 0 && !$(btn).is(e.target)){
				$(form).slideUp(200);
				$(btn).find("i").removeClass('fa-times').addClass('fa-search');
			}
		});
	}

	if ($('.megamenu').length > 0) {
		if ($('.menu-normal').length > 0) {
		}
		else{
			$('.megamenu').children("li").find("ul li").each(function(){
				if ($(this).find("ul li").length > 0) {
					$(this).prepend('<i class="fa fa-angle-right" aria-hidden="true"></i>');
					$(this).find("ul li").hover(function(){
						$(this).addClass("active");
					});
				}
			});

			$('.megamenu .row-list').find("li").each(function(){
				$(this).find("i").click(function(){
					$(this).parent("li").find("ul").slideToggle(200);
				});
			});
		}
	}

	
	if ($('.layout-2 .search-form-btn').length > 0 && $(window).width() < 768) {
		var btn = $('.layout-2 .search-form-btn');
		var form = $('.layout-2 .search-form-header');
		$(btn).click(function(){
			$(form).stop().slideToggle();
			$(btn).find("i").toggleClass("fa-search").toggleClass("fa-times");
		});

		$(window).click(function(e){
			if($(btn).has(e.target).length == 0 && !$(btn).is(e.target) && $(form).has(e.target).length == 0 && !$(form).is(e.target)){
				$(form).slideUp(200);
				$(btn).find("i").removeClass('fa-times').addClass('fa-search');
			}
		});
	}

	function owl_caroulsel_nav(){
		initialize_owl($('#owl1'));

		$('a[href="#home"]').on('shown.bs.tab', function () {
			initialize_owl($('#owl1'));
		}).on('hide.bs.tab', function () {
			destroy_owl($('#owl1'));
		});

		$('a[href="#profile"]').on('shown.bs.tab', function () {
			initialize_owl($('#owl2'));
		}).on('hide.bs.tab', function () {
			destroy_owl($('#owl2'));
		});

		$('a[href="#messages"]').on('shown.bs.tab', function () {
			initialize_owl($('#owl3'));
		}).on('hide.bs.tab', function () {
			destroy_owl($('#owl3'));
		});
	}

	function initialize_owl(el) {
		el.owlCarousel2({
			loop: false,
			nav:true,
			dots: false,
			rewind: true,
			stagePadding: 0,
			checkVisibility: true,
			margin: 30,
			navText : ["<i class='fa fa-angle-left'></i>","<i class='fa fa-angle-right'></i>"],
			responsive : {
				0 : {
					items: 1
				},
				479 : {
					items: 2
				},
				768 : {
					items: 3
				},
				991 : {
					items: 4
                }
			}
		});
	}

	owl_caroulsel_nav();

	function category_slider_tab(){
		if ($('#cate1 .deals-slider').length > 0) {
			initialize_owl_v2($('#cate1 .deals-slider'));
			$('.deals-right a[data-toggle="tab"]').each(function(){
				var $this = $(this);
				var $class  = ($this.attr('href'));
				var $slider = $($class).find('.deals-slider');

				$this.on('shown.bs.tab', function () {
					initialize_owl_v2($($slider));
				}).on('hide.bs.tab', function () {
					destroy_owl($($slider));
				});
			});
		}
	}

	category_slider_tab();

	function initialize_owl_v2(el){
		el.owlCarousel2({
			loop:true,
			margin:0,
			nav:true,
			items:1,
			dots: false,
			navText : ["<i class='fa fa-angle-left'></i>","<i class='fa fa-angle-right'></i>"],
		});
	}

	function initialize_owl_v3(el){
		el.owlCarousel2({
			loop:true,
			nav:true,
			dots: false,
			rewind: true,
			stagePadding: 0,
			checkVisibility: true,
			margin: 30,
			navText : ["<i class='fa fa-angle-left'></i>","<i class='fa fa-angle-right'></i>"],
			responsive : {
				0 : {
					items: 1
				},
				479 : {
					items: 2
				},
				768 : {
					items: 3
				},
				991 : {
					items: 4
				}
			}
		});
	}

	function owl_caroulsel_nav_v4(){
		initialize_owl_v3($('#category1'));

		$('li[data-active-content=".items-category-1"]').on('click.ltabs-tab', function () {
			initialize_owl_v3($('#category1'));
		}).on('hide.bs.tab', function () {
			destroy_owl($('#category1'));
		});

		$('li[data-active-content=".items-category-2"]').on('click.ltabs-tab', function () {
			initialize_owl_v3($('#category2'));
		}).on('hide.bs.tab', function () {
			destroy_owl($('#category2'));
		});

		$('li[data-active-content=".items-category-3"]').on('click.ltabs-tab', function () {
			initialize_owl_v3($('#category3'));
		}).on('hide.bs.tab', function () {
			destroy_owl($('#category3'));
		});
	}

	function category_tab_slider(el){
		el.owlCarousel2({
			loop:true,
			nav:true,
			dots: false,
			rewind: true,
			stagePadding: 0,
			checkVisibility: true,
			margin: 0,
			navText : ["<i class='fa fa-angle-left'></i>","<i class='fa fa-angle-right'></i>"],
			responsive : {
				0 : {
					items: 1
				},
				479 : {
					items: 2
				},
				768 : {
					items: 2
				},
				991 : {
					items: 3
				},
				1199 : {
					items: 4
				}
			}
		});
	}

	function featured_tab_slider(el){
		el.owlCarousel2({
			loop:true,
			nav:true,
			dots: false,
			rewind: true,
			stagePadding: 0,
			checkVisibility: true,
			margin: 0,
			navText : ["<i class='fa fa-angle-left'></i>","<i class='fa fa-angle-right'></i>"],
			responsive : {
				0 : {
					items: 1
				},
				479 : {
					items: 1
				},
				768 : {
					items: 1
				},
				991 : {
					items: 2
				},
				1199 : {
					items: 3
				},
			}
		});
	}

	function owl_caroulsel_featured_tab(){
		if ($('.featured-sale-tab .featured-tab-slider').length > 0) {
			featured_tab_slider($('.living-room').find(".featured-tab-slider"));

			$('.featured-sale-tab li[data-active-content]').each(function(){
				var $this = $(this);
				var $class  = ($this.attr('data-active-content'));

				$this.on('click.ltabs-tab', function () {
					featured_tab_slider($($class).find(".featured-tab-slider"));
				}).on('hide.bs.tab', function () {
					destroy_owl($($class).find(".featured-tab-slider"));
				});
			});
		}
	}

	owl_caroulsel_featured_tab();

	function owl_caroulsel_category_tab(){
		if ($('.category-tab li[data-active-content|=".living-room"]').length > 0) {
			category_tab_slider($('.living-room-1').find('.category-tab-slider'));
			$('.category-tab li[data-active-content|=".living-room"]').each(function(){
				var $this = $(this);
				var $class  = ($this.attr('data-active-content'));

				$this.on('click.ltabs-tab', function () {
					category_tab_slider($($class).find(".category-tab-slider"));
				}).on('hide.bs.tab', function () {
					destroy_owl($($class).find(".category-tab-slider"));
				});
			});
		}
	}
	owl_caroulsel_category_tab();

	function owl_caroulsel_category_tab_v2(){
		if ($('.category-tab li[data-active-content|=".bedroom"]').length > 0) {
			category_tab_slider($('.bedroom-1').find('.category-tab-slider'));
			$('.category-tab li[data-active-content|=".bedroom"]').each(function(){
				var $this = $(this);
				var $class  = ($this.attr('data-active-content'));

				$this.on('click.ltabs-tab', function () {
					category_tab_slider($($class).find(".category-tab-slider"));
				}).on('hide.bs.tab', function () {
					destroy_owl($($class).find(".category-tab-slider"));
				});
			});
		}
	}
	owl_caroulsel_category_tab_v2();

	if ($('.layout-4 .search-form-btn').length > 0 && $(window).width() < 768) {
		var btn = $('.search-form-btn');
		var form = $('.search-form').find("form");
		btn.click(function(){
			// alert("click");
			btn.find("i").toggleClass("fa-search").toggleClass("fa-times");
			form.toggleClass("active");
		});
		$(window).click(function(e){
			if($(form).has(e.target).length == 0 && !$(form).is(e.target) && $(btn).has(e.target).length == 0 && !$(btn).is(e.target)){
				$(form).slideUp(200);
				$(btn).find("i").removeClass('fa-times').addClass('fa-search');
			}
		});
	}

	if ($('.layout-3 .category-tab .new-arrivals-tab').length > 0) {
		initialize_owl_v3($('.items-category-1').find(".new-arrivals-tab"));

		$('.layout-3 .category-tab li[data-active-content]').each(function(){
			var $this = $(this);
			var $class  = ($this.attr('data-active-content'));

			$this.on('click.ltabs-tab', function () {
				initialize_owl_v3($($class).find(".new-arrivals-tab"));
			}).on('hide.bs.tab', function () {
				initialize_owl_v3($($class).find(".new-arrivals-tab"));
			});
		});
	}

	if ($('.filter-price').length > 0) {
		var lowerSlider = document.querySelector('#lower');
		var  upperSlider = document.querySelector('#upper');

		document.querySelector('#two').value=upperSlider.value;
		document.querySelector('#one').value=lowerSlider.value;

		var  lowerVal = parseInt(lowerSlider.value);
		var upperVal = parseInt(upperSlider.value);

		upperSlider.oninput = function () {
			lowerVal = parseInt(lowerSlider.value);
			upperVal = parseInt(upperSlider.value);

			if (upperVal < lowerVal + 4) {
				lowerSlider.value = upperVal - 4;
				if (lowerVal == lowerSlider.min) {
					upperSlider.value = 4;
				}
			}
			document.querySelector('#two').value=this.value
		};

		lowerSlider.oninput = function () {
			lowerVal = parseInt(lowerSlider.value);
			upperVal = parseInt(upperSlider.value);
			if (lowerVal > upperVal - 4) {
				upperSlider.value = lowerVal + 4;
				if (upperVal == upperSlider.max) {
					lowerSlider.value = parseInt(upperSlider.max) - 4;
				}
			}
			document.querySelector('#one').value=this.value
		}; 
	}

	if ($('.fillter-color span[data-color]').length > 0) {
		var span = $('.fillter-color span[data-color]');
		span.each(function(){
			var color = $(this).attr("data-color");
			$(this).css({'background-color': color});
		});
	}

	if ($('.product-layout ').length > 0) {
		var $layout = $('.product-layout');
		$('#list-view').click(function(){
			$('#table-view').removeClass("active");
			$(this).addClass("active");
			if ($layout.hasClass("product-layout-table col-lg-4 col-md-4 col-sm-6 col-xs-6")) {
				$layout.removeClass("product-layout-table col-lg-4 col-md-4 col-sm-6 col-xs-6").addClass("product-layout-list col-lg-12 col-md-12 col-sm-12");
			}
			else if ($layout.hasClass("col-xs-12 product-layout-table")){
				$layout.removeClass("product-layout-table").addClass("product-layout-list");
			}
		});
		$('#table-view').click(function(){
			$('#list-view').removeClass("active");
			$(this).addClass("active");
			if ($layout.hasClass("product-layout-list col-lg-12 col-md-12 col-sm-12")) {
				$('.product-layout').addClass("product-layout-table col-lg-4 col-md-4 col-sm-6 col-xs-6").removeClass("product-layout-list col-lg-12 col-md-12 col-sm-12");
			}
			else if ($layout.hasClass("col-xs-12 product-layout-list")) {
				$layout.addClass("product-layout-table").removeClass("product-layout-list");
			}
		});
	}


	if ($('.aside-left').length > 0 && $('.sidebar-toggle-btn').length > 0) {
		var btn = $('.sidebar-toggle-btn');
		var aside = $('.aside-left');
		var overlay = $('.sidebar-overlay');
		var close_btn = $('.close-aside-left');
		btn.click(function(){
			$(aside).addClass("active");
			$(overlay).addClass("show");
		});
		$(close_btn).click(function(){
			$(aside).removeClass("active");
			$(overlay).removeClass("show");
		});
		$(overlay).click(function(){
			$(aside).removeClass("active");
			$(overlay).removeClass("show");
		});
	} 

	if ($('.product-detail-slider').length > 0) {
		$('.product-detail-slider').slick({
			slidesToShow: 1,
			slidesToScroll: 1,
			arrows: false,
			fade: true,
			asNavFor: '.product-detail-slider-gallery'
		});
		$('.product-detail-slider-gallery').slick({
			slidesToShow: 4,
			slidesToScroll: 1,
			asNavFor: '.product-detail-slider',
			dots: false,
			nextArrow: '<i class="fa fa-angle-up next-arrow" aria-hidden="true"></i>',
			prevArrow: '<i class="fa fa-angle-down prev-arrow" aria-hidden="true"></i>',
			centerMode: false,
			focusOnSelect: true,
			vertical: true,
			infinite: true,
			responsive: [
			{
				breakpoint: 1199,
				settings: {
					slidesToShow: 3,
				}
			},
			{
				breakpoint: 991,
				settings: {
					slidesToShow: 4,
				}
			},
			{
				breakpoint: 768,
				settings: {
					vertical: false,
				}
			},
			]
		});
	}

	if ($('.product-detail-slider-sidebar').length > 0) {
		$('.product-detail-slider-sidebar').slick({
			slidesToShow: 1,
			slidesToScroll: 1,
			arrows: false,
			fade: true,
			asNavFor: '.product-slider-gallery-sidebar'
		});
		$('.product-slider-gallery-sidebar').slick({
			slidesToShow: 4,
			slidesToScroll: 1,
			asNavFor: '.product-detail-slider-sidebar',
			dots: false,
			nextArrow: '<i class="fa fa-angle-right next-arrow" aria-hidden="true"></i>',
			prevArrow: '<i class="fa fa-angle-left prev-arrow" aria-hidden="true"></i>',
			centerMode: false,
			focusOnSelect: true,
			infinite: true,
			responsive: [
			{
				breakpoint: 1199,
				settings: {
					slidesToShow: 3,
				}
			},
			{
				breakpoint: 991,
				settings: {
					slidesToShow: 4,
				}
			},
			]
		});
	}




	function add_minus_quantity(){

		if ($('.quantity-control-btn').length > 0) {
			var add = $('.quantity-add');
			var sub = $('.quantity-minus');
			var allinput = $('.quantity-control-box').find('input');
			add.click(function(){
				var input = $(this).parent().find('input');
				var val = parseInt((input).val());
				input.val(val + 1);
			});
			sub.click(function(){
				var input = $(this).parent().find('input');
				var val = parseInt((input).val());
				if (val > 1) {
					input.val(val - 1);
				}
				else {
					input.val(val);
				}
			});
			$.fn.numberOnly = function(){
				return this.each(function(){
					$(this).keydown(function(e){
						var key = e.charCode || e.keyCode || 0;
						return (
							key == 8 || key == 9 || key == 13 || key == 46 ||
							(key >= 35 && key <= 40) ||
							(key >= 48 && key <= 57) ||
							(key >= 96 && key <= 105)
							);
					});
				});
			}
			allinput.numberOnly();
		}
	}

	add_minus_quantity();

	if ($('.popup-gallery').length > 0) {
		$('.popup-gallery').magnificPopup({
			delegate: 'a',
			type: 'image',
			tLoading: 'Loading image #%curr%...',
			mainClass: 'mfp-img-mobile',
			gallery: {
				enabled: true,
				navigateByImgClick: true,
			preload: [0,1] // Will preload 0 - before current, and 1 after the current image
		},
		image: {
			tError: '<a href="%url%">The image #%curr%</a> could not be loaded.',
			titleSrc: function(item) {
				return item.el.attr('title') + '<small>by Marsel Van Oosten</small>';
			}
		}
	});
	}
	if ($('[data-zoom-image]').length > 0) {
		$('[data-zoom-image]').elevateZoom({
			zoomType: 'inner',
			cursor: 'crosshair'
		});
	}


});

