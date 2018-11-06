using System;
using System.Collections.Generic;
using Common;
using Models;
using Models.Ranging;

namespace Flop.FlopBoardSuitTextures
{
    public class RainbowSuitTexture : IFlopBoardSuitTexture
    {
        public Dictionary<Tuple<SuitEnum, SuitEnum>, bool> ShouldAGridFoldToBet(RangeGrid grid)
        {
            var result = new Dictionary<Tuple<SuitEnum, SuitEnum>, bool>();
            foreach (var tuple in grid.EnumerateAllPossibleHoles())
            {
                var hole1 = tuple.Item1;
                var hole2 = tuple.Item2;
                result.Add(new Tuple<SuitEnum, SuitEnum>(hole1.Suit, hole2.Suit), true);
            }
            return result;
        }

        public SuitedStatus<BoardRangeGridStatusEnum> TestSuitedGrid(RangeGrid grid)
        {
            return new SuitedStatus<BoardRangeGridStatusEnum>()
            {
                HeartStatus = BoardRangeGridStatusEnum.Trash,
                SpadeStatus = BoardRangeGridStatusEnum.Trash,
                DiamondStatus = BoardRangeGridStatusEnum.Trash,
                ClubStatus = BoardRangeGridStatusEnum.Trash
            };
        }

        public PairedStatus<BoardRangeGridStatusEnum> TestPairedGrid(RangeGrid grid)
        {
            return new PairedStatus<BoardRangeGridStatusEnum>()
            {
                HeartSpadeStatus = BoardRangeGridStatusEnum.Trash,
                HeartClubStatus = BoardRangeGridStatusEnum.Trash,
                HeartDiamondStatus = BoardRangeGridStatusEnum.Trash,
                SpadeDiamondStatus = BoardRangeGridStatusEnum.Trash,
                SpadeClubStatus = BoardRangeGridStatusEnum.Trash,
                DiamondClubStatus = BoardRangeGridStatusEnum.Trash,
            };
        }

        public OffsuitStatus<BoardRangeGridStatusEnum> TestOffsuitGrid(RangeGrid grid)
        {
            return new OffsuitStatus<BoardRangeGridStatusEnum>()
            {
                HeartSpadeStatus = BoardRangeGridStatusEnum.Trash,
                HeartClubStatus = BoardRangeGridStatusEnum.Trash,
                HeartDiamondStatus = BoardRangeGridStatusEnum.Trash,
                SpadeDiamondStatus = BoardRangeGridStatusEnum.Trash,
                SpadeClubStatus = BoardRangeGridStatusEnum.Trash,
                DiamondClubStatus = BoardRangeGridStatusEnum.Trash,
                SpadeHeartStatus = BoardRangeGridStatusEnum.Trash,
                ClubHeartStatus = BoardRangeGridStatusEnum.Trash,
                DiamondHeartStatus = BoardRangeGridStatusEnum.Trash,
                DiamondSpadeStatus = BoardRangeGridStatusEnum.Trash,
                ClubSpadeStatus = BoardRangeGridStatusEnum.Trash,
                ClubDiamondStatus = BoardRangeGridStatusEnum.Trash,
            };
        }

        public Tuple<SuitTextureOutcomeEnum, int> TestGridAgainstBoard(Card hole1, Card hole2)
        {
            return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.Nothing, 1);
        }
    }
}
