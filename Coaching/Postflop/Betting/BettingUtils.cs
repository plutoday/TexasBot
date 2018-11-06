using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coaching.Postflop.Boards;
using Coaching.Postflop.Ranging;
using Models;
using Models.Ranging;

namespace Coaching.Postflop.Betting
{
    public static class BettingUtils
    {
        public static PlayerRange GetVillainGuessOnHeroRange()
        {
            throw new NotImplementedException();
        }

        public static bool VillainIsWillingToCall(double villainEquity, double potOdds)
        {
            //Determines whether villain is willing to call the pot odds with the equity he estimated based on a guess to hero's range
            throw new NotImplementedException();
        }

        public static double CompareHoles(HoldingHoles holes1, HoldingHoles holes2, BoardStatus boardStatus)
        {
            var compareResult = new HolesCompareResult(holes1, holes2, boardStatus);
            
            throw new NotImplementedException();
        }

        public static BestFiveCardsHand FindBestFive(SevenCardsHand sevenCardsHand)
        {
            throw new NotImplementedException();
        }

        public static BestFiveCardsHand FindBestFive(SixCardsHand sixCardsHand)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a universal score for a 5-card hand
        /// which can be used to compare between hands
        /// </summary>
        /// <param name="bestFiveCardsHand"></param>
        /// <returns></returns>
        public static int GetScoreForFiveCardHand(BestFiveCardsHand bestFiveCardsHand)
        {
            throw new NotImplementedException();
        }
    }
}
