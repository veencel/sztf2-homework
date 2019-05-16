using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vatera.Implementation.AuctionManager;
using Vatera.Models;

namespace VateraTest
{
    [TestClass]
    public class AuctionManagerTest
    {
        private Product _product;
        private Customer[] _users = new[]
        {
            new Customer("Test", "1", 5),
            new Customer("Test", "2", 4),
            new Customer("test", "3", 3),
            new Customer("test", "4", 2),
            new Customer("test", "5", 1),
        };

        [TestInitialize]
        public void SetUp()
        {
            _product = new Product("Test product", "test-123", 1200, 24, 1);

            CreateOrders();
        }

        [TestMethod]
        public void TestBruteForceAuctionManager()
        {
            var manager = new BruteForceAuctionManager();
            var result = manager.FinishAuction(_product);

            AssertAuctionResultIsCorrect(result);
        }

        [TestMethod]
        public void TestBranchAndBoundAuctionManager()
        {
            var manager = new BranchAndBoundAuctionManager();
            var result = manager.FinishAuction(_product);

            AssertAuctionResultIsCorrect(result);
        }

        [TestMethod]
        public void TestGreedyAuctionManager()
        {
            var manager = new GreedyAuctionManager();
            var result = manager.FinishAuction(_product);

            AssertAuctionResultIsCorrect(result);
        }

        [TestMethod]
        public void TestBacktrackAuctionManager()
        {
            var manager = new BacktrackAuctionManager();
            var result = manager.FinishAuction(_product);

            AssertAuctionResultIsCorrect(result);
        }

        private void CreateOrders()
        {
            _product.Orders.Insert(new Order(_users[4], _product, 5));

            _product.Orders.Insert(new Order(_users[0], _product, 10));
            _product.Orders.Insert(new Order(_users[0], _product, 2));
            _product.Orders.Insert(new Order(_users[1], _product, 3));
            _product.Orders.Insert(new Order(_users[2], _product, 4));
            _product.Orders.Insert(new Order(_users[3], _product, 3));
            _product.Orders.Insert(new Order(_users[3], _product, 2));
        }

        private void AssertAuctionResultIsCorrect(AuctionResult result)
        {
            Assert.AreEqual(6, result.SuccessfulOrders.Count);
            Assert.AreEqual(1, result.FailedOrders.Count);

            Assert.AreEqual(1, result.FailedOrders.First().Customer.Rating);
            Assert.AreEqual(5, result.FailedOrders.First().Count);
        }
    }
}
