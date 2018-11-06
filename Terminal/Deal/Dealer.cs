using System;
using System.Collections.Generic;
using System.Linq;
using Models;

namespace Terminal.Deal
{
    public class Dealer
    {
        private readonly List<IPlayerProxy> _playerProxies;

        private readonly Round _round;

        private readonly IEnumerable<Card> _cards;

        private readonly IEnumerator<Card> _enumerator;

        private readonly RoundInput _input;

        public Dealer(RoundInput input)
        {
            _input = input;
            _round = new Round(_input);
            _enumerator = ShuffleCards().GetEnumerator();
            _playerProxies = new List<IPlayerProxy>();
            for (int i = 0; i < _input.PlayerNames.Count; i++)
            {
                _playerProxies.Add(new PlayerProxy());
            }
        }

        public void Deal()
        {
            for (int i = 0; i < _playerProxies.Count; i++)
            {
                _playerProxies[i].DealHoles(i, GetNextCard(), GetNextCard());
            }

            Ante();

            Poll();

            _round.Flop1 = GetNextCard();
            _round.Flop2 = GetNextCard();
            _round.Flop3 = GetNextCard();

            Poll();

            _round.Turn = GetNextCard();

            Poll();

            _round.River = GetNextCard();

            Poll();
        }

        private void Ante()
        {
            var smallBlind = _round.GetCurrentPlayer();
            _round.RecordMove(new Move(smallBlind, new Decision(DecisionType.Ante, _input.SmallBlindSize), StageEnum.Preflop));
            _round.MoveToNextPlayer();

            var bigBind = _round.GetCurrentPlayer();
            _round.RecordMove(new Move(bigBind, new Decision(DecisionType.Ante, _input.BigBlindSize), StageEnum.Preflop));
            _round.MoveToNextPlayer();
        }

        private void Poll()
        {
            while (!_round.IsSettled)
            {
                var player = _round.GetCurrentPlayer();
                var playerProxy = GetPlayerProxy(player);
                var decision = playerProxy.GetDecision(_round);
                _round.RecordMove(new Move(player, decision, _round.StageEnum));
                _round.MoveToNextPlayer();
            }
        }

        private IPlayerProxy GetPlayerProxy(Player player)
        {
            return _playerProxies[player.Index];
        }

        private Card GetNextCard()
        {
            _enumerator.MoveNext();
            return _enumerator.Current;
        }

        private IEnumerable<Card> ShuffleCards()
        {
            var suits = new List<SuitEnum>() { SuitEnum.Heart, SuitEnum.Spade, SuitEnum.Diamond, SuitEnum.Club };
            var ranks = new List<RankEnum>() {RankEnum.Ace, RankEnum.King, RankEnum.Queen, RankEnum.Jack, RankEnum.Ten, RankEnum.Nine,
                RankEnum.Eight, RankEnum.Seven, RankEnum.Six, RankEnum.Five, RankEnum.Four, RankEnum.Three, RankEnum.Two};

            var cards = (from suit in suits
                         from rank in ranks
                         select new Card(suit, rank)).ToList();

            Random r = new Random(DateTimeOffset.Now.Millisecond);

            while (cards.Count != 0)
            {
                int index = r.Next(0, cards.Count);
                yield return cards[index];
                cards.RemoveAt(index);
            }
        }
    }
}
