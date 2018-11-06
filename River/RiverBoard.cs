using System;
using Models;
using Turn;
using Turn.TurnBoardRankTextures;
using Turn.TurnBoardSuitTextures;

namespace River
{
    public class RiverBoard
    {
        public TurnBoard TurnBoard { get; set; }
        public Card River { get; set; }

        public RiverRankTextureEnum RankTexture { get; set; }
        public RiverSuitTextureEnum SuitTexture { get; set; }

        public RiverBoard(TurnBoard turnBoard, Card river)
        {
            TurnBoard = turnBoard;
            River = river;
            SetupTexture();
        }

        private void SetupTexture()
        {
            SetupRankTexture();
            SetupSuitTexture();
        }

        private void SetupRankTexture()
        {
            switch (TurnBoard.RankTexture)
            {
                case TurnBoardRankTextureEnum.Foursome:
                    RankTexture = RiverRankTextureEnum.Foursome;
                    break;
                case TurnBoardRankTextureEnum.HighTri:
                    var highTri = new HighTriRankTexture(TurnBoard);
                    if (River.Rank == highTri.TriCard1.Rank)
                    {
                        RankTexture = RiverRankTextureEnum.Foursome;
                    }
                    else if (River.Rank == highTri.SingleCard.Rank)
                    {
                        RankTexture = RiverRankTextureEnum.HighTriLowPair;
                    }
                    else if (River.Rank > highTri.TriCard1.Rank)
                    {
                        RankTexture = RiverRankTextureEnum.MiddleTri;
                    }
                    else
                    {
                        RankTexture = RiverRankTextureEnum.HighTri;
                    }
                    break;
                case TurnBoardRankTextureEnum.LowTri:
                    var lowTri = new LowTriRankTexture(TurnBoard);
                    if (River.Rank == lowTri.TriCard1.Rank)
                    {
                        RankTexture = RiverRankTextureEnum.Foursome;
                    }
                    else if (River.Rank == lowTri.SingleCard.Rank)
                    {
                        RankTexture = RiverRankTextureEnum.LowTriHighPair;
                    }
                    else if (River.Rank > lowTri.TriCard1.Rank)
                    {
                        RankTexture = RiverRankTextureEnum.LowTri;
                    }
                    else
                    {
                        RankTexture = RiverRankTextureEnum.MiddleTri;
                    }
                    break;
                case TurnBoardRankTextureEnum.TwoPairs:
                    var twoPairs = new TwoPairsRankTexture(TurnBoard);
                    if (River.Rank == twoPairs.HighPairCard1.Rank)
                    {
                        RankTexture = RiverRankTextureEnum.HighTriLowPair;
                    }
                    else if (River.Rank == twoPairs.LowPairCard1.Rank)
                    {
                        RankTexture = RiverRankTextureEnum.LowTriHighPair;
                    }
                    else if (River.Rank > twoPairs.HighPairCard1.Rank)
                    {
                        RankTexture = RiverRankTextureEnum.LowTwoPairs;
                    }
                    else if (River.Rank > twoPairs.LowPairCard1.Rank)
                    {
                        RankTexture = RiverRankTextureEnum.HighLowTwoPairs;
                    }
                    else
                    {
                        RankTexture = RiverRankTextureEnum.HighTwoPairs;
                    }
                    break;
                case TurnBoardRankTextureEnum.HighPair:
                    var highPair = new HighPairRankTexture(TurnBoard);
                    if (River.Rank == highPair.PairCard1.Rank)
                    {
                        RankTexture = RiverRankTextureEnum.HighTri;
                    }
                    else if (River.Rank == highPair.MiddleCard.Rank)
                    {
                        RankTexture = RiverRankTextureEnum.HighTwoPairs;
                    }
                    else if (River.Rank == highPair.LowCard.Rank)
                    {
                        RankTexture = RiverRankTextureEnum.HighLowTwoPairs;
                    }
                    else if (River.Rank > highPair.PairCard1.Rank)
                    {
                        RankTexture = RiverRankTextureEnum.SecondPair;
                    }
                    else
                    {
                        RankTexture = RiverRankTextureEnum.HighPair;
                    }
                    break;
                case TurnBoardRankTextureEnum.MiddlePair:
                    var middlePair = new MiddlePairRankTexture(TurnBoard);
                    if (River.Rank == middlePair.PairCard1.Rank)
                    {
                        RankTexture = RiverRankTextureEnum.MiddleTri;
                    }
                    else if (River.Rank == middlePair.HighCard.Rank)
                    {
                        RankTexture = RiverRankTextureEnum.HighTwoPairs;
                    }
                    else if (River.Rank == middlePair.LowCard.Rank)
                    {
                        RankTexture = RiverRankTextureEnum.LowTwoPairs;
                    }
                    else if (River.Rank > middlePair.PairCard1.Rank)
                    {
                        RankTexture = RiverRankTextureEnum.ThirdPair;
                    }
                    else
                    {
                        RankTexture = RiverRankTextureEnum.SecondPair;
                    }
                    break;
                case TurnBoardRankTextureEnum.LowPair:
                    var lowPair = new LowPairRankTexture(TurnBoard);
                    if (River.Rank == lowPair.PairCard1.Rank)
                    {
                        RankTexture = RiverRankTextureEnum.LowTri;
                    }
                    else if (River.Rank == lowPair.HighCard.Rank)
                    {
                        RankTexture = RiverRankTextureEnum.HighLowTwoPairs;
                    }
                    else if (River.Rank == lowPair.MiddleCard.Rank)
                    {
                        RankTexture = RiverRankTextureEnum.LowTwoPairs;
                    }
                    else if (River.Rank > lowPair.PairCard1.Rank)
                    {
                        RankTexture = RiverRankTextureEnum.LowPair;
                    }
                    else
                    {
                        RankTexture = RiverRankTextureEnum.ThirdPair;
                    }
                    break;
                case TurnBoardRankTextureEnum.Singles:
                    var singles = new SinglesRankTexture(TurnBoard);
                    if (River.Rank == singles.HighCard.Rank)
                    {
                        RankTexture = RiverRankTextureEnum.HighPair;
                    }
                    else if (River.Rank == singles.SecondCard.Rank)
                    {
                        RankTexture = RiverRankTextureEnum.SecondPair;
                    }
                    else if (River.Rank == singles.ThirdCard.Rank)
                    {
                        RankTexture = RiverRankTextureEnum.ThirdPair;
                    }
                    else if (River.Rank == singles.LowCard.Rank)
                    {
                        RankTexture = RiverRankTextureEnum.LowPair;
                    }
                    else
                    {
                        RankTexture = RiverRankTextureEnum.Singles;
                    }
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        private void SetupSuitTexture()
        {
            switch (TurnBoard.SuitTexture)
            {
                case TurnBoardSuitTextureEnum.SuitedFour:
                    var suitedFour = new SuitedFourTexture(TurnBoard);
                    if (River.Suit == suitedFour.SuitedSuit)
                    {
                        SuitTexture = RiverSuitTextureEnum.FiveSuited;
                    }
                    else
                    {
                        SuitTexture = RiverSuitTextureEnum.FourSuited;
                    }
                    break;
                case TurnBoardSuitTextureEnum.SuitedThree:
                    var suitedThree = new SuitedThreeTexture(TurnBoard);
                    if (River.Suit == suitedThree.SuitedSuit)
                    {
                        SuitTexture = RiverSuitTextureEnum.FourSuited;
                    }
                    else
                    {
                        SuitTexture = RiverSuitTextureEnum.ThreeSuited;
                    }
                    break;
                case TurnBoardSuitTextureEnum.SuitedTwoPairs:
                    var suitedTwoPairs = new SuitedTwoPairsTexture(TurnBoard);
                    if (River.Suit == suitedTwoPairs.SuitedSuit1)
                    {
                        SuitTexture = RiverSuitTextureEnum.ThreeSuited;
                    }
                    else if (River.Suit == suitedTwoPairs.SuitedSuit2)
                    {
                        SuitTexture = RiverSuitTextureEnum.ThreeSuited;
                    }
                    else
                    {
                        SuitTexture = RiverSuitTextureEnum.Offsuit;
                    }
                    break;
                case TurnBoardSuitTextureEnum.SuitedTwo:
                    var suitedTwo = new SuitedTwoTexture(TurnBoard);
                    if (River.Suit == suitedTwo.SuitedSuit)
                    {
                        SuitTexture = RiverSuitTextureEnum.ThreeSuited;
                    }
                    else
                    {
                        SuitTexture = RiverSuitTextureEnum.Offsuit;
                    }
                    break;
                case TurnBoardSuitTextureEnum.Offsuit:
                    SuitTexture = RiverSuitTextureEnum.Offsuit;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
