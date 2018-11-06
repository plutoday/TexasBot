//chrome.runtime.onInstalled.addListener(function () {
//    console.info('addListener');
//    chrome.declarativeContent.onPageChanged.removeRules(undefined, function () {
//        console.info('removeRules');
//        chrome.declarativeContent.onPageChanged.addRules([
//            {
//                conditions: [
//                    // 只有打开百度才显示pageAction
//                    new chrome.declarativeContent.PageStateMatcher({ pageUrl: { urlContains: 'globalpoker.com'}})
//                ],
//                actions: [new chrome.declarativeContent.ShowPageAction()]
//            }
//        ]);
//    });
//});

chrome.browserAction.onClicked.addListener(function (tab) {
    chrome.tabs.create({
        url: 'https://globalpoker.com/'

    });
    console.info(tab);
});