using System;
using System.Collections.Generic;
using Common;
using Flop.FlopBoardRankTextures;
using Flop.FlopBoardSuitTextures;
using Infra;
using Models;
using Models.Ranging;

namespace Flop
{
    public class FlopRangeSqueezer
    {
        private readonly PlayerRangeSqueezer _playerRangeSqueezer = new PlayerRangeSqueezer();

        public PlayerRange Squeeze(PlayerRange previousRange, Move lastMove, int bigBlindSize, FlopBoard flopBoard, HoldingHoles heroHoles)
        {
            Logger.Instance.Log($"Squeezing {lastMove.Player.Name}'s range based on his {lastMove.Decision.DecisionType} with {lastMove.Decision.ChipsAdded} chips");
            switch (lastMove.Decision.DecisionType)
            {
                case DecisionType.AllIn:
                case DecisionType.AllInRaise:
                case DecisionType.Raise:
                case DecisionType.Reraise:
                    return SqueezeOnRaise(previousRange, lastMove, bigBlindSize, flopBoard, heroHoles);
                case DecisionType.Ante:
                    return previousRange;
                case DecisionType.Call:
                    return SqueezeOnCall(previousRange, lastMove, bigBlindSize, flopBoard, heroHoles);
                case DecisionType.Check:
                    return SqueezeOnCheck(previousRange, lastMove, bigBlindSize, flopBoard, heroHoles);
                default:
                    throw new InvalidOperationException($"{lastMove.Decision.DecisionType} should not show in Preflop Squeeze");
            }
        }

        private PlayerRange SqueezeOnRaise(PlayerRange previousRange, Move lastMove, int bigBlindSize, FlopBoard flopBoard, HoldingHoles heroHoles)
        {
            return _playerRangeSqueezer.Squeeze(previousRange, grid => TestGrid(flopBoard, grid), TestOutcomeOnRaise, GetCards(flopBoard, heroHoles));
        }

        private PlayerRange SqueezeOnCall(PlayerRange previousRange, Move lastMove, int bigBlindSize, FlopBoard flopBoard, HoldingHoles heroHoles)
        {
            return _playerRangeSqueezer.Squeeze(previousRange, grid => TestGrid(flopBoard, grid), TestOutcomeOnCall, GetCards(flopBoard, heroHoles));
        }

        private PlayerRange SqueezeOnCheck(PlayerRange previousRange, Move lastMove, int bigBlindSize, FlopBoard flopBoard, HoldingHoles heroHoles)
        {
            return _playerRangeSqueezer.Squeeze(previousRange, grid => TestGrid(flopBoard, grid), TestOutcomeOnCheck, GetCards(flopBoard, heroHoles));
        }

        private List<Card> GetCards(FlopBoard flopBoard, HoldingHoles heroHoles)
        {
            return new List<Card>() { flopBoard.Flop1, flopBoard.Flop2, flopBoard.Flop3, heroHoles.Hole1, heroHoles.Hole2 };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outcome"></param>
        /// <returns>bool means if the result is meaningful</returns>
        private Tuple<bool, PlayerRangeGridStatusEnum> TestOutcomeOnRaise(BoardRangeGridStatusEnum outcome)
        {
            switch (outcome)
            {
                case BoardRangeGridStatusEnum.Trash:
                    return new Tuple<bool, PlayerRangeGridStatusEnum>(true, PlayerRangeGridStatusEnum.Excluded);
                case BoardRangeGridStatusEnum.Nuts:
                case BoardRangeGridStatusEnum.Elite:
                case BoardRangeGridStatusEnum.Good:
                    return new Tuple<bool, PlayerRangeGridStatusEnum>(true, PlayerRangeGridStatusEnum.Included);
            }
            return new Tuple<bool, PlayerRangeGridStatusEnum>(false, PlayerRangeGridStatusEnum.Undefined);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outcome"></param>
        /// <returns>bool means if the result is meaningful</returns>
        private Tuple<bool, PlayerRangeGridStatusEnum> TestOutcomeOnCall(BoardRangeGridStatusEnum outcome)
        {
            switch (outcome)
            {
                case BoardRangeGridStatusEnum.Trash:
                case BoardRangeGridStatusEnum.Nuts:
                    return new Tuple<bool, PlayerRangeGridStatusEnum>(true, PlayerRangeGridStatusEnum.Excluded);
            }
            return new Tuple<bool, PlayerRangeGridStatusEnum>(false, PlayerRangeGridStatusEnum.Undefined);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outcome"></param>
        /// <returns>bool means if the result is meaningful</returns>
        private Tuple<bool, PlayerRangeGridStatusEnum> TestOutcomeOnCheck(BoardRangeGridStatusEnum outcome)
        {
            switch (outcome)
            {
                case BoardRangeGridStatusEnum.Nuts:
                case BoardRangeGridStatusEnum.Elite:
                    //how about Elite?
                    return new Tuple<bool, PlayerRangeGridStatusEnum>(true, PlayerRangeGridStatusEnum.Excluded);
            }
            return new Tuple<bool, PlayerRangeGridStatusEnum>(false, PlayerRangeGridStatusEnum.Undefined);
        }

        private GridStatusInBoardRange TestGrid(FlopBoard flopBoard, RangeGrid grid)
        {
            var result = new GridStatusInBoardRange(grid.Category);
            var rankTester = GetGridRankTester(flopBoard);
            var rankResult = rankTester.Invoke(grid);
            result.RankWiseStatus = rankResult;

            switch (result.Category)
            {
                case GridCategoryEnum.Suited:
                    result.SuitedStatus = GetSuitedTester(flopBoard).Invoke(grid);
                    break;
                case GridCategoryEnum.Paired:
                    result.PairedStatus = GetPairedTester(flopBoard).Invoke(grid);
                    break;
                case GridCategoryEnum.Offsuit:
                    result.OffsuitStatus = GetOffsuitTester(flopBoard).Invoke(grid);
                    break;
            }

            return result;
        }

        private Func<RangeGrid, BoardRangeGridStatusEnum> GetGridRankTester(FlopBoard flopBoard)
        {
            switch (flopBoard.RankTexture)
            {
                case FlopBoardRankTextureEnum.ThreeSome:
                    return new ThreeSomeRankTexture(flopBoard).TestGridAgainstFlopBoard;
                case FlopBoardRankTextureEnum.HighPair:
                    return new HighPairRankTexture(flopBoard).TestGridAgainstFlopBoard;
                case FlopBoardRankTextureEnum.LowPair:
                    return new LowPairRankTexture(flopBoard).TestGridAgainstFlopBoard;
                case FlopBoardRankTextureEnum.Singles:
                    return new SinglesRankTexture(flopBoard).TestGridAgainstFlopBoard;
                default:
                    throw new NotImplementedException();
            }
        }

        private Func<RangeGrid, SuitedStatus<BoardRangeGridStatusEnum>> GetSuitedTester(FlopBoard flopBoard)
        {
            switch (flopBoard.SuitTexture)
            {
                case FlopBoardSuitTextureEnum.SuitedThree:
                    return new SuitedThreeSuitTexture(flopBoard).TestSuitedGrid;
                case FlopBoardSuitTextureEnum.SuitedTwo:
                    return new SuitedTwoSuitTexture(flopBoard).TestSuitedGrid;
                case FlopBoardSuitTextureEnum.Rainbow:
                    return new RainbowSuitTexture().TestSuitedGrid;
                default:
                    throw new NotImplementedException();
            }
        }

        private Func<RangeGrid, PairedStatus<BoardRangeGridStatusEnum>> GetPairedTester(FlopBoard flopBoard)
        {
            switch (flopBoard.SuitTexture)
            {
                case FlopBoardSuitTextureEnum.SuitedThree:
                    return new SuitedThreeSuitTexture(flopBoard).TestPairedGrid;
                case FlopBoardSuitTextureEnum.SuitedTwo:
                    return new SuitedTwoSuitTexture(flopBoard).TestPairedGrid;
                case FlopBoardSuitTextureEnum.Rainbow:
                    return new RainbowSuitTexture().TestPairedGrid;
                default:
                    throw new NotImplementedException();
            }
        }

        private Func<RangeGrid, OffsuitStatus<BoardRangeGridStatusEnum>> GetOffsuitTester(FlopBoard flopBoard)
        {
            switch (flopBoard.SuitTexture)
            {
                case FlopBoardSuitTextureEnum.SuitedThree:
                    return new SuitedThreeSuitTexture(flopBoard).TestOffsuitGrid;
                case FlopBoardSuitTextureEnum.SuitedTwo:
                    return new SuitedTwoSuitTexture(flopBoard).TestOffsuitGrid;
                case FlopBoardSuitTextureEnum.Rainbow:
                    return new RainbowSuitTexture().TestOffsuitGrid;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
