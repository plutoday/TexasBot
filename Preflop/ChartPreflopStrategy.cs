using Models;
using Models.Ranging;
using Preflop.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Infra;

namespace Preflop
{
    public class ChartPreflopStrategy
    {
        public Decision MakeDecision(PreflopStatusSummary statusSummary, HoldingHoles holes)
        {
            Logger.Instance.Log($"ChartPreflopStrategy to make decision. HeroHoles={holes.Hole1.GetStringForCard()},{holes.Hole2.GetStringForCard()}");

            var raiseMoves = FilterRaiseMoves(statusSummary.PreflopRaiseMoves, p => p.Name == statusSummary.Me.Name);
            Logger.Instance.Log($"Filtered raise moves: {string.Join("/", raiseMoves.Select(m => m.GetStringForMove()))}");
            var grid = new RangeGrid(holes);
            if (raiseMoves.Count == 0)
            {
                return GetDecisionOnUnraisedPot(statusSummary, grid);
            }

            if (raiseMoves.Count == 1)
            {
                return GetDecisionAgainstOpenRaise(statusSummary, grid, raiseMoves[0].Player.Position);
            }

            if (raiseMoves.Count == 2)
            {
                return GetDecisionAgainstThreeBet(statusSummary, grid, raiseMoves[1].Player.Position);
            }

            if (raiseMoves.Count == 3)
            {
                return GetDecisionAgainstFourBet(statusSummary, grid, raiseMoves[2].Player.Position);
            }

            if (raiseMoves.Count == 4)
            {
                return GetDecisionAgainstFiveBet(statusSummary, grid, raiseMoves[3].Player.Position);
            }

            throw new NotSupportedException();
        }

        /// <summary>
        /// Only keep hero's moves and the ones right before heros, and the last one if it's not hero's
        /// </summary>
        /// <param name="moves"></param>
        /// <param name="isHero"></param>
        /// <returns></returns>
        private List<Move> FilterRaiseMoves(List<Move> moves, Func<Player, bool> isHero)
        {
            if (moves.Count == 0)
            {
                return new List<Move>();
            }

            List<int> heroMoveIndexes = new List<int>();
            for (int i = 0; i < moves.Count; i++)
            {
                if (isHero(moves[i].Player))
                    heroMoveIndexes.Add(i);
            }

            if (heroMoveIndexes.Count == 0)
            {
                return new List<Move>() { moves.Last() };
            }

            var filtered = new List<Move>();
            foreach (var heroMoveIndex in heroMoveIndexes)
            {
                if (heroMoveIndex > 0)
                {
                    filtered.Add(moves[heroMoveIndex - 1]);
                }
                filtered.Add(moves[heroMoveIndex]);
            }

            if (filtered.Last() != moves.Last())
            {
                filtered.Add(moves.Last());
            }

            return filtered;
        }

        private Decision GetDecisionOnUnraisedPot(PreflopStatusSummary statusSummary, RangeGrid grid)
        {
            Logger.Instance.Log($"Pot not raised, GetDecisionOnUnraisedPot called.");
            var openRaiseChart = ChartUtils.GetOpenRaiseChart(statusSummary.Me.Position);
            var shouldRaise = openRaiseChart.Get(grid);
            if (shouldRaise)
            {
                var chipsToAdd = GetOpenRaiseSize(statusSummary);
                Logger.Instance.Log($"OpenRaiseChart says shouldRaise=true, grid={grid}, returning decision of Raise, chipsToAdd={chipsToAdd}.");
                return new Decision(DecisionType.Raise, chipsToAdd);
            }
            else
            {
                Logger.Instance.Log($"OpenRaiseChart says shouldRaise=false, grid={grid}, returning decision of Fold.");
                return new Decision(DecisionType.Fold, 0);
            }
        }

        private Decision GetDecisionAgainstOpenRaise(PreflopStatusSummary statusSummary, RangeGrid grid, PositionEnum openRaisePosition)
        {
            Logger.Instance.Log($"Pot open raised by {openRaisePosition}, GetDecisionAgainstOpenRaise called.");
            var threeBetChart = ChartUtils.GetDecisionAgaisntOpenRaiseChart(statusSummary.Me.Position, openRaisePosition);
            var decision = threeBetChart.Get(grid);
            switch (decision)
            {
                case DecisionAgainstOpenRaiseEnum.Fold:
                    Logger.Instance.Log($"ThreeBetChart says decision=Fold, grid={grid}, returning decision of Fold.");
                    return new Decision(DecisionType.Fold, 0);
                case DecisionAgainstOpenRaiseEnum.Call:
                    Logger.Instance.Log($"ThreeBetChart says decision=Call, grid={grid}, returning decision of Call.");
                    return new Decision(DecisionType.Call, statusSummary.ChipsToCall);
                case DecisionAgainstOpenRaiseEnum.BluffThreeBet:
                case DecisionAgainstOpenRaiseEnum.ValueThreeBet:
                    int chipsToAdd = GetThreeBetSize(statusSummary);
                    Logger.Instance.Log($"ThreeBetChart says decision={decision}, grid={grid}, returning decision=Reraise, chipsToAdd={chipsToAdd}.");
                    return new Decision(DecisionType.Reraise, chipsToAdd);
                default:
                    throw new InvalidCastException();
            }
        }

        private Decision GetDecisionAgainstThreeBet(PreflopStatusSummary statusSummary, RangeGrid grid, PositionEnum threeBetPosition)
        {
            Logger.Instance.Log($"Pot three bet by {threeBetPosition}, GetDecisionAgainstThreeBet called.");
            var fourBetChart = ChartUtils.GetDecisionAgainstThreeBetChart(statusSummary.Me.Position, threeBetPosition);
            var decision = fourBetChart.Get(grid);
            switch (decision)
            {
                case DecisionAgainstThreeBetEnum.Fold:
                    Logger.Instance.Log($"FourBetChart says decision=Fold, grid={grid}, returning decision of Fold.");
                    return new Decision(DecisionType.Fold, 0);
                case DecisionAgainstThreeBetEnum.Call:
                    Logger.Instance.Log($"FourBetChart says decision=Call, grid={grid}, returning decision of Call.");
                    return new Decision(DecisionType.Call, statusSummary.ChipsToCall);
                case DecisionAgainstThreeBetEnum.BluffFourBet:
                case DecisionAgainstThreeBetEnum.ValueFourBet:
                    int chipsToAdd = GetFourBetSize(statusSummary);
                    Logger.Instance.Log($"FourBetChart says decision={decision}, grid={grid}, returning decision=Reraise, chipsToAdd={chipsToAdd}.");
                    return new Decision(DecisionType.Reraise, chipsToAdd);
                default:
                    throw new InvalidOperationException();
            }
        }

        private Decision GetDecisionAgainstFourBet(PreflopStatusSummary statusSummary, RangeGrid grid,
            PositionEnum fourBetPosition)
        {
            var threeBetChart = ChartUtils.GetDecisionAgaisntOpenRaiseChart(statusSummary.Me.Position, fourBetPosition);
            var decision = threeBetChart.Get(grid);
            switch (decision)
            {
                case DecisionAgainstOpenRaiseEnum.Fold:
                case DecisionAgainstOpenRaiseEnum.Call:
                    throw new InvalidOperationException("Should not three bet in the first place");
                case DecisionAgainstOpenRaiseEnum.BluffThreeBet:
                    return new Decision(DecisionType.Fold, 0);
                case DecisionAgainstOpenRaiseEnum.ValueThreeBet:
                    return new Decision(DecisionType.Reraise, GetAllInSize(statusSummary));
                default:
                    throw new InvalidCastException();
            }
        }

        private Decision GetDecisionAgainstFiveBet(PreflopStatusSummary statusSummary, RangeGrid grid,
            PositionEnum fiveBetPosition)
        {
            var fourBetChart = ChartUtils.GetDecisionAgainstThreeBetChart(statusSummary.Me.Position, fiveBetPosition);
            var decision = fourBetChart.Get(grid);
            switch (decision)
            {
                case DecisionAgainstThreeBetEnum.Fold:
                case DecisionAgainstThreeBetEnum.Call:
                    throw new InvalidOperationException("Should not four bet in the first place");
                case DecisionAgainstThreeBetEnum.BluffFourBet:
                    return new Decision(DecisionType.Fold, 0);
                case DecisionAgainstThreeBetEnum.ValueFourBet:
                    return new Decision(DecisionType.Call, statusSummary.ChipsToCall);
                default:
                    throw new InvalidOperationException();
            }
        }

        private int GetOpenRaiseSize(PreflopStatusSummary statusSummary)
        {
            return statusSummary.BigBlindSize * 3;
        }

        private int GetThreeBetSize(PreflopStatusSummary statusSummary)
        {
            return statusSummary.BigBlindSize * 8;
        }

        private int GetFourBetSize(PreflopStatusSummary statusSummary)
        {
            return statusSummary.BigBlindSize * 24;
        }

        private int GetAllInSize(PreflopStatusSummary statusSummary)
        {
            return statusSummary.BigBlindSize * 72;
        }
    }
}
