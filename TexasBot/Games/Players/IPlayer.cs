using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasBot.Games.Dealers;
using TexasBot.Games.Recorders;
using TexasBot.Models;

namespace TexasBot.Games.Players
{
    public interface IPlayer
    {
        PlayerDecision Decide(GameStatus gameStatus, CardStatus cardStatus);
        void ReceiveHoles(List<Card> holes);
        string Name { get; }
        void ChargeChips(int chips);
        void EarnChips(int chips);
    }
}
