using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.Ranging;
using Turn;

namespace River.RiverBoardSuitTextures
{
    public class SuitedFiveTexture : IRiverBoardSuitTexture
    {
        public SuitEnum Suit { get; set; }
        public List<RankEnum> SuitedRanks { get; set; }

        public SuitedFiveTexture(RiverBoard riverBoard)
        {
            if (riverBoard.SuitTexture != RiverSuitTextureEnum.FiveSuited)
            {
                throw new InvalidOperationException();
            }
            switch (riverBoard.TurnBoard.SuitTexture)
            {
                case TurnBoardSuitTextureEnum.SuitedFour:
                    var suitedFourTurnBoard = new Turn.TurnBoardSuitTextures.SuitedFourTexture(riverBoard.TurnBoard);
                    Suit = suitedFourTurnBoard.SuitedSuit;
                    SuitedRanks = new List<RankEnum>(suitedFourTurnBoard.Ranks);
                    SuitedRanks.Add(riverBoard.River.Rank);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public Dictionary<Tuple<SuitEnum, SuitEnum>, bool> ShouldAGridFoldToBet(RangeGrid grid)
        {
            var result = new Dictionary<Tuple<SuitEnum, SuitEnum>, bool>();
            var suits = new List<SuitEnum>() {SuitEnum.Heart, SuitEnum.Spade, SuitEnum.Diamond, SuitEnum.Club};
            foreach (var suit1 in suits)
            {
                foreach (var suit2 in suits)
                {
                    bool shouldFold = (suit1 != Suit && suit2 != Suit);
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

            if (ranks.Count == 7)
            {
                if (ranks[6] == ranks[2] + 4)
                {
                    var outcome = ranks[6] == RankEnum.Ace
                        ? SuitTextureOutcomeEnum.RoyalFlush
                        : SuitTextureOutcomeEnum.StraightFlush;
                    return new Tuple<SuitTextureOutcomeEnum, int>(outcome, 1);
                }
            }

            if (ranks.Count >= 6)
            {
                if (ranks[5] == ranks[1] + 4)
                {
                    var outcome = ranks[5] == RankEnum.Ace
                        ? SuitTextureOutcomeEnum.RoyalFlush
                        : SuitTextureOutcomeEnum.StraightFlush;
                    return new Tuple<SuitTextureOutcomeEnum, int>(outcome, 1);
                }
            }

            if (ranks[4] == ranks[0] + 4)
            {
                var outcome = ranks[4] == RankEnum.Ace
                    ? SuitTextureOutcomeEnum.RoyalFlush
                    : SuitTextureOutcomeEnum.StraightFlush;
                return new Tuple<SuitTextureOutcomeEnum, int>(outcome, 1);
            }

            if (ranks.Count == 5)
            {
                return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.FlushWithNoneKicker, 0);
            }

            var kicker = ranks.Where(r => SuitedRanks.All(sr => sr != r)).Max();
            if (kicker == RankEnum.Ace)
            {
                return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.FlushWithTopKicker, 0);
            }
            if (kicker > RankEnum.Ten)
            {
                return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.FlushWithGoodKicker, 0);
            }
            return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.FlushWithWeakKicker, 0);
        }
    }
}
