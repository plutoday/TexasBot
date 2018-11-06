using System;
using Models;
using Models.Ranging;
using Turn;
using Turn.TurnBoardRankTextures;

namespace River.RiverBoardRankTextures
{
    public class FoursomeRankTexture : IRiverBoardRankTexture
    {
        public Card FoursomeCard1 { get; set; }
        public Card FoursomeCard2 { get; set; }
        public Card FoursomeCard3 { get; set; }
        public Card FoursomeCard4 { get; set; }
        public Card SingleCard { get; set; }
        public TurnBoard TurnBoard { get; set; }

        public FoursomeRankTexture(RiverBoard board)
        {
            TurnBoard = board.TurnBoard;
            switch (TurnBoard.RankTexture)
            {
                case TurnBoardRankTextureEnum.Foursome:
                    FoursomeCard1 = TurnBoard.FlopBoard.Flop1;
                    FoursomeCard2 = TurnBoard.FlopBoard.Flop2;
                    FoursomeCard3 = TurnBoard.FlopBoard.Flop3;
                    FoursomeCard4 = TurnBoard.TurnCard;
                    SingleCard = board.River;
                    break;
                case TurnBoardRankTextureEnum.HighTri:
                    var highTriTexture = new Turn.TurnBoardRankTextures.HighTriRankTexture(TurnBoard);
                    FoursomeCard1 = highTriTexture.TriCard1;
                    FoursomeCard2 = highTriTexture.TriCard2;
                    FoursomeCard3 = highTriTexture.TriCard3;
                    if (board.River.Rank != FoursomeCard1.Rank)
                    {
                        throw new InvalidOperationException();
                    }
                    FoursomeCard4 = board.River;
                    SingleCard = highTriTexture.SingleCard;
                    break;
                case TurnBoardRankTextureEnum.LowTri:
                    var lowTriTexture = new Turn.TurnBoardRankTextures.LowTriRankTexture(TurnBoard);
                    FoursomeCard1 = lowTriTexture.TriCard1;
                    FoursomeCard2 = lowTriTexture.TriCard2;
                    FoursomeCard3 = lowTriTexture.TriCard3;
                    if (board.River.Rank != FoursomeCard1.Rank)
                    {
                        throw new InvalidOperationException();
                    }
                    FoursomeCard4 = board.River;
                    SingleCard = lowTriTexture.SingleCard;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public Tuple<FoursomeOutcomeEnum, int> TestGridAgainstBoard(RangeGrid grid)
        {
            if (grid.HighRank > SingleCard.Rank)
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
            return new Tuple<FoursomeOutcomeEnum, int>(FoursomeOutcomeEnum.FoursomeNoneKicker, 0);
        }

        public bool ShouldAGridFoldToBet(RangeGrid grid)
        {
            var outcome = TestGridAgainstBoard(grid).Item1;
            return outcome == FoursomeOutcomeEnum.FoursomeWeakKicker
                   || outcome == FoursomeOutcomeEnum.FoursomeNoneKicker;
        }

        public GridHitNewRoundResultEnum HitRiver(RangeGrid grid)
        {
            RankHandGradeEnum turnGrade;
            int turnInvolved;
            switch (TurnBoard.RankTexture)
            {
                case TurnBoardRankTextureEnum.Foursome:
                    var fourSomeTuple = new Turn.TurnBoardRankTextures.FoursomeRankTexture(TurnBoard).TestGridAgainstBoard(grid);
                    turnGrade = fourSomeTuple.Item1.Grade();
                    turnInvolved = fourSomeTuple.Item2;
                    break;
                case TurnBoardRankTextureEnum.HighTri:
                    var highTriTuple = new Turn.TurnBoardRankTextures.HighTriRankTexture(TurnBoard).TestGridAgainstBoard(grid);
                    turnGrade = highTriTuple.Item1.Grade();
                    turnInvolved = highTriTuple.Item2;
                    break;
                case TurnBoardRankTextureEnum.LowTri:
                    var lowTriTuple = new Turn.TurnBoardRankTextures.LowTriRankTexture(TurnBoard).TestGridAgainstBoard(grid);
                    turnGrade = lowTriTuple.Item1.Grade();
                    turnInvolved = lowTriTuple.Item2;
                    break;
                default:
                    throw new InvalidOperationException($"Impossible to have {TurnBoard.RankTexture}");
            }
            var riverTuple = TestGridAgainstBoard(grid);
            var riverOutcomeGrade = riverTuple.Item1.Grade();
            var riverInvolved = riverTuple.Item2;

            if (riverOutcomeGrade > turnGrade)
            {
                return (riverInvolved > turnInvolved) ? GridHitNewRoundResultEnum.Promoted : GridHitNewRoundResultEnum.Enhanced;
            }
            return GridHitNewRoundResultEnum.None;
        }
    }
}
