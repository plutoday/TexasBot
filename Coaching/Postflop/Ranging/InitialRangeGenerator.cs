using System;
using Models;
using Models.Ranging;

namespace Coaching.Postflop.Ranging
{
    public class InitialRangeGenerator
    {
        public PlayerRange GeneratePreflopRange(PreflopPlayerStatus preflopPlayerStatus)
        {
            throw new NotImplementedException();
        }

        public PlayerRange GeneratePreflopRange(PositionEnum position, PreflopPlayerStatus preflopStatus)
        {
            switch (preflopStatus)
            {
                case PreflopPlayerStatus.Raised:
                    return GenerateOpenRaisedRange(position);
                case PreflopPlayerStatus.Limped:
                    return GenerateLimpedRange(position);
                case PreflopPlayerStatus.RaiseCalled:
                    return GenerateRaiseCalledRange(position);
                case PreflopPlayerStatus.ThreeBet:
                    return GenerateThreeBetRange(position);
                case PreflopPlayerStatus.ThreeBetCalled:
                    return GenerateThreeBetCalledRange(position);
                case PreflopPlayerStatus.FourBet:
                    return GenerateFourBetRange(position);
                case PreflopPlayerStatus.FourBetCalled:
                    return GenerateFourBetCalledRange(position);
                default:
                    throw new NotImplementedException();
            }
        }

        private PlayerRange GenerateOpenRaisedRange(PositionEnum position)
        {
            int[,] grids;
            switch (position)
            {
                case PositionEnum.UnderTheGun:
                    grids = GenerateOpenRaiseForUtgTight();
                    break;
                case PositionEnum.MiddlePositioin:
                    grids = GenerateOpenRaiseForMpTight();
                    break;
                case PositionEnum.CuttingOff:
                    grids = GenerateOpenRaiseForCoTight();
                    break;
                case PositionEnum.Button:
                    grids = GenerateOpenRaiseForBtnTight();
                    break;
                case PositionEnum.SmallBlind:
                    grids = GenerateOpenRaiseForSbTight();
                    break;
                case PositionEnum.BigBlind:
                    grids = GenerateOpenRaiseForBbTight();
                    break;

                default:
                    throw new InvalidOperationException();
            }

            var range = new PlayerRange();
            range.Init(grids);

            return range;
        }

        private PlayerRange GenerateLimpedRange(PositionEnum position)
        {
            throw new NotImplementedException();
        }

        private PlayerRange GenerateRaiseCalledRange(PositionEnum position)
        {
            throw new NotImplementedException();
        }

        private PlayerRange GenerateThreeBetRange(PositionEnum position)
        {
            throw new NotImplementedException();
        }

        private PlayerRange GenerateThreeBetCalledRange(PositionEnum position)
        {
            throw new NotImplementedException();
        }

        private PlayerRange GenerateFourBetRange(PositionEnum position)
        {
            throw new NotImplementedException();
        }

        private PlayerRange GenerateFourBetCalledRange(PositionEnum position)
        {
            throw new NotImplementedException();
        }

        private PlayerRange GenerateAllInRange(PositionEnum position)
        {
            throw new NotImplementedException();
        }

        private PlayerRange GenerateAllInCalledRange(PositionEnum position)
        {
            throw new NotImplementedException();
        }

        private int[,] GenerateOpenRaiseForUtgTight()
        {
            return new int[,]
            {
                //A  K  Q  J  T  9  8  7  6  5  4  3  2
                { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0},   //A
                { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0},   //K
                { 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0},   //Q
                { 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0},   //J
                { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0},   //T
                { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0},   //9
                { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0},   //8
                { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0},   //7
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0},   //6
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0},   //5
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0},   //4
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},   //3
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},   //2
            };
        }

        private int[,] GenerateOpenRaiseForMpTight()
        {
            return new int[,]
            {
                //A  K  Q  J  T  9  8  7  6  5  4  3  2
                { 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},   //A
                { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0},   //K
                { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0},   //Q
                { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0},   //J
                { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0},   //T
                { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0},   //9
                { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0},   //8
                { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0},   //7
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0},   //6
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0},   //5
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0},   //4
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},   //3
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},   //2
            };
        }

        private int[,] GenerateOpenRaiseForCoTight()
        {
            return new int[,]
            {
                //A  K  Q  J  T  9  8  7  6  5  4  3  2
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},   //A
                { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0},   //K
                { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0},   //Q
                { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0},   //J
                { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0},   //T
                { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0},   //9
                { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0},   //8
                { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0},   //7
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0},   //6
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0},   //5
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0},   //4
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},   //3
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},   //2
            };
        }

        private int[,] GenerateOpenRaiseForBtnTight()
        {
            return new int[,]
            {
                //A  K  Q  J  T  9  8  7  6  5  4  3  2
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},   //A
                { 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0},   //K
                { 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},   //Q
                { 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},   //J
                { 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},   //T
                { 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0},   //9
                { 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0},   //8
                { 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0},   //7
                { 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0},   //6
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0},   //5
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0},   //4
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},   //3
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},   //2
            };
        }

        private int[,] GenerateOpenRaiseForSbTight()
        {
            return new int[,]
            {
                //A  K  Q  J  T  9  8  7  6  5  4  3  2
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},   //A
                { 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},   //K
                { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0},   //Q
                { 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},   //J
                { 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},   //T
                { 1, 1, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0},   //9
                { 1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},   //8
                { 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0},   //7
                { 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0},   //6
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0},   //5
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0},   //4
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},   //3
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},   //2
            };
        }

        private int[,] GenerateOpenRaiseForBbTight()
        {
            return new int[,]
            {
                //A  K  Q  J  T  9  8  7  6  5  4  3  2
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},   //A
                { 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},   //K
                { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0},   //Q
                { 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},   //J
                { 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},   //T
                { 1, 1, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0},   //9
                { 1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},   //8
                { 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0},   //7
                { 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0},   //6
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0},   //5
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0},   //4
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},   //3
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},   //2
            };
        }
    }
}
