using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasBot.Models.Hands
{
    public class RoyalFlush : IHand
    {
        public int CompareTo(IHand other)
        {
            var otherRoyalFlush = other as RoyalFlush;
            if (otherRoyalFlush == null)
            {
                throw new InvalidOperationException($"other is not a RoyalFlush : {other.GetType()}");
            }
            //No tie breaker between RoyalFlush
            return 0;
        }

    }
}
