using System.Collections.Generic;
using System.Linq;
using Common;
using Models;

namespace Flop
{
    public class FlopFiveCardsEnumerator : IFiveCardsEnumerator
    {
        public FlopBoard FlopBoard { get; set; }
        public HoldingHoles HeroHoles { get; set; }

        public FlopFiveCardsEnumerator(FlopBoard flopBoard, HoldingHoles heroHoles)
        {
            FlopBoard = flopBoard;
            HeroHoles = heroHoles;
        }

        public IEnumerable<List<Card>> Enumerate()
        {
            var allCards = Models.Utils.GenerateAllCards().ToList();
            allCards.RemoveElementEqualsTo(FlopBoard.Flop1);
            allCards.RemoveElementEqualsTo(FlopBoard.Flop2);
            allCards.RemoveElementEqualsTo(FlopBoard.Flop3);
            allCards.RemoveElementEqualsTo(HeroHoles.Hole1);
            allCards.RemoveElementEqualsTo(HeroHoles.Hole2);

            var twoCardsCombinations = Models.Utils.Enumerate(allCards.ToArray(), 0, 2);
            foreach (var twoCardsCombination in twoCardsCombinations)
            {
                yield return new List<Card>() {FlopBoard.Flop1, FlopBoard.Flop2, FlopBoard.Flop3,
                    twoCardsCombination.ElementAt(0), twoCardsCombination.ElementAt(1)};
            }
        }
    }
}
