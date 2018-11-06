using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace River
{
    /// <summary>
    /// RoyalFlush/StraightFlush/FlushWith[Top/Good/Weak]Kicker/Nothing
    /// </summary>
    public enum SuitTextureOutcomeEnum
    {
        RoyalFlush,
        StraightFlush,
        FlushWithTopKicker,
        FlushWithGoodKicker,
        FlushWithWeakKicker,
        FlushWithNoneKicker,
        Nothing
    }
}
