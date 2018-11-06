using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasBot.Models;

namespace TexasBot.Games.Dealers
{
    public interface ICardDrawer
    {
        List<Card> DrawCards(int count);
        Card DrawSingleCard();
    }
}
