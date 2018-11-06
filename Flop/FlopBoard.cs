using System.Collections.Generic;
using Models;

namespace Flop
{
    public class FlopBoard
    {
        public Card Flop1 { get; set; }
        public Card Flop2 { get; set; }
        public Card Flop3 { get; set; }

        public FlopBoardRankTextureEnum RankTexture { get; set; }

        public FlopBoardSuitTextureEnum SuitTexture { get; set; }

        public FlopBoard(Card flop1, Card flop2, Card flop3)
        {
            Flop1 = flop1;
            Flop2 = flop2;
            Flop3 = flop3;
            SetTextures();
        }

        private void SetTextures()
        {
            SetRankTexture();
            SetSuitTexture();
        }

        private void SetRankTexture()
        {
            var ranks = new List<RankEnum>() {Flop1.Rank, Flop2.Rank, Flop3.Rank};
            ranks.Sort();

            if (ranks[0] == ranks[1] && ranks[1] == ranks[2])
            {
                RankTexture = FlopBoardRankTextureEnum.ThreeSome;
                return;
            }

            if (ranks[0] == ranks[1])
            {
                RankTexture = FlopBoardRankTextureEnum.LowPair;
                return;
            }

            if (ranks[1] == ranks[2])
            {
                RankTexture = FlopBoardRankTextureEnum.HighPair;
                return;
            }

            RankTexture = FlopBoardRankTextureEnum.Singles;
        }

        private void SetSuitTexture()
        {
            var suits = new List<SuitEnum>() {Flop1.Suit, Flop2.Suit, Flop3.Suit};
            suits.Sort();
            if (suits[0] == suits[2])
            {
                SuitTexture = FlopBoardSuitTextureEnum.SuitedThree;
                return;
            }
            if (suits[0] == suits[1] || suits[1] == suits[2])
            {
                SuitTexture = FlopBoardSuitTextureEnum.SuitedTwo;
                return;
            }
            SuitTexture = FlopBoardSuitTextureEnum.Rainbow;
        }
    }
}
