using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasBot.Games.Players;
using TexasBot.Models;

namespace TexasBot.Games.Dealers
{
    public interface IDealer
    {
        void RegisterPlayer(IPlayer player);
    }
}
