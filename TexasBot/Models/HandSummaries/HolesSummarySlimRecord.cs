using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TexasBot.Models.HandSummaries
{
    public class HolesSummarySlimRecord
    {
        public string RecordString { get; set; }
        public int Count { get; set; }

        [JsonIgnore]
        public List<HandOf5SummarySlimRecord> PossibleHandRecords { get; set; }
        public HolesSummarySlimRecord() { }

        public HolesSummarySlimRecord(string recordString, int count)
        {
            RecordString = recordString;
            Count = count;
        }
    }
}
