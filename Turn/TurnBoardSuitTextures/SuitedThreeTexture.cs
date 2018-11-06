using System;
using System.Collections.Generic;
using Flop;
using Models;
using Models.Ranging;

namespace Turn.TurnBoardSuitTextures
{
    public class SuitedThreeTexture : ITurnBoardSuitTexture
    {
        public SuitEnum SuitedSuit { get; set; }
        public List<RankEnum> SuitedRanks { get; set; }
        public SuitedThreeTexture(TurnBoard turnBoard)
        {
            if (turnBoard.FlopBoard.SuitTexture == FlopBoardSuitTextureEnum.SuitedThree)
            {
                SuitedSuit = turnBoard.FlopBoard.Flop1.Suit;
                SuitedRanks = new List<RankEnum>() { turnBoard.FlopBoard.Flop1.Rank, turnBoard.FlopBoard.Flop2.Rank, turnBoard.FlopBoard.Flop3.Rank };
            }
            else if (turnBoard.FlopBoard.SuitTexture == FlopBoardSuitTextureEnum.SuitedTwo)
            {
                SuitedSuit = turnBoard.TurnCard.Suit;
                SuitedRanks = new List<RankEnum>();
                SuitedRanks.Add(turnBoard.TurnCard.Rank);
                if (turnBoard.FlopBoard.Flop1.Suit == SuitedSuit) SuitedRanks.Add(turnBoard.FlopBoard.Flop1.Rank);
                if (turnBoard.FlopBoard.Flop2.Suit == SuitedSuit) SuitedRanks.Add(turnBoard.FlopBoard.Flop2.Rank);
                if (turnBoard.FlopBoard.Flop3.Suit == SuitedSuit) SuitedRanks.Add(turnBoard.FlopBoard.Flop3.Rank);
                if (SuitedRanks.Count != 3) throw new InvalidOperationException();
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
                    bool shouldFold = (suit1 != SuitedSuit && suit2 != SuitedSuit);
                    result.Add(new Tuple<SuitEnum, SuitEnum>(suit1, suit2), shouldFold);
                }
            }
            return result;
        }

        public Tuple<SuitTextureOutcomeEnum, int> TestGridAgainstTurnBoard(Card hole1, Card hole2)
        {
            var suitedRanks = new List<RankEnum>(SuitedRanks);
            if (hole1.Suit == SuitedSuit) suitedRanks.Add(hole1.Rank);
            if (hole2.Suit == SuitedSuit) suitedRanks.Add(hole2.Rank);

            if (suitedRanks.Count == 5)
            {
                suitedRanks.Sort();
                if (suitedRanks[4] == suitedRanks[0] + 4)
                {
                    return suitedRanks[4] == RankEnum.Ace
                        ? new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.RoyalFlush, 2)
                        : new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.StraightFlush, 2);
                }

                var kicker = hole1.Rank > hole2.Rank ? hole1.Rank : hole2.Rank;
                if (kicker == RankEnum.Ace) return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.FlushWithTopKicker, 2);
                if (kicker > RankEnum.Ten) return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.FlushWithGoodKicker, 2);
                return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.FlushWithWeakKicker, 2);
            }

            if (suitedRanks.Count == 4) return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.FlushDraw, 1);
            
            return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.Nothing, 0);
        }
    }
}
