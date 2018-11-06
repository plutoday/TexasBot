using System;
using Models;

namespace Preflop.StartingHands
{
    /// <summary>
    /// Starting Hand/Holes used in Preflop
    /// </summary>
    public class StartingHand
    {
        public StartingHandTypeEnum Type { get; set; }
        public StartingHandGradeEnum Grade { get; set; }
        public RankEnum Rank1 { get; set; }
        public RankEnum Rank2 { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }

        public StartingHand(StartingHandTypeEnum type, RankEnum rank1, RankEnum rank2)
        {
            Type = type;
            Rank1 = rank1;
            Rank2 = rank2;

            switch (Type)
            {
                case StartingHandTypeEnum.Pair:
                    Count = 6;
                    Name = GetNameForPair(this);
                    break;
                case StartingHandTypeEnum.Suited:
                case StartingHandTypeEnum.AceXSuited:
                    Count = 4;
                    Name = GetNameForSutied(this);
                    break;
                case StartingHandTypeEnum.OffSuit:
                case StartingHandTypeEnum.AceXOffsuit:
                    Count = 16;
                    Name = GetNameForOffSuit(this);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        private string GetNameForPair(StartingHand startingHand)
        {
            var rank = Utils.GetStringForRank(startingHand.Rank1);
            return $"{rank}{rank}";
        }

        private string GetNameForSutied(StartingHand startingHand)
        {
            var rank1 = Utils.GetStringForRank(startingHand.Rank1);
            var rank2 = Utils.GetStringForRank(startingHand.Rank2);
            return $"{rank1}{rank2}s";
        }

        private string GetNameForOffSuit(StartingHand startingHand)
        {
            var rank1 = Utils.GetStringForRank(startingHand.Rank1);
            var rank2 = Utils.GetStringForRank(startingHand.Rank2);
            return $"{rank1}{rank2}";
        }
    }
}
