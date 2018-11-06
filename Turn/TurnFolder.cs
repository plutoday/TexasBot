using System;
using System.Collections.Generic;
using Models;
using Models.Ranging;
using Turn.TurnBoardRankTextures;
using Turn.TurnBoardSuitTextures;

namespace Turn
{
    public class TurnFolder
    {
        public bool ShouldAGridFoldToBoardByRank(RangeGrid grid, TurnBoard turnBoard)
        {
            switch (turnBoard.RankTexture)
            {
                case TurnBoardRankTextureEnum.Foursome:
                    return new FoursomeRankTexture(turnBoard).ShouldAGridFoldToBet(grid);
                case TurnBoardRankTextureEnum.HighTri:
                    return new HighTriRankTexture(turnBoard).ShouldAGridFoldToBet(grid);
                case TurnBoardRankTextureEnum.LowTri:
                    return new LowTriRankTexture(turnBoard).ShouldAGridFoldToBet(grid);
                case TurnBoardRankTextureEnum.TwoPairs:
                    return new TwoPairsRankTexture(turnBoard).ShouldAGridFoldToBet(grid);
                case TurnBoardRankTextureEnum.HighPair:
                    return new HighPairRankTexture(turnBoard).ShouldAGridFoldToBet(grid);
                case TurnBoardRankTextureEnum.MiddlePair:
                    return new MiddlePairRankTexture(turnBoard).ShouldAGridFoldToBet(grid);
                case TurnBoardRankTextureEnum.LowPair:
                    return new LowPairRankTexture(turnBoard).ShouldAGridFoldToBet(grid);
                case TurnBoardRankTextureEnum.Singles:
                    return new SinglesRankTexture(turnBoard).ShouldAGridFoldToBet(grid);
            }
            throw new NotImplementedException();
        }

        public Dictionary<Tuple<SuitEnum, SuitEnum>, bool> ShouldAGridFoldToBoardBySuit(RangeGrid grid,
            TurnBoard turnBoard, HoldingHoles heroHoles)
        {
            var conflictCards = new List<Card>() { heroHoles.Hole1, heroHoles.Hole2, turnBoard.TurnCard, turnBoard.FlopBoard.Flop2, turnBoard.FlopBoard.Flop3 };
            grid.EliminateConflicts(conflictCards);
            switch (turnBoard.SuitTexture)
            {
                case TurnBoardSuitTextureEnum.SuitedFour:
                    return new SuitedFourTexture(turnBoard).ShouldAGridFoldToBet(grid);
                case TurnBoardSuitTextureEnum.SuitedThree:
                    return new SuitedThreeTexture(turnBoard).ShouldAGridFoldToBet(grid);
                case TurnBoardSuitTextureEnum.SuitedTwoPairs:
                    return new SuitedTwoPairsTexture(turnBoard).ShouldAGridFoldToBet(grid);
                case TurnBoardSuitTextureEnum.SuitedTwo:
                    return new SuitedTwoTexture(turnBoard).ShouldAGridFoldToBet(grid);
                case TurnBoardSuitTextureEnum.Offsuit:
                    return new OffsuitTexture(turnBoard).ShouldAGridFoldToBet(grid);
            }
            throw new NotImplementedException();
        }
    }
}
