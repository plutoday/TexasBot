using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace TexasBot.Models.HandSummaries
{
    public class HandOf7SummarySlimRecord
    {
        public string RecordString { get; set; }
        public string HandOf5RecordString { get; set; }
        public int Score { get; set; }
        public HandEnum HandEnum { get; set; }
        public int Count { get; set; }
    }
}
