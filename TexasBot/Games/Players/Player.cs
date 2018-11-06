using System;
using System.Collections.Generic;
using System.Linq;
using TexasBot.Models;
using TexasBot.Games.Consultants;
using TexasBot.Games.Recorders;

namespace TexasBot.Games.Players
{
    public class Player : IPlayer
    {
        private readonly IConsultant _consultant;

        private int _chips;

        public string Name { get; set; }
        public void ChargeChips(int chips)
        {
            _chips += chips;
            if (_chips < 0)
            {
                throw new InvalidOperationException();
            }
        }

        public void EarnChips(int chips)
        {
            _chips += chips;
        }

        public List<GameCard> Holes { get; set; }
        public bool HolesReceived => Holes.All(c => c.Known);

        public Player(string name, IConsultant consultant, int chips = int.MaxValue/2)
        {
            _chips = chips;
            Name = name;
            _consultant = consultant;
            Holes = new List<GameCard>()
            {
                new GameCard(),
                new GameCard()
            };
        }

        public PlayerDecision Decide(GameStatus gameStatus, CardStatus cardStatus)
        {
            var decision = _consultant.Consult(gameStatus, cardStatus, new PlayerStatus(_chips, Name));
            return new PlayerDecision(Name, decision);
        }

        public void ReceiveHoles(List<Card> holes)
        {
            if (holes.Count != 2)
            {
                throw new InvalidOperationException();
            }
            Holes[0].SetCard(holes[0]);
            Holes[1].SetCard(holes[1]);
        }
    }
}
