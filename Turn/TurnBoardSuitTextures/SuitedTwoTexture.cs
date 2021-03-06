﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.Ranging;

namespace Turn.TurnBoardSuitTextures
{
    public class SuitedTwoTexture : ITurnBoardSuitTexture
    {
        public SuitEnum SuitedSuit { get; set; }
        public List<RankEnum> SuitedRanks { get; set; }

        public SuitedTwoTexture(TurnBoard turnBoard)
        {
            var cards = new List<Card>() { turnBoard.FlopBoard.Flop1, turnBoard.FlopBoard.Flop2, turnBoard.FlopBoard.Flop3, turnBoard.TurnCard };
            if (cards.Count(c => c.Suit == SuitEnum.Heart) == 2) SuitedSuit = SuitEnum.Heart;
            if (cards.Count(c => c.Suit == SuitEnum.Spade) == 2) SuitedSuit = SuitEnum.Spade;
            if (cards.Count(c => c.Suit == SuitEnum.Diamond) == 2) SuitedSuit = SuitEnum.Diamond;
            if (cards.Count(c => c.Suit == SuitEnum.Club) == 2) SuitedSuit = SuitEnum.Club;

            SuitedRanks = cards.Where(c => c.Suit == SuitedSuit).Select(c => c.Rank).ToList();
        }

        public Dictionary<Tuple<SuitEnum, SuitEnum>, bool> ShouldAGridFoldToBet(RangeGrid grid)
        {
            var result = new Dictionary<Tuple<SuitEnum, SuitEnum>, bool>();
            var suits = new List<SuitEnum>() { SuitEnum.Heart, SuitEnum.Spade, SuitEnum.Diamond, SuitEnum.Club };
            foreach (var suit1 in suits)
            {
                foreach (var suit2 in suits)
                {
                    bool shouldFold = suit1 != suit2 || suit1 != SuitedSuit;
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

            if (hole1.Suit == SuitedSuit)
            {
                return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.FlushDraw, 2);
            }

            return new Tuple<SuitTextureOutcomeEnum, int>(SuitTextureOutcomeEnum.Nothing, 0);
        }
    }
}
