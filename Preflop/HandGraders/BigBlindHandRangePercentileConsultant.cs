using System;
using System.Collections.Generic;
using System.Linq;
using Models;

namespace Preflop.HandGraders
{
    public class BigBlindHandRangePercentileConsultant
    {
        private readonly Dictionary<int, HandRangePercentiles> _limpedPot = new Dictionary<int, HandRangePercentiles>()
        {
            //numOfLimpers, percentiles
            {1, new HandRangePercentiles(0, 0.15, 0.15, 0.16) },
            {2, new HandRangePercentiles(0, 0.09, 0.09, 0.1) },
            {3, new HandRangePercentiles(0, 0.06, 0.06, 0.06) },
            {4, new HandRangePercentiles(0, 0.03, 0.03, 0.035) },
            {5, new HandRangePercentiles(0, 0.03, 0.03, 0.035) },
        };

        private readonly Dictionary<PositionEnum, Dictionary<int, HandRangePercentiles>> _raisedPot = new Dictionary<PositionEnum, Dictionary<int, HandRangePercentiles>>()
        {
            {
                PositionEnum.UnderTheGun, new Dictionary<int, HandRangePercentiles>()
                {
                    //numOfCallers, percentiles
                    { 0, new HandRangePercentiles(0, 0.15, 0.15, 0.16) },
                    { 1, new HandRangePercentiles(0, 0.09, 0.09, 0.1) },
                    { 2, new HandRangePercentiles(0, 0.06, 0.06, 0.06) },
                    { 3, new HandRangePercentiles(0, 0.03, 0.03, 0.035) },
                    { 4, new HandRangePercentiles(0, 0.03, 0.03, 0.035) },
                }
            },
            {
                PositionEnum.MiddlePosition, new Dictionary<int, HandRangePercentiles>()
                {
                    //numOfCallers, percentiles
                    { 0, new HandRangePercentiles(0, 0.15, 0.15, 0.16) },
                    { 1, new HandRangePercentiles(0, 0.09, 0.09, 0.1) },
                    { 2, new HandRangePercentiles(0, 0.06, 0.06, 0.06) },
                    { 3, new HandRangePercentiles(0, 0.03, 0.03, 0.035) },
                }
            },
            {
                PositionEnum.CuttingOff, new Dictionary<int, HandRangePercentiles>()
                {
                    //numOfCallers, percentiles
                    { 0, new HandRangePercentiles(0, 0.15, 0.15, 0.16) },
                    { 1, new HandRangePercentiles(0, 0.09, 0.09, 0.1) },
                    { 2, new HandRangePercentiles(0, 0.03, 0.03, 0.035) },
                }
            },
            {
                PositionEnum.Button, new Dictionary<int, HandRangePercentiles>()
                {
                    //numOfCallers, percentiles
                    { 0, new HandRangePercentiles(0, 0.15, 0.15, 0.16) },
                    { 1, new HandRangePercentiles(0, 0.03, 0.03, 0.035) },
                }
            },
            {
                PositionEnum.SmallBlind, new Dictionary<int, HandRangePercentiles>()
                {
                    //numOfCallers, percentiles
                    //note that you are in better position than sb
                    { 0, new HandRangePercentiles(0, 0.15, 0.15, 0.16) },
                }
            }
        };


        public HandRangePercentiles GetPercentiles(PreflopStatusSummary statusSummary)
        {
            switch (statusSummary.Status)
            {
                case PreflopGameStatusEnum.LimpedPot:
                    return GetPercentilesOnLimpedPot(statusSummary);
                case PreflopGameStatusEnum.Raised:
                case PreflopGameStatusEnum.RaisedWithCallers:
                    return GetPercentileOnRaisedPot(statusSummary);
                default:
                    throw new NotImplementedException($"{statusSummary.Status} not implemented in BigBlindHandRangePercentileConsultant");
            }
        }

        private HandRangePercentiles GetPercentilesOnLimpedPot(PreflopStatusSummary statusSummary)
        {
            //#(limper) matters
            int numOfLimpers = statusSummary.Players.Count(p => p.PlayerStatus != PlayerStatusEnum.Folded) - 2; //2 blinds
            return _limpedPot[numOfLimpers];
        }

        private HandRangePercentiles GetPercentileOnRaisedPot(PreflopStatusSummary statusSummary)
        {
            /*
            //#(callers) matters, the position of raiser matters
            PositionEnum raiserPosition;
            List<PositionEnum> callerPositions = new List<PositionEnum>();
            if (statusSummary.Button.Decisions.Last().DecisionType == DecisionType.Raise)
            {
                raiserPosition = PositionEnum.Button;
            }
            else if (statusSummary.CuttingOff.Decisions.Last().DecisionType == DecisionType.Raise)
            {
                raiserPosition = PositionEnum.CuttingOff;
                if (statusSummary.Button.Decisions.Last().DecisionType == DecisionType.Call)
                {
                    callerPositions.Add(PositionEnum.Button);
                }
            }
            else if (statusSummary.MiddlePosition.Decisions.Last().DecisionType == DecisionType.Raise)
            {
                raiserPosition = PositionEnum.MiddlePosition;
                if (statusSummary.CuttingOff.Decisions.Last().DecisionType == DecisionType.Call)
                {
                    callerPositions.Add(PositionEnum.CuttingOff);
                }
                if (statusSummary.Button.Decisions.Last().DecisionType == DecisionType.Call)
                {
                    callerPositions.Add(PositionEnum.Button);
                }
            }
            else if (statusSummary.UnderTheGun.Decisions.Last().DecisionType == DecisionType.Raise)
            {
                if (statusSummary.MiddlePosition.Decisions.Last().DecisionType == DecisionType.Call)
                {
                    callerPositions.Add(PositionEnum.MiddlePosition);
                }
                if (statusSummary.CuttingOff.Decisions.Last().DecisionType == DecisionType.Call)
                {
                    callerPositions.Add(PositionEnum.CuttingOff);
                }
                if (statusSummary.Button.Decisions.Last().DecisionType == DecisionType.Call)
                {
                    callerPositions.Add(PositionEnum.Button);
                }
                raiserPosition = PositionEnum.UnderTheGun;
            }
            else
            {
                throw new InvalidOperationException();
            }

            return GetPercentilesOnRaisedPot(raiserPosition, callerPositions);*/
            throw new NotImplementedException();
        }

        private HandRangePercentiles GetPercentilesOnRaisedPot(PositionEnum raiserPosition,
            List<PositionEnum> callerPositions)
        {
            //Currently only consider the number of callers and ignore their positions
            return _raisedPot[raiserPosition][callerPositions.Count];
        }
    }
}
