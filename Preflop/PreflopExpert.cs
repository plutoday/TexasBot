using System;
using Models;
using Models.Ranging;
using Preflop.StartingHands;

namespace Preflop
{
    /// <summary>
    /// 
    /// </summary>
    public class PreflopExpert
    {
        private readonly PercentilePreflopStrategy _preflopStrategy = new PercentilePreflopStrategy();

        public PlayerRange GenerateInitialRange(PlayerRoundProfile profile)
        {
            return GenerateSmallBlindInitialRange();
        }

        public Decision GetPreflopDecision(PreflopStatusSummary statusSummary, HoldingHoles heroHoles)
        {
            return _preflopStrategy.MakeDecision(statusSummary, GetStartingHand(heroHoles.Hole1, heroHoles.Hole2));
        }

        private StartingHand GetStartingHand(Card hole1, Card hole2)
        {
            if (hole1.Rank < hole2.Rank)
            {
                var temp = hole1;
                hole1 = hole2;
                hole2 = temp;
            }

            if (hole1.Rank == hole2.Rank)
            {
                return new StartingHand(StartingHandTypeEnum.Pair, hole1.Rank, hole2.Rank);
            }

            if (hole1.Rank == RankEnum.Ace)
            {
                if (hole1.Suit == hole2.Suit)
                {
                    return new StartingHand(StartingHandTypeEnum.AceXSuited, hole1.Rank, hole2.Rank);
                }
                else
                {
                    return new StartingHand(StartingHandTypeEnum.AceXOffsuit, hole1.Rank, hole1.Rank);
                }
            }

            if (hole1.Suit == hole2.Suit)
            {
                return new StartingHand(StartingHandTypeEnum.Suited, hole1.Rank, hole2.Rank);
            }

            return new StartingHand(StartingHandTypeEnum.OffSuit, hole1.Rank, hole2.Rank);
        }

        private PlayerRange GenerateSmallBlindInitialRange()
        {
            var range = new PlayerRange();
            int[,] grids = new int[,]
            {
                {1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {1,1,1,1,1,1,1,1,1,1,1,1,1 },
            };
            range.Init(grids);

            return range;
        }

        private PlayerRange GenerateBigBlindInitialRange()
        {
            var range = new PlayerRange();
            int[,] grids = new int[,]
            {
                {1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {1,1,1,1,1,1,1,1,1,1,1,1,1 },
            };
            range.Init(grids);

            return range;
        }

        /// <summary>
        /// https://www.bestpokercoaching.com/60-minute-master-nl-6-max-starting-hands-by-position/
        /// http://www.acepokersolutions.com/files/6-max%20starting%20hand%20charts%20v3.pdf
        /// </summary>
        /// <returns></returns>
        private PlayerRange GenerateUnderTheGunInitialRange()
        {
            var range = new PlayerRange();
            int[,] grids = new int[,]
            {
                {1,1,1,1,1,0,0,0,0,0,0,0,0 },
                {1,1,1,1,1,0,0,0,0,0,0,0,0 },
                {1,1,1,1,1,0,0,0,0,0,0,0,0 },
                {1,0,0,1,1,0,0,0,0,0,0,0,0 },
                {0,0,0,0,1,1,0,0,0,0,0,0,0 },
                {0,0,0,0,0,1,1,0,0,0,0,0,0 },
                {0,0,0,0,0,0,1,0,0,0,0,0,0 },
                {0,0,0,0,0,0,0,1,0,0,0,0,0 },
                {0,0,0,0,0,0,0,0,1,0,0,0,0 },
                {0,0,0,0,0,0,0,0,0,1,0,0,0 },
                {0,0,0,0,0,0,0,0,0,0,1,0,0 },
                {0,0,0,0,0,0,0,0,0,0,0,1,0 },
                {0,0,0,0,0,0,0,0,0,0,0,0,1 },
            };
            range.Init(grids);

            return range;
        }

        private PlayerRange GenerateMiddlePositionInitialRange()
        {
            var range = new PlayerRange();
            int[,] grids = new int[,]
            {
                {1,1,1,1,1,0,0,0,0,1,1,1,1 },
                {1,1,1,1,1,0,0,0,0,0,0,0,0 },
                {1,1,1,1,1,0,0,0,0,0,0,0,0 },
                {1,0,0,1,1,1,0,0,0,0,0,0,0 },
                {0,0,0,0,1,1,1,0,0,0,0,0,0 },
                {0,0,0,0,0,1,1,0,0,0,0,0,0 },
                {0,0,0,0,0,0,1,1,0,0,0,0,0 },
                {0,0,0,0,0,0,0,1,1,0,0,0,0 },
                {0,0,0,0,0,0,0,0,1,0,0,0,0 },
                {0,0,0,0,0,0,0,0,0,1,0,0,0 },
                {0,0,0,0,0,0,0,0,0,0,1,0,0 },
                {0,0,0,0,0,0,0,0,0,0,0,1,0 },
                {0,0,0,0,0,0,0,0,0,0,0,0,1 },
            };
            range.Init(grids);

            return range;
        }

        private PlayerRange GenerateCuttingOffInitialRange()
        {
            var range = new PlayerRange();
            int[,] grids = new int[,]
            {
                {1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {1,1,1,1,1,1,1,0,0,0,0,0,0 },
                {1,1,1,1,1,1,1,0,0,0,0,0,0 },
                {1,1,1,1,1,1,1,0,0,0,0,0,0 },
                {1,1,1,1,1,1,1,0,0,0,0,0,0 },
                {1,0,0,0,0,1,1,1,0,0,0,0,0 },
                {1,0,0,0,0,0,1,1,1,0,0,0,0 },
                {0,0,0,0,0,0,0,1,1,1,0,0,0 },
                {0,0,0,0,0,0,0,0,1,1,0,0,0 },
                {0,0,0,0,0,0,0,0,0,1,0,0,0 },
                {0,0,0,0,0,0,0,0,0,0,1,0,0 },
                {0,0,0,0,0,0,0,0,0,0,0,1,0 },
                {0,0,0,0,0,0,0,0,0,0,0,0,1 },
            };
            range.Init(grids);

            return range;
        }

        private PlayerRange GenerateButtonInitialRange()
        {
            var range = new PlayerRange();
            int[,] grids = new int[,]
            {
                {1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {1,1,1,1,1,1,1,1,1,1,1,1,0 },
                {1,1,1,1,1,1,1,0,0,0,0,0,0 },
                {1,1,1,1,1,1,1,0,0,0,0,0,0 },
                {1,1,1,1,1,1,1,0,0,0,0,0,0 },
                {1,1,1,1,1,1,1,1,0,0,0,0,0 },
                {1,1,1,1,1,1,1,1,1,0,0,0,0 },
                {1,1,0,0,0,0,1,1,1,0,0,0,0 },
                {1,0,0,0,0,0,0,0,1,0,0,0,0 },
                {1,0,0,0,0,0,0,0,0,1,0,0,0 },
                {1,0,0,0,0,0,0,0,0,0,1,0,0 },
                {1,0,0,0,0,0,0,0,0,0,0,1,0 },
                {1,0,0,0,0,0,0,0,0,0,0,0,1 },
            };
            range.Init(grids);

            return range;
        }
    }
}
