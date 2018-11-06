using TexasBot.Games.Players;
using TexasBot.Games.Recorders;

namespace TexasBot.Games.Consultants
{
    public interface IConsultant
    {
        Decision Consult(GameStatus gameStatus, CardStatus cardStatus, PlayerStatus playerStatus);
    }
}
