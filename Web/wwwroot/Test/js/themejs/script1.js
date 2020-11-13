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

GUI._init_home_slider = function(){
	if ($(".home-slider").length > 0) {
		$('.home-slider').owlCarousel2({
			loop:true,
			margin:0,
			nav:true,
			items:1,
			dots: true,
			navText : ["<i class='fa fa-angle-left'></i>","<i class='fa fa-angle-right'></i>"],
			responsive : {
				479 : {
					nav: false,
				},
				991 : {
					nav:true,
				}
			}
		});
	}
}

GUI.init = function() {
	GUI._init_home_slider();
	GUI._init_header_search_form();
	GUI._init_hover_dropdown_menu();
};
$(function(){
	GUI.init();
})