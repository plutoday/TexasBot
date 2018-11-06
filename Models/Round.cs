using System;
using System.Collections.Generic;
using System.Linq;

namespace Models
{
    /// <summary>
    /// One round
    /// </summary>
    public class Round
    {
        public readonly Guid RoundId;

        public int BigBlindSize { get; set; }
        public int SmallBlindSize { get; set; }

        /// <summary>
        /// In the order of SB/BB/Under the gun/.../Button
        /// </summary>
        public List<Player> Players { get; set; }

        public int CurrentPotSize => Players.Sum(p => p.ChipsBetAlready);

        public Dictionary<StageEnum, int> MostChipsBetByRound { get; set; }

        public Player PreflopRaiser { get; set; }

        public Player CurrentRaiser { get; set; }

        public StageEnum StageEnum { get; set; }

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

        public bool IsRaised { get; set; }
        public int ButtonSeat { get; set; }

        public int Index { get; set; }

        public int UnpolledPlayersCount => Players.Count(p => p.PlayerStatus == PlayerStatusEnum.Unpolled);

        private int NumOfPlayers => Players.Count;

        public Round(RoundInput input) : this(input.NumOfPlayers, input.ButtonIndex, input.PlayerNames, input.PlayerStackSizes, input.SmallBlindSize, input.BigBlindSize) { }

        private Round(int numOfPlayers, int buttonSeat, List<string> playerNames,
            List<int> playerStackSizes, int smallBlindSize, int bigBlindSize)
        {
            SmallBlindSize = smallBlindSize;
            BigBlindSize = bigBlindSize;
            RoundId = Guid.NewGuid();
            
            ButtonSeat = buttonSeat;

            Players = new List<Player>();
            for (int i = 0; i < numOfPlayers; i++)
            {
                var player = new Player(playerNames[i], playerStackSizes[i], i,
                    GetPositionFromPositionIndex(i, buttonSeat, numOfPlayers));
                Players.Add(player);
            }

            AllMoves = new List<Move>();
            PreflopMoves = new List<Move>();
            FlopMoves = new List<Move>();
            TurnMoves = new List<Move>();
            RiverMoves = new List<Move>();

            Index = (buttonSeat + 1) % numOfPlayers;

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
                    return PositionEnum.MiddlePosition;
                case 4:
                    return PositionEnum.CuttingOff;
                case 5:
                    return PositionEnum.Button;
                default:
                    throw new InvalidOperationException();
            }
        }

        public Player GetNextPlayer()
        {
            while (true)
            {
                Index %= NumOfPlayers;
                var player = Players[Index++];
                if (player.PlayerStatus == PlayerStatusEnum.Unpolled)
                {
                    return player;
                }
            }
        }

        public Player GetCurrentPlayer()
        {
            return Players[Index];
        }

        public void MoveToNextPlayer()
        {
            Index++;
            Index %= NumOfPlayers;
            while (true)
            {
                if (Players[Index].PlayerStatus != PlayerStatusEnum.Folded && Players[Index].PlayerStatus != PlayerStatusEnum.AllIned)
                {
                    break;
                }
                Index++;
                Index %= NumOfPlayers;
            }
        }

        public void MoveToNextStage()
        {
            StageEnum++;
            ResetForNewStage();
            Index = ButtonSeat;
            MoveToNextPlayer();
        }

        private void ResetForNewStage()
        {
            foreach (var player in Players)
            {
                player.ResetPolled();
            }
        }

        public bool IsSettled => UnpolledPlayersCount == 0;

        public void RecordMove(Move move)
        {
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
                    throw new InvalidOperationException($"Move not accepted in {StageEnum}");
            }

            if (move.Decision.DecisionType == DecisionType.Raise ||
                move.Decision.DecisionType == DecisionType.Reraise)
            {
                if (StageEnum == StageEnum.Preflop)
                {
                    PreflopRaiser = move.Player;
                }
                CurrentRaiser = move.Player;
            }

            int chipsToCall = MostChipsBetByRound[StageEnum] - move.Player.ChipsBetByStage[StageEnum];

            if (move.Decision.DecisionType == DecisionType.AllIn && move.Decision.ChipsAdded > chipsToCall)
            {
                CurrentRaiser = move.Player;
            }

            if (move.Decision.DecisionType != DecisionType.Check && move.Decision.DecisionType != DecisionType.Fold)
            {
                IsRaised = true;
            }
        }
    }

    public class RoundInput
    {
        public int NumOfPlayers { get; set; }
        public int ButtonIndex { get; set; }
        public List<string> PlayerNames { get; set; } 
        public List<int> PlayerStackSizes { get; set; } 
        public int SmallBlindSize { get; set; }
        public int BigBlindSize { get; set; }

        public RoundInput(int numOfPlayers, int buttonSeat, List<string> playerNames,
            List<int> playerStackSizes, int smallBlindSize, int bigBlindSize)
        {
            NumOfPlayers = numOfPlayers;
            ButtonIndex = buttonSeat;
            PlayerNames = new List<string>(playerNames);
            PlayerStackSizes = new List<int>(playerStackSizes);
            SmallBlindSize = smallBlindSize;
            BigBlindSize = bigBlindSize;
        }
    }
}
