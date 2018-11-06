using System;
using System.Collections.Generic;
using Common;
using Infra;
using Models;
using Models.Ranging;
using River.RiverBoardRankTextures;
using Turn;
using Turn.TurnBoardSuitTextures;
using OffsuitTexture = Turn.TurnBoardSuitTextures.OffsuitTexture;
using SuitedThreeTexture = Turn.TurnBoardSuitTextures.SuitedThreeTexture;

namespace River
{
    public class RiverRangeSqueezer
    {
        private readonly PlayerRangeSqueezer _playerRangeSqueezer = new PlayerRangeSqueezer();

        public PlayerRange Squeeze(PlayerRange previousRange, Move lastMove, int bigBlindSize, RiverBoard riverBoard, HoldingHoles heroHoles)
        {
            Logger.Instance.Log($"Squeezing {lastMove.Player.Name}'s range based on his {lastMove.Decision.DecisionType} with {lastMove.Decision.ChipsAdded} chips");
            switch (lastMove.Decision.DecisionType)
            {
                case DecisionType.AllIn:
                case DecisionType.AllInRaise:
                case DecisionType.Raise:
                case DecisionType.Reraise:
                    return SqueezeOnRaise(previousRange, lastMove, bigBlindSize, riverBoard, heroHoles);
                case DecisionType.Call:
                    return SqueezeOnCall(previousRange, lastMove, bigBlindSize, riverBoard, heroHoles);
                case DecisionType.Check:
                    return SqueezeOnCheck(previousRange, lastMove, bigBlindSize, riverBoard, heroHoles);
                default:
                    throw new InvalidOperationException($"{lastMove.Decision.DecisionType} should not show in Turn Squeeze");
            }
        }

        private PlayerRange SqueezeOnRaise(PlayerRange previousRange, Move lastMove, int bigBlindSize, RiverBoard riverBoard, HoldingHoles heroHoles)
        {
            return _playerRangeSqueezer.Squeeze(previousRange, grid => TestGrid(riverBoard, grid), TestOutcomeOnRaise, GetCards(riverBoard, heroHoles));
        }

        private PlayerRange SqueezeOnCall(PlayerRange previousRange, Move lastMove, int bigBlindSize, RiverBoard riverBoard, HoldingHoles heroHoles)
        {
            return _playerRangeSqueezer.Squeeze(previousRange, grid => TestGrid(riverBoard, grid), TestOutcomeOnCall, GetCards(riverBoard, heroHoles));
        }

        private PlayerRange SqueezeOnCheck(PlayerRange previousRange, Move lastMove, int bigBlindSize, RiverBoard riverBoard, HoldingHoles heroHoles)
        {
            return _playerRangeSqueezer.Squeeze(previousRange, grid => TestGrid(riverBoard, grid), TestOutcomeOnCheck, GetCards(riverBoard, heroHoles));
        }

        private List<Card> GetCards(RiverBoard riverBoard, HoldingHoles heroHoles)
        {
            var flopBoard = riverBoard.TurnBoard.FlopBoard;
            var turnBoard = riverBoard.TurnBoard;
            return new List<Card>() { flopBoard.Flop1, flopBoard.Flop2, flopBoard.Flop3, turnBoard.TurnCard, riverBoard.River, heroHoles.Hole1, heroHoles.Hole2 };
        }

        private Tuple<bool, PlayerRangeGridStatusEnum> TestOutcomeOnRaise(GridHitNewRoundResultEnum outcome)
        {
            switch (outcome)
            {
                case GridHitNewRoundResultEnum.None:
                case GridHitNewRoundResultEnum.Unavailable:
                    return new Tuple<bool, PlayerRangeGridStatusEnum>(true, PlayerRangeGridStatusEnum.Excluded);
            }
            return new Tuple<bool, PlayerRangeGridStatusEnum>(false, PlayerRangeGridStatusEnum.Undefined);
        }

        private Tuple<bool, PlayerRangeGridStatusEnum> TestOutcomeOnCall(GridHitNewRoundResultEnum outcome)
        {
            switch (outcome)
            {
                case GridHitNewRoundResultEnum.None:
                case GridHitNewRoundResultEnum.Unavailable:
                    return new Tuple<bool, PlayerRangeGridStatusEnum>(true, PlayerRangeGridStatusEnum.Excluded);
            }
            return new Tuple<bool, PlayerRangeGridStatusEnum>(false, PlayerRangeGridStatusEnum.Undefined);
        }

        private Tuple<bool, PlayerRangeGridStatusEnum> TestOutcomeOnCheck(GridHitNewRoundResultEnum outcome)
        {
            switch (outcome)
            {
                case GridHitNewRoundResultEnum.Promoted:
                case GridHitNewRoundResultEnum.Unavailable:
                    //how about Elite?
                    return new Tuple<bool, PlayerRangeGridStatusEnum>(true, PlayerRangeGridStatusEnum.Excluded);
            }
            return new Tuple<bool, PlayerRangeGridStatusEnum>(false, PlayerRangeGridStatusEnum.Undefined);
        }

        private GridStatusHittingTurn TestGrid(RiverBoard riverBoard, RangeGrid grid)
        {
            var result = new GridStatusHittingTurn(grid.Category);
            var rankTester = GetGridRankTester(riverBoard);
            var rankResult = rankTester.Invoke(grid);
            result.RankWiseStatus = rankResult;

            switch (result.Category)
            {
                case GridCategoryEnum.Suited:
                    result.SuitedStatus = GetSuitedTester(riverBoard).Invoke(grid);
                    break;
                case GridCategoryEnum.Paired:
                    result.PairedStatus = GetPairedTester(riverBoard).Invoke(grid);
                    break;
                case GridCategoryEnum.Offsuit:
                    result.OffsuitStatus = GetOffsuitTester(riverBoard).Invoke(grid);
                    break;
            }

            return result;
        }

        private Func<RangeGrid, GridHitNewRoundResultEnum> GetGridRankTester(RiverBoard riverBoard)
        {
            switch (riverBoard.RankTexture)
            {
                case RiverRankTextureEnum.Foursome:
                    return new RiverBoardRankTextures.FoursomeRankTexture(riverBoard).HitRiver;
                case RiverRankTextureEnum.HighTriLowPair:
                    return new HighTriLowPairRankTexture(riverBoard).HitRiver;
                case RiverRankTextureEnum.LowTriHighPair:
                    return new LowTriHighPairRankTexture(riverBoard).HitRiver;
                case RiverRankTextureEnum.HighTri:
                    return new RiverBoardRankTextures.HighTriRankTexture(riverBoard).HitRiver;
                case RiverRankTextureEnum.MiddleTri:
                    return new MiddleTriRankTexture(riverBoard).HitRiver;
                case RiverRankTextureEnum.LowTri:
                    return new RiverBoardRankTextures.LowTriRankTexture(riverBoard).HitRiver;
                case RiverRankTextureEnum.HighTwoPairs:
                    return new HighTwoPairsRankTexture(riverBoard).HitRiver;
                case RiverRankTextureEnum.HighLowTwoPairs:
                    return new HighLowTwoPairsRankTexture(riverBoard).HitRiver;
                case RiverRankTextureEnum.LowTwoPairs:
                    return new LowTwoPairsRankTexture(riverBoard).HitRiver;
                case RiverRankTextureEnum.HighPair:
                    return new RiverBoardRankTextures.HighPairRankTexture(riverBoard).HitRiver;
                case RiverRankTextureEnum.SecondPair:
                    return new SecondPairRankTexture(riverBoard).HitRiver;
                case RiverRankTextureEnum.ThirdPair:
                    return new ThirdPairRankTexture(riverBoard).HitRiver;
                case RiverRankTextureEnum.LowPair:
                    return new RiverBoardRankTextures.LowPairRankTexture(riverBoard).HitRiver;
                case RiverRankTextureEnum.Singles:
                    return new RiverBoardRankTextures.SinglesRankTexture(riverBoard).HitRiver;
                default:
                    throw new InvalidOperationException();
            }
        }

        private Func<RangeGrid, SuitedStatus<GridHitNewRoundResultEnum>> GetSuitedTester(RiverBoard riverBoard)
        {
            return GenerateSuitTester(riverBoard).TestSuitedGrid;
        }

        private Func<RangeGrid, PairedStatus<GridHitNewRoundResultEnum>> GetPairedTester(RiverBoard riverBoard)
        {
            return GenerateSuitTester(riverBoard).TestPairedGrid;
        }

        private Func<RangeGrid, OffsuitStatus<GridHitNewRoundResultEnum>> GetOffsuitTester(RiverBoard riverBoard)
        {
            return GenerateSuitTester(riverBoard).TestOffsuitGrid;
        }

        private GridSuitTester GenerateSuitTester(RiverBoard riverBoard)
        {
            return new GridSuitTester(GetTurnGridGrader(riverBoard.TurnBoard), GetRiverGridGrader(riverBoard));
        }

        private Func<Card, Card, Tuple<SuitHandGradeEnum, int>> GetRiverGridGrader(RiverBoard riverBoard)
        {
            switch (riverBoard.SuitTexture)
            {
                case RiverSuitTextureEnum.FiveSuited:
                    return (c1, c2) =>
                    {
                        var tuple =
                            new River.RiverBoardSuitTextures.SuitedFiveTexture(riverBoard).TestGridAgainstBoard(c1, c2);
                        return new Tuple<SuitHandGradeEnum, int>(tuple.Item1.Grade(), tuple.Item2);
                    };

                case RiverSuitTextureEnum.FourSuited:
                    return (c1, c2) =>
                    {
                        var tuple = new River.RiverBoardSuitTextures.SuitedFourTexture(riverBoard).TestGridAgainstBoard(c1, c2);
                        return new Tuple<SuitHandGradeEnum, int>(tuple.Item1.Grade(), tuple.Item2);
                    };

                case RiverSuitTextureEnum.ThreeSuited:
                    return (c1, c2) =>
                    {
                        var tuple = new River.RiverBoardSuitTextures.SuitedThreeTexture(riverBoard).TestGridAgainstBoard(c1, c2);
                        return new Tuple<SuitHandGradeEnum, int>(tuple.Item1.Grade(), tuple.Item2);
                    };
                case RiverSuitTextureEnum.Offsuit:
                    return (c1, c2) =>
                    {
                        var tuple = new River.RiverBoardSuitTextures.OffsuitTexture().TestGridAgainstBoard(c1, c2);
                        return new Tuple<SuitHandGradeEnum, int>(tuple.Item1.Grade(), tuple.Item2);
                    };
                default:
                    throw new InvalidOperationException();
            }
        }

        private Func<Card, Card, Tuple<SuitHandGradeEnum, int>> GetTurnGridGrader(TurnBoard turnBoard)
        {
            switch (turnBoard.SuitTexture)
            {
                case TurnBoardSuitTextureEnum.SuitedFour:
                    return (c1, c2) =>
                    {
                        var tuple = new Turn.TurnBoardSuitTextures.SuitedFourTexture(turnBoard).TestGridAgainstTurnBoard(c1, c2);
                        return new Tuple<SuitHandGradeEnum, int>(tuple.Item1.Grade(), tuple.Item2);
                    };
                case TurnBoardSuitTextureEnum.SuitedThree:
                    return (c1, c2) =>
                    {
                        var tuple = new SuitedThreeTexture(turnBoard).TestGridAgainstTurnBoard(c1, c2);
                        return new Tuple<SuitHandGradeEnum, int>(tuple.Item1.Grade(), tuple.Item2);
                    };
                case TurnBoardSuitTextureEnum.SuitedTwoPairs:
                    return (c1, c2) =>
                    {
                        var tuple = new SuitedTwoPairsTexture(turnBoard).TestGridAgainstTurnBoard(c1, c2);
                        return new Tuple<SuitHandGradeEnum, int>(tuple.Item1.Grade(), tuple.Item2);
                    };
                case TurnBoardSuitTextureEnum.SuitedTwo:
                    return (c1, c2) =>
                    {
                        var tuple = new SuitedTwoTexture(turnBoard).TestGridAgainstTurnBoard(c1, c2);
                        return new Tuple<SuitHandGradeEnum, int>(tuple.Item1.Grade(), tuple.Item2);
                    };
                case TurnBoardSuitTextureEnum.Offsuit:
                    return (c1, c2) =>
                    {
                        var tuple = new OffsuitTexture(turnBoard).TestGridAgainstTurnBoard(c1, c2);
                        return new Tuple<SuitHandGradeEnum, int>(tuple.Item1.Grade(), tuple.Item2);
                    };
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
