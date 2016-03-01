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
        }

        [Test]
        public void FoodToNY_TaxExempt()
        {
            var order = new Order(
                new Customer(new DateTime(1988, 3, 4), false),
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

        [Test]
        public void ClothesToCT_TaxExempt()
        {
            var order = new Order(
                new Customer(new DateTime(1988, 3, 4), false),
                "CT",
                PaymentMethod.Visa,
                new DateTime(2015, 11, 27));
            order.Items.Add(new ProductOrder(new Product(20, 2, ProductType.Clothing), 3));
            order.Items.Add(new ProductOrder(new Product(10, 2, ProductType.Food), 1));

            var processor = new PlanMartOrderProcessor();
            var isValid = processor.ProcessOrder(order);
            var tax = order.LineItems.SingleOrDefault(x => x.Type == LineItemType.Tax);

            Assert.IsTrue(isValid);
            Assert.IsNotNull(tax);
            Assert.AreEqual(0.8M, tax.Amount);  // only Food is taxed
        }

        // --------------------------------------------------------------------
        // -- Shipping tests
        // --------------------------------------------------------------------
        [Test]
        public void NonProfit_NoShipping()
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

            Assert.IsNull(shipping);
        }

        [Test]
        public void FoodShippingToHI_Invalid()
        {
            var order = new Order(
                new Customer(new DateTime(1988, 3, 4), true),
                "HI",
                PaymentMethod.Visa,
                new DateTime(2015, 11, 27));
            
            order.Items.Add(new ProductOrder(new Product(25, 3, ProductType.Food), 3));

            var processor = new PlanMartOrderProcessor();
            var isValid = processor.ProcessOrder(order);
            var tax = order.LineItems.SingleOrDefault(x => x.Type == LineItemType.Tax);
            var shipping = order.LineItems.SingleOrDefault(x => x.Type == LineItemType.Shipping);
            var rewardPoints = order.LineItems.SingleOrDefault(x => x.Type == LineItemType.RewardsPoints);

            Assert.IsFalse(isValid);
        }

        [Test]
        public void ShippingToHIandAK_35()
        {
            var order = new Order(
                new Customer(new DateTime(1988, 3, 4), false),
                "HI",
                PaymentMethod.Visa,
                new DateTime(2015, 11, 27));

            order.Items.Add(new ProductOrder(new Product(25, 3, ProductType.Clothing), 3));

            var processor = new PlanMartOrderProcessor();
            var isValid = processor.ProcessOrder(order);
            var shipping = order.LineItems.SingleOrDefault(x => x.Type == LineItemType.Shipping);
            
            Assert.IsTrue(isValid);
            Assert.AreEqual(35M, shipping.Amount);

            order = new Order(
                new Customer(new DateTime(1988, 3, 4), false),
                "AK",
                PaymentMethod.Visa,
                new DateTime(2015, 11, 27));

            order.Items.Add(new ProductOrder(new Product(25, 3, ProductType.Clothing), 3));
            isValid = processor.ProcessOrder(order);
            shipping = order.LineItems.SingleOrDefault(x => x.Type == LineItemType.Shipping);
            Assert.IsTrue(isValid);
            Assert.AreEqual(35M, shipping.Amount);
        }

        // --------------------------------------------------------------------
        // -- Reward Point tests
        // --------------------------------------------------------------------
        [Test]
        public void EachPoint_2Dollars()
        {
            
            var order = new Order(
                new Customer(new DateTime(1980, 1, 1), true),
                "CA",
                PaymentMethod.Visa,
                new DateTime(2015, 11, 27));
            order.Items.Add(new ProductOrder(new Product(100, 2, ProductType.Clothing), 3));

            var processor = new PlanMartOrderProcessor();
            var isValid = processor.ProcessOrder(order);
            var rewards = order.LineItems.SingleOrDefault(x => x.Type == LineItemType.RewardsPoints);

            Assert.IsTrue(isValid);
            Assert.AreEqual(150, rewards.Amount);
        }

        [Test]
        public void DoublePoints_IfUsingRewardsCard()
        {

            var order = new Order(
                new Customer(new DateTime(1980, 1, 1), true),
                "CA",
                PaymentMethod.PlanMartRewardsCard,
                new DateTime(2015, 11, 27));
            order.Items.Add(new ProductOrder(new Product(100, 2, ProductType.Clothing), 3));

            var processor = new PlanMartOrderProcessor();
            var isValid = processor.ProcessOrder(order);
            var rewards = order.LineItems.SingleOrDefault(x => x.Type == LineItemType.RewardsPoints);

            Assert.IsTrue(isValid);
            Assert.AreEqual(300, rewards.Amount);
        }

        [Test]
        public void BlackFridayDayTest()
        {
            Assert.IsTrue(DateUtil.IsABlackFriday(new DateTime(2015, 11, 27)));
            Assert.IsTrue(DateUtil.IsABlackFriday(new DateTime(2015, 12, 18)));
            Assert.IsTrue(DateUtil.IsABlackFriday(new DateTime(2015, 4, 3)));
        }

        [Test]
        public void VeteransDayTest()
        {
            Assert.IsTrue(DateUtil.IsVeteransDay(new DateTime(2015, 11, 11)));
        }

        [Test]
        public void MemorialDayTest()
        {
            Assert.IsTrue(DateUtil.IsMemorialDay(new DateTime(2015, 5, 25)));
        }

        [Test]
        public void DoublePoints_IfMeets3Criteria()
        {
            //  Multiple different products in the same order
            //  Orders over $200 shipped to US regions other than AZ
            //  Orders over $100 shipped to AZ
            //  Orders on:
            //      Any of the 3 recurring Black Fridays
            //      Memorial Day
            //      Veteran’s Day

            var order = new Order(
                new Customer(new DateTime(1980, 1, 1), true),
                "CA",
                PaymentMethod.Visa,
                new DateTime(2015, 11, 11));    // Veteran's day
            order.Items.Add(new ProductOrder(new Product(100, 2, ProductType.Clothing), 3));
            order.Items.Add(new ProductOrder(new Product(100, 2, ProductType.Food), 3));
            
            var processor = new PlanMartOrderProcessor();
            var isValid = processor.ProcessOrder(order);
            var rewards = order.LineItems.SingleOrDefault(x => x.Type == LineItemType.RewardsPoints);

            Assert.IsTrue(isValid);
            Assert.AreEqual(600, rewards.Amount);
        }

        // --------------------------------------------------------------------
        // -- Alcohol tests
        // --------------------------------------------------------------------
        [Test]
        public void AlcoholUnderDrinkingAge_Invalid()
        {
            var birthdate = new DateTime(DateTime.Today.Year - 18, 1, 1);
            var order = new Order(
                new Customer(birthdate, true),
                "AZ",
                PaymentMethod.Visa,
                new DateTime(2015, 11, 27));
            order.Items.Add(new ProductOrder(new Product(20, 2, ProductType.Alcohol), 3));
            order.Items.Add(new ProductOrder(new Product(25, 3, ProductType.Food), 3));

            var processor = new PlanMartOrderProcessor();
            var isValid = processor.ProcessOrder(order);
            Assert.IsFalse(isValid);
        }

        [Test]
        public void AlcoholAboveDrinkingAge_Valid()
        {
            var birthdate = new DateTime(DateTime.Today.Year - 22, 1, 1);
            var order = new Order(
                new Customer(birthdate, true),
                "AZ",
                PaymentMethod.Visa,
                new DateTime(2015, 11, 27));
            order.Items.Add(new ProductOrder(new Product(20, 2, ProductType.Alcohol), 3));
            order.Items.Add(new ProductOrder(new Product(25, 3, ProductType.Food), 3));

            var processor = new PlanMartOrderProcessor();
            var isValid = processor.ProcessOrder(order);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void AlcoholShippingToRestrictedState_Invalid()
        {
            // restricted states: VA, NC, SC, TN, AK, KY, AL

            var birthdate = new DateTime(1970, 1, 1);
            var order = new Order(
                new Customer(birthdate, true),
                "VA",
                PaymentMethod.Visa,
                new DateTime(2015, 11, 27));
            order.Items.Add(new ProductOrder(new Product(20, 2, ProductType.Alcohol), 3));
            order.Items.Add(new ProductOrder(new Product(25, 3, ProductType.Food), 3));

            var processor = new PlanMartOrderProcessor();
            var isValid = processor.ProcessOrder(order);
            Assert.IsFalse(isValid);
        }
        
    }
}