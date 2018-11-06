console.info('load gamewatcher.js');

var host = null;
chrome.storage.sync.get(null,
    function (data) {
        if (data) {
            host = data.server;
        }
    });
// var renderConsole = function () {
//     var height = $(window).height() - 300;
//     $('#viewPort').height(height);
//     var src = chrome.extension.getURL('html/console.html');
//      = $('<iframe src="' + src + '" style="height: 290px; position: absolute; top:' + height + 'px; background: white; width: 100%"></iframe>');
//     $('body').append($iframe)
// }
var GameStatus = {
    Waiting: 'Waiting',
    Playing: 'Playing',
    Finished: 'Finished'
};
var ProcessStatus = {
    None: 'None',
    //发牌
    Deal: 'Deal',
    //发出底牌后,公共牌出现以前的第一轮叫注阶段
    PreFlop: 'PreFlop',
    //翻牌：第一次发出的3张公共牌
    FlopCard: 'FlopCard',
    //前三张公共牌出现以后的押注圈。
    FlopRound: 'FlopRound',
    // 转牌：第二次发出的1张公共牌
    TurnCard: 'TurnCard',
    TurnRound: 'TurnRound',
    // 河牌：第三次发出的1张公共牌
    RiverCard: 'RiverCard',
    RiverRound: 'RiverRound',
    //翻开底牌
    Showdown: 'Showdown'
};
var PlayerStatus = {
    None: 'None',
    Playing: 'Playing',
    SittingOut: 'SittingOut',
    Disconnect: 'Disconnect'
};

var DesitionType = {
    Undefined: 'Undefined',
    Ante: 'Ante',
    Check: 'Check',
    Fold: 'Fold',
    Raise: 'Raise',
    Reraise: 'Reraise',
    Call: 'Call',
    AllIn: 'AllIn',
    AllInRaise: 'AllInRaise',
};

var watchGame = function (id) {
    console.info('start watchGame');
    var $table = $('#tableView-' + id);
    var game = {
        id: id,
        processStatus: ProcessStatus.None,
        gameStatus: GameStatus.Waiting,
        communityCards: [],
        players: [],
        //彩池
        pot: 0,
        activeSeat: -1,
        activeId: -1,
        dealerSeat: -1,
        dealerId: -1,
        heroId: -1,
        logs: [],
        smallBlinds: 0,
        bigBlinds: 0,
        setActiveSeat: function (seat) {
            var me = this;
            me.set('activeSeat', seat);
            $.each(this.players, function (index, item) {
                if (item.seat == seat) {
                    me.set('activeId', item.id);
                    return false;
                }
            })
        },
        setDealerSeat: function (seat) {
            var me = this;
            if (me.dealerSeat != seat) {
                me.set('dealerSeat', seat);

                $.each(this.players, function (index, item) {
                    if (item.seat == seat) {
                        me.set('dealerId', item.id);
                    }
                });
                var position = 0;
                for (var i = 0; i < me.players.length; ++i) {
                    var player = me.players[(i + me.dealerId) % me.players.length];
                    if (player.playerStatus == PlayerStatus.Playing) {
                        player.position = position++;
                    }
                }
                if (seat != -1) {
                    me.reset();
                    startNewRound();
                }
            }
        },
        reset: function () {
            $.each(this.players, function (index, item) {
                item.reset();
            });
        },
        setHeroId: function (id) {
            var me = this;
            me.set('heroId', id);

        },
        set: function (key, value) {
            if (this[key] != value) {
                this[key] = value;
                log(key + "=" + value);
            }
        }
    };
    var currentRoundId;
    var log = function (str) {
        var tmp = 'id:' + game.id + '->' + str;
        game.logs.push(tmp);
        console.log(tmp);
    };
    var sendRequest = function (query, request, callback) {
        log('send request:' + JSON.stringify(request));
        $.ajax({
            url: host + query,
            type: 'post',
            data: JSON.stringify(request),
            dataType: 'json',
            contentType: 'application/json',
            success: function (data) {
                log('receive response:' + JSON.stringify(data));
                callback(data);
            }
        });
    };
    var startNewRound = function () {
        //观战
        if (game.heroId == -1) {
            return;
        }
        var request = {
            HeroIndex: game.heroId,
            ButtonIndex: game.dealerId,
            BigBlindSize: game.bigBlinds,
            SmallBlindSize: game.smallBlinds,
            Players: []
        };
        // var players = [];
        // for (var i = 0; i < game.players.length; ++i) {
        //     players.push(game.players[i]);
        // }
        // players.sort(function (a, b) {
        //     return a.seat - b.seat;
        // });
        $.each(game.players, function (index, player) {

            request.Players.push({
                Name: player.name,
                StackSize: player.balance,
                SittingOut: player.playerStatus !== PlayerStatus.Playing
            });
        });
        sendRequest('Rounds/StartNewRound', request, function (data) {
            if (data) {
                currentRoundId = data.RoundId;
            } else {
                currentRoundId = null;
            }
        });
    };
    var convertCard = function (card) {
        var rank = 'Undefined';
        var suit = 'Undefined';
        switch (card.charAt(1)) {
            case 'd':
                suit = 'Diamond';
                break;
            case 'c':
                suit = 'Club';
                break;
            case 'h':
                suit = 'Heart';
                break;
            case 's':
                suit = 'Spade';
                break;
        }
        switch (card.charAt(0)) {
            case '2':
                rank = 'Two';
                break;
            case '3':
                rank = 'Three';
                break;
            case '4':
                rank = 'Four';
                break;
            case '5':
                rank = 'Five';
                break;
            case '6':
                rank = 'Six';
                break;
            case '7':
                rank = 'Seven';
                break;
            case '8':
                rank = 'Eight';
                break;
            case '9':
                rank = 'Nine';
                break;
            case 't':
                rank = 'Ten';
                break;
            case 'j':
                rank = 'Jack';
                break;
            case 'q':
                rank = 'Queen';
                break;
            case 'k':
                rank = 'King';
                break;
            case 'a':
                rank = 'Ace';
                break;
        }
        return {
            Suit: suit,
            Rank: rank
        };
    };
    var notifyHeroHoles = function () {
        if (!currentRoundId) {
            return;
        }
        var hero = game.players[game.heroId];
        var request = {
            RoundId: currentRoundId,
            Holes: [convertCard(hero.cards[0]), convertCard(hero.cards[1])]
        };
        sendRequest('Rounds/NotifyHeroHoles', request, function () {

        });

    };
    var notifyDecision = function (id) {
        if (!currentRoundId) {
            return;
        }
        var player = null;
        for (var i = 0; i < game.players.length; ++i) {
            if (game.players[i].id == id) {
                player = game.players[i];
                break;
            }
        }
        var length = player.actions[game.processStatus].length;
        if (length == 0) {
            log('no action');
            return;
        }
        var action = player.actions[game.processStatus][length - 1];
        var request = {
            RoundId: currentRoundId,
            PlayerName: player.name,
            Decision: {
                DecisionType: action.decision,
                TempChipsAdded: action.amount
            }
        };
        sendRequest('Rounds/NotifyDecision', request, function () {

        });
    };
    var notifyFlops = function () {
        if (!currentRoundId) {
            return;
        }
        sendRequest('Rounds/NotifyFlops', {
            RoundId: currentRoundId,
            Flops: [convertCard(game.communityCards[0]), convertCard(game.communityCards[1]), convertCard(game.communityCards[2])]
        }, function () {

        });
    };
    var notifyTurn = function () {
        if (!currentRoundId) {
            return;
        }
        sendRequest('Rounds/NotifyTurn', {
            RoundId: currentRoundId,
            Turn: convertCard(game.communityCards[3])
        }, function () {

        });
    };
    var notifyRiver = function () {
        if (!currentRoundId) {
            return;
        }
        sendRequest('Rounds/NotifyRiver', {
            RoundId: currentRoundId,
            Flops: convertCard(game.communityCards[4])
        }, function () {

        });
    };
    var getDecision = function () {
        if (!currentRoundId) {
            return;
        }
        $.ajax({
            url: host + 'Rounds/GetDecision',
            type: 'get',
            data: {roundId: currentRoundId},
            dataType: 'json',
            success: function (data) {
                log('receive response:' + JSON.stringify(data));
            }
        });
    }
    var getAmount = function ($q) {
        var text = $q.text();
        if (text) {
            return parseFloat(text.replace(/,/g, ''));
        }
        return NaN;
    };
    var bindDomChanged = function ($container, callback) {

        $container.mutationSummary('connect', function () {
            callback($container);
        }, [{all: true}]);
        callback($container);
    };
    var $communityCards = $table.find('.community-cards');
    var watchCommunityCards = function ($container) {
        //d=方片, h=红桃,c=草花,s=黑桃
        //<img class="card-image c-10-97277 hid-cb3145a5-8ff0-4bf9-ade4-dca326f6e124" src="/poker-client/skins/globalpoker/images/cards/8h.svg" id="communityCardImage-10-97277" alt="8h">
        var $cards = $container.find('.card-image:visible');
        if ($cards.length !== game.communityCards.length) {
            game.communityCards = [];
            $cards.each(function () {
                var alt = $(this).attr('alt');
                game.communityCards.push(alt);
            });
            switch ($cards.length) {
                case 0:
                    game.set('processStatus', ProcessStatus.PreFlop);
                    break;
                case 3:
                    game.set('processStatus', ProcessStatus.FlopRound);
                    notifyFlops();
                    break;
                case 4:
                    game.set('processStatus', ProcessStatus.TurnRound);
                    notifyTurn();
                    break;
                case 5:
                    game.set('processStatus', ProcessStatus.RiverRound);
                    notifyRiver();
                    break;
            }
            log('底盘变化:' + game.communityCards.join(','));
            $.each(game.players, function (index, player) {
                player.hasAddAction = false;
            });
        }
    };
    var watchTotalPot = function ($container) {
        var pot = getAmount($container.find('.amount'));
        game.set('pot', pot);
        if (pot < 0.01) {
            game.set('gameStatus', GameStatus.Playing);
            game.set('processStatus', ProcessStatus.PreFlop);
        }
    };
    var watchActiveTurn = function ($container) {
        var cls = $container.attr('class');
        var activeTurn = -1;
        if (cls != '') {
            activeTurn = parseInt(cls.substring(cls.lastIndexOf('-') + 1));
        }
        game.setActiveSeat(activeTurn);
    };
    var convertAction = function (actionText, amount) {
        var decision;
        switch (actionText.toLowerCase()) {
            case 'raise':
                decision = DesitionType.Raise;
                break;
            case 'check':
                decision = DesitionType.Check;
                break;
            case 'fold':
            case 'sitting out':
                decision = DesitionType.Fold;
                break;
            case 'call':
                decision = DesitionType.Call;
                break;
            case 'bet':
                decision = DesitionType.Raise;
                break;
            default:
                log('unknown decision:' + actionText);
                return null;
        }
        if (decision == DesitionType.Raise ||
            decision == DesitionType.Call ||
            decision == DesitionType.Ante) {
            if (!amount) return null;
            return {
                decision: decision,
                amount: amount
            }
        }
        else if (decision) {
            return {
                decision: decision,
                amount: null
            }
        }
    }
    var watchPlayer = function ($container, player) {
        if (player.id == game.heroId) {
            return;
        }
        player.$container = $container;
        var status = PlayerStatus.Playing;
        if ($container.hasClass('empty-seat')) {
            status = PlayerStatus.None;
        } else if ($container.hasClass('seat-sit-out')) {
            status = PlayerStatus.SittingOut;
        }
        player.set('playerStatus', status);
        if (status == PlayerStatus.None) {
            player.reset();
            return;
        }
        var name = $container.find('.player-name').text().trim();
        player.set('name', name);
        var balance = 0;
        var strBalance = $container.find('.seat-balance').text().trim();
        if (strBalance.toLowerCase().indexOf('all in') >= 0) {
            balance = 0;
        } else {
            balance = parseFloat(strBalance.replace(/,/g, ''))
        }
        player.set('balance', balance);
        //seat
        var indexRegex = $container.prop('class').match(/seat-pos-(?<index>\d*)/);
        if (indexRegex) {
            player.set('seat', parseInt(indexRegex.groups['index']));
        } else {
            game.setHeroId(player.id);
        }
        var amount = getAmount($container.find('.action-amount.balance').find('.value'));
        //Decition
        var actionText = $container.find('.action-text:visible').text();
        if (actionText) {
            var action = convertAction(actionText, amount);
            if (action) {
                player.addAction(action);
                for (var i = 0; i < game.players.length; ++i) {
                    if (game.players[i].id !== player.id) {
                        game.players[i].hasAddAction = false;
                    }
                }
            }
        }
    };


    var watchHero = function ($container) {

        if (game.heroId == -1) {
            return;
        }
        var hero = game.players[game.heroId];
        var name = $container.find('.player-name').text().trim();
        hero.set('name', name);

        var balance = 0;
        var strBalance = $container.find('.seat-balance').text().trim();
        if (strBalance.toLowerCase().indexOf('all in') >= 0) {
            balance = 0;
        } else {
            balance = parseFloat(strBalance.replace(/,/g, ''))
        }
        hero.set('balance', balance);
        hero.set('playerStatus', PlayerStatus.Playing);
        var $card1 = $container.find('.number-1');
        var $card2 = $container.find('.number-2');
        if ($card1.length > 0 && $card2.length > 0) {
            var cards = [$card1.attr('alt'), $card2.attr('alt')];
            if (cards.toString() != hero.cards.toString()) {
                hero.cards = cards;
                notifyHeroHoles();
            }
        }
        else {
            hero.cards = [];
        }
    };
    var watchTableInfo = function ($container) {
        var str = $container.find('.table-blinds-value').text();
        var smallBlind = 0;
        var bigBlind = 0;
        if (str !== null) {
            var blinds = str.trim().split('/');
            if (blinds.length == 2) {
                smallBlind = parseInt(blinds[0].replace(/,/g, ''));
                bigBlind = parseInt(blinds[1].replace(/,/g, ''));
            }
        }
        game.set('bigBlinds', bigBlind);
        game.set('smallBlinds', smallBlind);

    };
    var watchDealerButton = function ($container) {
        var style = $container.prop('style')['transform'];
        var regex = style.match(/translate3d\((?<x>\d*)px, (?<y>\d*)px, 0px\)/);
        var x = 0;
        var y = 0;
        var dealerSeat = -1;

        do {
            if (regex.length == 0) {
                break;
            }
            x = parseFloat(regex.groups.x) + $container.width();
            y = parseFloat(regex.groups.y) + $container.height();
            var isInArea = function ($area) {
                var position = $area.position();
                return x >= position.left && x <= position.left + $area.width() &&
                    y >= position.top && y <= position.top + $area.height();
            };
            $.each(game.players, function (index, player) {
                var $this = $(player.$container);
                if ($this.hasClass('empty-seat')) {
                    return;
                }
                if (isInArea($this)) {
                    dealerSeat = player.seat;
                    return false;
                }
            });
        } while (false);
        game.setDealerSeat(dealerSeat);
    };
    var watchTable = function ($container) {
        var hero = game.players[game.heroId];
        if ($container.hasClass('your-turn') &&
            hero.getDecision[game.processStatus] === false
        ) {
            hero.getDecision[game.processStatus] = true;
            getDecision();
        }
    };
    bindDomChanged($communityCards, watchCommunityCards);
    bindDomChanged($table.find('.total-pot'), watchTotalPot);
    bindDomChanged($($table.children()[0]), watchActiveTurn);
    bindDomChanged($table.find('.blinds.normal'), watchTableInfo);
    bindDomChanged($table.find('.dealer-button'), watchDealerButton);
    bindDomChanged($table.find('.my-player-seat'), watchHero);
    bindDomChanged($table, watchTable);
    var bindPlayer = function (seat) {
        bindDomChanged($('#seat' + seat + '-' + game.id), function ($container) {
            watchPlayer($container, game.players[seat]);
        });
    };
    for (var i = 0; i < 10; ++i) {
        game.players.push({
            $container: null,
            name: null,
            id: i,
            seat: null,
            balance: 0,
            cards: [],
            // // 1-小盲 2-大盲
            position: null,
            isHero: false,
            playerStatus: PlayerStatus.None,
            actions: {},
            // //有可能重复添加action，用这个位表示当前action已经添加了
            hasAddAction: false,
            getDecision: {},
            reset: function () {
                this.actions = {};
                for (var key in ProcessStatus) {
                    this.actions[key] = [];
                    this.getDecision[key] = false;
                }
                this.hasAddAction = false;
            },
            addAction: function (action) {
                if (game.activeId == this.id && !this.hasAddAction) {
                    this.hasAddAction = true;
                    this.actions[game.processStatus].push(action);
                    notifyDecision(this.id);
                    log('seat ' + this.seat + ',name=' + this.name + ",id=" + this.id + " add " + game.processStatus + " actions: " + JSON.stringify(action));
                }
            },
            set: function (key, value) {
                if (this[key] != value) {
                    this[key] = value;
                    log('seat=' + this.seat + ',name=' + this.name + ",id=" + this.id + key + "=" + value);
                }
            }
        });
        game.players[i].reset();
        bindPlayer(i);
    }
    log('开始监控：' + game.id);
    return game;
};

var watchedIds = [];
var watchedGames = [];
var interval = setInterval(function () {
    var $tableView = $('.table-view-container:visible');
    if ($tableView.length == 0) {
        return;
    }
    var $tables = $tableView.find('.table-container');
    if ($tables.length == 0) {
        return;
    }

    $tables.each(function () {
        var tableViewId = $(this).prop('id');
        var id = parseInt(tableViewId.substring(tableViewId.indexOf('-') + 1));
        if (watchedIds.indexOf(id) < 0) {
            watchedIds.push(id);
            watchedGames.push(watchGame(id));
        }
    })
}, 1000);