using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Models
{
    public class Decision
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public DecisionType DecisionType { get; set; }

        public int ChipsAdded { get; set; }
        //临时绕过
        public decimal? TempChipsAdded
        {
            get { return ChipsAdded; }
            set
            {
                if (value.HasValue)
                {
                    ChipsAdded = (int) value;
                }
                else
                {
                    ChipsAdded = 0;
                }
            }
        }

        public Decision()
        {
        }

        public Decision(DecisionType decisionType, int chipsAdded)
        {
            DecisionType = decisionType;
            ChipsAdded = chipsAdded;
        }
    }

    public enum DecisionType
    {
        Undefined,
        Ante,
        Check,
        Fold,
        Raise,
        Reraise,
        Call,
        AllIn,
        AllInRaise,
    }
}