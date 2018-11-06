using System.Collections.Generic;
using Common;
using Models;

namespace River
{
    public class RiverFiveCardsEnumerator : IFiveCardsEnumerator
    {
        private readonly RiverBoard _riverBoard;
        public RiverFiveCardsEnumerator(RiverBoard riverBoard)
        {
            _riverBoard = riverBoard;
        }

        public IEnumerable<List<Card>> Enumerate()
        {
            yield return new List<Card>()
            {
                _riverBoard.TurnBoard.FlopBoard.Flop1,
                _riverBoard.TurnBoard.FlopBoard.Flop2,
                _riverBoard.TurnBoard.FlopBoard.Flop3,
                _riverBoard.TurnBoard.TurnCard,
                _riverBoard.River
            };
        }
    }
}
