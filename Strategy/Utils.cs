using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Flop;
using Models;
using Preflop;
using River;
using Turn;

namespace Strategy
{
    public static class Utils
    {
        public static PreflopStatusSummary GeneratePreflopStatusSummary(Round round, int heroIndex)
        {
            var statusSummary = new PreflopStatusSummary();

            foreach (var player in round.Players)
            {
                var playerStatus = new PreflopPlayerSummary()
                {
                    Name = player.Name,
                    Decisions = round.PreflopMoves.Where(m => m.Player.Name.Equals(player.Name)).Select(m => m.Decision).ToList(),
                    Position = player.Position,
                    StackSize = player.StackSize,
                };

                statusSummary.Players.Add(playerStatus);
            }

            statusSummary.PreflopRaiseMoves.AddRange(round.PreflopMoves.Where(m => m.Decision.DecisionType.IsRaiseMove() 
                && m.Decision.DecisionType != DecisionType.Ante));
            /*
            statusSummary.SmallBlind = statusSummary.Players.First(p => p.Position == PositionEnum.SmallBlind);
            statusSummary.BigBlind = statusSummary.Players.First(p => p.Position == PositionEnum.BigBlind);
            statusSummary.UnderTheGun = statusSummary.Players.First(p => p.Position == PositionEnum.UnderTheGun);
            statusSummary.MiddlePosition = statusSummary.Players.First(p => p.Position == PositionEnum.MiddlePosition);
            statusSummary.CuttingOff = statusSummary.Players.First(p => p.Position == PositionEnum.CuttingOff);
            statusSummary.Button = statusSummary.Players.First(p => p.Position == PositionEnum.Button);
            */

            var mePlayer = round.Players.First(p => p.Index == heroIndex);
            statusSummary.Me = statusSummary.Players.First(p => p.Name.Equals(mePlayer.Name));

            statusSummary.Status = GetPreflopGameStatus(round);
            statusSummary.IsRaised = round.IsRaised;

            var chipsRaised = round.Players.Max(p => p.ChipsBetByStage[StageEnum.Preflop]);

            statusSummary.ChipsToCall = chipsRaised - GetChipsBetByPlayerThisRound(mePlayer, round);

            statusSummary.BigBlindSize = round.BigBlindSize;
            statusSummary.PotSize = round.CurrentPotSize;

            if (round.CurrentRaiser != null)
            {
                statusSummary.CurrentRaiser = statusSummary.Players.First(p =>
                    p.Position == round.CurrentRaiser.Position
                    && string.Equals(p.Name, round.CurrentRaiser.Name));
            }

            return statusSummary;
        }

        private static PreflopGameStatusEnum GetPreflopGameStatus(Round round)
        {
            var moves = round.PreflopMoves.Where(m => m.Decision.DecisionType != DecisionType.Ante).ToList();
            if (!moves.Any() || moves.All(m => m.Decision.DecisionType == DecisionType.Fold))
            {
                return PreflopGameStatusEnum.FoldedToMe;
            }

            if (
                moves.All(
                    m => (
                    m.Decision.DecisionType == DecisionType.Fold
                    || (m.Decision.DecisionType == DecisionType.Check && m.Player.Position == PositionEnum.BigBlind) //possible for bb
                    || m.Decision.DecisionType == DecisionType.Call)))
            {
                return PreflopGameStatusEnum.LimpedPot;
            }

            var raiseMoves = moves.Where(m => m.Decision.DecisionType == DecisionType.Raise
                                              || m.Decision.DecisionType == DecisionType.Reraise
                                              || m.Decision.DecisionType == DecisionType.AllIn);

            var lastRaise = raiseMoves.Last();

            return GetRaiseStageBasedOnTheRaisedAmount(lastRaise.Decision.ChipsAdded);
        }
        private static PreflopGameStatusEnum GetRaiseStageBasedOnTheRaisedAmount(int chipsToCall)
        {
            //todo : implement this method
            return PreflopGameStatusEnum.Raised;
        }

        private static int GetChipsBetByPlayerThisRound(Player player, Round round)
        {
            int chipsBetByMeThisRound;
            switch (round.StageEnum)
            {
                case StageEnum.Preflop:
                    chipsBetByMeThisRound = player.ChipsBetByStage[StageEnum.Preflop];
                    break;
                case StageEnum.Flop:
                    chipsBetByMeThisRound = player.ChipsBetByStage[StageEnum.Flop];
                    break;
                case StageEnum.Turn:
                    chipsBetByMeThisRound = player.ChipsBetByStage[StageEnum.Turn];
                    break;
                case StageEnum.River:
                    chipsBetByMeThisRound = player.ChipsBetByStage[StageEnum.River];
                    break;
                default:
                    throw new InvalidOperationException();
            }

            return chipsBetByMeThisRound;
        }

        public static FlopDecisionContext GenerateFlopDecisionContext(Round round, List<PlayerRoundProfile> playerProfiles, RoundSetup roundSetup)
        {
            var hero = round.Players.First(p => p.Index == roundSetup.HeroIndex);
            var context = new FlopDecisionContext()
            {
                BigBlindSize = round.BigBlindSize,
                CurrentPotSize = round.CurrentPotSize,
                FlopBoard = new FlopBoard(round.Flop1, round.Flop2, round.Flop3),
                Players = playerProfiles,
                HeroName = hero.Name,
                IsHeadsUp = round.Players.Count(p => p.IsAlive) == 2,
                HeroHoles = new HoldingHoles(roundSetup.Hole1, roundSetup.Hole2),
                PreflopRaiserName = round.PreflopRaiser.Name,
                FlopRaiserName = round.FlopMoves.LastOrDefault(m => m.Decision.DecisionType.IsRaiseMove())?.Player.Name,
                IsRaised = round.FlopMoves.Any(m => m.Decision.DecisionType.IsRaiseMove()),
            };

            if (context.IsHeadsUp)
            {
                context.HeadsUpVillainName = round.Players.First(p => p.IsAlive && p.Index != roundSetup.HeroIndex).Name;
            }

            return context;
        }

        public static TurnDecisionContext GenerateTurnDecisionContext(Round round, List<PlayerRoundProfile> playerProfiles, RoundSetup roundSetup)
        {
            var hero = round.Players.First(p => p.Index == roundSetup.HeroIndex);
            var context = new TurnDecisionContext()
            {
                BigBlindSize = round.BigBlindSize,
                CurrentPotSize = round.CurrentPotSize,
                TurnBoard = new TurnBoard(new FlopBoard(round.Flop1, round.Flop2, round.Flop3), round.Turn),
                Players = playerProfiles,
                HeroName = hero.Name,
                IsHeadsUp = round.Players.Count(p => p.IsAlive) == 2,
                HeroHoles = new HoldingHoles(roundSetup.Hole1, roundSetup.Hole2),
                PreflopRaiserName = round.PreflopRaiser.Name,
                FlopRaiserName = round.FlopMoves.LastOrDefault(m => m.Decision.DecisionType.IsRaiseMove())?.Player.Name,
                TurnRaiserName = round.TurnMoves.LastOrDefault(m => m.Decision.DecisionType.IsRaiseMove())?.Player.Name,
                IsRaised = round.TurnMoves.Any(m => m.Decision.DecisionType.IsRaiseMove()),
            };

            if (context.IsHeadsUp)
            {
                context.HeadsUpVillainName = round.Players.First(p => p.IsAlive && p.Index != roundSetup.HeroIndex).Name;
            }

            return context;
        }

        public static RiverDecisionContext GenerateRiverDecisionContext(Round round, List<PlayerRoundProfile> playerProfiles, RoundSetup roundSetup)
        {
            var hero = round.Players.First(p => p.Index == roundSetup.HeroIndex);
            var context = new RiverDecisionContext()
            {
                BigBlindSize = round.BigBlindSize,
                CurrentPotSize = round.CurrentPotSize,
                RiverBoard = new RiverBoard(new TurnBoard(new FlopBoard(round.Flop1, round.Flop2, round.Flop3), round.Turn), round.River),
                Players = playerProfiles,
                HeroName = hero.Name,
                IsHeadsUp = round.Players.Count(p => p.IsAlive) == 2,
                HeroHoles = new HoldingHoles(roundSetup.Hole1, roundSetup.Hole2),
                PreflopRaiserName = round.PreflopRaiser.Name,
                FlopRaiserName = round.FlopMoves.LastOrDefault(m => m.Decision.DecisionType.IsRaiseMove())?.Player.Name,
                TurnRaiserName = round.TurnMoves.LastOrDefault(m => m.Decision.DecisionType.IsRaiseMove())?.Player.Name,
                RiverRaiserName = round.RiverMoves.LastOrDefault(m => m.Decision.DecisionType.IsRaiseMove())?.Player.Name,
                IsRaised = round.RiverMoves.Any(m => m.Decision.DecisionType.IsRaiseMove()),
            };

            if (context.IsHeadsUp)
            {
                context.HeadsUpVillainName = round.Players.First(p => p.IsAlive && p.Index != roundSetup.HeroIndex).Name;
            }

            return context;
        }
    }
}
