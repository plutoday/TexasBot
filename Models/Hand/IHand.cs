using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Hand
{
    public interface IHand
    {
        int HandRank { get; }

        /// <summary>
        /// positive means win
        /// zero means tie
        /// negative means lose
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        int CompareTo(IHand other);
    }
}
