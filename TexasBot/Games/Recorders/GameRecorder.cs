using System;
using System.Collections.Generic;
using TexasBot.Games.Players;
using TexasBot.Games.Reporters;
using TexasBot.Models;

namespace TexasBot.Games.Recorders
{
    public class GameRecorder : IGameRecorder
    {
        private GameStatus _gameStatus;
        private readonly IGameReporter _gameReporter;
        private List<Card> _flops = new List<Card>();
        private Card _turn;
        private Card _river;
        public Dictionary<string, List<Card>> _playerHoles = new Dictionary<string, List<Card>>();

        public GameRecorder(IGameReporter gameReporter)
        {
            _gameReporter = gameReporter;
        }

        public bool AllPlayerAgreed()
        {
            return _gameStatus.StageSettled();
        }

        public void StartNewGame(List<string> playerNames)
        {
            _gameStatus = new GameStatus()
            {
                GameStage = GameStage.Initialized,
                PlayerRecords = new List<PlayerRecord>(),
                HolesDecisions = new List<PlayerDecision>(),
                FlopsDecisions = new List<PlayerDecision>(),
                TurnDecisions = new List<PlayerDecision>(),
                RiverDecisions = new List<PlayerDecision>()
            };

            foreach (var playerName in playerNames)
            {
                _gameStatus.AddPlayer(playerName);
            }

            _flops = new List<Card>();
            _turn = null;
            _river = null;
        }

        public void RecordDecision(PlayerDecision playerDecision)
        {
            _gameStatus.RecordDecision(playerDecision);
            _gameReporter.ReportDecision(playerDecision);
        }

        public void FinishStage(GameStage gameStage)
        {
            _gameStatus.FinishStage();
        }

        public void RecordFlopsDealt(List<Card> flops)
        {
            foreach (var flop in flops)
            {
                _flops.Add(flop);
            }

            _gameReporter.ReportFlopsDealt(flops);
        }

        public void RecordTurnDealt(Card turn)
        {
            _turn = turn;

            _gameReporter.ReportTurnDealt(turn);
        }

        public void RecordRiverDealt(Card river)
        {
            _river = river;

            _gameReporter.ReportRiverDealt(river);
        }

        public void RecordHolesDealt(string playerName, List<Card> holes)
        {
            _playerHoles[playerName] = new List<Card>(holes);
            _gameReporter.ReportHolesDealt(playerName, holes);
        }

        public GameStatus GetGameStatus()
        {
            return _gameStatus;
        }

        public CardStatus GetCardStatus(string playerName)
        {
            return new CardStatus()
            {
                Holes = _playerHoles[playerName],
                Flops = new List<Card>(_flops),
                Turn = _turn,
                River = _river
            };
        }

        public void RecordBestHand(string playerName, HandOf5 bestHand)
        {
            _gameReporter.ShowBestHand(playerName, bestHand);
        }

        public void RecordPlayerPrize(string playerName, int outcome)
        {
            _gameReporter.ReportGainAndLoss(playerName, outcome - _gameStatus.FindPlayerRecord(playerName).ChipsBet);
        }
    }
}
