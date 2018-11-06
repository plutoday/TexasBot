using System;
using Flop;
using Flop.FlopBoardRankTextures;
using Flop.FlopBoardSuitTextures;
using Models;

namespace Turn
{
    public class TurnBoard
    {
        public FlopBoard FlopBoard { get; set; }
        public Card TurnCard { get; set; }

        public TurnBoardRankTextureEnum RankTexture { get; set; }
        public TurnBoardSuitTextureEnum SuitTexture { get; set; }

        public TurnBoard(FlopBoard flopBoard, Card turnCard)
        {
            FlopBoard = flopBoard;
            TurnCard = turnCard;
            SetTextures();
        }

        private void SetTextures()
        {
            SetRankTexture();
            SetSuitTexture();
        }

        private void SetRankTexture()
        {
            switch (FlopBoard.RankTexture)
            {
                case FlopBoardRankTextureEnum.ThreeSome:
                    if (TurnCard.Rank == FlopBoard.Flop1.Rank) RankTexture = TurnBoardRankTextureEnum.Foursome;
                    else if (TurnCard.Rank > FlopBoard.Flop1.Rank) RankTexture = TurnBoardRankTextureEnum.LowTri;
                    else RankTexture = TurnBoardRankTextureEnum.HighTri;
                    break;
                case FlopBoardRankTextureEnum.HighPair:
                    var highPairBoard = new HighPairRankTexture(FlopBoard);
                    if (TurnCard.Rank == highPairBoard.PairCard1.Rank) RankTexture = TurnBoardRankTextureEnum.HighTri;
                    else if (TurnCard.Rank == highPairBoard.SingleCard.Rank) RankTexture = TurnBoardRankTextureEnum.TwoPairs;
                    else if (TurnCard.Rank > highPairBoard.PairCard1.Rank) RankTexture = TurnBoardRankTextureEnum.MiddlePair;
                    else RankTexture = TurnBoardRankTextureEnum.HighPair;
                    break;
                case FlopBoardRankTextureEnum.LowPair:
                    var lowPairBoard = new LowPairRankTexture(FlopBoard);
                    if (TurnCard.Rank == lowPairBoard.PairCard1.Rank) RankTexture = TurnBoardRankTextureEnum.LowTri;
                    else if (TurnCard.Rank == lowPairBoard.SingleCard.Rank) RankTexture = TurnBoardRankTextureEnum.TwoPairs;
                    else if (TurnCard.Rank < lowPairBoard.PairCard1.Rank) RankTexture = TurnBoardRankTextureEnum.MiddlePair;
                    else RankTexture = TurnBoardRankTextureEnum.LowPair;
                    break;
                case FlopBoardRankTextureEnum.Singles:
                    var singlesBoard = new SinglesRankTexture(FlopBoard);
                    if (TurnCard.Rank == singlesBoard.HighCard.Rank) RankTexture = TurnBoardRankTextureEnum.HighPair;
                    else if (TurnCard.Rank == singlesBoard.MiddleCard.Rank) RankTexture = TurnBoardRankTextureEnum.MiddlePair;
                    else if (TurnCard.Rank == singlesBoard.LowCard.Rank) RankTexture = TurnBoardRankTextureEnum.LowPair;
                    else RankTexture = TurnBoardRankTextureEnum.Singles;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        private void SetSuitTexture()
        {
            switch (FlopBoard.SuitTexture)
            {
                case FlopBoardSuitTextureEnum.SuitedThree:
                    SuitTexture = TurnCard.Suit == FlopBoard.Flop1.Suit ? TurnBoardSuitTextureEnum.SuitedFour : TurnBoardSuitTextureEnum.SuitedThree;
                    break;
                case FlopBoardSuitTextureEnum.SuitedTwo:
                    var suitedTwoBoard = new SuitedTwoSuitTexture(FlopBoard);
                    if (TurnCard.Suit == suitedTwoBoard.SuitedCard1.Suit) SuitTexture = TurnBoardSuitTextureEnum.SuitedThree;
                    else if (TurnCard.Suit == suitedTwoBoard.OffsuitCard.Suit) SuitTexture = TurnBoardSuitTextureEnum.SuitedTwoPairs;
                    else SuitTexture = TurnBoardSuitTextureEnum.SuitedTwo;
                    break;
                case FlopBoardSuitTextureEnum.Rainbow:
                    if (TurnCard.Suit == FlopBoard.Flop1.Suit || TurnCard.Suit == FlopBoard.Flop2.Suit || TurnCard.Suit == FlopBoard.Flop3.Suit)
                        SuitTexture = TurnBoardSuitTextureEnum.SuitedTwo;
                    else SuitTexture = TurnBoardSuitTextureEnum.Offsuit;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
