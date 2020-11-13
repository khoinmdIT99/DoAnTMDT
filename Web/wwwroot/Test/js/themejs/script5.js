var GUI = {};

GUI._init_hover_dropdown_menu = function() {
	if ($('.header-user').length > 0) {
		$('.header-user').find("ul li").each(function(){
			if ($(this).find("ul li").length > 0) {
				$(this).hover(function(){
					$(this).toggleClass("open");
				});
			}
		}); 
	}
}

GUI._init_hover_cart = function(){
	if ($('header .cart').length >0) {
		$('.cart').hover(function(){
			$(this).toggleClass("open");
		})
	}
}

GUI._init_megamenu = function(){
	if ($('.megamenu').length > 0) {
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
		})
	}
}

GUI._init_header_search_form = function(){
	if ($('.search-form-btn').length > 0 && $(window).width() < 768) {
		var btn = $('.search-form-btn');
		var form = $('.search-form').find("form");
		btn.click(function(){
			form.stop().slideToggle();
			btn.find("i").toggleClass("fa-search").toggleClass("fa-times");
		});
		$(window).click(function(e){
			if($(form).has(e.target).length == 0 && !$(form).is(e.target) && $(btn).has(e.target).length == 0 && !$(btn).is(e.target)){
				$(form).slideUp(200);
				$(btn).find("i").removeClass('fa-times').addClass('fa-search');
			}
		});
	}
}

// GUI._init_home_slider = function(){
// 	if ($(".home-slider").length > 0) {
// 		$('.home-slider').owlCarousel2({
// 			loop:true,
// 			margin:0,
// 			nav:true,
// 			items:1,
// 			dots: true,
// 			navText : ["<i class='fa fa-angle-left'></i>","<i class='fa fa-angle-right'></i>"],
// 			responsive : {
// 				0 : {
// 					nav: false,
// 				},
// 				991 : {
// 					nav:true,
// 				}
// 			}
// 		});
// 	}
// }

GUI._init_destroy_owl = function(el){
	el.data('owlCarousel').destroy();
}


// GUI._init_popular_product_slider = function(el){
// 	if (el.length > 0) {
// 		el.owlCarousel2({
// 			loop:true,
// 			nav:true,
// 			dots: false,
// 			rewind: true,
// 			stagePadding: 0,
// 			checkVisibility: true,
// 			items: 1,
// 			navText : ["<i class='fa fa-angle-left'></i>","<i class='fa fa-angle-right'></i>"],
// 			responsive : {
// 				0 : {
// 					items: 1,
// 				},
// 				768 : {
// 					items: 1,
// 				},
// 				991 : {
// 					items: 1,
// 				},
// 				1199 : {
// 					items: 1,
// 				}

// 			}
// 		});
// 	}
// }

GUI._init_countdown = function(){
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
}

GUI._init_deals_center_slider = function(){
	if ($(".deals-center-slider").length > 0) {
		$('.deals-center-slider').owlCarousel2({
			loop:true,
			margin:0,
			nav:true,
			items:1,
			dots: true,
			navText : ["<i class='fa fa-angle-left'></i>","<i class='fa fa-angle-right'></i>"],
			responsive : {
				0 : {
					nav: true,
				},
				991 : {
					nav:true,
				}
			}
		});
	}
}

GUI._init_category_slider =	function(el){
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
				items: 1,
			},
			479 : {
				items: 2,
			},
			768 : {
				items: 2,
			},
			991 : {
				items: 3,
			},
			1199 : {
				items: 3,
			}
		}
	});
}


GUI._init_category_slider_tab = function() {
	if ($('.category-tab li[data-active-content|=".living-room"]').length > 0) {
		GUI._init_category_slider($('.living-room-1').find('.category-tab-slider'));
		$('.category-tab li[data-active-content|=".living-room"]').each(function(){
			var $this = $(this);
			var $class  = ($this.attr('data-active-content'));

			$this.on('click.ltabs-tab', function () {
				GUI._init_category_slider($($class).find(".category-tab-slider"));
			}).on('hide.bs.tab', function () {
				GUI._init_destroy_owl($($class).find(".category-tab-slider"));
			});
		});
	}
}

GUI._init_category_slider_tab_v2 = function() {
	if ($('.category-tab li[data-active-content|=".bedroom"]').length > 0) {
		GUI._init_category_slider($('.bedroom-1').find('.category-tab-slider'));
		$('.category-tab li[data-active-content|=".bedroom"]').each(function(){
			var $this = $(this);
			var $class  = ($this.attr('data-active-content'));

			$this.on('click.ltabs-tab', function () {
				GUI._init_category_slider($($class).find(".category-tab-slider"));
			}).on('hide.bs.tab', function () {
				GUI._init_destroy_owl($($class).find(".category-tab-slider"));
			});
		});
	}
}

GUI._init_category_slider_tab_v3 = function() {
	if ($('.category-tab li[data-active-content|=".dinning-room"]').length > 0) {
		GUI._init_category_slider($('.dinning-room-1').find('.category-tab-slider'));
		$('.category-tab li[data-active-content|=".dinning-room"]').each(function(){
			var $this = $(this);
			var $class  = ($this.attr('data-active-content'));

			$this.on('click.ltabs-tab', function () {
				GUI._init_category_slider($($class).find(".category-tab-slider"));
			}).on('hide.bs.tab', function () {
				GUI._init_destroy_owl($($class).find(".category-tab-slider"));
			});
		});
	}
}



// GUI._init_cutommer_opinion_slider = function(){
// 	if ($('.customer-opinion-slider').length > 0) {
// 		$('.customer-opinion-slider').owlCarousel2({
// 			loop:true,
// 			margin:0,
// 			nav:true,
// 			items:1,
// 			dots: false,
// 			navText : ["<i class='fa fa-angle-left'></i>","<i class='fa fa-angle-right'></i>"],
// 		});
// 	}
// }

GUI._init_top_selling_slider =  function(el){
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
				items: 1,
			},
			479 : {
				items: 1,
			},
			768 : {
				items: 1,
			},
			991 : {
				items: 2,
			},
			1199 : {
				items: 3,
			},
		}
	});
}

GUI._init_top_selling_tab =  function(){
	if ($('.featured-sale-tab .featured-tab-slider').length > 0) {
		GUI._init_top_selling_slider($('.living-room').find(".featured-tab-slider"));

		$('.featured-sale-tab li[data-active-content]').each(function(){
			var $this = $(this);
			var $class  = ($this.attr('data-active-content'));

			$this.on('click.ltabs-tab', function () {
				GUI._init_top_selling_slider($($class).find(".featured-tab-slider"));
			}).on('hide.bs.tab', function () {
				GUI._init_destroy_owl($($class).find(".featured-tab-slider"));
			});
		});
	}
}

// GUI._init_blogs_slider = function(){
// 	if ($('.blog-slider').length > 0) {
// 		$('.blog-slider').owlCarousel2({
// 			loop:true,
// 			margin:0,
// 			nav:true,
// 			items:2,
// 			dots: false,
// 			navText : ["<i class='fa fa-angle-left'></i>","<i class='fa fa-angle-right'></i>"],
// 			responsive : {
// 				0 : {
// 					items: 1,
// 				},
// 				768 : {
// 					items: 1,
// 				},
// 				991 : {
// 					items: 2,
// 				}
// 			}
// 		});
// 	}
// }

GUI.init = function(){
	// GUI._init_home_slider();
	// GUI._init_popular_product_slider($(".popular-product-slider"));
	GUI._init_countdown();
	GUI._init_hover_cart();
	GUI._init_deals_center_slider();
	GUI._init_category_slider_tab();
	GUI._init_category_slider_tab_v2();
	GUI._init_category_slider_tab_v3();
	// GUI._init_cutommer_opinion_slider();
	GUI._init_top_selling_tab();
	// GUI._init_blogs_slider();
	GUI._init_header_search_form();
	GUI._init_hover_dropdown_menu();
	GUI._init_megamenu();
}

$(function(){
	GUI.init();
});