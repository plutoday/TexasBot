using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flop.Strategy;
using Flop.Strategy.Multiway;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;

namespace Unittest
{
    [TestClass]
    public class FlopTest
    {
        [TestMethod]
        public void TestEnumerateProbabilities()
        {
            var result = Common.Utils.EnumerateProbabilities(0, new List<VillainProbabilityResult>()
            {
                new VillainProbabilityResult(new Dictionary<ProbabilityEnum, double>()
                {
                    {ProbabilityEnum.Fold, 0.1},
                    {ProbabilityEnum.CallWin, 0.2},
                    {ProbabilityEnum.CallTie, 0.3},
                    {ProbabilityEnum.CallLose, 0.4}

                }, "Alice"),
                new VillainProbabilityResult(new Dictionary<ProbabilityEnum, double>()
                {
                    {ProbabilityEnum.Fold, 0.1},
                    {ProbabilityEnum.CallWin, 0.2},
                    {ProbabilityEnum.CallTie, 0.3},
                    {ProbabilityEnum.CallLose, 0.4}

                }, "Bob"),
                new VillainProbabilityResult(new Dictionary<ProbabilityEnum, double>()
                {
                    {ProbabilityEnum.Fold, 0.1},
                    {ProbabilityEnum.CallWin, 0.2},
                    {ProbabilityEnum.CallTie, 0.3},
                    {ProbabilityEnum.CallLose, 0.4}

                }, "Chris"),
            }).ToList();
            

            Console.WriteLine(result);
        }
    }
}
