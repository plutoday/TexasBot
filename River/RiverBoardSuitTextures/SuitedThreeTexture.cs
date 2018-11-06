using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Models.Ranging;
using Turn;
using Turn.TurnBoardSuitTextures;

namespace River.RiverBoardSuitTextures
{
    public class SuitedThreeTexture : IRiverBoardSuitTexture
    {
        public SuitEnum Suit { get; set; }
        public List<RankEnum> SuitedRanks { get; set; }

        public SuitedThreeTexture(RiverBoard riverBoard)
        {
            if (riverBoard.SuitTexture != RiverSuitTextureEnum.ThreeSuited)
            {
                throw new InvalidOperationException();
            }

            switch (riverBoard.TurnBoard.SuitTexture)
            {
                case TurnBoardSuitTextureEnum.SuitedThree:
                    var suitedThreeTurnBoard = new Turn.TurnBoardSuitTextures.SuitedThreeTexture(riverBoard.TurnBoard);
                    Suit = suitedThreeTurnBoard.SuitedSuit;
                    SuitedRanks = new List<RankEnum>(suitedThreeTurnBoard.SuitedRanks);
                    break;
                case TurnBoardSuitTextureEnum.SuitedTwoPairs:
                    var suitedTwoPairsTurnBoard = new SuitedTwoPairsTexture(riverBoard.TurnBoard);
                    Suit = riverBoard.River.Suit;
                    if (suitedTwoPairsTurnBoard.SuitedSuit1 == Suit)
                    {
                        SuitedRanks = new List<RankEnum>(suitedTwoPairsTurnBoard.Suit1Ranks);
                        SuitedRanks.Add(riverBoard.River.Rank);
                    }
                    else if (suitedTwoPairsTurnBoard.SuitedSuit2 == Suit)
                    {
                        SuitedRanks = new List<RankEnum>(suitedTwoPairsTurnBoard.Suit2Ranks);
                        SuitedRanks.Add(riverBoard.River.Rank);
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                    break;
                case TurnBoardSuitTextureEnum.SuitedTwo:
                    var suitedTwoTurnBoard = new SuitedTwoTexture(riverBoard.TurnBoard);
                    Suit = suitedTwoTurnBoard.SuitedSuit;
                    SuitedRanks = new List<RankEnum>(suitedTwoTurnBoard.SuitedRanks);
                    SuitedRanks.Add(riverBoard.River.Rank);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public Dictionary<Tuple<SuitEnum, SuitEnum>, bool> ShouldAGridFoldToBet(RangeGrid grid)
        {
            var result = new Dictionary<Tuple<SuitEnum, SuitEnum>, bool>();
            var suits = new List<SuitEnum>() { SuitEnum.Heart, SuitEnum.Spade, SuitEnum.Diamond, SuitEnum.Club };
            foreach (var suit1 in suits)
            {
                foreach (var suit2 in suits)
                {
                    bool shouldFold = (suit1 != Suit || suit2 != Suit);
                    result.Add(new Tuple<SuitEnum, SuitEnum>(suit1, suit2), shouldFold);
                }
            }
            return result;
        }

        public Tuple<SuitTextureOutcomeEnum, int> TestGridAgainstBoard(Card hole1, Card hole2)
        {
            var ranks = new List<RankEnum>(SuitedRanks);
            if (hole1.Suit == Suit) ranks.Add(hole1.Rank);
            if (hole2.Suit == Suit) ranks.Add(hole2.Rank);

            if (ranks.Count < 5)
            {
                return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.Nothing, 0);
            }

            ranks.Sort();

            if (ranks[4] == ranks[0] + 4)
            {
                var outcome = ranks[4] == RankEnum.Ace
                    ? SuitTextureOutcomeEnum.RoyalFlush
                    : SuitTextureOutcomeEnum.StraightFlush;
                return new Tuple<SuitTextureOutcomeEnum, int>(outcome, 1);
            }

            var kicker = ranks.Where(r => SuitedRanks.All(sr => sr != r)).Max();
            if (kicker == RankEnum.Ace)
            {
                return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.FlushWithTopKicker, 1);
            }
            if (kicker > RankEnum.Ten)
            {
                return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.FlushWithGoodKicker, 1);
            }
            return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.FlushWithWeakKicker, 1);
        }
    }
}
