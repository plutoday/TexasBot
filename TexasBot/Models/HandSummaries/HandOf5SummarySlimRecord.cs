using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasBot.Models.HandSummaries
{
    public class HandOf5SummarySlimRecord
    {
        public string RecordString { get; set; }
        public int Score { get; set; }
        public int Count { get; set; }
        public HandOf5SummarySlimRecord() { }

        public HandOf5SummarySlimRecord(string recordString, int score, int count)
        {
            RecordString = recordString;
            Score = score;
            Count = count;
        }

        public bool IsPossibleWith(HolesSummarySlimRecord record)
        {
            var ranks = RecordString.Substring(0, 5);
            var sameColor = RecordString.Last();
            var hole1 = record.RecordString[0];
            var hole2 = record.RecordString[1];
            var holesSameColor = record.RecordString[2];

            bool colorPossible = holesSameColor == 'S' || sameColor == 'N';
            if (!colorPossible)
            {
                return false;
            }

            if (hole1 != hole2)
            {
                return ranks.Contains(hole1) && ranks.Contains(hole2);
            }
            else
            {
                return ranks.Count(r => r == hole1) >= 2;
            }
        }

        public bool IsPossibleWithout(HolesSummarySlimRecord record)
        {
            var ranks = RecordString.Substring(0, 5);
            var holes = record.RecordString.Substring(0, 2);
            var totalRanks = (ranks + holes);
            var totalRankCount = new Dictionary<char, int>();
            foreach (var rank in totalRanks)
            {
                if (!totalRankCount.ContainsKey(rank))
                {
                    totalRankCount.Add(rank, 0);
                }

                totalRankCount[rank]++;
            }

            return totalRankCount.Values.Max() <= 4;
        }
    }
}
