using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace TexasBot.Models.Hands
{
    public class Flush : IHand
    {
        public List<RankEnum> Ranks { get; set; }

        public Flush(List<RankEnum> ranks)
        {
            Ranks = new List<RankEnum>(ranks);
            Ranks.Sort((r1, r2) => -r1.CompareTo(r2));
        }

        public int CompareTo(IHand other)
        {
            var otherFlush = other as Flush;
            if (otherFlush == null)
            {
                throw new InvalidOperationException();
            }

            for (int i = 0; i < 5; i++)
            {
                if (Ranks[i] != otherFlush.Ranks[i])
                {
                    return Ranks[i].CompareTo(otherFlush.Ranks[i]);
                }
            }

            return 0;
        }
    }
}
