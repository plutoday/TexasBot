using System;
using System.Collections.Generic;
using Models;

namespace Coaching
{
    public class Input
    {
        private readonly Brain _brain = new Brain();

        private List<Tuple<string, int>> _players = new List<Tuple<string, int>>()
        {
            new Tuple<string, int>("Alice", 10000),
            new Tuple<string, int>("Bob", 10000),
            new Tuple<string, int>("Chris", 10000),
            new Tuple<string, int>("David", 10000),
            new Tuple<string, int>("Ellen", 10000),
            new Tuple<string, int>("Frank", 10000),
        }; 

        public int GetNumOfPlayers()
        {
            return GetInputOfInt("Please input the number of players");
        }

        public int GetMyPosition()
        {
            return GetInputOfInt("Please input my position");
        }

        public int GetButtonPosition()
        {
            return GetInputOfInt("Please input Button's position");
        }

        public IEnumerable<Tuple<string, int>> GetPlayerNamesAndStackSizes(int numOfPlayers)
        {
            for (int i = 0; i < numOfPlayers; i++)
            {
                var name = _players[i].Item1;
                var stackSize = _players[i].Item2;
                /*
                var name = GetStringInput($"Please input {i}th player's name");
                var stackSize = GetInputOfInt($"Please input the Stack size for {i}th player:{name}");
                */
                yield return new Tuple<string, int>(name, stackSize);
            }
        }

        public Decision GetDecision(Player player, Player currentRaiser,
            ISet<DecisionType> candidateDecisionTypes, int chipsToCall)
        {
            DecisionType decisionType = GetDecisionTypeInput($"Please input the move made by {player.Name}: {string.Join("/", candidateDecisionTypes)}");

            if (decisionType == DecisionType.Raise || decisionType == DecisionType.Reraise)
            {
                int chipsToRaise = GetInputOfInt($"Please input the raise size by {player.Name}");
                return new Decision(decisionType, chipsToRaise);
            }

            if (decisionType == DecisionType.Call)
            {
                return new Decision(decisionType, chipsToCall);
            }

            return new Decision(decisionType, 0);
        }

        public Decision GetMyDecision(Round round, ISet<DecisionType> candidateDecisionTypes)
        {
            var decision = _brain.GetDecision(round);

            Console.WriteLine($"My decision is {decision.DecisionType} - {decision.ChipsAdded}");
            return decision;
        }

        public Card GetHole1()
        {
            var card = GetCardInput("Input Hole1");
            return card;
        }

        public Card GetHole2()
        {
            var card = GetCardInput("Input Hole2");
            return card;
        }

        public Card GetFlop1()
        {
            var card = GetCardInput("Input Flop1");
            return card;
        }

        public Card GetFlop2()
        {
            var card = GetCardInput("Input Flop2");
            return card;
        }

        public Card GetFlop3()
        {
            var card = GetCardInput("Input Flop3");
            return card;
        }

        public Card GetTurn()
        {
            var card = GetCardInput("Input Turn");
            return card;
        }

        public Card GetRiver()
        {
            var card = GetCardInput("Input River");
            return card;
        }

        public string GetWinnerName()
        {
            var name = GetStringInput($"Please input the name of the winner");
            return name;
        }

        private int GetInputOfInt(string hint)
        {
            while (true)
            {
                Console.WriteLine(hint);
                string input = Console.ReadLine();
                int intResult;
                if (int.TryParse(input, out intResult))
                {
                    return intResult;
                }
                Console.WriteLine($"Unrecgonized: {input}");
            }
        }

        private string GetStringInput(string hint)
        {
            while (true)
            {
                Console.WriteLine(hint);
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }
            }
        }

        private DecisionType GetDecisionTypeInput(string hint)
        {
            while (true)
            {
                Console.WriteLine(hint);
                DecisionType decisionType;
                string input = Console.ReadLine();
                if (Enum.TryParse(input, out decisionType))
                {
                    return decisionType;
                }

                Console.WriteLine($"Unrecgonized: {input}");
            }
        }

        private SuitEnum GetSuitEnumInput(string hint)
        {
            while (true)
            {
                Console.WriteLine(hint);
                SuitEnum suit;
                string input = Console.ReadLine();
                if (Enum.TryParse(input, out suit))
                {
                    return suit;
                }

                Console.WriteLine($"Unrecgonized: {input}");
            }
        }

        private RankEnum GetRankEnumInput(string hint)
        {
            while (true)
            {
                Console.WriteLine(hint);
                RankEnum rank;
                string input = Console.ReadLine();
                if (Enum.TryParse(input, out rank))
                {
                    return rank;
                }

                Console.WriteLine($"Unrecgonized: {input}");
            }
        }

        private Card GetCardInput(string hint)
        {
            while (true)
            {
                Console.WriteLine(hint);
                string input = Console.ReadLine();

                try
                {
                    if (input.Length != 2)
                    {
                        throw new InvalidOperationException($"input got length of {input.Length}, 2 required.");
                    }
                    SuitEnum suit = ParseSuit(input[0]);
                    RankEnum rank = ParseRank(input[1]);
                    return new Card(suit, rank);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private SuitEnum ParseSuit(char suit)
        {
            switch (suit)
            {
                case 'D':
                    return SuitEnum.Diamond;
                case 'H':
                    return SuitEnum.Heart;
                case 'C':
                    return SuitEnum.Club;
                case 'S':
                    return SuitEnum.Spade;
                default:
                    throw new InvalidOperationException($"Unrecognized suit {suit}");
            }
        }

        private RankEnum ParseRank(char rank)
        {
            switch (rank)
            {
                case 'A':
                    return RankEnum.Ace;
                case '2':
                    return RankEnum.Two;
                case '3':
                    return RankEnum.Three;
                case '4':
                    return RankEnum.Four;
                case '5':
                    return RankEnum.Five;
                case '6':
                    return RankEnum.Six;
                case '7':
                    return RankEnum.Seven;
                case '8':
                    return RankEnum.Eight;
                case '9':
                    return RankEnum.Nine;
                case 'T':
                    return RankEnum.Ten;
                case 'J':
                    return RankEnum.Jack;
                case 'Q':
                    return RankEnum.Queen;
                case 'K':
                    return RankEnum.King;
                default:
                    throw new InvalidOperationException($"{rank} not recognized for rank");

            }
        }
    }
}
