// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$('.select2').select2();
$('.date-picker').daterangepicker({
	timePicker: false,
	singleDatePicker: true,
	autoApply: true
});
$(document).ready(function () {
	bsCustomFileInput.init();
});