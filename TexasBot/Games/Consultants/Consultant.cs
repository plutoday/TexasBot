using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasBot.Games.Players;
using TexasBot.Games.Recorders;
using TexasBot.Models;

namespace TexasBot.Games.Consultants
{
    public class Consultant : BaseConsultant
    {
        private double _chanllengedRaiseBar = 0.8;
        private double _chanllengedCallBar = 0.65;
        private double _unchanllengedRaiseBar = 0.75;

        private int _unchanllengedRaiseChips = 10;

        protected override Decision ConsultChanllenged(GameStatus gameStatus, CardStatus cardStatus, PlayerStatus playerStatus, int betShort)
        {
            var compareResult = Evaluate(gameStatus, cardStatus, playerStatus);

            if (compareResult.WinRatio > _chanllengedRaiseBar)
            {
                return new Decision()
                {
                    DecisionEnum = DecisionEnum.Raise,
                    Chips = betShort + 10
                };
            }

            if (compareResult.WinRatio > _chanllengedCallBar)
            {
                return new Decision()
                {
                    DecisionEnum = DecisionEnum.Call,
                    Chips = betShort
                };
            }

            return new Decision()
            {
                DecisionEnum = DecisionEnum.Fold
            };
        }

        protected override Decision ConsultUnchanllenged(GameStatus gameStatus, CardStatus cardStatus, PlayerStatus playerStatus)
        {
            var compareResult = Evaluate(gameStatus, cardStatus, playerStatus);

            if (compareResult.WinRatio > _unchanllengedRaiseBar)
            {
                return new Decision()
                {
                    DecisionEnum = DecisionEnum.Raise,
                    Chips = _unchanllengedRaiseChips
                };
            }

            return new Decision()
            {
                DecisionEnum = DecisionEnum.Check
            };
        }

        private CompareResult Evaluate(GameStatus gameStatus, CardStatus cardStatus, PlayerStatus playerStatus)
        {
            var hostFighter = new Fighter(cardStatus.Holes);
            var round = new Round();
            round.SetHostFighter(hostFighter);
            if (cardStatus.Flops.Count != 0)
            {
                round.SetFlops(cardStatus.Flops);
            }

            if (cardStatus.Turn != null)
            {
                round.SetTurn(cardStatus.Turn);
            }

            if (cardStatus.River != null)
            {
                round.SetRiver(cardStatus.River);
            }

            var compareResult = round.Evaluate();

            return compareResult;
        }
    }
}
