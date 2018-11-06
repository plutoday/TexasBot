using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Preflop.Charts
{
    /// <summary>
    /// Fold/Call/BluffThreeBet/ValueThreeBet
    /// </summary>
    public enum DecisionAgainstOpenRaiseEnum
    {
        Fold = 0,
        Call,
        BluffThreeBet,
        ValueThreeBet,
    }

    public enum DecisionAgainstThreeBetEnum
    {
        Fold = 0,
        Call,
        BluffFourBet,
        ValueFourBet,
    }
}
