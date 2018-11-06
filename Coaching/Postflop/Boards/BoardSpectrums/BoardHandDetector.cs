using System;
using System.Collections.Generic;
using Models;
using TexasBot.Models;
using Card = Models.Card;

namespace Coaching.Postflop.Boards.BoardSpectrums
{
    public class BoardHandDetector
    {
        public BoardHandTypeEnum DetectBoardHand(BoardStatus boardStatus, HoldingHoles holes)
        {
            var boardCards = new List<Card>() { boardStatus.Flop1, boardStatus.Flop2, boardStatus.Flop3 };
            if (boardStatus.Turn != null)
            {
                boardCards.Add(boardStatus.Turn);
            }
            if (boardStatus.River != null)
            {
                boardCards.Add(boardStatus.River);
            }

            var holesCards = new List<Card>() { holes.Hole1, holes.Hole2 };

            return DetectBoardHand(boardCards, holesCards);
        }

        private BoardHandTypeEnum DetectBoardHand(List<Card> boardCards, List<Card> holesCards)
        {
            var cards = new List<Card>();
            cards.AddRange(boardCards);
            cards.AddRange(holesCards);

            cards = Utils.SortCards(cards);
            var hand = FindingHandsUtils.FindBestHand(cards);

            switch (hand)
            {
                case HandEnum.RoyalFlush:
                    return BoardHandTypeEnum.RoyalFlush;
                case HandEnum.StraightFlush:
                    return CategorizeStraightFlush(boardCards, holesCards);
                case HandEnum.FourOfAKind:
                    return BoardHandTypeEnum.FourOfAKind;
                case HandEnum.FullHouse:
                    return CategorizeFullHouse(boardCards, holesCards);
                case HandEnum.Flush:
                    return CategorizeFlush(boardCards, holesCards);
                case HandEnum.Straight:
                    return CategorizeStraight(boardCards, holesCards);
                case HandEnum.ThreeOfAKind:
                    return CategorizeThreeOfAKind(boardCards, holesCards);
                case HandEnum.TwoPairs:
                    return CategorizeTwoPairs(boardCards, holesCards);
                case HandEnum.OnePair:
                    return CategorizeOnePair(boardCards, holesCards);
                case HandEnum.HighCard:
                    return CategorizeHighCard(boardCards, holesCards);
                default:
                    throw new InvalidOperationException();
            }
        }

        private BoardHandTypeEnum CategorizeStraightFlush(List<Card> boardCards, List<Card> holesCards)
        {
            throw new NotImplementedException();
        }

        private BoardHandTypeEnum CategorizeFullHouse(List<Card> boardCards, List<Card> holesCards)
        {
            throw new NotImplementedException();

        }

        private BoardHandTypeEnum CategorizeFlush(List<Card> boardCards, List<Card> holesCards)
        {
            throw new NotImplementedException();

        }

        private BoardHandTypeEnum CategorizeStraight(List<Card> boardCards, List<Card> holesCards)
        {
            throw new NotImplementedException();

        }

        private BoardHandTypeEnum CategorizeThreeOfAKind(List<Card> boardCards, List<Card> holesCards)
        {
            throw new NotImplementedException();

        }

        private BoardHandTypeEnum CategorizeTwoPairs(List<Card> boardCards, List<Card> holesCards)
        {

            throw new NotImplementedException();
        }

        private BoardHandTypeEnum CategorizeOnePair(List<Card> boardCards, List<Card> holesCards)
        {

            throw new NotImplementedException();
        }

        private BoardHandTypeEnum CategorizeHighCard(List<Card> boardCards, List<Card> holesCards)
        {
            throw new NotImplementedException();

        }
    }
}
