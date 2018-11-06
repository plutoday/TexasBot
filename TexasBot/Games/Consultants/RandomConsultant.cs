using System;
using System.Linq;
using TexasBot.Games.Players;
using TexasBot.Games.Recorders;

namespace TexasBot.Games.Consultants
{
    public class RandomConsultant : BaseConsultant
    {

        protected override Decision ConsultChanllenged(GameStatus gameStatus, CardStatus cardStatus, PlayerStatus playerStatus, int betShort)
        {
            bool raise = DateTimeOffset.UtcNow.Ticks % 17 == 0;

            if (raise)
            {
                return new Decision()
                {
                    DecisionEnum = DecisionEnum.Raise,
                    Chips = betShort + 10
                };
            }
            else
            {
                bool fold = DateTimeOffset.UtcNow.Ticks % 3 == 0;
                if (fold)
                {
                    return new Decision()
                    {
                        DecisionEnum = DecisionEnum.Fold
                    };
                }
                else
                {
                    return new Decision()
                    {
                        DecisionEnum = DecisionEnum.Call,
                        Chips = betShort
                    };
                }
            }
        }

        protected override Decision ConsultUnchanllenged(GameStatus gameStatus, CardStatus cardStatus, PlayerStatus playerStatus)
        {
            bool raise = DateTimeOffset.UtcNow.Ticks % 3 == 0;

            if (raise)
            {
                return new Decision()
                {
                    DecisionEnum = DecisionEnum.Raise,
                    Chips = 10
                };
            }
            else
            {
                return new Decision()
                {
                    DecisionEnum = DecisionEnum.Check
                };
            }
        }
    }
}
