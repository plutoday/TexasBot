using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasBot.Tools;

namespace TexasBot.Models
{
    public class OpenHandOf7
    {
        /// <summary>
        /// Cards already known, holes or flopped
        /// </summary>
        public List<Card> SeenCards { get; set; }

        /// <summary>
        /// Cards impossible to get, hold by others
        /// </summary>
        public List<Card> ImpossibleCards { get; set; }

        /// <summary>
        /// Cards possible to get, no one holds them
        /// </summary>
        public List<Card> PossibleCards { get; set; }

        public IEnumerable<HandOf7> EnumerateAllPossibleHandOf7s()
        {
            if (SeenCards.Count == 7)
            {
                yield return new HandOf7(SeenCards);
                yield break;
            }

            var possibleCombinations = Utils.Enumerate(PossibleCards.ToArray(), 0, 7 - SeenCards.Count);
            foreach (var combination in possibleCombinations)
            {
                var cards = combination.ToList();
                cards.AddRange(SeenCards);
                cards = Utils.SortCards(cards);
                yield return new HandOf7(cards);
            }
        }
    }
}
