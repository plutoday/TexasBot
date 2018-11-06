using System.Collections.Generic;
using System.Linq;
using Common;
using Models;

namespace Turn
{
    public class TurnFiveCardsEnumerator : IFiveCardsEnumerator
    {
        public HoldingHoles HeroHoles { get; set; }
        public TurnBoard TurnBoard { get; set; }

        public TurnFiveCardsEnumerator(TurnBoard turnBoard, HoldingHoles heroHoles)
        {
            TurnBoard = turnBoard;
            HeroHoles = heroHoles;
        }

        public IEnumerable<List<Card>> Enumerate()
        {
            var allCards = Models.Utils.GenerateAllCards().ToList();
            allCards.RemoveElementEqualsTo(TurnBoard.FlopBoard.Flop1);
            allCards.RemoveElementEqualsTo(TurnBoard.FlopBoard.Flop2);
            allCards.RemoveElementEqualsTo(TurnBoard.FlopBoard.Flop3);
            allCards.RemoveElementEqualsTo(TurnBoard.TurnCard);
            allCards.RemoveElementEqualsTo(HeroHoles.Hole1);
            allCards.RemoveElementEqualsTo(HeroHoles.Hole2);

            foreach (var card in allCards)
            {
                yield return new List<Card>()
                {
                    TurnBoard.FlopBoard.Flop1,
                    TurnBoard.FlopBoard.Flop2,
                    TurnBoard.FlopBoard.Flop3,
                    TurnBoard.TurnCard,
                    card
                };
            }
        }
    }
}
