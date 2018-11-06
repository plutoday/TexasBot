using System;
using System.Collections.Generic;
using Models;
using Models.Ranging;

namespace Turn.TurnBoardSuitTextures
{
    public class SuitedFourTexture : ITurnBoardSuitTexture
    {
        public SuitEnum SuitedSuit { get; set; }
        public List<RankEnum> Ranks { get; set; }

        public SuitedFourTexture(TurnBoard turnBoard)
        {
            if (turnBoard.SuitTexture != TurnBoardSuitTextureEnum.SuitedFour)
            {
                throw new InvalidOperationException();
            }

            SuitedSuit = turnBoard.TurnCard.Suit;
            Ranks = new List<RankEnum>()
            {
                turnBoard.FlopBoard.Flop1.Rank,
                turnBoard.FlopBoard.Flop2.Rank,
                turnBoard.FlopBoard.Flop3.Rank,
                turnBoard.TurnCard.Rank
            };
        }

        public Dictionary<Tuple<SuitEnum, SuitEnum>, bool> ShouldAGridFoldToBet(RangeGrid grid)
        {
            var result = new Dictionary<Tuple<SuitEnum, SuitEnum>, bool>();
            var suits = new List<SuitEnum>() { SuitEnum.Heart, SuitEnum.Spade, SuitEnum.Diamond, SuitEnum.Club };
            foreach (var suit1 in suits)
            {
                foreach (var suit2 in suits)
                {
                    bool shouldFold = (suit1 != SuitedSuit && suit2 != SuitedSuit);
                    result.Add(new Tuple<SuitEnum, SuitEnum>(suit1, suit2), shouldFold);
                }
            }
            return result;
        }

        public Tuple<SuitTextureOutcomeEnum, int> TestGridAgainstTurnBoard(Card hole1, Card hole2)
        {
            if (hole1.Suit != SuitedSuit && hole2.Suit != SuitedSuit)
            {
                return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.FlushDraw, 0);
            }

            var ranks = new List<RankEnum>(Ranks);
            if (hole1.Suit == SuitedSuit)
            {
                ranks.Add(hole1.Rank);
            }
            if (hole2.Suit == SuitedSuit)
            {
                ranks.Add(hole2.Rank);
            }

            ranks.Sort();
            int involved = 0;
            if (ranks.Count == 6)
            {
                if (ranks[5] == ranks[1] + 4)
                {
                    involved = 0;
                    if (hole1.Rank >= ranks[1] && hole1.Rank <= ranks[5]) involved++;
                    if (hole2.Rank >= ranks[1] && hole2.Rank <= ranks[5]) involved++;
                    return ranks[5] == RankEnum.Ace
                        ? new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.RoyalFlush, involved)
                        : new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.StraightFlush, involved);
                }
            }
            if (ranks[4] == ranks[0] + 4)
            {
                involved = 0;
                if (hole1.Rank >= ranks[0] && hole1.Rank <= ranks[4]) involved++;
                if (hole2.Rank >= ranks[0] && hole2.Rank <= ranks[4]) involved++;
                return ranks[4] == RankEnum.Ace
                    ? new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.RoyalFlush, involved)
                     : new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.StraightFlush, involved);
            }

            var kicker = ranks[ranks.Count - 1];

            involved = 0;
            if (hole1.Rank >= ranks[ranks.Count - 5] && hole1.Rank <= ranks[ranks.Count - 5]) involved++;
            if (hole2.Rank >= ranks[ranks.Count - 5] && hole2.Rank <= ranks[ranks.Count - 5]) involved++;
            if (kicker == RankEnum.Ace) return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.FlushWithTopKicker, involved);
            if (kicker > RankEnum.Ten) return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.FlushWithGoodKicker, involved);
            return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.FlushWithWeakKicker, involved);
        }
    }
}
