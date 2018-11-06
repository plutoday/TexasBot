using System;
using System.Collections.Generic;
using Flop;
using Models;
using Models.Ranging;

namespace Turn.TurnBoardSuitTextures
{
    public class SuitedTwoPairsTexture : ITurnBoardSuitTexture
    {
        public SuitEnum SuitedSuit1 { get; set; }
        public SuitEnum SuitedSuit2 { get; set; }
        public List<RankEnum> Suit1Ranks { get; set; } 
        public List<RankEnum> Suit2Ranks { get; set; } 
        
        public SuitedTwoPairsTexture(TurnBoard turnBoard)
        {
            if (turnBoard.FlopBoard.SuitTexture == FlopBoardSuitTextureEnum.SuitedTwo)
            {
                Suit1Ranks = new List<RankEnum>();
                Suit2Ranks = new List<RankEnum>();
                SuitedSuit1 = turnBoard.TurnCard.Suit;
                Suit1Ranks.Add(turnBoard.TurnCard.Rank);

                if (turnBoard.FlopBoard.Flop1.Suit != SuitedSuit1)
                {
                    SuitedSuit2 = turnBoard.FlopBoard.Flop1.Suit;
                }
                else if (turnBoard.FlopBoard.Flop2.Suit != SuitedSuit1)
                {
                    SuitedSuit2 = turnBoard.FlopBoard.Flop2.Suit;
                }
                else if (turnBoard.FlopBoard.Flop3.Suit != SuitedSuit1)
                {
                    SuitedSuit2 = turnBoard.FlopBoard.Flop3.Suit;
                }
                else
                {
                    throw new InvalidOperationException();
                }

                if (turnBoard.FlopBoard.Flop1.Suit == SuitedSuit1)
                {
                    Suit1Ranks.Add(turnBoard.FlopBoard.Flop1.Rank);
                }
                else
                {
                    Suit2Ranks.Add(turnBoard.FlopBoard.Flop1.Rank);
                }

                if (turnBoard.FlopBoard.Flop2.Suit == SuitedSuit1)
                {
                    Suit1Ranks.Add(turnBoard.FlopBoard.Flop2.Rank);
                }
                else
                {
                    Suit2Ranks.Add(turnBoard.FlopBoard.Flop2.Rank);
                }

                if (turnBoard.FlopBoard.Flop3.Suit == SuitedSuit1)
                {
                    Suit1Ranks.Add(turnBoard.FlopBoard.Flop3.Rank);
                }
                else
                {
                    Suit2Ranks.Add(turnBoard.FlopBoard.Flop3.Rank);
                }
            }
            else
            {
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
                    bool shouldFold = suit1 != suit2 || (suit1 != SuitedSuit1 && suit1 != SuitedSuit2);
                    result.Add(new Tuple<SuitEnum, SuitEnum>(suit1, suit2), shouldFold);
                }
            }
            return result;
        }

        public Tuple<SuitTextureOutcomeEnum, int> TestGridAgainstTurnBoard(Card hole1, Card hole2)
        {
            if (hole1.Suit != hole2.Suit)
            {
                return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.Nothing, 0);
            }

            if (hole1.Suit == SuitedSuit1 || hole1.Suit == SuitedSuit2)
            {
                return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.FlushDraw, 2);
            }

            return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.Nothing, 0);
        }
    }
}
