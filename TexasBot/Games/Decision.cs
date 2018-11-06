using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasBot.Games
{
    public class Decision
    {
        public DecisionEnum DecisionEnum { get; set; }

        /// <summary>
        /// Required for Raise and Call
        /// </summary>
        public int Chips { get; set; }
    }
}
