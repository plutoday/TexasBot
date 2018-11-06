$(function () {
    var $main=
    var watchGame = function ($table) {
        console.info('start watchGame');

        var $communityCards = $table.find('.community-cards');
        var watchCommunityCards = function () {
            //d=方片, h=红桃,c=草花,s=黑桃
            //<img class="card-image c-10-97277 hid-cb3145a5-8ff0-4bf9-ade4-dca326f6e124" src="/poker-client/skins/globalpoker/images/cards/8h.svg" id="communityCardImage-10-97277" alt="8h">
            $communityCards.find('.card-image').each(function (index) {
                var alt = $(this).attr('alt');
                console.info('底盘' + (index + 1) + ':' + alt);
                sendMessage(alt);
            });
        }
        $communityCards.bind('DOMNodeInserted', function (e) {
            watchCommunityCards();
        });

        watchCommunityCards();
    };
    var interval = setInterval(function () {
        if ($('.table-view-container:visible').length > 0) {
            clearInterval(interval);
            watchGame($('.table-view-container:visible'));
        }
    }, 1000);
});