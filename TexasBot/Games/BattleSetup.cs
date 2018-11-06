using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasBot.Models;

namespace TexasBot.Games
{
    public class BattleSetup
    {
        public List<Card> CommunityCards { get; set; }

        public BattleSetup(List<Card> communityCards)
        {
            if (communityCards.Count != 5)
            {
                throw new InvalidOperationException();
            }
            CommunityCards = new List<Card>(communityCards);
        }
    }
}
