using System.Collections.Generic;
using System.Linq;
using TexasBot.Models;
using TexasBot.Tools;

namespace TexasBot.Games
{
    public class Round
    {
        public Fighter HostFighter { get; set; }
        public List<Card> Flops { get; set; }
        public Card Turn { get; set; }
        public Card River { get; set; }

        public void SetHostFighter(Fighter hostFighter)
        {
            HostFighter = hostFighter;
        }

        public void SetFlops(List<Card> flops)
        {
            Flops = new List<Card>(flops);
        }

        public void SetTurn(Card turn)
        {
            Turn = turn;
        }

        public void SetRiver(Card river)
        {
            River = river;
        }

        public CompareResult Evaluate()
        {
            var possibleGuestFighters = EnumerateGuestFighters();
            var compareResult = new CompareResult(0, 0, 0);
            
            foreach (var guestFighter in possibleGuestFighters)
            {
                var battleGround = new BattleGround();
                if (Flops != null)
                {
                    battleGround.SetFlops(Flops);
                }
                if (Turn != null)
                {
                    battleGround.SetTurn(Turn);
                }

                if (River != null)
                {
                    battleGround.SetRiver(River);
                }

                compareResult.Add(battleGround.Fight(HostFighter, guestFighter));
            }

            return compareResult;
        }

        private IEnumerable<Fighter> EnumerateGuestFighters()
        {
            var allCards = Utils.GenerateAllCards().Except(HostFighter.Holes);
            if (Flops != null)
            {
                allCards = allCards.Except(Flops);
            }

            if (Turn != null)
            {
                allCards = allCards.Except(new []{Turn});
            }

            if (River != null)
            {
                allCards = allCards.Except(new[] {River});
            }

            var twoCardsCombinations = Utils.Enumerate(allCards.ToArray(), 0, 2);
            foreach (var twoCardsCombination in twoCardsCombinations)
            {
                yield return new Fighter(twoCardsCombination.ToList());
            }
        }


    }
}
