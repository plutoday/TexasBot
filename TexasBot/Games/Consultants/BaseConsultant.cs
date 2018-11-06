using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasBot.Games.Players;
using TexasBot.Games.Recorders;

namespace TexasBot.Games.Consultants
{
    public abstract class BaseConsultant : IConsultant
    {
        public Decision Consult(GameStatus gameStatus, CardStatus cardStatus, PlayerStatus playerStatus)
        {
            var highestBet = gameStatus.PlayerRecords.Max(p => p.ChipsBet);
            var myBet = gameStatus.PlayerRecords.First(p => string.Equals(playerStatus.PlayerName, p.Name)).ChipsBet;

            var betShort = highestBet - myBet;
            if (betShort > 0)
            {
                return ConsultChanllenged(gameStatus, cardStatus, playerStatus, betShort);
            }
            else
            {
                return ConsultUnchanllenged(gameStatus, cardStatus, playerStatus);
            }
        }

        protected abstract Decision ConsultChanllenged(GameStatus gameStatus, CardStatus cardStatus,
            PlayerStatus playerStatus, int betShort);

        protected abstract Decision ConsultUnchanllenged(GameStatus gameStatus, CardStatus cardStatus,
            PlayerStatus playerStatus);
    }
}
