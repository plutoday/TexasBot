$(function () {
    console.info('load login.js');
    //var iframe = ;
    var onload = function () {
        console.info('load login.js+' + $('a').length);
        console.info('pokerClientFrame loaded');
        //iframe加载完成后你需要进行的操作    
        chrome.storage.sync.get(null,
            function (data) {
                if (!(data && data.autoLogin === true)) {
                    console.info('disabled auto login');
                    return;
                }
                var $loginBtn = $("a[role='button']:contains('Login')");
                if ($loginBtn.length !== 1) {
                    console.info('no login button. has already logged');
                    return;
                }
                $loginBtn[0].click();
                $('input[name="email"]').val(data.username);
                $('input[name="password"]').val(data.password);
                $('button:contains("Log in")')[0].click();
            });
    };
    var interval = setInterval(function () {
        if ($('header').length > 0) {
            clearInterval(interval);
            onload();
        }
    }, 1000);
});