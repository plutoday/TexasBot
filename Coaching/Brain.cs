using System;
using System.Collections.Generic;
using System.Linq;
using Coaching.Postflop;
using Flop;
using Models;
using Models.Ranging;
using Preflop;
using River;
using River.Strategy;
using Turn;
using Turn.Strategy;

namespace Coaching
{
    public class Brain
    {
        public PreflopRangeSqueezer PreflopRangeSqueezer = new PreflopRangeSqueezer();
        public PreflopExpert PreflopExpert = new PreflopExpert();
        public FlopRangeSqueezer FlopRangeSqueezer = new FlopRangeSqueezer();
        public TurnRangeSqueezer TurnRangeSqueezer = new TurnRangeSqueezer();
        public RiverRangeSqueezer RiverRangeSqueezer = new RiverRangeSqueezer();
        public PostflopExpert PostflopExpert = new PostflopExpert();
        public FlopStrategy FlopStrategy = new FlopStrategy();
        public TurnStrategy TurnStrategy = new TurnStrategy();
        public RiverStrategy RiverStrategy = new RiverStrategy();

        public Dictionary<string, PlayerRoundProfile> PlayerProfiles = new Dictionary<string, PlayerRoundProfile>();

        public Decision GetDecision(Round round)
        {
            UpdatePlayerProfiles(round);

            switch (round.StageEnum)
            {
                case StageEnum.Preflop:
                    return PreflopExpert.GetPreflopDecision(Utils.GeneratePreflopStatusSummary(round), new HoldingHoles(round.Hole1, round.Hole2));
                case StageEnum.Flop:
                    return FlopStrategy.MakeDecision(Utils.GenerateFlopDecisionContext(round, PlayerProfiles.Values.ToList()));
                case StageEnum.Turn:
                    return
                        TurnStrategy.MakeDecision(Utils.GenerateTurnDecisionContext(round,
                            PlayerProfiles.Values.ToList()));
                case StageEnum.River:
                    return
                        RiverStrategy.MakeDecision(Utils.GenerateRiverDecisionContext(round,
                            PlayerProfiles.Values.ToList()));
                default:
                    throw new InvalidOperationException();
            }
        }

        private void UpdatePlayerProfiles(Round round)
        {
            foreach (var player in IterateOpponentsInOrder(round))
            {
                if (!PlayerProfiles.ContainsKey(player.Name))
                {
                    PlayerProfiles.Add(player.Name, GenerateNewProfile(player, round.Players.First(round.IsMe).Position));
                }
                UpdateProfile(PlayerProfiles[player.Name], player, round);
            }
        }

        /// <summary>
        /// Iterate the players since hero, round up the ring, until hero is reached again.
        /// </summary>
        /// <param name="round"></param>
        /// <returns></returns>
        private IEnumerable<Player> IterateOpponentsInOrder(Round round)
        {
            int index = round.Players.FindIndex(round.IsMe);
            for (int i = index + 1; ; i++)
            {
                yield return round.Players[i % round.Players.Count];
                if (i % round.Players.Count == index)
                {
                    yield break;
                }
            }
        }

        private PlayerRoundProfile GenerateNewProfile(Player player, PositionEnum heroPosition)
        {
            var profile = new PlayerRoundProfile()
            {
                Name = player.Name,
                Position = player.Position,
                IsAlive = true,
                IsAllIn = false,
                IsHero = player.Position == heroPosition
            };

            if (!profile.IsHero)
            {
                profile.InPositionAgainstHero = profile.Position > heroPosition;
            }

            var range = new PlayerRange();
            range.Init(PlayerRangeGridStatusEnum.Included);

            profile.PlayerRange = range;
            profile.PreflopRange = range;

            return profile;
        }

        private void UpdateProfile(PlayerRoundProfile profile, Player player, Round round)
        {
            if (profile.IsAlive == false || profile.IsAllIn)
            {
                //Unnecessary to update
                return;
            }

            profile.IsAlive = player.IsAlive;
            profile.IsAllIn = player.IsAllIn;
            profile.StackSize = player.StackSize;

            switch (round.StageEnum)
            {
                case StageEnum.Preflop:
                    profile.PreflopDecisions = new List<Decision>(round.PreflopMoves.Where(m => string.Equals(m.Player.Name, player.Name)).Select(m => m.Decision));
                    profile.PreflopBet = player.ChipsBetByStage[StageEnum.Preflop];
                    profile.PreflopPlayerStatus = GetPlayerStreetStatus(profile.PreflopDecisions);
                    var preflopLastRaiserName =
                        round.PreflopMoves.Last(m => m.Decision.DecisionType.IsRaiseMove()).Player.Name;
                    profile.IsPreflopRaiser = string.Equals(preflopLastRaiserName, profile.Name);
                    break;
                case StageEnum.Flop:
                    profile.FlopDecisions = new List<Decision>(round.FlopMoves.Where(m => string.Equals(m.Player.Name, player.Name)).Select(m => m.Decision));
                    profile.FlopBet = player.ChipsBetByStage[StageEnum.Flop];
                    profile.FlopPlayerStatus = GetPlayerStreetStatus(profile.FlopDecisions);
                    var flopLastRaiserName =
                        round.FlopMoves.LastOrDefault(m => m.Decision.DecisionType.IsRaiseMove())?.Player.Name;
                    profile.IsFlopRaiser = string.Equals(flopLastRaiserName, profile.Name);
                    break;
                case StageEnum.Turn:
                    profile.TurnDecisions = new List<Decision>(round.TurnMoves.Where(m => string.Equals(m.Player.Name, player.Name)).Select(m => m.Decision));
                    profile.TurnBet = player.ChipsBetByStage[StageEnum.Turn];
                    profile.TurnPlayerStatus = GetPlayerStreetStatus(profile.TurnDecisions);
                    var turnLastRaiserName =
                        round.TurnMoves.LastOrDefault(m => m.Decision.DecisionType.IsRaiseMove())?.Player.Name;
                    profile.IsTurnRaiser = string.Equals(turnLastRaiserName, profile.Name);
                    break;
                case StageEnum.River:
                    profile.RiverDecisions = new List<Decision>(round.RiverMoves.Where(m => string.Equals(m.Player.Name, player.Name)).Select(m => m.Decision));
                    profile.RiverBet = player.ChipsBetByStage[StageEnum.River];
                    profile.RiverPlayerStatus = GetPlayerStreetStatus(profile.RiverDecisions);
                    var riverLastRaiserName =
                        round.RiverMoves.LastOrDefault(m => m.Decision.DecisionType.IsRaiseMove())?.Player.Name;
                    profile.IsRiverRaiser = string.Equals(riverLastRaiserName, profile.Name);
                    break;
            }

            if (profile.IsHero)
            {
                //unnecessary to squeeze hero's range
                return;
            }

            if (round.AllMoves.Any(m => string.Equals(m.Player.Name, player.Name)))
            {
                Move lastMove = round.AllMoves.LastOrDefault(m => string.Equals(m.Player.Name, player.Name));

                if ( lastMove != null && lastMove.Decision.DecisionType != DecisionType.Ante
                    && lastMove.Decision.DecisionType != DecisionType.Fold)
                {
                    //squeeze player's range according to his last move
                    switch (lastMove.Stage)
                    {
                        case StageEnum.Preflop:
                            SqueezePreflopRange(round, profile, lastMove);
                            break;
                        case StageEnum.Flop:
                            SqueezeFlopRange(round, profile, lastMove);
                            break;
                        case StageEnum.Turn:
                            SqueezeTurnRange(round, profile, lastMove);
                            break;
                        case StageEnum.River:
                            SqueezeRiverRange(round, profile, lastMove);
                            break;
                    }
                }
            }
        }

        private void SqueezePreflopRange(Round round, PlayerRoundProfile profile, Move lastMove)
        {
            var newRange = PreflopRangeSqueezer.Squeeze(profile.PlayerRange, lastMove, round.BigBlindSize);
            switch (round.StageEnum)
            {
                case StageEnum.Preflop:
                    profile.PreflopRange = newRange;
                    break;
                case StageEnum.Flop:
                    profile.FlopRange = newRange;
                    break;
                default:
                    throw new InvalidOperationException($"Should not happen with {round.StageEnum}");
            }
        }

        private void SqueezeFlopRange(Round round, PlayerRoundProfile profile, Move lastMove)
        {
            var newRange = FlopRangeSqueezer.Squeeze(profile.PlayerRange, lastMove, round.BigBlindSize, new FlopBoard(round.Flop1, round.Flop2, round.Flop3), 
                new HoldingHoles(round.Hole1, round.Hole2));
            switch (round.StageEnum)
            {
                case StageEnum.Flop:
                    profile.FlopRange = newRange;
                    break;
                case StageEnum.Turn:
                    profile.TurnRange = newRange;
                    break;
                default:
                    throw new InvalidOperationException($"Should not happen with {round.StageEnum}");
            }
            profile.PlayerRange = newRange;
        }

        private void SqueezeTurnRange(Round round, PlayerRoundProfile profile, Move lastMove)
        {
            var newRange = TurnRangeSqueezer.Squeeze(profile.PlayerRange, lastMove, round.BigBlindSize,
                new TurnBoard(new FlopBoard(round.Flop1, round.Flop2, round.Flop3), round.Turn), new HoldingHoles(round.Hole1, round.Hole2));
            switch (round.StageEnum)
            {
                    case StageEnum.Turn:
                    profile.TurnRange = newRange;
                    break;
                    case StageEnum.River:
                    profile.RiverRange = newRange;
                    break;
                default:
                    throw new InvalidOperationException($"Should not happen with {round.StageEnum}");
            }
            profile.PlayerRange = newRange;
        }

        private void SqueezeRiverRange(Round round, PlayerRoundProfile profile, Move lastMove)
        {
            var newRange = RiverRangeSqueezer.Squeeze(profile.PlayerRange, lastMove, round.BigBlindSize,
                new RiverBoard(new TurnBoard(new FlopBoard(round.Flop1, round.Flop2, round.Flop3), round.Turn), round.River), new HoldingHoles(round.Hole1, round.Hole2));
            switch (round.StageEnum)
            {
                case StageEnum.Turn:
                    profile.TurnRange = newRange;
                    break;
                case StageEnum.River:
                    profile.RiverRange = newRange;
                    break;
                default:
                    throw new InvalidOperationException($"Should not happen with {round.StageEnum}");
            }
            profile.PlayerRange = newRange;
        }

        private PlayerStreetStatusEnum GetPlayerStreetStatus(List<Decision> streetDecisions)
        {
            if (!streetDecisions.Any())
            {
                return PlayerStreetStatusEnum.NotPolledYet;
            }

            if (
                streetDecisions.Any(
                    d => d.DecisionType == DecisionType.AllIn || d.DecisionType == DecisionType.AllInRaise))
            {
                return PlayerStreetStatusEnum.AllIn;
            }

            if (streetDecisions.Any(d => d.DecisionType == DecisionType.Reraise))
            {
                return PlayerStreetStatusEnum.Reraise;
            }

            if (streetDecisions.Any(d => d.DecisionType == DecisionType.Raise))
            {
                return PlayerStreetStatusEnum.Raise;
            }

            if (streetDecisions.Any(d => d.DecisionType == DecisionType.Call))
            {
                return PlayerStreetStatusEnum.Call;
            }

            if (streetDecisions.All(d => d.DecisionType == DecisionType.Check))
            {
                return PlayerStreetStatusEnum.Check;
            }

            if (streetDecisions.Any(d => d.DecisionType == DecisionType.Fold))
            {
                return PlayerStreetStatusEnum.Fold;
            }

            if (streetDecisions.All(d => d.DecisionType == DecisionType.Ante))
            {
                return PlayerStreetStatusEnum.NotPolledYet;
            }

            throw new InvalidOperationException("Should never reach here!");
        }
    }
}
