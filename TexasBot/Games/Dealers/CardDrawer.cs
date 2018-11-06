using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasBot.Models;
using TexasBot.Tools;

namespace TexasBot.Games.Dealers
{
    public class CardDrawer : ICardDrawer
    {
        private readonly List<Card> _cards;
        private int _index;

        public CardDrawer()
        {
            _cards = new List<Card>();
            var cards = Utils.GenerateAllCards().ToList();
            var random = new Random(DateTimeOffset.UtcNow.Millisecond);
            while (cards.Count != 0)
            {
                int index = random.Next() % cards.Count;
                _cards.Add(cards[index]);
                cards.RemoveAt(index);
            }
        }

        public List<Card> DrawCards(int count)
        {
            var cards = _cards.Skip(_index).Take(count).ToList();
            _index += count;
            return cards;
        }

        public Card DrawSingleCard()
        {
            var card = _cards[_index];
            _index++;
            return card;
        }
    }
}
