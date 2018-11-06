using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using TexasBot.Games.Players;
using TexasBot.Games.Recorders;
using TexasBot.Models;
using TexasBot.Tools;

namespace TexasBot.Games.Dealers
{
    public class Dealer : IDealer
    {
        private readonly ICardDrawer _cardDrawer;
        private readonly IGameRecorder _recorder;
        public List<PlayerRegistry> PlayerRegistries { get; set; }

        public Dealer(ICardDrawer cardDrawer, IGameRecorder recorder)
        {
            PlayerRegistries = new List<PlayerRegistry>();
            _cardDrawer = cardDrawer;
            _recorder = recorder;
        }

        public void RegisterPlayer(IPlayer player)
        {
            PlayerRegistries.Add(new PlayerRegistry(player));
        }

        public void Deal()
        {
            foreach (var playerRegistry in PlayerRegistries)
            {
                playerRegistry.NewGameReset();
            }

            _recorder.StartNewGame(PlayerRegistries.Select(p => p.Player.Name).ToList());

            DealHoles();
            PollPlayers(GameStage.HolesReceived);

            if (CheckEarlyFinish())
            {
                return;
            }

            DealFlops();
            PollPlayers(GameStage.FlopsSeen);

            if (CheckEarlyFinish())
            {
                return;
            }

            DealTurn();
            PollPlayers(GameStage.TurnSeen);

            if (CheckEarlyFinish())
            {
                return;
            }

            DealRiver();
            PollPlayers(GameStage.RiverSeen);

            if (CheckEarlyFinish())
            {
                return;
            }

            Judge(FindWinners());
        }

        private void PollPlayers(GameStage gameStage)
        {
            foreach (var playerRegistry in PlayerRegistries)
            {
                playerRegistry.NewStageReset();
            }

            var playerIndex = GetIndexOfPlayerToStartForStage(gameStage);
            while (true)
            {
                var playerRegistry = PlayerRegistries[playerIndex];
                playerIndex++;

                if (!playerRegistry.IsAlive())
                {
                    continue;
                }

                var decision = playerRegistry.Player.Decide(_recorder.GetGameStatus(),
                    _recorder.GetCardStatus(playerRegistry.Player.Name));

                AbsorbDecision(decision);

                playerRegistry.Polled();

                if (AllPlayersAgreed(gameStage))
                {
                    break;
                }
                playerIndex %= PlayerRegistries.Count;
            }
        }

        private bool CheckEarlyFinish()
        {
            if (PlayerRegistries.Count(p => p.IsAlive()) == 1)
            {
                string winner = PlayerRegistries.First(p => p.IsAlive()).Player.Name;
                Judge(new List<string>() { winner });
                return true;
            }

            return false;
        }

        private void AbsorbDecision(PlayerDecision playerDecision)
        {
            var decisionPlayerRegistry = PlayerRegistries.First(r => string.Equals(r.Player.Name, playerDecision.PlayerName));

            switch (playerDecision.Decision.DecisionEnum)
            {
                case DecisionEnum.Check:
                    break;
                case DecisionEnum.Fold:
                    decisionPlayerRegistry.Fold();
                    break;
                case DecisionEnum.Raise:
                case DecisionEnum.Call:
                    decisionPlayerRegistry.Player.ChargeChips(-playerDecision.Decision.Chips);
                    break;
            }

            _recorder.RecordDecision(playerDecision);
        }

        public void DealHoles()
        {
            foreach (var playerRegistry in PlayerRegistries)
            {
                _recorder.RecordHolesDealt(playerRegistry.Player.Name, DrawCards(2));
            }

            _recorder.FinishStage(GameStage.HolesReceived);
        }

        public void DealFlops()
        {
            var flops = DrawCards(3);
            _recorder.RecordFlopsDealt(flops);

            _recorder.FinishStage(GameStage.FlopsSeen);
        }

        public void DealTurn()
        {
            var turn = DrawSingleCard();
            _recorder.RecordTurnDealt(turn);

            _recorder.FinishStage(GameStage.TurnSeen);
        }

        public void DealRiver()
        {
            var river = DrawSingleCard();
            _recorder.RecordRiverDealt(river);

            _recorder.FinishStage(GameStage.RiverSeen);
        }

        private void Judge(IEnumerable<string> winners)
        {
            int winnersCount = winners.Count();

            int prize = _recorder.GetGameStatus().PotSize / winnersCount;

            foreach (var playerRegistry in PlayerRegistries)
            {
                if (winners.Contains(playerRegistry.Player.Name))
                {
                    playerRegistry.Player.ChargeChips(prize);
                    _recorder.RecordPlayerPrize(playerRegistry.Player.Name, prize);
                }
                else
                {
                    _recorder.RecordPlayerPrize(playerRegistry.Player.Name, 0);
                }
            }
        }

        private IEnumerable<string> FindWinners()
        {
            var playerScores = new Dictionary<string, int>();

            foreach (var playerRegistry in PlayerRegistries)
            {
                var playerName = playerRegistry.Player.Name;
                var hand = _recorder.GetCardStatus(playerName);
                var cards = new List<Card>();
                cards.AddRange(hand.Holes);
                cards.AddRange(hand.Flops);
                cards.Add(hand.Turn);
                cards.Add(hand.River);
                var handOf7 = new HandOf7(cards);

                var handOf5 = handOf7.FindBestHandOf5();
                handOf5.Score = Utils.GetScoreFor5Cards(handOf5);
                playerScores.Add(playerName, handOf5.Score);

                _recorder.RecordBestHand(playerName, handOf5);
            }

            var bestScore = playerScores.Values.Max();
            var winners = playerScores.Where(p => p.Value == bestScore).Select(p => p.Key);

            return winners;
        }

        private List<Card> DrawCards(int count)
        {
            return _cardDrawer.DrawCards(count);
        }

        private Card DrawSingleCard()
        {
            return _cardDrawer.DrawSingleCard();
        }

        private int GetIndexOfPlayerToStartForStage(GameStage gameStage)
        {
            return 1;
        }

        private bool AllPlayersAgreed(GameStage gameStage)
        {
            return _recorder.AllPlayerAgreed();
        }
    }
}
