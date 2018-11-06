using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasBot.Models.Hands
{
    public interface IHand
    {
        int CompareTo(IHand other);
    }
}
