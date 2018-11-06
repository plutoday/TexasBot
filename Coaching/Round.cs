using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using log4net;
using Models;

namespace Coaching
{
    /// <summary>
    /// One round
    /// </summary>
    public class Round
    {
        private readonly string _roundId;
        

        public int BigBlindSize => 10;

        /// <summary>
        /// In the order of SB/BB/Under the gun/.../Button
        /// </summary>
        public List<Player> Players { get; set; }

        public int CurrentPotSize => Players.Sum(p => p.ChipsBetAlready);

        public Dictionary<StageEnum, int> MostChipsBetByRound { get; set; }

        public Player PreflopRaiser { get; set; }

        public Player CurrentRaiser { get; set; }
        
        public StageEnum StageEnum { get; set; }

        public Card Hole1 { get; set; }

        public Card Hole2 { get; set; }

        public Card Flop1 { get; set; }

        public Card Flop2 { get; set; }

        public Card Flop3 { get; set; }

        public Card Turn { get; set; }

        public Card River { get; set; }

        public List<Move> PreflopMoves { get; set; }
        public List<Move> FlopMoves { get; set; }
        public List<Move> TurnMoves { get; set; }
        public List<Move> RiverMoves { get; set; }

        public List<Move> AllMoves { get; set; }

        public bool IsRaised { get; private set; }

        public int MySeat { get; set; }
        public int ButtonSeat { get; set; }

        private int _index;

        private int NumOfPlayers => Players.Count;

        private readonly Input _input;

        public Round(int numOfPlayers, int mySeat, int buttonSeat, List<string> playerNames, List<int> playerStackSizes, Input input)
        {
            Models.Utils.LogStartNewRound();
            Models.Utils.Log($"{_roundId}|A new round started with {numOfPlayers} players.");
            MySeat = mySeat;
            ButtonSeat = buttonSeat;

            Players = new List<Player>();
            for (int i = 0; i < numOfPlayers; i++)
            {
                int seat = (buttonSeat + i + 1 + numOfPlayers) % numOfPlayers;
                var player = new Player(playerNames[seat], playerStackSizes[seat], seat,
                    GetPositionFromPositionIndex(seat, buttonSeat, numOfPlayers));
                Players.Add(player);
                Models.Utils.Log($"{_roundId}|{player.Name}|{player.Position}|seat:{seat}");
            }

            AllMoves = new List<Move>();
            PreflopMoves = new List<Move>();
            FlopMoves = new List<Move>();
            TurnMoves = new List<Move>();
            RiverMoves = new List<Move>();

            _index = 0;
            _input = input;

            MostChipsBetByRound = new Dictionary<StageEnum, int>()
            {
                { StageEnum.Preflop, 0},
                { StageEnum.Flop, 0},
                { StageEnum.Turn, 0},
                { StageEnum.River, 0}
            };
        }

        private PositionEnum GetPositionFromPositionIndex(int seat, int buttonSeat, int numOfPlayers)
        {
            int positionIndex = (seat - buttonSeat - 1 + numOfPlayers) % numOfPlayers;
            switch (positionIndex)
            {
                case 0:
                    return PositionEnum.SmallBlind;
                case 1:
                    return PositionEnum.BigBlind;
                case 2:
                    return PositionEnum.UnderTheGun;
                case 3:
                    return PositionEnum.MiddlePositioin;
                case 4:
                    return PositionEnum.CuttingOff;
                case 5:
                    return PositionEnum.Button;
                default:
                    throw new InvalidOperationException();
            }
        }

        public void Drive()
        {
            Player winner = null;

            StageEnum = StageEnum.Preflop;
            RecordHole1ForMe(_input.GetHole1());
            RecordHole2ForMe(_input.GetHole2());
            winner = PollPlayers();
            if (winner != null)
            {
                Models.Utils.Log($"{_roundId}|Game ended on preflop, and the winner is {winner.Position}-{winner.Name}, potSize={CurrentPotSize}");
                return;
            }

            StageEnum = StageEnum.Flop;

            var flop1 = _input.GetFlop1();
            RecordFlop1(flop1);
            var flop2 = _input.GetFlop2();
            RecordFlop2(flop2);
            var flop3 = _input.GetFlop3();
            RecordFlop3(flop3);
            winner = PollPlayers();
            if (winner != null)
            {
                Models.Utils.Log($"{_roundId}|Game ended on flop, and the winner is {winner.Position}-{winner.Name}, potSize={CurrentPotSize}");
                return;
            }

            StageEnum = StageEnum.Turn;
            var turn = _input.GetTurn();
            RecordTurn(turn);
            winner = PollPlayers();
            if (winner != null)
            {
                Models.Utils.Log($"{_roundId}|Game ended on turn, and the winner is {winner.Position}-{winner.Name}, potSize={CurrentPotSize}");
                return;
            }

            StageEnum = StageEnum.River;
            var river = _input.GetRiver();
            RecordRiver(river);
            winner = PollPlayers();
            if (winner != null)
            {
                Models.Utils.Log($"{_roundId}|Game ended on river, and the winner is {winner.Position}-{winner.Name}, potSize={CurrentPotSize}");
                return;
            }

            //todo Showdown, implement the comparison of hands
        }

        private void RecordHole1ForMe(Card hole1)
        {
            Models.Utils.Log($"{_roundId}|Hole1 Dealt:{hole1.Suit}-{hole1.Rank}");
            Hole1 = hole1;
        }

        private void RecordHole2ForMe(Card hole2)
        {
            Models.Utils.Log($"{_roundId}|Hole2 Dealt:{hole2.Suit}-{hole2.Rank}");
            Hole2 = hole2;
        }

        private Player PollPlayers()
        {
            Models.Utils.Log($"{_roundId}|Polling players for {StageEnum}");
            foreach (var player in Players.Where(p => p.IsAlive))
            {
                Models.Utils.Log($"{_roundId}|{player.Position}:{player.Name} still alive.");
                player.ResetPolled();
            }

            _index = 0;
            IsRaised = false;

            if (StageEnum == StageEnum.Preflop)
            {
                _index = 2;
                RecordMove(new Move(Players[0], new Decision(DecisionType.Ante, BigBlindSize / 2), StageEnum));
                RecordMove(new Move(Players[1], new Decision(DecisionType.Ante, BigBlindSize), StageEnum));
                IsRaised = true;
            }

            while (true)
            {
                var player = GetNextPlayer();

                var candidateDecisionTypes = GetCandidateDecisionTypes(IsRaised);
                int chipsToCall = MostChipsBetByRound[StageEnum] - player.ChipsBetByStage[StageEnum];
                var decision = IsMe(player) ? _input.GetMyDecision(this, candidateDecisionTypes)
                    : _input.GetDecision(player, CurrentRaiser, candidateDecisionTypes, chipsToCall);

                if (!candidateDecisionTypes.Contains(decision.DecisionType))
                {
                    throw new InvalidOperationException($"{decision.DecisionType} is out of the candidates {string.Join("/", candidateDecisionTypes)}");
                }

                var move = new Move(player, decision, StageEnum);
                RecordMove(move);
                if (move.Decision.DecisionType == DecisionType.Raise ||
                    move.Decision.DecisionType == DecisionType.Reraise)
                {
                    if (StageEnum == StageEnum.Preflop)
                    {
                        PreflopRaiser = move.Player;
                    }
                    CurrentRaiser = move.Player;
                }

                if (move.Decision.DecisionType == DecisionType.AllIn && move.Decision.ChipsAdded > chipsToCall)
                {
                    CurrentRaiser = move.Player;
                }

                if (decision.DecisionType != DecisionType.Check && decision.DecisionType != DecisionType.Fold)
                {
                    IsRaised = true;
                }

                if (IsSettled())
                {
                    break;
                }
            }

            if (Players.Count(p => p.IsAlive) == 1)
            {
                //Winner
                return Players.First(p => p.IsAlive);
            }

            //Still more than one alive players, game continues
            return null;
        }

        private HashSet<DecisionType> GetCandidateDecisionTypes(bool isRaised)
        {
            if (isRaised)
            {
                return new HashSet<DecisionType>() { DecisionType.Fold, DecisionType.Call, DecisionType.Reraise, DecisionType.AllIn };
            }
            else
            {
                return new HashSet<DecisionType>() { DecisionType.Fold, DecisionType.Check, DecisionType.Raise, DecisionType.AllIn };
            }
        }

        public bool IsSettled()
        {
            if (Players.Any(p => p.IsAlive && !p.Polled))
            {
                return false;
            }

            return true;
        }

        public void RecordMove(Move move)
        {
            Models.Utils.Log($"{_roundId}|{move.Player.Position}-{move.Player.Name} made a move:{move.Decision.DecisionType}-{move.Decision.ChipsAdded}");
            var decisionPlayer = Players.First(p => p.Equals(move.Player));

            if (move.Decision.DecisionType != DecisionType.Ante)
            {
                decisionPlayer.SetPolled();
            }

            decisionPlayer.ChipsBetByStage[StageEnum] += move.Decision.ChipsAdded;

            MostChipsBetByRound[StageEnum] = Players.Max(p => p.ChipsBetByStage[StageEnum]);

            switch (move.Decision.DecisionType)
            {
                case DecisionType.Raise:
                case DecisionType.Reraise:
                    foreach (var player in Players)
                    {
                        if (!player.Equals(decisionPlayer))
                        {
                            player.ResetPolled();
                        }
                    }
                    break;
                case DecisionType.Fold:
                    decisionPlayer.Fold();
                    break;
                case DecisionType.AllIn:
                    decisionPlayer.AllIn();
                    break;
            }

            AllMoves.Add(move);

            switch (StageEnum)
            {
                case StageEnum.Preflop:
                    PreflopMoves.Add(move);
                    break;
                case StageEnum.Flop:
                    FlopMoves.Add(move);
                    break;
                case StageEnum.Turn:
                    TurnMoves.Add(move);
                    break;
                case StageEnum.River:
                    RiverMoves.Add(move);
                    break;
                default:
                    throw new InvalidOleVariantTypeException($"Move not accepted in {StageEnum}");
            }
        }

        public void RecordFlop1(Card flop1)
        {
            Flop1 = flop1;
        }
        public void RecordFlop2(Card flop2)
        {
            Flop2 = flop2;
        }
        public void RecordFlop3(Card flop3)
        {
            Flop3 = flop3;
        }
        public void RecordTurn(Card turn)
        {
            Turn = turn;
        }
        public void RecordRiver(Card river)
        {
            River = river;
        }

        public Player GetNextPlayer()
        {
            while (true)
            {
                _index %= NumOfPlayers;
                var player = Players[_index++];
                if (player.IsAlive)
                {
                    return player;
                }
            }
        }

        public Player GetCurrentPlayer()
        {
            return Players[_index];
        }

        private int GetPositionOfPlayer(Player player)
        {
            int index = Players.IndexOf(player);
            return (index - (ButtonSeat + 1)) % Players.Count;
        }

        public bool IsMe(Player player)
        {
            return player.Index == MySeat;
        }
    }
}
