using System;
using Models;
using Models.Ranging;

namespace Preflop.Charts
{
    public static class ChartUtils
    {
        private static IChart<T> Translate<T>(int[,] matrix, Func<int, T> translator)
        {
            var chart = new Chart<T>();

            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    chart.Add(GenerateRangeGridForIndex(i, j), translator.Invoke(matrix[i, j]));
                }
            }

            return chart;
        }

        private static readonly RankEnum[] Ranks = new[]
        {
                RankEnum.Ace, RankEnum.King, RankEnum.Queen, RankEnum.Jack, RankEnum.Ten, RankEnum.Nine, RankEnum.Eight,
                RankEnum.Seven, RankEnum.Six, RankEnum.Five, RankEnum.Four, RankEnum.Three, RankEnum.Two
        };

        private static RangeGrid GenerateRangeGridForIndex(int i, int j)
        {
            return new RangeGrid(Ranks[i], Ranks[j], j > i);
        }

        #region IChart

        public static IChart<bool> GetOpenRaiseChart(PositionEnum position)
        {
            switch (position)
            {
                case PositionEnum.UnderTheGun:
                    return Translate(ConvertStringArrayToIntegerMatrix(GetUtgOpenRaiseLines()), i => i == 1);
                case PositionEnum.MiddlePosition:
                    return Translate(ConvertStringArrayToIntegerMatrix(GetMpOpenRaiseLines()), i => i == 1);
                case PositionEnum.CuttingOff:
                    return Translate(ConvertStringArrayToIntegerMatrix(GetCoOpenRaiseLines()), i => i == 1);
                case PositionEnum.Button:
                    return Translate(ConvertStringArrayToIntegerMatrix(GetBtnOpenRaiseLines()), i => i == 1);
                case PositionEnum.SmallBlind:
                    return Translate(ConvertStringArrayToIntegerMatrix(GetSbOpenRaiseLines()), i => i == 1);
                case PositionEnum.BigBlind:
                    return Translate(ConvertStringArrayToIntegerMatrix(GetBbOpenRaiseLines()), i => i == 1);
                default:
                    throw new InvalidOperationException();
            }
        }

        public static IChart<DecisionAgainstOpenRaiseEnum> GetDecisionAgaisntOpenRaiseChart(PositionEnum heroPosition, PositionEnum openRaisePosition)
        {
            switch (heroPosition)
            {
                case PositionEnum.UnderTheGun:
                    throw new InvalidOperationException("UTG won't face open raise");
                case PositionEnum.MiddlePosition:
                    switch (openRaisePosition)
                    {
                        case PositionEnum.UnderTheGun:
                            return Translate(ConvertStringArrayToIntegerMatrix(GetMpAgainstOpenRaiseFromUtgLines()),
                                TranslateIntToDecisionAgainstOpenRaiseEnum);
                        default:
                            throw new InvalidOperationException();
                    }
                case PositionEnum.CuttingOff:
                    switch (openRaisePosition)
                    {
                        case PositionEnum.UnderTheGun:
                            return Translate(ConvertStringArrayToIntegerMatrix(GetCoAgainstOpenRaiseFromUtgLines()),
                                TranslateIntToDecisionAgainstOpenRaiseEnum);
                        case PositionEnum.MiddlePosition:
                            return Translate(ConvertStringArrayToIntegerMatrix(GetCoAgainstOpenRaiseFromMpLines()),
                                TranslateIntToDecisionAgainstOpenRaiseEnum);
                        default:
                            throw new InvalidOperationException();
                    }
                case PositionEnum.Button:
                    switch (openRaisePosition)
                    {
                        case PositionEnum.UnderTheGun:
                            return Translate(ConvertStringArrayToIntegerMatrix(GetBtnAgainstOpenRaiseFromUtgLines()),
                                TranslateIntToDecisionAgainstOpenRaiseEnum);
                        case PositionEnum.MiddlePosition:
                            return Translate(ConvertStringArrayToIntegerMatrix(GetBtnAgainstOpenRaiseFromMpLines()),
                                TranslateIntToDecisionAgainstOpenRaiseEnum);
                        case PositionEnum.CuttingOff:
                            return Translate(ConvertStringArrayToIntegerMatrix(GetBtnAgainstOpenRaiseFromCoLines()),
                                TranslateIntToDecisionAgainstOpenRaiseEnum);
                        default:
                            throw new InvalidOperationException();
                    }
                case PositionEnum.SmallBlind:
                    switch (openRaisePosition)
                    {
                        case PositionEnum.UnderTheGun:
                            return Translate(ConvertStringArrayToIntegerMatrix(GetSbAgainstOpenRaiseFromUtgLines()),
                                TranslateIntToDecisionAgainstOpenRaiseEnum);
                        case PositionEnum.MiddlePosition:
                            return Translate(ConvertStringArrayToIntegerMatrix(GetSbAgainstOpenRaiseFromMpLines()),
                                TranslateIntToDecisionAgainstOpenRaiseEnum);
                        case PositionEnum.CuttingOff:
                            return Translate(ConvertStringArrayToIntegerMatrix(GetSbAgainstOpenRaiseFromCoLines()),
                                TranslateIntToDecisionAgainstOpenRaiseEnum);
                        case PositionEnum.Button:
                            return Translate(ConvertStringArrayToIntegerMatrix(GetSbAgainstOpenRaiseFromBtnLines()),
                                TranslateIntToDecisionAgainstOpenRaiseEnum);
                        default:
                            throw new InvalidOperationException();
                    }
                case PositionEnum.BigBlind:
                    switch (openRaisePosition)
                    {
                        case PositionEnum.UnderTheGun:
                            return Translate(ConvertStringArrayToIntegerMatrix(GetBbAgainstOpenRaiseFromUtgLines()),
                                TranslateIntToDecisionAgainstOpenRaiseEnum);
                        case PositionEnum.MiddlePosition:
                            return Translate(ConvertStringArrayToIntegerMatrix(GetBbAgainstOpenRaiseFromMpLines()),
                                TranslateIntToDecisionAgainstOpenRaiseEnum);
                        case PositionEnum.CuttingOff:
                            return Translate(ConvertStringArrayToIntegerMatrix(GetBbAgainstOpenRaiseFromCoLines()),
                                TranslateIntToDecisionAgainstOpenRaiseEnum);
                        case PositionEnum.Button:
                            return Translate(ConvertStringArrayToIntegerMatrix(GetBbAgainstOpenRaiseFromBtnLines()),
                                TranslateIntToDecisionAgainstOpenRaiseEnum);
                        case PositionEnum.SmallBlind:
                            return Translate(ConvertStringArrayToIntegerMatrix(GetBbAgainstOpenRaiseFromSbLines()),
                                TranslateIntToDecisionAgainstOpenRaiseEnum);
                        default:
                            throw new InvalidOperationException();
                    }
                default:
                    throw new InvalidOperationException();
            }
        }

        private static DecisionAgainstOpenRaiseEnum TranslateIntToDecisionAgainstOpenRaiseEnum(this int value)
        {
            switch (value)
            {
                case 0:
                    return DecisionAgainstOpenRaiseEnum.Fold;
                case 1:
                    return DecisionAgainstOpenRaiseEnum.Call;
                case 2:
                    return DecisionAgainstOpenRaiseEnum.BluffThreeBet;
                case 3:
                    return DecisionAgainstOpenRaiseEnum.ValueThreeBet;
                default:
                    throw new InvalidOperationException();
            }
        }

        public static IChart<DecisionAgainstThreeBetEnum> GetDecisionAgainstThreeBetChart(PositionEnum heroPosition, PositionEnum threeBetPosition)
        {
            switch (heroPosition)
            {
                case PositionEnum.UnderTheGun:
                    switch (threeBetPosition)
                    {
                        case PositionEnum.MiddlePosition:
                            return Translate(ConvertStringArrayToIntegerMatrix(GetUtgAgainst3BetFromMpLines()), TranslateIntToDecisionAgainstThreeBetEnum);
                        case PositionEnum.CuttingOff:
                            return Translate(ConvertStringArrayToIntegerMatrix(GetUtgAgainst3BetFromCoLines()), TranslateIntToDecisionAgainstThreeBetEnum);
                        case PositionEnum.Button:
                            return Translate(ConvertStringArrayToIntegerMatrix(GetUtgAgainst3BetFromBtnLines()), TranslateIntToDecisionAgainstThreeBetEnum);
                        case PositionEnum.SmallBlind:
                            return Translate(ConvertStringArrayToIntegerMatrix(GetUtgAgainst3BetFromSbLines()), TranslateIntToDecisionAgainstThreeBetEnum);
                        case PositionEnum.BigBlind:
                            return Translate(ConvertStringArrayToIntegerMatrix(GetUtgAgainst3BetFromBbLines()), TranslateIntToDecisionAgainstThreeBetEnum);
                        default:
                            throw new InvalidOperationException();
                    }
                case PositionEnum.MiddlePosition:
                    switch(threeBetPosition)
                    {
                        case PositionEnum.CuttingOff:
                            return Translate(ConvertStringArrayToIntegerMatrix(GetMpAgainst3BetFromCoLines()), TranslateIntToDecisionAgainstThreeBetEnum);
                        case PositionEnum.Button:
                            return Translate(ConvertStringArrayToIntegerMatrix(GetMpAgainst3BetFromBtnLines()), TranslateIntToDecisionAgainstThreeBetEnum);
                        case PositionEnum.SmallBlind:
                            return Translate(ConvertStringArrayToIntegerMatrix(GetMpAgainst3BetFromSbLines()), TranslateIntToDecisionAgainstThreeBetEnum);
                        case PositionEnum.BigBlind:
                            return Translate(ConvertStringArrayToIntegerMatrix(GetMpAgainst3BetFromBbLines()), TranslateIntToDecisionAgainstThreeBetEnum);
                        default:
                            throw new InvalidOperationException();
                    }
                case PositionEnum.CuttingOff:
                    switch (threeBetPosition)
                    {
                        case PositionEnum.Button:
                            return Translate(ConvertStringArrayToIntegerMatrix(GetCoAgainst3BetFromBtnLines()), TranslateIntToDecisionAgainstThreeBetEnum);
                        case PositionEnum.SmallBlind:
                            return Translate(ConvertStringArrayToIntegerMatrix(GetCoAgainst3BetFromSbLines()), TranslateIntToDecisionAgainstThreeBetEnum);
                        case PositionEnum.BigBlind:
                            return Translate(ConvertStringArrayToIntegerMatrix(GetCoAgainst3BetFromBbLines()), TranslateIntToDecisionAgainstThreeBetEnum);
                        default:
                            throw new InvalidOperationException();
                    }
                case PositionEnum.Button:
                    switch (threeBetPosition)
                    {
                        case PositionEnum.SmallBlind:
                            return Translate(ConvertStringArrayToIntegerMatrix(GetBtnAgainst3BetFromSbLines()), TranslateIntToDecisionAgainstThreeBetEnum);
                        case PositionEnum.BigBlind:
                            return Translate(ConvertStringArrayToIntegerMatrix(GetBtnAgainst3BetFromBbLines()), TranslateIntToDecisionAgainstThreeBetEnum);
                        default:
                            throw new InvalidOperationException();
                    }
                case PositionEnum.SmallBlind:
                    switch (threeBetPosition)
                    {
                        case PositionEnum.BigBlind:
                            return Translate(ConvertStringArrayToIntegerMatrix(GetSbAgainst3BetFromBbLines()), TranslateIntToDecisionAgainstThreeBetEnum);
                        default:
                            throw new InvalidOperationException();
                    }
                case PositionEnum.BigBlind:
                    throw new InvalidOperationException();
                default:
                    throw new InvalidOperationException();
            }
        }

        private static DecisionAgainstThreeBetEnum TranslateIntToDecisionAgainstThreeBetEnum(this int value)
        {
            switch (value)
            {
                case 0:
                    return DecisionAgainstThreeBetEnum.Fold;
                case 1:
                    return DecisionAgainstThreeBetEnum.Call;
                case 2:
                    return DecisionAgainstThreeBetEnum.BluffFourBet;
                case 3:
                    return DecisionAgainstThreeBetEnum.ValueFourBet;
                default:
                    throw new InvalidOperationException();
            }
        }

        #endregion

        private static int[,] ConvertStringArrayToIntegerMatrix(string[] lines)
        {
            var matrix = new int[13, 13];
            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    var ch = lines[i][j];
                    if (ch == ' ')
                        matrix[i, j] = 0;
                    else
                        matrix[i, j] = (int)char.GetNumericValue(ch);
                }
            }

            return matrix;
        }

        #region string[]

        //UTG 

        //OPEN RAISE
        private static string[] GetUtgOpenRaiseLines()
        {
            return new[]
            {
                "1111100000000",
                "1111100000000",
                "1111100000000",
                "1001110000000",
                "0000111000000",
                "0000011100000",
                "0000001100000",
                "0000000110000",
                "0000000010000",
                "0000000001000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
            };
        }


        //AGAINST OPEN RAISE

        //AGAINST 3 BET
        private static string[] GetUtgAgainst3BetFromMpLines()
        {
            return new[]
            {
                "3220000000000",
                "3320000000000",
                "1120000000000",
                "0002000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
            };
        }

        private static string[] GetUtgAgainst3BetFromCoLines()
        {
            return new[]
            {
                "3221000000000",
                "3320000000000",
                "1120000000000",
                "0002000000000",
                "0000200000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
            };
        }

        private static string[] GetUtgAgainst3BetFromBtnLines()
        {
            return new[]
            {
                "3221000000000",
                "3320000000000",
                "1120000000000",
                "0001000000000",
                "0000100000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
            };
        }

        private static string[] GetUtgAgainst3BetFromSbLines()
        {
            return new[]
            {
                "3220000000000",
                "2300000000000",
                "1020000000000",
                "0002000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
            };
        }

        private static string[] GetUtgAgainst3BetFromBbLines()
        {
            return new[]
            {
                "2220000000000",
                "2220000000000",
                "1022000000000",
                "0002200000000",
                "0000200000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
            };
        }

        //MP
        //OPEN RAISE
        private static string[] GetMpOpenRaiseLines()
        {
            return new[]
            {
                "1111100001111",
                "1111100000000",
                "1111100000000",
                "1101110000000",
                "0000111000000",
                "0000011100000",
                "0000001100000",
                "0000000110000",
                "0000000011000",
                "0000000001000",
                "0000000000100",
                "0000000000010",
                "0000000000001"
            };
        }

        //AGAINST OPEN RAISE
        private static string[] GetMpAgainstOpenRaiseFromUtgLines()
        {
            return new[]
            {
                "3221000000000",
                "2320000000000",
                "1020000000000",
                "0002000000000",
                "0000210000000",
                "0000021000000",
                "0000002100000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
            };
        }

        //AGAINST 3 BET
        private static string[] GetMpAgainst3BetFromCoLines()
        {
            return new[]
            {
                "3221000000000",
                "2320000000000",
                "1120000000000",
                "1002000000000",
                "0000100000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
            };
        }

        private static string[] GetMpAgainst3BetFromBtnLines()
        {
            return new[]
            {
                "3221000000000",
                "2320000000000",
                "1120000000000",
                "1002000000000",
                "0000200000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
            };
        }

        private static string[] GetMpAgainst3BetFromSbLines()
        {
            return new[]
            {
                "3220000000000",
                "3320000000000",
                "1120000000000",
                "0002000000000",
                "0000200000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
            };
        }

        private static string[] GetMpAgainst3BetFromBbLines()
        {
            return new[]
            {
                "2220000000000",
                "2210000000000",
                "0020000000000",
                "0002200000000",
                "0000200000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
            };
        }

        //CO
        //OPEN RAISE

        private static string[] GetCoOpenRaiseLines()
        {
            return new[]
            {
                "1111111111100",
                "1111111100000",
                "1111111000000",
                "1111111100000",
                "1111111100000",
                "1111111110000",
                "0000011111000",
                "0000000111000",
                "0000000011000",
                "0000000001100",
                "0000000000100",
                "0000000000010",
                "0000000000001",
            };
        }

        //AGAINST OPEN RAISE
        private static string[] GetCoAgainstOpenRaiseFromUtgLines()
        {
            return new[]
            {
                "3221000000000",
                "2320000000000",
                "1022000000000",
                "0002200000000",
                "0000220000000",
                "0000022000000",
                "0000002000000",
                "0000000200000",
                "0000000020000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
            };
        }

        private static string[] GetCoAgainstOpenRaiseFromMpLines()
        {
            return new[]
            {
                "3222200000000",
                "2322200000000",
                "2122200000000",
                "1102200000000",
                "0000220000000",
                "0000022000000",
                "0000002100000",
                "0000000210000",
                "0000000021000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
            };
        }

        //AGAINST 3 BET
        private static string[] GetCoAgainst3BetFromBtnLines()
        {
            return new[]
            {
                "3222100000000",
                "2322100000000",
                "2122000000000",
                "1102200000000",
                "0000200000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
            };
        }
        private static string[] GetCoAgainst3BetFromSbLines()
        {
            return new[]
            {
                "3222210000000",
                "2322210000000",
                "2222200000000",
                "2112200000000",
                "1000220000000",
                "0000022000000",
                "0000002000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
            };
        }
        private static string[] GetCoAgainst3BetFromBbLines()
        {
            return new[]
            {
                "3222200000000",
                "2322200000000",
                "2222200000000",
                "2112200000000",
                "1000220000000",
                "0000022000000",
                "0000002200000",
                "0000000200000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
            };
        }

        //BTN
        //OPEN RAISE
        private static string[] GetBtnOpenRaiseLines()
        {
            return new[]
            {
                "1111111111111",
                "1111111111111",
                "1111111111111",
                "1111111111110",
                "1111111111110",
                "1111111111100",
                "1111111111100",
                "1111111111110",
                "1100111111110",
                "1100000011110",
                "1000000000110",
                "1000000000010",
                "1000000000001",
            };
        }

        //AGAINST OPEN RAISE
        private static string[] GetBtnAgainstOpenRaiseFromUtgLines()
        {
            return new[]
            {
                "3222000000000",
                "3320000000000",
                "1130000000000",
                "0002200000000",
                "0000220000000",
                "0000022000000",
                "0000002100000",
                "0000000210000",
                "0000000021000",
                "0000000002000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
            };
        }

        private static string[] GetBtnAgainstOpenRaiseFromMpLines()
        {
            return new[]
            {
                "3222100000000",
                "2321000000000",
                "2222000000000",
                "1102210000000",
                "0000221000000",
                "0000022100000",
                "0000002100000",
                "0000000210000",
                "0000000020000",
                "0000000002000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
            };
        }

        private static string[] GetBtnAgainstOpenRaiseFromCoLines()
        {
            return new[]
            {
                "3322220001111",
                "2322221000000",
                "2222221000000",
                "2222221000000",
                "1000222100000",
                "0000022210000",
                "0000002210000",
                "0000000210000",
                "0000000021000",
                "0000000002100",
                "0000000000000",
                "0000000000000",
                "0000000000000",
            };
        }

        //AGAINST 3 BET
        private static string[] GetBtnAgainst3BetFromSbLines()
        {
            return new[]
            {
                "3222200002222",
                "2322200000000",
                "2222200000000",
                "2112220000000",
                "1100222000000",
                "0000022200000",
                "0000002200000",
                "0000000220000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
            };
        }

        private static string[] GetBtnAgainst3BetFromBbLines()
        {
            return new[]
            {
                "3222200002222",
                "2322200000000",
                "2222200000000",
                "2112220000000",
                "1100222000000",
                "0000022200000",
                "0000002200000",
                "0000000220000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
            };
        }

        //SB
        //OPEN RAISE
        private static string[] GetSbOpenRaiseLines()
        {
            return new[]
            {
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
        }

        //AGAINST OPEN RAISE
        private static string[] GetSbAgainstOpenRaiseFromUtgLines()
        {
            return new[]
            {
                "2220000000000",
                "2210000000000",
                "1020000000000",
                "0002000000000",
                "0000200000000",
                "0000020000000",
                "0000002000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
            };
        }

        private static string[] GetSbAgainstOpenRaiseFromMpLines()
        {
            return new[]
            {
                "2220000000000",
                "2220000000000",
                "1120000000000",
                "0002200000000",
                "0000220000000",
                "0000020000000",
                "0000002000000",
                "0000000200000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
            };
        }

        private static string[] GetSbAgainstOpenRaiseFromCoLines()
        {
            return new[]
            {
                "3222100001111",
                "2322100000000",
                "2222100000000",
                "2102210000000",
                "0000221000000",
                "0000022100000",
                "0000002110000",
                "0000000210000",
                "0000000020000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
            };
        }

        private static string[] GetSbAgainstOpenRaiseFromBtnLines()
        {
            return new[]
            {
                "3222210001111",
                "2322210000000",
                "2222210000000",
                "2222210000000",
                "2110221000000",
                "0000022100000",
                "0000002210000",
                "0000000221000",
                "0000000021000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
            };
        }

        //AGAINST 3 BET
        private static string[] GetSbAgainst3BetFromBbLines()
        {
            return new[]
            {
                "3222110000000",
                "2322100000000",
                "2232100000000",
                "1112200000000",
                "0000220000000",
                "0000002000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
            };
        }

        //BB
        //OPEN RAISE
        private static string[] GetBbOpenRaiseLines()
        {
            throw new NotImplementedException();
        }

        //AGAINST OPEN RAISE
        private static string[] GetBbAgainstOpenRaiseFromUtgLines()
        {
            return new[]
            {
                "2221000000000",
                "2220000000000",
                "2120000000000",
                "0002200000000",
                "0000220000000",
                "0000020000000",
                "0000002000000",
                "0000000200000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
            };
        }

        private static string[] GetBbAgainstOpenRaiseFromMpLines()
        {
            return new[]
            {
                "2222000000000",
                "2221000000000",
                "2220000000000",
                "1002200000000",
                "0000220000000",
                "0000020000000",
                "0000002000000",
                "0000000200000",
                "0000000020000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
            };
        }

        private static string[] GetBbAgainstOpenRaiseFromCoLines()
        {
            return new[]
            {
                "3222222222222",
                "2322222110000",
                "1222221000000",
                "2222221000000",
                "2211222100000",
                "1000022200000",
                "0000002210000",
                "0000000210000",
                "0000000021000",
                "0000000002000",
                "0000000000000",
                "0000000000000",
                "0000000000000",
            };
        }

        private static string[] GetBbAgainstOpenRaiseFromBtnLines()
        {
            return new[]
            {
                "3222222222222",
                "2322222222222",
                "2222222222222",
                "2222222221100",
                "2222222221100",
                "2222222221100",
                "2222222222100",
                "2222111222100",
                "2110000122210",
                "2000000002210",
                "2000000000210",
                "2000000000020",
                "2000000000002",
            };
        }

        private static string[] GetBbAgainstOpenRaiseFromSbLines()
        {
            return new[]
            {
                "3222222222222",
                "2322222211111",
                "2222222211111",
                "2222222200000",
                "2222222200000",
                "2222222200000",
                "2222222220000",
                "2110000222000",
                "1110000022200",
                "1000000002200",
                "1000000000200",
                "1000000000020",
                "1000000000002",
            };
        }

        //AGAINST 3 BET

        #endregion
    }
}
