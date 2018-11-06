using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Terminal
{
    public class Recorder
    {
        public List<Round> Rounds { get; set; }

        public Input Input { get; set; }

        public Recorder()
        {
            Rounds = new List<Round>();
        }

        public void Drive()
        {
            int numOfPlayers = Input.GetNumOfPlayers();
            int buttonPosition = Input.GetButtonPosition();
            int myPosition = Input.GetMyPosition();

            List<Tuple<string, int>> players = Input.GetPlayerNamesAndStackSizes(numOfPlayers).ToList();

            var round = new Round(new RoundInput(numOfPlayers, buttonPosition, players.Select(p => p.Item1).ToList(), players.Select(p => p.Item2).ToList(), 5, 10));

            new RoundDriver().Drive(round, new RoundSetup() { HeroIndex = myPosition });
        }
    }
}
