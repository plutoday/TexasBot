using System.Collections.Generic;
using System.Linq;
using Models;

namespace Coaching.Postflop.Boards
{
    public class BoardStatus
    {
        public Card Flop1 { get; set; }
        public Card Flop2 { get; set; }
        public Card Flop3 { get; set; }
        public Card Turn { get; set; }
        public Card River { get; set; }

        public BoardStageEnum BoardStage 
        {
            get
            {
                if (River != null)
                {
                    return BoardStageEnum.River;
                }
                if (Turn != null)
                {
                    return BoardStageEnum.Turn;
                }

                return BoardStageEnum.Flop;
            }
        }

        public BoardStatus(Card flop1, Card flop2, Card flop3, Card turn = null, Card river = null)
        {
            Flop1 = flop1;
            Flop2 = flop2;
            Flop3 = flop3;
            Turn = turn;
            River = river;
        }

        public List<Board> GenerateAllPossibleBoards(List<Card> cardsToRemove)
        {
            if (River != null)
            {
                return new List<Board>() {new Board(Flop1, Flop2, Flop3, Turn, River)};
            }

            var cards = TexasBot.Tools.Utils.GenerateAllCards().Select(c => c.ConvertCard()).ToList();
            foreach (var card in cardsToRemove)
            {
                cards.RemoveElementEqualsTo(card);
            }
            if (Turn != null)
            {
                cards.RemoveElementEqualsTo(Turn);
            }
            if (River != null)
            {
                cards.RemoveElementEqualsTo(River);
            }

            if (Turn != null)
            {
                return GenerateAllPossibleBoardsOnTurn(cards);
            }

            return GenerateAllPossibleBoardsOnFlop(cards);
        }

        private List<Board> GenerateAllPossibleBoardsOnTurn(List<Card> candidateCards)
        {
            return candidateCards.Select(candidateCard => new Board(Flop1, Flop2, Flop3, Turn, candidateCard)).ToList();
        }

        private List<Board> GenerateAllPossibleBoardsOnFlop(List<Card> candidateCards)
        {
            var result = new List<Board>();
            for (int i = 0; i < candidateCards.Count; i++)
            {
                for (int j = 0; j < candidateCards.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    result.Add(new Board(Flop1, Flop2, Flop3, candidateCards[i], candidateCards[j]));
                }
            }

            return result;
        }
    }

    public enum BoardStageEnum
    {
        Flop,
        Turn,
        River
    }
}
