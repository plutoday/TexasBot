using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Terminal.Deal;

namespace Unittest.ServerLogic
{
    [TestClass]
    public class DealerTest
    {
        [TestMethod]
        public void Test()
        {
            var dealer = new Dealer(new RoundInput(6, 5, new List<string>() {"Alice", "Bob", "Chris", "David", "Ellen", "Frank"},
                new List<int>() {10000, 10000, 10000, 10000, 10000, 10000}, 50, 100 ));
            dealer.Deal();
        }
    }
}
