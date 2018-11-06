using System;
using System.Collections.Generic;
using Models;
using Models.Ranging;

namespace Preflop
{
    public class PreflopRangeConsultant
    {
        private readonly Dictionary<PositionEnum, Dictionary<PreflopRaiseStageEnum, PlayerRange>> _raiseRangeDict;
        private readonly Dictionary<PositionEnum, Dictionary<PreflopRaiseStageEnum, PlayerRange>> _callRangeDict;

        public PreflopRangeConsultant()
        {
            _raiseRangeDict = new Dictionary<PositionEnum, Dictionary<PreflopRaiseStageEnum, PlayerRange>>();
            _callRangeDict = new Dictionary<PositionEnum, Dictionary<PreflopRaiseStageEnum, PlayerRange>>();
            Init();
        }

        private void Init()
        {
            _raiseRangeDict[PositionEnum.UnderTheGun] = new Dictionary<PreflopRaiseStageEnum, PlayerRange>
            {
                [PreflopRaiseStageEnum.OpenRaise] = GetUtgOpenRaiseRange(),
                [PreflopRaiseStageEnum.ThreeBet] = GetUtg3BetRange(),
                [PreflopRaiseStageEnum.FourBet] = GetUtg4BetRange(),
                [PreflopRaiseStageEnum.FiveBet] = GetUtg5BetRange()
            };

            _raiseRangeDict[PositionEnum.MiddlePosition] = new Dictionary<PreflopRaiseStageEnum, PlayerRange>
            {
                [PreflopRaiseStageEnum.OpenRaise] = GetMpOpenRaiseRange(),
                [PreflopRaiseStageEnum.ThreeBet] = GetMp3BetRange(),
                [PreflopRaiseStageEnum.FourBet] = GetMp4BetRange(),
                [PreflopRaiseStageEnum.FiveBet] = GetMp5BetRange()
            };

            _raiseRangeDict[PositionEnum.CuttingOff] = new Dictionary<PreflopRaiseStageEnum, PlayerRange>
            {
                [PreflopRaiseStageEnum.OpenRaise] = GetCoOpenRaiseRange(),
                [PreflopRaiseStageEnum.ThreeBet] = GetCo3BetRange(),
                [PreflopRaiseStageEnum.FourBet] = GetCo4BetRange(),
                [PreflopRaiseStageEnum.FiveBet] = GetCo5BetRange()
            };
            _raiseRangeDict[PositionEnum.Button] = new Dictionary<PreflopRaiseStageEnum, PlayerRange>
            {
                [PreflopRaiseStageEnum.OpenRaise] = GetBtnOpenRaiseRange(),
                [PreflopRaiseStageEnum.ThreeBet] = GetBtn3BetRange(),
                [PreflopRaiseStageEnum.FourBet] = GetBtn4BetRange(),
                [PreflopRaiseStageEnum.FiveBet] = GetBtn5BetRange()
            };
            _raiseRangeDict[PositionEnum.SmallBlind] = new Dictionary<PreflopRaiseStageEnum, PlayerRange>
            {
                [PreflopRaiseStageEnum.OpenRaise] = GetSbOpenRaiseRange(),
                [PreflopRaiseStageEnum.ThreeBet] = GetSb3BetRange(),
                [PreflopRaiseStageEnum.FourBet] = GetSb4BetRange(),
                [PreflopRaiseStageEnum.FiveBet] = GetSb5BetRange()
            };
            _raiseRangeDict[PositionEnum.BigBlind] = new Dictionary<PreflopRaiseStageEnum, PlayerRange>
            {
                [PreflopRaiseStageEnum.ThreeBet] = GetBb3BetRange(),
                [PreflopRaiseStageEnum.FourBet] = GetBb4BetRange(),
                [PreflopRaiseStageEnum.FiveBet] = GetBb5BetRange()
            };

            _callRangeDict[PositionEnum.UnderTheGun] = new Dictionary<PreflopRaiseStageEnum, PlayerRange>
            {
                [PreflopRaiseStageEnum.Ante] = GetUtgLimpedRange(),
                //[PreflopRaiseStageEnum.OpenRaise] = GetUtgOpenRaiseCalledRange(),
                //[PreflopRaiseStageEnum.ThreeBet] = GetUtg3BetCalledRange(),
                //[PreflopRaiseStageEnum.FourBet] = GetUtg4BetCalledRange(),
                //[PreflopRaiseStageEnum.FiveBet] = GetUtg5BetCalledRange(),
            };

            _callRangeDict[PositionEnum.MiddlePosition] = new Dictionary<PreflopRaiseStageEnum, PlayerRange>
            {
                [PreflopRaiseStageEnum.Ante] = GetMpLimpedRange(),
                [PreflopRaiseStageEnum.OpenRaise] = GetMpOpenRaiseCalledRange(),
                //[PreflopRaiseStageEnum.ThreeBet] = GetMp3BetCalledRange(),
                //[PreflopRaiseStageEnum.FourBet] = GetMp4BetCalledRange(),
                //[PreflopRaiseStageEnum.FiveBet] = GetMp5BetCalledRange(),
            };

            _callRangeDict[PositionEnum.CuttingOff] = new Dictionary<PreflopRaiseStageEnum, PlayerRange>
            {
                [PreflopRaiseStageEnum.Ante] = GetCoLimpedRange(),
                [PreflopRaiseStageEnum.OpenRaise] = GetCoOpenRaiseCalledRange(),
                //[PreflopRaiseStageEnum.ThreeBet] = GetCo3BetCalledRange(),
                //[PreflopRaiseStageEnum.FourBet] = GetCo4BetCalledRange(),
                //[PreflopRaiseStageEnum.FiveBet] = GetCo5BetCalledRange(),
            };

            _callRangeDict[PositionEnum.Button] = new Dictionary<PreflopRaiseStageEnum, PlayerRange>
            {
                [PreflopRaiseStageEnum.Ante] = GetBtnLimpedRange(),
                [PreflopRaiseStageEnum.OpenRaise] = GetBtnOpenRaiseCalledRange(),
                //[PreflopRaiseStageEnum.ThreeBet] = GetBtn3BetCalledRange(),
                //[PreflopRaiseStageEnum.FourBet] = GetBtn4BetCalledRange(),
                //[PreflopRaiseStageEnum.FiveBet] = GetBtn5BetCalledRange(),
            };

            _callRangeDict[PositionEnum.SmallBlind] = new Dictionary<PreflopRaiseStageEnum, PlayerRange>
            {
                [PreflopRaiseStageEnum.Ante] = GetSbLimpedRange(),
                [PreflopRaiseStageEnum.OpenRaise] = GetSbOpenRaiseCalledRange(),
                //[PreflopRaiseStageEnum.ThreeBet] = GetSb3BetCalledRange(),
                //[PreflopRaiseStageEnum.FourBet] = GetSb4BetCalledRange(),
                //[PreflopRaiseStageEnum.FiveBet] = GetSb5BetCalledRange(),
            };

            _callRangeDict[PositionEnum.BigBlind] = new Dictionary<PreflopRaiseStageEnum, PlayerRange>
            {
                [PreflopRaiseStageEnum.Ante] = GetBbLimpedRange(),
                //[PreflopRaiseStageEnum.OpenRaise] = GetBbOpenRaiseCalledRange(),
                //[PreflopRaiseStageEnum.ThreeBet] = GetBb3BetCalledRange(),
                //[PreflopRaiseStageEnum.FourBet] = GetBb4BetCalledRange(),
                //[PreflopRaiseStageEnum.FiveBet] = GetBb5BetCalledRange(),
            };
        }

        #region OPEN RAISE
        private PlayerRange GetUtgOpenRaiseRange()
        {
            var range = new PlayerRange();
            string[] grids = new string[]
            {
            //done
              "11111        ",
              "11111        ",
              "11111        ",
              "1  111       ",
              "    111      ",
              "     111     ",
              "      11     ",
              "       11    ",
              "        1    ",
              "         1   ",
              "             ",
              "             ",
              "             ",
            };
            range.Init(grids);

            return range;
        }

        private PlayerRange GetMpOpenRaiseRange()
        {
            var range = new PlayerRange();
            string[] grids = new string[]
            {
            //done
              "11111    1111",
              "11111        ",
              "11111        ",
              "11 111       ",
              "    111      ",
              "     111     ",
              "      11     ",
              "       11    ",
              "        11   ",
              "         1   ",
              "          1  ",
              "           1 ",
              "            1",
            };
            range.Init(grids);

            return range;
        }

        private PlayerRange GetCoOpenRaiseRange()
        {
            var range = new PlayerRange();
            string[] grids = new string[]
            {
            //done
              "1111111111111",
              "11111111     ",
              "1111111      ",
              "11111111     ",
              "11111111     ",
              "111111111    ",
              "     11111   ",
              "       111   ",
              "        11   ",
              "         11  ",
              "          1  ",
              "           1 ",
              "            1",
            };
            range.Init(grids);

            return range;
        }

        private PlayerRange GetBtnOpenRaiseRange()
        {
            var range = new PlayerRange();
            string[] grids = new string[]
            {
                //done
              "1111111111111",
              "1111111111111",
              "1111111111111",
              "111111111111 ",
              "111111111111 ",
              "11111111111  ",
              "11111111111  ",
              "111111111111 ",
              "11  11111111 ",
              "11      1111 ",
              "1         11 ",
              "1          1 ",
              "1           1",
            };
            range.Init(grids);

            return range;
        }

        private PlayerRange GetSbOpenRaiseRange()
        {
            var range = new PlayerRange();
            string[] grids = new string[]
            {
            //done
              "1111111111111",
              "1111111100000",
              "1111111100000",
              "1111111100000",
              "1111111100000",
              "1111111110000",
              "0000011111000",
              "0000000111000",
              "0000000011100",
              "0000000000100",
              "0000000000000",
              "0000000000000",
              "0000000000000",
            };
            range.Init(grids);

            return range;
        }

        #endregion

        #region LIMPED RANGE
        private PlayerRange GetUtgLimpedRange()
        {
            var range = new PlayerRange();
            string[] grids = new string[]
            {
                //done
              "1111111111111",
              "1111111111111",
              "111111111    ",
              "111111111    ",
              "111111111    ",
              "1111111      ",
              "11111111     ",
              "11111111     ",
              "11  11111    ",
              "11      11   ",
              "1         1  ",
              "1          1 ",
              "1           1",
            };
            range.Init(grids);

            return range;
        }
        private PlayerRange GetMpLimpedRange()
        {
            return GetUtgLimpedRange();
        }
        private PlayerRange GetCoLimpedRange()
        {
            return GetUtgLimpedRange();
        }
        private PlayerRange GetBtnLimpedRange()
        {
            return GetUtgLimpedRange();
        }
        private PlayerRange GetSbLimpedRange()
        {
            return GetUtgLimpedRange();
        }
        private PlayerRange GetBbLimpedRange()
        {
            return GetUtgLimpedRange();
        }
        #endregion

        #region 3 BET RANGE

        private PlayerRange GetUtg3BetRange()
        {
            var range = new PlayerRange();
            string[] grids = new string[]
            {
                //done
              "1            ",
              " 1           ",
              "             ",
              "             ",
              "             ",
              "             ",
              "             ",
              "             ",
              "             ",
              "             ",
              "             ",
              "             ",
              "             ",
            };
            range.Init(grids);

            return range;
        }
        private PlayerRange GetMp3BetRange()
        {
            return GetUtg3BetRange();
        }
        private PlayerRange GetCo3BetRange()
        {
            return GetUtg3BetRange();
        }
        private PlayerRange GetBtn3BetRange()
        {
            return GetUtg3BetRange();
        }
        private PlayerRange GetSb3BetRange()
        {
            return GetUtg3BetRange();
        }
        private PlayerRange GetBb3BetRange()
        {
            return GetUtg3BetRange();
        }
        #endregion



        #region 4 BET RANGE

        private PlayerRange GetUtg4BetRange()
        {
            return GetUtg3BetRange();
        }
        private PlayerRange GetMp4BetRange()
        {
            return GetUtg4BetRange();
        }
        private PlayerRange GetCo4BetRange()
        {
            return GetUtg4BetRange();
        }
        private PlayerRange GetBtn4BetRange()
        {
            return GetUtg4BetRange();
        }
        private PlayerRange GetSb4BetRange()
        {
            return GetUtg4BetRange();
        }
        private PlayerRange GetBb4BetRange()
        {
            return GetUtg4BetRange();
        }
        #endregion

        #region 5 BET RANGE

        private PlayerRange GetUtg5BetRange()
        {
            return GetUtg4BetRange();
        }
        private PlayerRange GetMp5BetRange()
        {
            return GetUtg5BetRange();
        }
        private PlayerRange GetCo5BetRange()
        {
            return GetUtg5BetRange();
        }
        private PlayerRange GetBtn5BetRange()
        {
            return GetUtg5BetRange();
        }
        private PlayerRange GetSb5BetRange()
        {
            return GetUtg5BetRange();
        }
        private PlayerRange GetBb5BetRange()
        {
            return GetUtg5BetRange();
        }
        #endregion

        #region OPEN RAISE CALL RANGE

        private PlayerRange GetUtgOpenRaiseCalledRange()
        {
            throw new NotImplementedException();
        }
        private PlayerRange GetMpOpenRaiseCalledRange()
        {
            var range = new PlayerRange();
            string[] grids = new string[]
            {
                //done
              " 111         ",
              "1 1          ",
              "1 1          ",
              "   1         ",
              "    1        ",
              "     1       ",
              "      1      ",
              "             ",
              "             ",
              "             ",
              "             ",
              "             ",
              "             ",
            };
            range.Init(grids);

            return range;
        }
        private PlayerRange GetCoOpenRaiseCalledRange()
        {
            var range = new PlayerRange();
            string[] grids = new string[]
            {
                //done
              " 1111        ",
              "1 111        ",
              "11111        ",
              "11 11        ",
              "    11       ",
              "     11      ",
              "      11     ",
              "       11    ",
              "        11   ",
              "             ",
              "             ",
              "             ",
              "             ",
            };
            range.Init(grids);

            return range;
        }
        private PlayerRange GetBtnOpenRaiseCalledRange()
        {
            var range = new PlayerRange();
            string[] grids = new string[]
            {
                //done
              " 1111        ",
              "1 11         ",
              "1111         ",
              "11 111       ",
              "    111      ",
              "     111     ",
              "      11     ",
              "       11    ",
              "        1    ",
              "         1   ",
              "             ",
              "             ",
              "             ",
            };
            range.Init(grids);

            return range;
        }
        private PlayerRange GetSbOpenRaiseCalledRange()
        {
            var range = new PlayerRange();
            string[] grids = new string[]
            {
                //done
              " 11111       ",
              "1 1111       ",
              "11111        ",
              "11111        ",
              "111 11       ",
              "     11      ",
              "      11     ",
              "       11    ",
              "        1    ",
              "             ",
              "             ",
              "             ",
              "             ",
            };
            range.Init(grids);

            return range;
        }
        private PlayerRange GetBbOpenRaiseCalledRange()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 3 BET CALLED RANGE
        private PlayerRange GetUtg3BetCalledRange()
        {
            throw new NotImplementedException();
        }
        private PlayerRange GetMp3BetCalledRange()
        {
            throw new NotImplementedException();
        }
        private PlayerRange GetCo3BetCalledRange()
        {
            throw new NotImplementedException();
        }
        private PlayerRange GetBtn3BetCalledRange()
        {
            throw new NotImplementedException();
        }
        private PlayerRange GetSb3BetCalledRange()
        {
            throw new NotImplementedException();
        }
        private PlayerRange GetBb3BetCalledRange()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 4 BET CALLED RANGE
        private PlayerRange GetUtg4BetCalledRange()
        {
            throw new NotImplementedException();
        }
        private PlayerRange GetMp4BetCalledRange()
        {
            throw new NotImplementedException();
        }
        private PlayerRange GetCo4BetCalledRange()
        {
            throw new NotImplementedException();
        }
        private PlayerRange GetBtn4BetCalledRange()
        {
            throw new NotImplementedException();
        }
        private PlayerRange GetSb4BetCalledRange()
        {
            throw new NotImplementedException();
        }
        private PlayerRange GetBb4BetCalledRange()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 5 BET CALLED RANGE
        private PlayerRange GetUtg5BetCalledRange()
        {
            throw new NotImplementedException();
        }
        private PlayerRange GetMp5BetCalledRange()
        {
            throw new NotImplementedException();
        }
        private PlayerRange GetCo5BetCalledRange()
        {
            throw new NotImplementedException();
        }
        private PlayerRange GetBtn5BetCalledRange()
        {
            throw new NotImplementedException();
        }
        private PlayerRange GetSb5BetCalledRange()
        {
            throw new NotImplementedException();
        }
        private PlayerRange GetBb5BetCalledRange()
        {
            throw new NotImplementedException();
        }
        #endregion

        public PlayerRange GetRaiseRange(PreflopRaiseStageEnum raiseStage, PositionEnum position)
        {
            return _raiseRangeDict[position][raiseStage];
        }

        public PlayerRange GetCallRange(PreflopRaiseStageEnum raiseStageCalled, PositionEnum position)
        {
            return _callRangeDict[position][raiseStageCalled];
        }
    }
}
