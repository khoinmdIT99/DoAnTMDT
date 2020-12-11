$(function () {
    process_click_hide_content();
});
var process_click_hide_content = function () {
    $('#panel-heading-box-chat').click(function () {
        $('#content-box-chat').toggle("fast", function () {
        });
    });
};