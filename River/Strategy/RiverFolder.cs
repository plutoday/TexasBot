using System;
using System.Collections.Generic;
using Models;
using Models.Ranging;
using River.RiverBoardRankTextures;
using River.RiverBoardSuitTextures;

namespace River.Strategy
{
    public class RiverFolder
    {
        public bool ShouldAGridFoldToBoardByRank(RangeGrid grid, RiverBoard riverBoard)
        {
            switch (riverBoard.RankTexture)
            {
                case RiverRankTextureEnum.Foursome:
                    return new FoursomeRankTexture(riverBoard).ShouldAGridFoldToBet(grid);
                case RiverRankTextureEnum.HighTriLowPair:
                    return new HighTriLowPairRankTexture(riverBoard).ShouldAGridFoldToBet(grid);
                case RiverRankTextureEnum.LowTriHighPair:
                    return new LowTriHighPairRankTexture(riverBoard).ShouldAGridFoldToBet(grid);
                case RiverRankTextureEnum.HighTri:
                    return new HighTriRankTexture(riverBoard).ShouldAGridFoldToBet(grid);
                case RiverRankTextureEnum.MiddleTri:
                    return new MiddleTriRankTexture(riverBoard).ShouldAGridFoldToBet(grid);
                case RiverRankTextureEnum.LowTri:
                    return new LowTriRankTexture(riverBoard).ShouldAGridFoldToBet(grid);
                case RiverRankTextureEnum.HighTwoPairs:
                    return new HighTwoPairsRankTexture(riverBoard).ShouldAGridFoldToBet(grid);
                case RiverRankTextureEnum.HighLowTwoPairs:
                    return new HighLowTwoPairsRankTexture(riverBoard).ShouldAGridFoldToBet(grid);
                case RiverRankTextureEnum.LowTwoPairs:
                    return new LowTwoPairsRankTexture(riverBoard).ShouldAGridFoldToBet(grid);
                case RiverRankTextureEnum.HighPair:
                    return new HighPairRankTexture(riverBoard).ShouldAGridFoldToBet(grid);
                case RiverRankTextureEnum.SecondPair:
                    return new SecondPairRankTexture(riverBoard).ShouldAGridFoldToBet(grid);
                case RiverRankTextureEnum.ThirdPair:
                    return new ThirdPairRankTexture(riverBoard).ShouldAGridFoldToBet(grid);
                case RiverRankTextureEnum.LowPair:
                    return new LowPairRankTexture(riverBoard).ShouldAGridFoldToBet(grid);
                case RiverRankTextureEnum.Singles:
                    return new SinglesRankTexture(riverBoard).ShouldAGridFoldToBet(grid);
            }
            throw new InvalidOperationException();
        }

        public Dictionary<Tuple<SuitEnum, SuitEnum>, bool> ShouldAGridFoldToBoardBySuit(RangeGrid grid,
            RiverBoard riverBoard, HoldingHoles heroHoles)
        {
            var conflictCards = new List<Card>() { heroHoles.Hole1, heroHoles.Hole2, riverBoard.River, riverBoard.TurnBoard.TurnCard,
                riverBoard.TurnBoard.FlopBoard.Flop1, riverBoard.TurnBoard.FlopBoard.Flop2, riverBoard.TurnBoard.FlopBoard.Flop3 };
            grid.EliminateConflicts(conflictCards);
            switch (riverBoard.SuitTexture)
            {
                case RiverSuitTextureEnum.FiveSuited:
                    return new SuitedFiveTexture(riverBoard).ShouldAGridFoldToBet(grid);
                case RiverSuitTextureEnum.FourSuited:
                    return new SuitedFourTexture(riverBoard).ShouldAGridFoldToBet(grid);
                case RiverSuitTextureEnum.ThreeSuited:
                    return new SuitedThreeTexture(riverBoard).ShouldAGridFoldToBet(grid);
                case RiverSuitTextureEnum.Offsuit:
                    return new OffsuitTexture().ShouldAGridFoldToBet(grid);
            }
            throw new InvalidOperationException();
        }
    }
}
