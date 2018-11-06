using System;
using Flop;
using Models;
using Models.Ranging;

namespace Turn.TurnBoardRankTextures
{
    public class FoursomeRankTexture : ITurnFlopBoardRankTexture
    {
        public Card FoursomeCard1 { get; set; }
        public Card FoursomeCard2 { get; set; }
        public Card FoursomeCard3 { get; set; }
        public Card FoursomeCard4 { get; set; }

        public FlopBoard FlopBoard { get; set; }

        public FoursomeRankTexture(TurnBoard board)
        {
            if (board.FlopBoard.RankTexture != FlopBoardRankTextureEnum.ThreeSome)
            {
                throw new InvalidOperationException();
            }

            if (board.FlopBoard.Flop1.Rank != board.TurnCard.Rank)
            {
                throw new InvalidOperationException();
            }

            FlopBoard = board.FlopBoard;

            FoursomeCard1 = board.FlopBoard.Flop1;
            FoursomeCard2 = board.FlopBoard.Flop2;
            FoursomeCard3 = board.FlopBoard.Flop3;
            FoursomeCard4 = board.TurnCard;
        }

        public BoardRangeGridStatusEnum TestGridAgainstFlopBoard(RangeGrid grid)
        {
            throw new NotImplementedException();
        }

        public bool ShouldAGridFoldToBet(RangeGrid grid)
        {
            var outcome = TestGridAgainstBoard(grid).Item1;
            return outcome == FoursomeOutcomeEnum.FoursomeWeakKicker;
        }

        public GridHitNewRoundResultEnum HitTurn(RangeGrid grid)
        {
            return GridHitNewRoundResultEnum.Enhanced;
        }

        public Tuple<FoursomeOutcomeEnum, int> TestGridAgainstBoard(RangeGrid grid)
        {
            if (grid.HighRank == RankEnum.Ace)
            {
                return new Tuple<FoursomeOutcomeEnum, int>(FoursomeOutcomeEnum.FoursomeTopKicker, 0);
            }
            if (grid.HighRank > RankEnum.Ten)
            {
                return new Tuple<FoursomeOutcomeEnum, int>(FoursomeOutcomeEnum.FoursomeGoodKicker, 0);
            }
            return new Tuple<FoursomeOutcomeEnum, int>(FoursomeOutcomeEnum.FoursomeWeakKicker, 0);
        }
    }
}
