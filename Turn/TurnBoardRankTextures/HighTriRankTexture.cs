using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flop;
using Models;
using Models.Ranging;

namespace Turn.TurnBoardRankTextures
{
    public class HighTriRankTexture : ITurnFlopBoardRankTexture
    {
        public Card TriCard1 { get; set; }
        public Card TriCard2 { get; set; }
        public Card TriCard3 { get; set; }
        public Card SingleCard { get; set; }
        public FlopBoard FlopBoard { get; set; }

        public HighTriRankTexture(TurnBoard board)
        {
            FlopBoard = board.FlopBoard;
            if (FlopBoard.RankTexture == FlopBoardRankTextureEnum.ThreeSome)
            {
                SingleCard = board.TurnCard;
                if (FlopBoard.Flop1.Rank <= SingleCard.Rank)
                {
                    throw new InvalidOperationException();
                }
                TriCard1 = FlopBoard.Flop1;
                TriCard2 = FlopBoard.Flop2;
                TriCard3 = FlopBoard.Flop3;
            }
            else if (FlopBoard.RankTexture == FlopBoardRankTextureEnum.HighPair)
            {
                var cards = new List<Card>() {FlopBoard.Flop1, FlopBoard.Flop2, FlopBoard.Flop3, board.TurnCard};
                cards.Sort();
                if (!(cards[1].Rank == cards[2].Rank && cards[2].Rank == cards[3].Rank))
                {
                    throw new InvalidOperationException();
                }
                SingleCard = cards[0];
                TriCard1 = cards[1];
                TriCard2 = cards[2];
                TriCard3 = cards[3];
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public BoardRangeGridStatusEnum TestGridAgainstFlopBoard(RangeGrid grid)
        {
            throw new NotImplementedException();
        }

        public bool ShouldAGridFoldToBet(RangeGrid grid)
        {
            var grade = TestGridAgainstBoard(grid).Item1.Grade();
            var hit = HitTurn(grid);

            return hit == GridHitNewRoundResultEnum.None || grade <= RankHandGradeEnum.Threesome;
        }

        public GridHitNewRoundResultEnum HitTurn(RangeGrid grid)
        {
            RankHandGradeEnum flopGrade;
            int flopInvolved;
            switch (FlopBoard.RankTexture)
            {
                case FlopBoardRankTextureEnum.HighPair:
                    var highPairTuple = new Flop.FlopBoardRankTextures.HighPairRankTexture(FlopBoard).TestGridAgainstBoard(grid);
                    flopGrade = highPairTuple.Item1.Grade();
                    flopInvolved = highPairTuple.Item2;
                    break;
                case FlopBoardRankTextureEnum.ThreeSome:
                    var threesomeTuple = new Flop.FlopBoardRankTextures.ThreeSomeRankTexture(FlopBoard).TestGridAgainstBoard(grid);
                    flopGrade = threesomeTuple.Item1.Grade();
                    flopInvolved = threesomeTuple.Item2;
                    break;
                default:
                    throw new InvalidOperationException($"Impossible to have {FlopBoard.RankTexture}");
            }
            var turnTuple = TestGridAgainstBoard(grid);
            var turnOutcomeGrade = turnTuple.Item1.Grade();
            var turnInvolved = turnTuple.Item2;

            if (turnOutcomeGrade > flopGrade)
            {
                return (turnInvolved > flopInvolved) ? GridHitNewRoundResultEnum.Promoted : GridHitNewRoundResultEnum.Enhanced;
            }
            return GridHitNewRoundResultEnum.None;
        }

        public Tuple<TriOutcomeEnum, int> TestGridAgainstBoard(RangeGrid grid)
        {/*
        Foursome,
        GoodOverFullHouse,
        WeakOverFullHouse,
        MiddleFullHouse,
        UnderFullHouse,
        ThreesomeTopKicker,
        ThreesomeOverGoodKicker,
        ThreesomeOverWeakKicker,
        ThreesomeNoneKicker*/
            if (grid.HighRank == TriCard1.Rank || grid.LowRank == TriCard1.Rank)
            {
                return new Tuple<TriOutcomeEnum, int>(TriOutcomeEnum.Foursome, 1);
            }
            if (grid.Category == GridCategoryEnum.Paired)
            {
                if (grid.HighRank > SingleCard.Rank)
                {
                    return grid.HighRank > RankEnum.Ten
                        ? new Tuple<TriOutcomeEnum, int>(TriOutcomeEnum.GoodOverFullHouse, 2)
                        : new Tuple<TriOutcomeEnum, int>(TriOutcomeEnum.OverFullHouse, 2);
                }
                if (grid.HighRank < SingleCard.Rank)
                {
                    return new Tuple<TriOutcomeEnum, int>(TriOutcomeEnum.UnderFullHouse, 2);
                }
            }
            if (grid.HighRank == SingleCard.Rank || grid.LowRank == SingleCard.Rank)
            {
                return new Tuple<TriOutcomeEnum, int>(TriOutcomeEnum.FullHouse, 1);
            }
            if (grid.HighRank == RankEnum.Ace)
            {
                return new Tuple<TriOutcomeEnum, int>(TriOutcomeEnum.ThreesomeTopKicker, 0);
            }
            if (grid.HighRank > SingleCard.Rank)
            {
                return grid.HighRank > RankEnum.Ten ? 
                    new Tuple<TriOutcomeEnum, int>(TriOutcomeEnum.ThreesomeOverGoodKicker, 0)
                    : new Tuple<TriOutcomeEnum, int>(TriOutcomeEnum.ThreesomeOverWeakKicker, 0);
            }
            return new Tuple<TriOutcomeEnum, int>(TriOutcomeEnum.ThreesomeNoneKicker, 0);

        }
    }
}
