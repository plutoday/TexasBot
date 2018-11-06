using System;
using System.Linq;
using Flop.FlopBoardRankTextures;
using Flop.Strategy.Headsup;
using Flop.Strategy.Multiway;
using Infra;
using Models;
using Models.Ranging;

namespace Flop
{
    public class FlopStrategy
    {
        private FlopHeadsUpBettingStrategy _bettingStrategy;
        private FlopHeadsUpCallingStrategy _callingStrategy;
        private FlopMultiwayBettingStrategy _multiwayBettingStrategy;
        private FlopMultiwayCallingStrategy _multiwayCallingStrategy;

        private bool _initialized = false;
        
        private void Init(FlopDecisionContext context)
        {
            _bettingStrategy = new FlopHeadsUpBettingStrategy(context.FlopBoard, context.HeroHoles);
            _callingStrategy = new FlopHeadsUpCallingStrategy();
            _multiwayBettingStrategy = new FlopMultiwayBettingStrategy(context.FlopBoard, context.HeroHoles);
            _multiwayCallingStrategy = new FlopMultiwayCallingStrategy(context.FlopBoard, context.HeroHoles);
            _initialized = true;
        }

        public Decision MakeDecision(FlopDecisionContext context)
        {
            if (!_initialized)
            {
                Init(context);
            }
            if (context.IsHeadsUp)
            {
                Logger.Instance.Log($"Heads up: villain: {context.HeadsUpVillain.Position}-{context.HeadsUpVillainName} hero: {context.Hero.Position}-{context.HeroName}");
                if (context.IsRaised)
                {
                    Logger.Instance.Log($"Pot is raised, employing callingStrategy to make decision...");
                    return _callingStrategy.MakeDecision(context);
                }
                else
                {
                    Logger.Instance.Log($"Pot is not raised, employing bettingStrategy to make decision...");
                    return _bettingStrategy.MakeDecision(context);
                }
            }
            else
            {
                Logger.Instance.Log($"Multiway: villains: {string.Join(",", context.AliveVillains.Select(v => $"{v.Position}-{v.Name}"))} hero: {context.Hero.Position}-{context.HeroName}");
                if (context.IsRaised)
                {
                    Logger.Instance.Log($"Pot is raised, employing callingStrategy to make decision...");
                    return _multiwayCallingStrategy.MakeDecision(context);
                }
                else
                {
                    Logger.Instance.Log($"Pot is not raised, employing bettingStrategy to make decision...");
                    return _multiwayBettingStrategy.MakeDecision(context);
                }
            }
        }

        private BoardRangeGridStatusEnum TestGrid(FlopBoard flopBoard, RangeGrid grid)
        {
            switch (flopBoard.RankTexture)
            {
                case FlopBoardRankTextureEnum.ThreeSome:
                    return new ThreeSomeRankTexture(flopBoard).TestGridAgainstFlopBoard(grid);
                case FlopBoardRankTextureEnum.HighPair:
                    return new HighPairRankTexture(flopBoard).TestGridAgainstFlopBoard(grid);
                case FlopBoardRankTextureEnum.LowPair:
                    return new LowPairRankTexture(flopBoard).TestGridAgainstFlopBoard(grid);
                case FlopBoardRankTextureEnum.Singles:
                    return new SinglesRankTexture(flopBoard).TestGridAgainstFlopBoard(grid);

            }
            throw new NotImplementedException();
        }
    }
}
