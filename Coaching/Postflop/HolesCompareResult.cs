using System.Collections.Generic;
using System.Linq;
using Coaching.Postflop.Betting;
using Coaching.Postflop.Boards;
using Models;

namespace Coaching.Postflop
{
    public class HolesCompareResult
    {
        public HoldingHoles HeroHoles { get; set; }
        public HoldingHoles VillainHoles { get; set; }
        public BoardStatus BoardStatus { get; set; }

        public double HeroEquityAgainstAverage { get; set; }
        public double VillainEquityAgainstAverage { get; set; }

        public bool HeroIsAhead { get; set; }

        /// <summary>
        /// 哪些牌的出现（只需要一张）就能帮助Villain反超
        /// </summary>
        public List<Card> VillainOuts { get; set; }

        /// <summary>
        /// 哪些牌的出现（只需要一张）就能帮助Hero反超
        /// </summary>
        public List<Card> HeroOuts { get; set; }

        public HolesCompareResult(HoldingHoles heroHoles, HoldingHoles villainHoles, BoardStatus boardStatus)
        {
            HeroHoles = heroHoles;
            VillainHoles = villainHoles;

            HeroOuts = new List<Card>();
            VillainOuts = new List<Card>();

            if (boardStatus.Turn == null)
            {
                CompareOnFlop();
            }
            else if (boardStatus.River == null)
            {
                CompareOnTurn();
            }
            else
            {
                CompareOnRiver();
            }
        }

        private void CompareOnFlop()
        {
            var heroBest = new BestFiveCardsHand(new List<Card>() {HeroHoles.Hole1, HeroHoles.Hole2, BoardStatus.Flop1, BoardStatus.Flop2, BoardStatus.Flop3});
            var villainBest = new BestFiveCardsHand(new List<Card>() { VillainHoles.Hole1, VillainHoles.Hole2, BoardStatus.Flop1, BoardStatus.Flop2, BoardStatus.Flop3 });

            var heroScore = BettingUtils.GetScoreForFiveCardHand(heroBest);
            var villainScore = BettingUtils.GetScoreForFiveCardHand(villainBest);

            HeroIsAhead = heroScore > villainScore;

            var allCards = TexasBot.Tools.Utils.GenerateAllCards().Select(c => c.ConvertCard()).ToList();

            allCards.RemoveElementEqualsTo(HeroHoles.Hole1);
            allCards.RemoveElementEqualsTo(HeroHoles.Hole2);
            allCards.RemoveElementEqualsTo(VillainHoles.Hole1);
            allCards.RemoveElementEqualsTo(VillainHoles.Hole2);
            allCards.RemoveElementEqualsTo(BoardStatus.Flop1);
            allCards.RemoveElementEqualsTo(BoardStatus.Flop2);
            allCards.RemoveElementEqualsTo(BoardStatus.Flop3);
            allCards.RemoveElementEqualsTo(BoardStatus.Turn);

            BestFiveCardsHand leader = HeroIsAhead ? heroBest : villainBest;
            BestFiveCardsHand follower = HeroIsAhead ? villainBest : heroBest;
            List<Card> outs = HeroIsAhead ? VillainOuts : HeroOuts;
            outs.AddRange(allCards.Where(card => CardCanHelp(leader, follower, card)));
        }

        private void CompareOnTurn()
        {
            var heroSix = new SixCardsHand(HeroHoles.Hole1, HeroHoles.Hole2,
                BoardStatus.Flop1, BoardStatus.Flop2, BoardStatus.Flop3, BoardStatus.Turn);
            var villainSix = new SixCardsHand(VillainHoles.Hole1, VillainHoles.Hole2,
                BoardStatus.Flop1, BoardStatus.Flop2, BoardStatus.Flop3, BoardStatus.Turn);

            var heroBest = BettingUtils.FindBestFive(heroSix);
            var villainBest = BettingUtils.FindBestFive(villainSix);

            var heroScore = BettingUtils.GetScoreForFiveCardHand(heroBest);
            var villainScore = BettingUtils.GetScoreForFiveCardHand(villainBest);

            HeroIsAhead = heroScore > villainScore;

            var allCards = TexasBot.Tools.Utils.GenerateAllCards().Select(c => c.ConvertCard()).ToList();

            allCards.RemoveElementEqualsTo(HeroHoles.Hole1);
            allCards.RemoveElementEqualsTo(HeroHoles.Hole2);
            allCards.RemoveElementEqualsTo(VillainHoles.Hole1);
            allCards.RemoveElementEqualsTo(VillainHoles.Hole2);
            allCards.RemoveElementEqualsTo(BoardStatus.Flop1);
            allCards.RemoveElementEqualsTo(BoardStatus.Flop2);
            allCards.RemoveElementEqualsTo(BoardStatus.Flop3);
            allCards.RemoveElementEqualsTo(BoardStatus.Turn);

            SixCardsHand leader = HeroIsAhead ? heroSix : villainSix;
            SixCardsHand follower = HeroIsAhead ? villainSix : heroSix;
            List<Card> outs = HeroIsAhead ? VillainOuts : HeroOuts;
            outs.AddRange(allCards.Where(card => CardCanHelp(leader, follower, card)));
        }

        /// <summary>
        /// If card can help follower to get ahead of leader
        /// </summary>
        /// <param name="leader"></param>
        /// <param name="follower"></param>
        /// <param name="card"></param>
        /// <returns></returns>
        private bool CardCanHelp(BestFiveCardsHand leader, BestFiveCardsHand follower, Card card)
        {
            var leaderSix = new SixCardsHand(leader.Cards[0], leader.Cards[1], leader.Cards[2], leader.Cards[3], leader.Cards[4], card);
            var followerSix = new SixCardsHand(follower.Cards[0], follower.Cards[1], follower.Cards[2], follower.Cards[3], follower.Cards[4], card);

            var leaderBest = BettingUtils.FindBestFive(leaderSix);
            var followerBest = BettingUtils.FindBestFive(followerSix);

            var leaderScore = BettingUtils.GetScoreForFiveCardHand(leaderBest);
            var followerScore = BettingUtils.GetScoreForFiveCardHand(followerBest);

            return followerScore > leaderScore;
        }

        /// <summary>
        /// If card can help follower to get ahead of leader
        /// </summary>
        /// <param name="leader"></param>
        /// <param name="follower"></param>
        /// <param name="card"></param>
        /// <returns></returns>
        private bool CardCanHelp(SixCardsHand leader, SixCardsHand follower, Card card)
        {
            var leaderSeven = new SevenCardsHand(leader.Hole1, leader.Hole2, leader.Flop1, leader.Flop2, leader.Flop3, leader.Turn, card);
            var followerSeven = new SevenCardsHand(follower.Hole1, follower.Hole2, follower.Flop1, follower.Flop2, follower.Flop3, follower.Turn, card);

            var leaderBest = BettingUtils.FindBestFive(leaderSeven);
            var followerBest = BettingUtils.FindBestFive(followerSeven);

            var leaderScore = BettingUtils.GetScoreForFiveCardHand(leaderBest);
            var followerScore = BettingUtils.GetScoreForFiveCardHand(followerBest);

            return followerScore > leaderScore;
        }

        private void CompareOnRiver()
        {
            var heroSeven = new SevenCardsHand(HeroHoles.Hole1, HeroHoles.Hole2, 
                BoardStatus.Flop1, BoardStatus.Flop2, BoardStatus.Flop3, BoardStatus.Turn, BoardStatus.River);
            var villainSeven = new SevenCardsHand(VillainHoles.Hole1, VillainHoles.Hole2,
                BoardStatus.Flop1, BoardStatus.Flop2, BoardStatus.Flop3, BoardStatus.Turn, BoardStatus.River);

            var heroBest = BettingUtils.FindBestFive(heroSeven);
            var villainBest = BettingUtils.FindBestFive(villainSeven);

            var heroScore = BettingUtils.GetScoreForFiveCardHand(heroBest);
            var villainScore = BettingUtils.GetScoreForFiveCardHand(villainBest);

            HeroIsAhead = heroScore > villainScore;
        }
    }
}
