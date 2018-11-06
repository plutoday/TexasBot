using System;
using System.Collections.Generic;
using System.Linq;

namespace Coaching
{
    public class Recorder
    {
        public List<Round> Rounds { get; set; } 

        public Round CurrentRound { get; set; }

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

            StartNewRound(numOfPlayers, myPosition, buttonPosition, players.Select(p =>p.Item1).ToList(), players.Select(p => p.Item2).ToList());
            
            CurrentRound.Drive();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numOfPlayers"></param>
        /// <param name="myPosition">0 to numOfPlayers-1</param>
        /// <param name="buttonPosition">0 to numOfPlayers-1</param>
        /// <param name="playerNames"></param>
        /// <param name="playerStackSizes"></param>
        public void StartNewRound(int numOfPlayers, int myPosition, int buttonPosition, List<string> playerNames, List<int> playerStackSizes)
        {
            if (CurrentRound != null)
            {
                throw new Exception("CurrentRound should be null ");
            }
            CurrentRound = new Round(numOfPlayers, myPosition, buttonPosition, playerNames, playerStackSizes, Input);
        }
    }
}
