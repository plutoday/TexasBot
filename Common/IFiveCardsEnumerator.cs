using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Common
{
    public interface IFiveCardsEnumerator
    {
        IEnumerable<List<Card>> Enumerate();
    }
}
