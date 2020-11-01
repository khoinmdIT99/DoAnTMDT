function CoreTableDeleteRow(tableId, url, id) {
	var r = confirm("Bạn có chắc chắn muốn xóa bản ghi này?");
	if (r == true) {
		$.ajax(
			{
				type: "POST",
				url: url,
				data: {
					id: id
				},
				error: function (result) {
					toastr.error(result);
				},
				success: function (result) {
					if (result == true) {
						toastr.success('Xóa thành công');
						$(`#${tableId}`).DataTable().ajax.reload();
					}
					else {
						toastr.error('Có lỗi xảy ra. Vui lòng kiểm tra lại.')
					}
				}
			});
	}
}

(function ($) {

	$.fn.CoreTable = function (options) {
		var tableId = $(this).attr("id");
		options.ajax = $.extend({
			type: 'POST',
			"contentType": "application/json",
			data: function (d) {
				return JSON.stringify(d);
			}
		}, options.ajax)
		options.defaultAction = $.extend({
			update: null,
			delete: null,
			width: "80px"
		}, options.defaultAction);
		var lengthMenu = tableLengthMenu || null;
		var pageLength = tableItemPerPage || 10;

		var defaultOptions = {
			orderCellsTop: true,
			fixedHeader: true,
			processing: true,
			serverSide: true,
			pageLength: pageLength,
			language: {
				decimal: ",",
				processing: "Đang tải dữ liệu...",
				search: "Tìm kiếm:",
				lengthMenu: "Hiển thị _MENU_ bản ghi",
				info: "Hiển thị _START_ đến _END_ trong số _TOTAL_ bản ghi",
				infoEmpty: "Hiển thị 0 đến 0 trong số 0 bản ghi",
				infoPostFix: "",
				loadingRecords: "Đang tải dữ liệu...",
				zeroRecords: "Không tìm thấy bản ghi hợp lệ",
				emptyTable: "Không có bản ghi nào",
				paginate: {
					first: '<i class="fa fa-angle-double-left"></i>',
					previous: '<i class="fa fa-arrow-left"></i>',
					next: '<i class="fa fa-arrow-right"></i>',
					last: '<i class="fa fa-angle-double-right"></i>'
				},
			},
			responsive: true,
			dom: '<"top"l>rt<"row"<"col-sm-12 col-md-5"i><"col-sm-12 col-md-7"p>><"clear">'
		};
		if (lengthMenu) {
			defaultOptions.lengthMenu = lengthMenu;
		}

		var settings = $.extend(defaultOptions, options);

		if (settings.defaultAction && (settings.defaultAction.update || settings.defaultAction.delete)) {
			if (!settings.columns)
				settings.columns = [];
			$(`#${tableId} thead tr`).each(function () {
				$(this).append(`<th style="width: ${settings.defaultAction.width};"></th>`);
			});
			settings.columns.push({
				orderable: false,
				data: function (data) {
					var updateButton = settings.defaultAction.update ? `
						<a role="button" class="btn btn-info" href="${settings.defaultAction.update}/${data.id}">
							<i class="fas fa-edit"></i>
						</a>
					` : '';
					var deleteButton = settings.defaultAction.delete ? `
						<a role="button" class="btn btn-danger" href="#" onclick="CoreTableDeleteRow('${tableId}','${settings.defaultAction.delete}','${data.id}')">
							<i class="fas fa-trash"></i>
						</a>
					`: '';
					return `
						<div class="btn-group">
							${updateButton}
							${deleteButton}
						</div>
					`;
				}
			});
		}


        var delaySearch;
		$(`#${tableId} thead tr:eq(1) th`).each(function (i) {
			$('input, select', this).on('keyup change', function () {
				if (table.column(i).search() !== this.value) {
					clearTimeout(delaySearch);
					var value = this.value;
					delaySearch = setTimeout(function () {
						table
							.column(i)
							.search(value)
							.draw();
					}, 300);
				}
			});
		});
		var table = $(`#${tableId}`).DataTable(settings);
		return table;
    };

}(jQuery));