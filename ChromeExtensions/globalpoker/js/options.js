/// <reference path="jquery-3.3.1.min.js" />
$(function () {
    chrome.storage.sync.get(null,
        function (data) {
            if (data) {
                // $('#exampleInputEmail').val(data.username);
                // $('#exampleInputPassword').val(data.password);
                // $('#autoLogin').prop("checked",data.autoLogin);
                $('#url').val(data.server)
            }

        });
    $('#submit').click(function () {
        chrome.storage.sync.set({
                // username: $('#exampleInputEmail').val(),
                // password: $('#exampleInputPassword').val(),
                // autoLogin: $('#autoLogin').prop("checked")
                server: $('#url').val()
            },
            function () {
                console.log('保存成功！');
            });
    });
});