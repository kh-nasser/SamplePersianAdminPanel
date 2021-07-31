function AjaxHelper(url, jsonData, tag) {

    $.ajax({
        type: "post",
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: jsonData,
        cache: false,
        beforeSend: function () {
            //showAjaxLoadingModal(tag);
        },
        success: function (data) {
            //hideAjaxLoadingModal(tag);
            if (data.code == success) {
                //todo
            }
            else {
                //Func_ShowModal('توجه', data.message);
            }
        },
        error: function (xhr, status, error) {
            //hideAjaxLoadingModal(tag);
            //Func_ShowModal('توجه', 'خطایی در سیستم رخ داد');
        }
    });
}