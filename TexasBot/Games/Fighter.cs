using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasBot.Models;

namespace TexasBot.Games
{
    public class Fighter
    {
        public List<Card> Holes { get; set; }

        public Fighter(List<Card> holes)
        {
            Holes = new List<Card>(holes);
        }
    }
}
