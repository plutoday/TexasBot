using System;
using System.Collections.Generic;
using Common;
using Flop;
using Flop.FlopBoardSuitTextures;
using Infra;
using Models;
using Models.Ranging;
using Turn.TurnBoardRankTextures;
using Turn.TurnBoardSuitTextures;

namespace Turn
{
    public class TurnRangeSqueezer
    {
        private readonly PlayerRangeSqueezer _playerRangeSqueezer = new PlayerRangeSqueezer();

        public PlayerRange Squeeze(PlayerRange previousRange, Move lastMove, int bigBlindSize, TurnBoard turnBoard, HoldingHoles heroHoles)
        {
            Logger.Instance.Log($"Squeezing {lastMove.Player.Name}'s range based on his {lastMove.Decision.DecisionType} with {lastMove.Decision.ChipsAdded} chips");
            switch (lastMove.Decision.DecisionType)
            {
                case DecisionType.AllIn:
                case DecisionType.AllInRaise:
                case DecisionType.Raise:
                case DecisionType.Reraise:
                    return SqueezeOnRaise(previousRange, lastMove, bigBlindSize, turnBoard, heroHoles);
                case DecisionType.Call:
                    return SqueezeOnCall(previousRange, lastMove, bigBlindSize, turnBoard, heroHoles);
                case DecisionType.Check:
                    return SqueezeOnCheck(previousRange, lastMove, bigBlindSize, turnBoard, heroHoles);
                default:
                    throw new InvalidOperationException($"{lastMove.Decision.DecisionType} should not show in Turn Squeeze");
            }
        }

        private PlayerRange SqueezeOnRaise(PlayerRange previousRange, Move lastMove, int bigBlindSize, TurnBoard turnBoard, HoldingHoles heroHoles)
        {
            return _playerRangeSqueezer.Squeeze(previousRange, grid => TestGrid(turnBoard, grid), TestOutcomeOnRaise, GetCards(turnBoard, heroHoles));
        }

        private PlayerRange SqueezeOnCall(PlayerRange previousRange, Move lastMove, int bigBlindSize, TurnBoard turnBoard, HoldingHoles heroHoles)
        {
            return _playerRangeSqueezer.Squeeze(previousRange, grid => TestGrid(turnBoard, grid), TestOutcomeOnCall, GetCards(turnBoard, heroHoles));
        }

        private PlayerRange SqueezeOnCheck(PlayerRange previousRange, Move lastMove, int bigBlindSize, TurnBoard turnBoard, HoldingHoles heroHoles)
        {
            return _playerRangeSqueezer.Squeeze(previousRange, grid => TestGrid(turnBoard, grid), TestOutcomeOnCheck, GetCards(turnBoard, heroHoles));
        }

        private List<Card> GetCards(TurnBoard turnBoard, HoldingHoles heroHoles)
        {
            var flopBoard = turnBoard.FlopBoard;
            return new List<Card>() { flopBoard.Flop1, flopBoard.Flop2, flopBoard.Flop3, turnBoard.TurnCard, heroHoles.Hole1, heroHoles.Hole2 };
        }

        private Tuple<bool, PlayerRangeGridStatusEnum> TestOutcomeOnRaise(GridHitNewRoundResultEnum outcome)
        {
            switch (outcome)
            {
                case GridHitNewRoundResultEnum.None:
                    return new Tuple<bool, PlayerRangeGridStatusEnum>(true, PlayerRangeGridStatusEnum.Excluded);
            }
            return new Tuple<bool, PlayerRangeGridStatusEnum>(false, PlayerRangeGridStatusEnum.Undefined);
        }

        private Tuple<bool, PlayerRangeGridStatusEnum> TestOutcomeOnCall(GridHitNewRoundResultEnum outcome)
        {
            switch (outcome)
            {
                case GridHitNewRoundResultEnum.None:
                    return new Tuple<bool, PlayerRangeGridStatusEnum>(true, PlayerRangeGridStatusEnum.Excluded);
            }
            return new Tuple<bool, PlayerRangeGridStatusEnum>(false, PlayerRangeGridStatusEnum.Undefined);
        }

        private Tuple<bool, PlayerRangeGridStatusEnum> TestOutcomeOnCheck(GridHitNewRoundResultEnum outcome)
        {
            switch (outcome)
            {
                case GridHitNewRoundResultEnum.Promoted:
                    //how about Elite?
                    return new Tuple<bool, PlayerRangeGridStatusEnum>(true, PlayerRangeGridStatusEnum.Excluded);
            }
            return new Tuple<bool, PlayerRangeGridStatusEnum>(false, PlayerRangeGridStatusEnum.Undefined);
        }

        private GridStatusHittingTurn TestGrid(TurnBoard turnBoard, RangeGrid grid)
        {
            var result = new GridStatusHittingTurn(grid.Category);
            var rankTester = GetGridRankTester(turnBoard);
            var rankResult = rankTester.Invoke(grid);
            result.RankWiseStatus = rankResult;

            switch (result.Category)
            {
                case GridCategoryEnum.Suited:
                    result.SuitedStatus = GetSuitedTester(turnBoard).Invoke(grid);
                    break;
                case GridCategoryEnum.Paired:
                    result.PairedStatus = GetPairedTester(turnBoard).Invoke(grid);
                    break;
                case GridCategoryEnum.Offsuit:
                    result.OffsuitStatus = GetOffsuitTester(turnBoard).Invoke(grid);
                    break;
            }

            return result;
        }

        private Func<RangeGrid, GridHitNewRoundResultEnum> GetGridRankTester(TurnBoard turnBoard)
        {
            switch (turnBoard.RankTexture)
            {
                case TurnBoardRankTextureEnum.Foursome:
                    return new FoursomeRankTexture(turnBoard).HitTurn;
                case TurnBoardRankTextureEnum.HighTri:
                    return new HighTriRankTexture(turnBoard).HitTurn;
                case TurnBoardRankTextureEnum.LowTri:
                    return new LowTriRankTexture(turnBoard).HitTurn;
                case TurnBoardRankTextureEnum.TwoPairs:
                    return new TwoPairsRankTexture(turnBoard).HitTurn;
                case TurnBoardRankTextureEnum.HighPair:
                    return new Turn.TurnBoardRankTextures.HighPairRankTexture(turnBoard).HitTurn;
                case TurnBoardRankTextureEnum.MiddlePair:
                    return new Turn.TurnBoardRankTextures.MiddlePairRankTexture(turnBoard).HitTurn;
                case TurnBoardRankTextureEnum.LowPair:
                    return new Turn.TurnBoardRankTextures.LowPairRankTexture(turnBoard).HitTurn;
                case TurnBoardRankTextureEnum.Singles:
                    return new Turn.TurnBoardRankTextures.SinglesRankTexture(turnBoard).HitTurn;
                default:
                    throw new NotImplementedException();
            }
        }

        private Func<RangeGrid, SuitedStatus<GridHitNewRoundResultEnum>> GetSuitedTester(TurnBoard turnBoard)
        {
            return GenerateSuitTester(turnBoard).TestSuitedGrid;
        }

        private Func<RangeGrid, PairedStatus<GridHitNewRoundResultEnum>> GetPairedTester(TurnBoard turnBoard)
        {
            return GenerateSuitTester(turnBoard).TestPairedGrid;
        }

        private Func<RangeGrid, OffsuitStatus<GridHitNewRoundResultEnum>> GetOffsuitTester(TurnBoard turnBoard)
        {
            return GenerateSuitTester(turnBoard).TestOffsuitGrid;
        }

        private Func<Card, Card, Tuple<SuitHandGradeEnum, int>> GetFlopGridGrader(FlopBoard flopBoard)
        {
            switch (flopBoard.SuitTexture)
            {
                case FlopBoardSuitTextureEnum.SuitedThree:
                    return (c1, c2) =>
                    {
                        var tuple = new SuitedThreeSuitTexture(flopBoard).TestGridAgainstBoard(c1, c2);
                        return new Tuple<SuitHandGradeEnum, int>(tuple.Item1.Grade(), tuple.Item2);
                    };
                case FlopBoardSuitTextureEnum.SuitedTwo:
                    return (c1, c2) =>
                    {
                        var tuple = new SuitedTwoSuitTexture(flopBoard).TestGridAgainstBoard(c1, c2);
                        return new Tuple<SuitHandGradeEnum, int>(tuple.Item1.Grade(), tuple.Item2);
                    };
                case FlopBoardSuitTextureEnum.Rainbow:
                    return (c1, c2) =>
                    {
                        var tuple = new RainbowSuitTexture().TestGridAgainstBoard(c1, c2);
                        return new Tuple<SuitHandGradeEnum, int>(tuple.Item1.Grade(), tuple.Item2);
                    };
                default:
                    throw new InvalidOperationException();
            }
        }

        private GridSuitTester GenerateSuitTester(TurnBoard turnBoard)
        {
            return new GridSuitTester(GetFlopGridGrader(turnBoard.FlopBoard), GetTurnGridGrader(turnBoard));
        }

        private Func<Card, Card, Tuple<SuitHandGradeEnum, int>> GetTurnGridGrader(TurnBoard turnBoard)
        {
            switch (turnBoard.SuitTexture)
            {
                case TurnBoardSuitTextureEnum.SuitedFour:
                    return (c1, c2) =>
                    {
                        var tuple = new SuitedFourTexture(turnBoard).TestGridAgainstTurnBoard(c1, c2);
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

        #region removable
        private PlayerRange Squeeze(PlayerRange previousRange, TurnBoard turnBoard, Func<GridHitNewRoundResultEnum, Tuple<bool, PlayerRangeGridStatusEnum>> tester)
        {
            var newRange = previousRange.Clone();

            Logger.Instance.Log($"Before squeezing:\r\n{newRange.ToString()}");

            foreach (var playerRangeGrid in newRange.GetAliveGrids())
            {
                GridStatusHittingTurn result = TestGrid(turnBoard, playerRangeGrid.Grid);
                Logger.Instance.Log($"{playerRangeGrid.Grid.ToString()} tested against the flop, result is {result.RankWiseStatus}");
                Tuple<bool, PlayerRangeGridStatusEnum> testResult = tester.Invoke(result.RankWiseStatus);
                if (testResult.Item1)
                {
                    playerRangeGrid.PlayerRangeGridStatus.RankWiseStatus = testResult.Item2;
                }
                switch (result.Category)
                {
                    case GridCategoryEnum.Suited:
                        testResult = tester.Invoke(result.SuitedStatus.HeartStatus);
                        if (testResult.Item1) playerRangeGrid.PlayerRangeGridStatus.SuitedStatus.HeartStatus = testResult.Item2;
                        testResult = tester.Invoke(result.SuitedStatus.SpadeStatus);
                        if (testResult.Item1) playerRangeGrid.PlayerRangeGridStatus.SuitedStatus.SpadeStatus = testResult.Item2;
                        testResult = tester.Invoke(result.SuitedStatus.DiamondStatus);
                        if (testResult.Item1) playerRangeGrid.PlayerRangeGridStatus.SuitedStatus.DiamondStatus = testResult.Item2;
                        testResult = tester.Invoke(result.SuitedStatus.ClubStatus);
                        if (testResult.Item1) playerRangeGrid.PlayerRangeGridStatus.SuitedStatus.ClubStatus = testResult.Item2;
                        break;
                    case GridCategoryEnum.Paired:
                        testResult = tester.Invoke(result.PairedStatus.HeartSpadeStatus);
                        if (testResult.Item1) playerRangeGrid.PlayerRangeGridStatus.PairedStatus.HeartSpadeStatus = testResult.Item2;
                        testResult = tester.Invoke(result.PairedStatus.HeartDiamondStatus);
                        if (testResult.Item1) playerRangeGrid.PlayerRangeGridStatus.PairedStatus.HeartDiamondStatus = testResult.Item2;
                        testResult = tester.Invoke(result.PairedStatus.HeartClubStatus);
                        if (testResult.Item1) playerRangeGrid.PlayerRangeGridStatus.PairedStatus.HeartClubStatus = testResult.Item2;
                        testResult = tester.Invoke(result.PairedStatus.SpadeDiamondStatus);
                        if (testResult.Item1) playerRangeGrid.PlayerRangeGridStatus.PairedStatus.SpadeDiamondStatus = testResult.Item2;
                        testResult = tester.Invoke(result.PairedStatus.SpadeClubStatus);
                        if (testResult.Item1) playerRangeGrid.PlayerRangeGridStatus.PairedStatus.SpadeClubStatus = testResult.Item2;
                        testResult = tester.Invoke(result.PairedStatus.DiamondClubStatus);
                        if (testResult.Item1) playerRangeGrid.PlayerRangeGridStatus.PairedStatus.DiamondClubStatus = testResult.Item2;
                        break;
                    case GridCategoryEnum.Offsuit:
                        testResult = tester.Invoke(result.OffsuitStatus.HeartSpadeStatus);
                        if (testResult.Item1) playerRangeGrid.PlayerRangeGridStatus.OffsuitStatus.HeartSpadeStatus = testResult.Item2;
                        testResult = tester.Invoke(result.OffsuitStatus.HeartDiamondStatus);
                        if (testResult.Item1) playerRangeGrid.PlayerRangeGridStatus.OffsuitStatus.HeartDiamondStatus = testResult.Item2;
                        testResult = tester.Invoke(result.OffsuitStatus.HeartClubStatus);
                        if (testResult.Item1) playerRangeGrid.PlayerRangeGridStatus.OffsuitStatus.HeartClubStatus = testResult.Item2;
                        testResult = tester.Invoke(result.OffsuitStatus.SpadeDiamondStatus);
                        if (testResult.Item1) playerRangeGrid.PlayerRangeGridStatus.OffsuitStatus.SpadeDiamondStatus = testResult.Item2;
                        testResult = tester.Invoke(result.OffsuitStatus.SpadeClubStatus);
                        if (testResult.Item1) playerRangeGrid.PlayerRangeGridStatus.OffsuitStatus.SpadeClubStatus = testResult.Item2;
                        testResult = tester.Invoke(result.OffsuitStatus.DiamondClubStatus);
                        if (testResult.Item1) playerRangeGrid.PlayerRangeGridStatus.OffsuitStatus.DiamondClubStatus = testResult.Item2;
                        testResult = tester.Invoke(result.OffsuitStatus.SpadeHeartStatus);
                        if (testResult.Item1) playerRangeGrid.PlayerRangeGridStatus.OffsuitStatus.SpadeHeartStatus = testResult.Item2;
                        testResult = tester.Invoke(result.OffsuitStatus.DiamondHeartStatus);
                        if (testResult.Item1) playerRangeGrid.PlayerRangeGridStatus.OffsuitStatus.DiamondHeartStatus = testResult.Item2;
                        testResult = tester.Invoke(result.OffsuitStatus.ClubHeartStatus);
                        if (testResult.Item1) playerRangeGrid.PlayerRangeGridStatus.OffsuitStatus.ClubHeartStatus = testResult.Item2;
                        testResult = tester.Invoke(result.OffsuitStatus.DiamondSpadeStatus);
                        if (testResult.Item1) playerRangeGrid.PlayerRangeGridStatus.OffsuitStatus.DiamondSpadeStatus = testResult.Item2;
                        testResult = tester.Invoke(result.OffsuitStatus.ClubSpadeStatus);
                        if (testResult.Item1) playerRangeGrid.PlayerRangeGridStatus.OffsuitStatus.ClubSpadeStatus = testResult.Item2;
                        testResult = tester.Invoke(result.OffsuitStatus.ClubDiamondStatus);
                        if (testResult.Item1) playerRangeGrid.PlayerRangeGridStatus.OffsuitStatus.ClubDiamondStatus = testResult.Item2;
                        break;
                }
            }

            Logger.Instance.Log($"After squeezing:\r\n{newRange.ToString()}");

            return newRange;
        }
        #endregion
    }
}
