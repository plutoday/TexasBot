using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Coaching.Postflop;
using Coaching.Postflop.Boards;
using Flop;
using log4net;
using Models;
using Preflop;
using River;
using Turn;

namespace Coaching
{
    public static class Utils
    {
        public static PreflopStatusSummary GeneratePreflopStatusSummary(Round round)
        {
            var statusSummary = new PreflopStatusSummary();

            foreach (var player in round.Players)
            {
                var playerStatus = new PreflopPlayerSummary()
                {
                    Name = player.Name,
                    Decisions = round.PreflopMoves.Where(m => m.Player.Name.Equals(player.Name)).Select(m => m.Decision).ToList(),
                    IsAlive = player.IsAlive,
                    Polled = player.Polled,
                    Position = player.Position,
                    StackSize = player.StackSize,
                };

                statusSummary.Players.Add(playerStatus);
            }

            statusSummary.SmallBlind = statusSummary.Players.First(p => p.Position == PositionEnum.SmallBlind);
            statusSummary.BigBlind = statusSummary.Players.First(p => p.Position == PositionEnum.BigBlind);
            statusSummary.UnderTheGun = statusSummary.Players.First(p => p.Position == PositionEnum.UnderTheGun);
            statusSummary.MiddlePosition = statusSummary.Players.First(p => p.Position == PositionEnum.MiddlePositioin);
            statusSummary.CuttingOff = statusSummary.Players.First(p => p.Position == PositionEnum.CuttingOff);
            statusSummary.Button = statusSummary.Players.First(p => p.Position == PositionEnum.Button);

            var mePlayer = round.Players.First(round.IsMe);
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
            throw new NotImplementedException();
        }

        public static PostflopStatusSummary GeneratePostflopStatusSummary(Round round)
        {
            var statusSummary = new PostflopStatusSummary();
            statusSummary.BoardStatus = new BoardStatus(round.Flop1, round.Flop2, round.Flop3, round.Turn, round.River);
            var mePlayer = round.Players.First(round.IsMe);
            statusSummary.Me = GetPostflopPlayerSummary(round, mePlayer);
            statusSummary.AliveVillains =
                round.Players.Where(p => !round.IsMe(p)).Select(p => GetPostflopPlayerSummary(round, p)).Where(p => p.IsAlive).ToList();
            statusSummary.IsRaised = round.IsRaised;
            if (statusSummary.IsRaised)
            {
                string raiserName;
                switch (round.StageEnum)
                {
                    case StageEnum.Flop:
                        raiserName = round.FlopMoves.Last(m => m.Decision.DecisionType.IsRaiseMove()).Player.Name;
                        break;
                    case StageEnum.Turn:
                        raiserName = round.TurnMoves.Last(m => m.Decision.DecisionType.IsRaiseMove()).Player.Name;
                        break;
                    case StageEnum.River:
                        raiserName = round.RiverMoves.Last(m => m.Decision.DecisionType.IsRaiseMove()).Player.Name;
                        break;

                    default:
                        throw new InvalidOperationException($"{round.StageEnum} is not implemented");

                }
                statusSummary.Raiser = statusSummary.AliveVillains.First(v => string.Equals(v.Name, raiserName));
            }

            var chipsRaised = round.Players.Max(p => p.ChipsBetByStage[round.StageEnum]);
            statusSummary.ChipsToCall = chipsRaised - GetChipsBetByPlayerThisRound(mePlayer, round);
            statusSummary.PotSize = round.CurrentPotSize;
            statusSummary.PreflopRaiserPosition = round.PreflopRaiser.Position;
            statusSummary.BigBlindSize = round.BigBlindSize;

            return statusSummary;
        }

        private static PostflopPlayerSummary GetPostflopPlayerSummary(Round round, Player player)
        {
            var playerSummary = new PostflopPlayerSummary
            {
                Name = player.Name,
                Position = player.Position,
                Holes = new HoldingHoles(round.Hole1, round.Hole2),
                ChipsBet = player.ChipsBetAlready,
                IsMe = round.IsMe(player),
                IsAlive = player.IsAlive,
                PreflopDecisions =
                    round.PreflopMoves.Where(m => m.Player.Equals(player)).Select(m => m.Decision).ToList(),
                FlopDecisions = round.FlopMoves.Where(m => m.Player.Equals(player)).Select(m => m.Decision).ToList(),
                TurnDecisions = round.TurnMoves.Where(m => m.Player.Equals(player)).Select(m => m.Decision).ToList(),
                RiverDecisions = round.RiverMoves.Where(m => m.Player.Equals(player)).Select(m => m.Decision).ToList(),
                StackSize = player.StackSize,
                Tag = ""
            };

            return playerSummary;
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

        public static string GetDescriptorForSevenCardsHand(SevenCardsHand sevenCardsHand)
        {
            SuitEnum targetSuit = SuitEnum.Heart;
            if (sevenCardsHand.Cards.Count(c => c.Suit == targetSuit) >= 5)
            {
                var suitedCards = sevenCardsHand.Cards.Where(c => c.Suit == targetSuit).ToList();
                suitedCards.Sort((c1, c2) => -c1.Rank.CompareTo(c2.Rank));
                var offsuitCards = sevenCardsHand.Cards.Where(c => c.Suit != targetSuit).ToList();
                offsuitCards.Sort((c1, c2) => -c1.Rank.CompareTo(c2.Rank));
                return GetDescriptorForSevenCardsHandSutied(suitedCards, targetSuit, offsuitCards);
            }

            targetSuit = SuitEnum.Spade;
            if (sevenCardsHand.Cards.Count(c => c.Suit == targetSuit) >= 5)
            {
                var suitedCards = sevenCardsHand.Cards.Where(c => c.Suit == targetSuit).ToList();
                suitedCards.Sort((c1, c2) => -c1.Rank.CompareTo(c2.Rank));
                var offsuitCards = sevenCardsHand.Cards.Where(c => c.Suit != targetSuit).ToList();
                offsuitCards.Sort((c1, c2) => -c1.Rank.CompareTo(c2.Rank));
                return GetDescriptorForSevenCardsHandSutied(suitedCards, targetSuit, offsuitCards);
            }

            targetSuit = SuitEnum.Diamond;
            if (sevenCardsHand.Cards.Count(c => c.Suit == targetSuit) >= 5)
            {
                var suitedCards = sevenCardsHand.Cards.Where(c => c.Suit == targetSuit).ToList();
                suitedCards.Sort((c1, c2) => -c1.Rank.CompareTo(c2.Rank));
                var offsuitCards = sevenCardsHand.Cards.Where(c => c.Suit != targetSuit).ToList();
                offsuitCards.Sort((c1, c2) => -c1.Rank.CompareTo(c2.Rank));
                return GetDescriptorForSevenCardsHandSutied(suitedCards, targetSuit, offsuitCards);
            }

            targetSuit = SuitEnum.Club;
            if (sevenCardsHand.Cards.Count(c => c.Suit == targetSuit) >= 5)
            {
                var suitedCards = sevenCardsHand.Cards.Where(c => c.Suit == targetSuit).ToList();
                suitedCards.Sort((c1, c2) => -c1.Rank.CompareTo(c2.Rank));
                var offsuitCards = sevenCardsHand.Cards.Where(c => c.Suit != targetSuit).ToList();
                offsuitCards.Sort((c1, c2) => -c1.Rank.CompareTo(c2.Rank));
                return GetDescriptorForSevenCardsHandSutied(suitedCards, targetSuit, offsuitCards);
            }

            return GetDescriptorForSevenCardsHandOffsuit(sevenCardsHand);
        }

        private static string GetDescriptorForSevenCardsHandOffsuit(SevenCardsHand sevenCardsHand)
        {
            return string.Join("", sevenCardsHand.Cards.Select(c => TexasBot.Tools.Utils.GetStringForRank(c.Rank)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="suitedCards">sorted already</param>
        /// <param name="suitedSuit"></param>
        /// <param name="otherCards"></param>
        /// <returns></returns>
        private static string GetDescriptorForSevenCardsHandSutied(List<Card> suitedCards, SuitEnum suitedSuit, List<Card> otherCards)
        {
            return
                $"{TexasBot.Tools.Utils.GetStringForSuit(suitedSuit)}[{string.Join("", suitedCards.Select(c => TexasBot.Tools.Utils.GetStringForRank(c.Rank)))}]" +
                $"{string.Join("", otherCards.Select(c => TexasBot.Tools.Utils.GetStringForRank(c.Rank)))}";
        }

        public static string GetDescriptorForFiveCardsHand(BestFiveCardsHand bestFiveCardsHand)
        {
            throw new NotImplementedException();
        }

        public static Card ConvertCard(this TexasBot.Models.Card card)
        {
            return new Card(card.Suit, card.Rank);
        }

        public static List<Card> SortCards(List<Card> cards)
        {
            var sortedCards = new List<Card>(cards);
            sortedCards.Sort();
            sortedCards.Reverse();
            return sortedCards;
        }

        public static bool IsRaiseMove(this DecisionType decisionType)
        {
            return decisionType == DecisionType.AllIn
                   || decisionType == DecisionType.Ante
                   || decisionType == DecisionType.Raise
                   || decisionType == DecisionType.Reraise;
        }

        public static bool IsProactiveRaiseMove(this DecisionType decisionType)
        {
            return decisionType == DecisionType.AllIn
                   || decisionType == DecisionType.Raise
                   || decisionType == DecisionType.Reraise;
        }

        public static PreflopPlayerStatus DeterminesPreflopPlayerStatus(List<Decision> preflopDecisions, int bigBlindSize)
        {
            if (preflopDecisions.Count == 0)
            {
                return PreflopPlayerStatus.NotPooledYet;
            }

            if (preflopDecisions.Count == 1)
            {
                switch (preflopDecisions[0].DecisionType)
                {
                    case DecisionType.Call:
                        return PreflopPlayerStatus.Limped;
                    case DecisionType.Ante:
                        return PreflopPlayerStatus.NotPooledYet;
                    case DecisionType.Fold:
                        return PreflopPlayerStatus.Fold;
                    case DecisionType.AllIn:
                        return PreflopPlayerStatus.AllIn;
                    case DecisionType.Raise:
                    case DecisionType.Reraise:
                        return DeterminePreflopPlayerStatusBasedOnRaisedChips(preflopDecisions[0], bigBlindSize);
                }
            }

            if (!preflopDecisions.Any(d => d.DecisionType.IsRaiseMove()))
            {
                switch (preflopDecisions.Last().DecisionType)
                {
                    case DecisionType.Call:
                        return PreflopPlayerStatus.Limped;
                    case DecisionType.Fold:
                        return PreflopPlayerStatus.Fold;
                    default:
                        throw new InvalidOperationException($"{preflopDecisions.Last().DecisionType}");
                }
            }

            var lastRaise = preflopDecisions.Last(d => d.DecisionType.IsRaiseMove());
            return DeterminePreflopPlayerStatusBasedOnRaisedChips(lastRaise, bigBlindSize);
        }

        private static PreflopPlayerStatus DeterminePreflopPlayerStatusBasedOnRaisedChips(
            Decision raiseDecision,
            int bigBlindSize)
        {
            if (raiseDecision.DecisionType == DecisionType.AllIn)
            {
                return PreflopPlayerStatus.AllIn;
            }

            if (raiseDecision.ChipsAdded <= bigBlindSize * 5)
            {
                return PreflopPlayerStatus.Raised;
            }
            if (raiseDecision.ChipsAdded <= bigBlindSize * 15)
            {
                return PreflopPlayerStatus.ThreeBet;
            }
            if (raiseDecision.ChipsAdded <= bigBlindSize * 45)
            {
                return PreflopPlayerStatus.FourBet;
            }
            return PreflopPlayerStatus.AllIn;
        }

        public static FlopPlayerStatus DeterminePlayerStatusOnFlop(List<Decision> flopDecisions, 
            PositionEnum playerPosition, PositionEnum preflopRaiserPosition, PositionEnum currentRaiserPosition)
        {
            if (flopDecisions == null || flopDecisions.Count == 0)
            {
                return FlopPlayerStatus.NotPolledYet;
            }

            if (flopDecisions.Count == 1)
            {
                switch (flopDecisions[0].DecisionType)
                {
                    case DecisionType.Check:
                        return playerPosition < preflopRaiserPosition ? FlopPlayerStatus.CheckRaiser : FlopPlayerStatus.Check;
                    case DecisionType.Raise:
                        return FlopPlayerStatus.Raise;
                    case DecisionType.AllIn:
                        return FlopPlayerStatus.AllIn;
                    case DecisionType.Reraise:
                        return FlopPlayerStatus.RaiseRaise;
                    case DecisionType.AllInRaise:
                        return FlopPlayerStatus.AllInRaise;
                    default:
                        throw new InvalidOperationException($"{flopDecisions[0].DecisionType} should not be here");
                }
            }

            if (flopDecisions.Count == 2)
            {
                if (flopDecisions[0].DecisionType == DecisionType.Check)
                {
                    switch (flopDecisions[1].DecisionType)
                    {
                        case DecisionType.Check:
                            return FlopPlayerStatus.Check;
                        case DecisionType.Reraise:
                            return FlopPlayerStatus.RaiseRaise;
                        case DecisionType.AllInRaise:
                            return FlopPlayerStatus.AllInRaise;
                        default:
                            throw new InvalidOperationException($"{flopDecisions[0].DecisionType} should not be here");
                    }
                }
                switch (flopDecisions[0].DecisionType)
                {
                    case DecisionType.Call:
                        return FlopPlayerStatus.CallRaise;
                    case DecisionType.Raise:
                        return FlopPlayerStatus.Raise;
                    case DecisionType.AllIn:
                        return FlopPlayerStatus.AllIn;
                    case DecisionType.Reraise:
                        return FlopPlayerStatus.RaiseRaise;
                    case DecisionType.AllInRaise:
                        return FlopPlayerStatus.AllInRaise;
                    default:
                        throw new InvalidOperationException($"{flopDecisions[0].DecisionType} should not be here");
                }
            }

            throw new NotImplementedException("3 not implemented yet");
        }

        public static FlopDecisionContext GenerateFlopDecisionContext(Round round, List<PlayerRoundProfile> playerProfiles)
        {
            var hero = round.Players.First(round.IsMe);
            var context = new FlopDecisionContext()
            {
                BigBlindSize = round.BigBlindSize,
                CurrentPotSize = round.CurrentPotSize,
                FlopBoard = new FlopBoard(round.Flop1, round.Flop2, round.Flop3),
                Players = playerProfiles,
                HeroName = hero.Name,
                IsHeadsUp = round.Players.Count(p => p.IsAlive) == 2,
                HeroHoles = new HoldingHoles(round.Hole1, round.Hole2),
                PreflopRaiserName = round.PreflopRaiser.Name,
                FlopRaiserName = round.FlopMoves.LastOrDefault(m => m.Decision.DecisionType.IsRaiseMove())?.Player.Name,
                IsRaised = round.FlopMoves.Any(m => m.Decision.DecisionType.IsRaiseMove()),
            };

            if (context.IsHeadsUp)
            {
                context.HeadsUpVillainName = round.Players.First(p => p.IsAlive && !round.IsMe(p)).Name;
            }

            return context;
        }

        public static TurnDecisionContext GenerateTurnDecisionContext(Round round, List<PlayerRoundProfile> playerProfiles)
        {
            var hero = round.Players.First(round.IsMe);
            var context = new TurnDecisionContext()
            {
                BigBlindSize = round.BigBlindSize,
                CurrentPotSize = round.CurrentPotSize,
                TurnBoard = new TurnBoard(new FlopBoard(round.Flop1, round.Flop2, round.Flop3), round.Turn),
                Players = playerProfiles,
                HeroName = hero.Name,
                IsHeadsUp = round.Players.Count(p => p.IsAlive) == 2,
                HeroHoles = new HoldingHoles(round.Hole1, round.Hole2),
                PreflopRaiserName = round.PreflopRaiser.Name,
                FlopRaiserName = round.FlopMoves.LastOrDefault(m => m.Decision.DecisionType.IsRaiseMove())?.Player.Name,
                TurnRaiserName = round.TurnMoves.LastOrDefault(m => m.Decision.DecisionType.IsRaiseMove())?.Player.Name,
                IsRaised = round.TurnMoves.Any(m => m.Decision.DecisionType.IsRaiseMove()),
            };

            if (context.IsHeadsUp)
            {
                context.HeadsUpVillainName = round.Players.First(p => p.IsAlive && !round.IsMe(p)).Name;
            }

            return context;
        }

        public static RiverDecisionContext GenerateRiverDecisionContext(Round round, List<PlayerRoundProfile> playerProfiles)
        {
            var hero = round.Players.First(round.IsMe);
            var context = new RiverDecisionContext()
            {
                BigBlindSize = round.BigBlindSize,
                CurrentPotSize = round.CurrentPotSize,
                RiverBoard = new RiverBoard(new TurnBoard(new FlopBoard(round.Flop1, round.Flop2, round.Flop3), round.Turn), round.River),
                Players = playerProfiles,
                HeroName = hero.Name,
                IsHeadsUp = round.Players.Count(p => p.IsAlive) == 2,
                HeroHoles = new HoldingHoles(round.Hole1, round.Hole2),
                PreflopRaiserName = round.PreflopRaiser.Name,
                FlopRaiserName = round.FlopMoves.LastOrDefault(m => m.Decision.DecisionType.IsRaiseMove())?.Player.Name,
                TurnRaiserName = round.TurnMoves.LastOrDefault(m => m.Decision.DecisionType.IsRaiseMove())?.Player.Name,
                RiverRaiserName = round.RiverMoves.LastOrDefault(m => m.Decision.DecisionType.IsRaiseMove())?.Player.Name,
                IsRaised = round.RiverMoves.Any(m => m.Decision.DecisionType.IsRaiseMove()),
            };

            if (context.IsHeadsUp)
            {
                context.HeadsUpVillainName = round.Players.First(p => p.IsAlive && !round.IsMe(p)).Name;
            }

            return context;
        }
    }
}
