using System;
using System.Collections.Generic;
using Models;

namespace Preflop.HandGraders
{
    public class HandRangePercentileConsultant
    {
        private readonly double[][] _ftm =
        {
            //AllIn, Value, Flat, Bluff
            new double[]{0.01, 0.12, 0.14, 0.16}, //UTG
            new double[]{0.01, 0.15, 0.17, 0.19}, //MP
            new double[]{0.01, 0.17, 0.19, 0.21}, //CO
            new double[]{0.02, 0.20, 0.22, 0.3}, //BTN

        };

        private readonly Dictionary<PositionEnum, HandRangePercentiles> _foldedToMe;

        private readonly double[][] _lp =
        {
            //AllIn, Value, Flat, Bluff
            new double[]{0.01, 0.12, 0.14, 0.16}, //UTG
            new double[]{0.01, 0.15, 0.17, 0.19}, //MP
            new double[]{0.01, 0.17, 0.19, 0.21}, //CO
            new double[]{0.02, 0.20, 0.22, 0.3}, //BTN

        };

        private readonly Dictionary<PositionEnum, HandRangePercentiles> _limpedPot;

        private readonly double[][] _r =
        {
            //AllIn, Value, Flat, Bluff
            new double[]{0.01, 0.12, 0.14, 0.16}, //UTG
            new double[]{0.01, 0.15, 0.17, 0.19}, //MP
            new double[]{0.01, 0.17, 0.19, 0.21}, //CO
            new double[]{0.02, 0.20, 0.22, 0.3}, //BTN

        };

        private readonly Dictionary<PositionEnum, HandRangePercentiles> _raisedToMe;

        private readonly double[][] _3b =
        {
            //AllIn, Value, Flat, Bluff
            new double[]{0.01, 0.12, 0.14, 0.16}, //UTG
            new double[]{0.01, 0.15, 0.17, 0.19}, //MP
            new double[]{0.01, 0.17, 0.19, 0.21}, //CO
            new double[]{0.02, 0.20, 0.22, 0.3}, //BTN

        };

        private readonly Dictionary<PositionEnum, HandRangePercentiles> _3Bet;

        private readonly double[][] _4b =
        {
            //AllIn, Value, Flat, Bluff
            new double[]{0.01, 0.12, 0.14, 0.16}, //UTG
            new double[]{0.01, 0.15, 0.17, 0.19}, //MP
            new double[]{0.01, 0.17, 0.19, 0.21}, //CO
            new double[]{0.02, 0.20, 0.22, 0.3}, //BTN

        };

        private readonly Dictionary<PositionEnum, HandRangePercentiles> _4Bet;

        private readonly double[][] _all =
        {
            //AllIn, Value, Flat, Bluff
            new double[]{0.01, 0.12, 0.14, 0.16}, //UTG
            new double[]{0.01, 0.15, 0.17, 0.19}, //MP
            new double[]{0.01, 0.17, 0.19, 0.21}, //CO
            new double[]{0.02, 0.20, 0.22, 0.3}, //BTN

        };

        private readonly Dictionary<PositionEnum, HandRangePercentiles> _allIn;

        public HandRangePercentileConsultant()
        {
            double[][] percentiles = _ftm;
            _foldedToMe
               = new Dictionary<PositionEnum, HandRangePercentiles>()
               {
                   { PositionEnum.UnderTheGun, new HandRangePercentiles(percentiles[0][0],percentiles[0][1],percentiles[0][2],percentiles[0][3]) },
                   { PositionEnum.MiddlePosition, new HandRangePercentiles(percentiles[1][0],percentiles[1][1],percentiles[1][2],percentiles[1][3]) },
                   { PositionEnum.CuttingOff, new HandRangePercentiles(percentiles[2][0],percentiles[2][1],percentiles[2][2],percentiles[2][3]) },
                   { PositionEnum.Button, new HandRangePercentiles(percentiles[3][0],percentiles[3][1],percentiles[3][2],percentiles[3][3]) }
               };

            percentiles = _lp;
            _limpedPot
               = new Dictionary<PositionEnum, HandRangePercentiles>()
           {
                {PositionEnum.UnderTheGun, new HandRangePercentiles(percentiles[0][0],percentiles[0][1],percentiles[0][2],percentiles[0][3]) },
                {PositionEnum.MiddlePosition, new HandRangePercentiles(percentiles[1][0],percentiles[1][1],percentiles[1][2],percentiles[1][3]) },
                {PositionEnum.CuttingOff, new HandRangePercentiles(percentiles[2][0],percentiles[2][1],percentiles[2][2],percentiles[2][3]) },
                {PositionEnum.Button, new HandRangePercentiles(percentiles[3][0],percentiles[3][1],percentiles[3][2],percentiles[3][3]) }
           };

            percentiles = _r;
            _raisedToMe
               = new Dictionary<PositionEnum, HandRangePercentiles>()
           {
                {PositionEnum.UnderTheGun, new HandRangePercentiles(percentiles[0][0],percentiles[0][1],percentiles[0][2],percentiles[0][3]) },
                {PositionEnum.MiddlePosition, new HandRangePercentiles(percentiles[1][0],percentiles[1][1],percentiles[1][2],percentiles[1][3]) },
                {PositionEnum.CuttingOff, new HandRangePercentiles(percentiles[2][0],percentiles[2][1],percentiles[2][2],percentiles[2][3]) },
                {PositionEnum.Button, new HandRangePercentiles(percentiles[3][0],percentiles[3][1],percentiles[3][2],percentiles[3][3]) }
           };

            percentiles = _3b;
            _3Bet
               = new Dictionary<PositionEnum, HandRangePercentiles>()
           {
                {PositionEnum.UnderTheGun, new HandRangePercentiles(percentiles[0][0],percentiles[0][1],percentiles[0][2],percentiles[0][3]) },
                {PositionEnum.MiddlePosition, new HandRangePercentiles(percentiles[1][0],percentiles[1][1],percentiles[1][2],percentiles[1][3]) },
                {PositionEnum.CuttingOff, new HandRangePercentiles(percentiles[2][0],percentiles[2][1],percentiles[2][2],percentiles[2][3]) },
                {PositionEnum.Button, new HandRangePercentiles(percentiles[3][0],percentiles[3][1],percentiles[3][2],percentiles[3][3]) }
           };

            percentiles = _4b;
            _4Bet
               = new Dictionary<PositionEnum, HandRangePercentiles>()
           {
                {PositionEnum.UnderTheGun, new HandRangePercentiles(percentiles[0][0],percentiles[0][1],percentiles[0][2],percentiles[0][3]) },
                {PositionEnum.MiddlePosition, new HandRangePercentiles(percentiles[1][0],percentiles[1][1],percentiles[1][2],percentiles[1][3]) },
                {PositionEnum.CuttingOff, new HandRangePercentiles(percentiles[2][0],percentiles[2][1],percentiles[2][2],percentiles[2][3]) },
                {PositionEnum.Button, new HandRangePercentiles(percentiles[3][0],percentiles[3][1],percentiles[3][2],percentiles[3][3]) }
           };

            percentiles = _all;
            _allIn
               = new Dictionary<PositionEnum, HandRangePercentiles>()
           {
                {PositionEnum.UnderTheGun, new HandRangePercentiles(percentiles[0][0],percentiles[0][1],percentiles[0][2],percentiles[0][3]) },
                {PositionEnum.MiddlePosition, new HandRangePercentiles(percentiles[1][0],percentiles[1][1],percentiles[1][2],percentiles[1][3]) },
                {PositionEnum.CuttingOff, new HandRangePercentiles(percentiles[2][0],percentiles[2][1],percentiles[2][2],percentiles[2][3]) },
                {PositionEnum.Button, new HandRangePercentiles(percentiles[3][0],percentiles[3][1],percentiles[3][2],percentiles[3][3]) }
           };
        }

        public HandRangePercentiles GetPercentiles(PreflopStatusSummary statusSummary)
        {
            switch (statusSummary.Status)
            {
                case PreflopGameStatusEnum.FoldedToMe:
                    return GetFoldedToMePercentiles(statusSummary);
                case PreflopGameStatusEnum.LimpedPot:
                    return GetLimpedPotPercentiles(statusSummary);
                case PreflopGameStatusEnum.Raised:
                case PreflopGameStatusEnum.RaisedWithCallers:
                    return GetRaisedPercentiles(statusSummary);
                case PreflopGameStatusEnum.TriBet:
                case PreflopGameStatusEnum.TriBetWithCallers:
                    return GetTriBetPercentiles(statusSummary);
                case PreflopGameStatusEnum.FourBet:
                case PreflopGameStatusEnum.FourBetWithCallers:
                    return GetFourBetPercentiles(statusSummary);
                case PreflopGameStatusEnum.FiveBet:
                case PreflopGameStatusEnum.FiveBetWithCallers:
                case PreflopGameStatusEnum.AllIn:
                    return GetAllInPercentiles(statusSummary);
            }

            throw new NotImplementedException();
        }

        private HandRangePercentiles GetFoldedToMePercentiles(PreflopStatusSummary statusSummary)
        {
            return _foldedToMe[statusSummary.Me.Position];
        }

        private HandRangePercentiles GetLimpedPotPercentiles(PreflopStatusSummary statusSummary)
        {
            //var numOfLimpers = GetNumOfLimpers(statusSummary);
            return _limpedPot[statusSummary.Me.Position];
        }

        private int GetNumOfLimpers(PreflopStatusSummary statusSummary)
        {
            throw new NotImplementedException();
        }

        private HandRangePercentiles GetRaisedPercentiles(PreflopStatusSummary statusSummary)
        {
            return _raisedToMe[statusSummary.Me.Position];
        }

        private HandRangePercentiles GetTriBetPercentiles(PreflopStatusSummary statusSummary)
        {
            return _3Bet[statusSummary.Me.Position];
        }

        private HandRangePercentiles GetFourBetPercentiles(PreflopStatusSummary statusSummary)
        {
            return _4Bet[statusSummary.Me.Position];
        }

        private HandRangePercentiles GetAllInPercentiles(PreflopStatusSummary statusSummary)
        {
            return _allIn[statusSummary.Me.Position];
        }
    }
}
