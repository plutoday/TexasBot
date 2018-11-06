using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace TexasBot.Models.Hands
{
    public class FullHouse : IHand
    {
        public RankEnum RankOfThree { get; set; }

        public FullHouse(RankEnum rankOfThree)
        {
            RankOfThree = rankOfThree;
        }
        
        public int CompareTo(IHand other)
        {
            var otherFullHouse = other as FullHouse;
            if (otherFullHouse == null)
            {
                throw new InvalidOperationException();
            }

            return RankOfThree.CompareTo(otherFullHouse.RankOfThree);
        }
    }
}
