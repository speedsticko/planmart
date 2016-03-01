using System;
using System.Linq;
using NUnit.Framework;

namespace PlanMart.Processors.Tests
{
    [TestFixture]
    public class PlanMartOrderProcessorTests
    {
        [Test]
        public void SampleTest()
        {
            var order = new Order(
                new Customer(new DateTime(1988, 3, 4), false),
                "AZ",
                PaymentMethod.Visa,
                new DateTime(2015, 11, 27));
            order.Items.Add(new ProductOrder(new Product(20, 2, ProductType.Alcohol), 3));
            order.Items.Add(new ProductOrder(new Product(25, 3, ProductType.Food), 3));

            var processor = new PlanMartOrderProcessor();
            var isValid = processor.ProcessOrder(order);
            var tax = order.LineItems.SingleOrDefault(x => x.Type == LineItemType.Tax);
            var shipping = order.LineItems.SingleOrDefault(x => x.Type == LineItemType.Shipping);
            var rewardPoints = order.LineItems.SingleOrDefault(x => x.Type == LineItemType.RewardsPoints);

            Assert.IsTrue(isValid);
            Assert.IsNotNull(tax);
            Assert.AreEqual(10.80M, tax.Amount);
            Assert.IsNotNull(shipping);
            Assert.AreEqual(10, shipping.Amount);
            Assert.IsNotNull(rewardPoints);
            Assert.AreEqual(134, rewardPoints.Amount);
        }

        // --------------------------------------------------------------------
        // -- Sanity tests
        // --------------------------------------------------------------------
        [Test]
        public void NullOrder_Invalid()
        {
            var processor = new PlanMartOrderProcessor();
            var isValid = processor.ProcessOrder(null);
            Assert.IsFalse(isValid);
        }

        [Test]
        public void NullCustomer_Invalid()
        {
            var processor = new PlanMartOrderProcessor();
            var isValid = processor.ProcessOrder(new Order(null, "NY", PaymentMethod.Visa, DateTime.Now));
            Assert.IsFalse(isValid);
        }

        [Test]
        public void EmptyOrder_Invalid()
        {
            var processor = new PlanMartOrderProcessor();
            var isValid = processor.ProcessOrder(new Order(
                new Customer(new DateTime(1999, 9, 9), false),
                "NY",
                PaymentMethod.Visa,
                DateTime.Now));
            Assert.IsFalse(isValid);
        }


        // --------------------------------------------------------------------
        // -- Tax tests
        // --------------------------------------------------------------------
        [Test]
        public void NonProfit_TaxExempt()
        {
            var order = new Order(
                new Customer(new DateTime(1988, 3, 4), true),
                "AZ",
                PaymentMethod.Visa,
                new DateTime(2015, 11, 27));
            order.Items.Add(new ProductOrder(new Product(20, 2, ProductType.Alcohol), 3));
            order.Items.Add(new ProductOrder(new Product(25, 3, ProductType.Food), 3));

            var processor = new PlanMartOrderProcessor();
            var isValid = processor.ProcessOrder(order);
            var tax = order.LineItems.SingleOrDefault(x => x.Type == LineItemType.Tax);
            var shipping = order.LineItems.SingleOrDefault(x => x.Type == LineItemType.Shipping);
            var rewardPoints = order.LineItems.SingleOrDefault(x => x.Type == LineItemType.RewardsPoints);

            Assert.IsTrue(isValid);
            Assert.IsNull(tax);
            
            Assert.IsNotNull(shipping);
            Assert.AreEqual(10, shipping.Amount);
            Assert.IsNotNull(rewardPoints);
            Assert.AreEqual(134, rewardPoints.Amount);
        }

        [Test]
        public void FoodToNY_TaxExempt()
        {
            var order = new Order(
                new Customer(new DateTime(1988, 3, 4), true),
                "NY",
                PaymentMethod.Visa,
                new DateTime(2015, 11, 27));
            order.Items.Add(new ProductOrder(new Product(20, 2, ProductType.Food), 3));
            order.Items.Add(new ProductOrder(new Product(25, 3, ProductType.Food), 3));

            var processor = new PlanMartOrderProcessor();
            var isValid = processor.ProcessOrder(order);
            var tax = order.LineItems.SingleOrDefault(x => x.Type == LineItemType.Tax);
            var shipping = order.LineItems.SingleOrDefault(x => x.Type == LineItemType.Shipping);
            var rewardPoints = order.LineItems.SingleOrDefault(x => x.Type == LineItemType.RewardsPoints);

            Assert.IsTrue(isValid);
            Assert.IsNull(tax);

            Assert.IsNotNull(shipping);
            Assert.AreEqual(10, shipping.Amount);
            Assert.IsNotNull(rewardPoints);
            Assert.AreEqual(67, rewardPoints.Amount);
        }

        // --------------------------------------------------------------------
        // -- Shipping tests
        // --------------------------------------------------------------------

        // --------------------------------------------------------------------
        // -- Reward Point tests
        // --------------------------------------------------------------------


        // --------------------------------------------------------------------
        // -- Alcohol tests
        // --------------------------------------------------------------------


    }
}