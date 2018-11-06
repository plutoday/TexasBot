using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLogic.Contracts
{
    public class RoundIdResponse
    {
        public ExpectedAction Action { get; set; }
        public Guid RoundId { get; set; }
    }

    public class DecisionResponse
    {
        public ExpectedAction Action { get; set; }
        public Decision Decision { get; set; } 
    }

    public class DummyResponse
    {
        public ExpectedAction Action { get; set; }
    }

    public class ExpectedAction
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ExpectedActionEnum Action { get; set; }
        public string PlayerName { get; set; }
    }

    public enum ExpectedActionEnum
    {
        StartNewRound,
        HeroHoles,
        Flops,
        Turn,
        River,
        Decision,
        VillainHoles,
    }
}
