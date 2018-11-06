using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flop.FlopBoardRankTextures;
using Flop.FlopBoardSuitTextures;
using Models;
using Models.Ranging;

namespace Flop
{
    public class FlopFolder
    {
        public bool ShouldAGridFoldToBoardByRank(RangeGrid grid, FlopBoard flopBoard)
        {
            switch (flopBoard.RankTexture)
            {
                case FlopBoardRankTextureEnum.ThreeSome:
                    return new ThreeSomeRankTexture(flopBoard).ShouldAGridFoldToBet(grid);
                case FlopBoardRankTextureEnum.HighPair:
                    return new HighPairRankTexture(flopBoard).ShouldAGridFoldToBet(grid);
                case FlopBoardRankTextureEnum.LowPair:
                    return new LowPairRankTexture(flopBoard).ShouldAGridFoldToBet(grid);
                case FlopBoardRankTextureEnum.Singles:
                    return new SinglesRankTexture(flopBoard).ShouldAGridFoldToBet(grid);
            }
            throw new NotImplementedException();
        }

        public Dictionary<Tuple<SuitEnum, SuitEnum>, bool> ShouldAGridFoldToBoardBySuit(RangeGrid grid,
            FlopBoard flopBoard, HoldingHoles heroHoles)
        {
            var conflictCards = new List<Card>() {heroHoles.Hole1, heroHoles.Hole2, flopBoard.Flop1, flopBoard.Flop2, flopBoard.Flop3};
            grid.EliminateConflicts(conflictCards);
            switch (flopBoard.SuitTexture)
            {
                case FlopBoardSuitTextureEnum.SuitedThree:
                    return new SuitedThreeSuitTexture(flopBoard).ShouldAGridFoldToBet(grid);
                case FlopBoardSuitTextureEnum.SuitedTwo:
                    return new SuitedTwoSuitTexture(flopBoard).ShouldAGridFoldToBet(grid);
                case FlopBoardSuitTextureEnum.Rainbow:
                    return new RainbowSuitTexture().ShouldAGridFoldToBet(grid);
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
