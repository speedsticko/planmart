using System;

namespace PlanMart.Processors
{
    /// <summary>
    /// Your implementation of IOrderProcessor should go here.
    /// </summary>
    public class PlanMartOrderProcessor : IOrderProcessor
    {


        private bool IsProductTaxExempt(Product product, string shipping_region)
        {
            if(product.Type == ProductType.Food)
            {
                return shipping_region == "CA" || shipping_region == "NY";
            }
            else if(product.Type == ProductType.Clothing)
            {
                return shipping_region == "CT";
            }

            return false;
        }

        private bool IsDoubleRewards(Order order)
        {
            if (order.PaymentMethod == PaymentMethod.PlanMartRewardsCard)
            {
                return true;
            }

            decimal order_total = 0;
            ProductType prev_product_type = order.Items[0].Product.Type;
            bool multiple_products = false;
            foreach (var item in order.Items)
            {
                order_total += item.Product.Price * item.Quantity;

                if (item.Product.Type != prev_product_type)
                {
                    multiple_products = true;
                }
            }
            var critera_satisfied = 0;

            if (multiple_products)
            {
                critera_satisfied += 1;
            }

            if (order_total > 200 && order.ShippingRegion != "AZ")
            {
                critera_satisfied += 1;
            }

            if (order_total > 100 && order.ShippingRegion == "AZ")
            {
                critera_satisfied += 1;
            }

            if (DateUtil.IsMemorialDay(order.Placed) || 
                DateUtil.IsVeteransDay(order.Placed) || 
                DateUtil.IsABlackFriday(order.Placed))
            {
                critera_satisfied += 1;
            }

            return critera_satisfied >= 3;
            
        }

        private bool CanShipAlcohol(string shippingRegion)
        {
            return !(shippingRegion == "VA" ||
                shippingRegion == "NC" ||
                shippingRegion == "SC" ||
                shippingRegion == "TN" ||
                shippingRegion == "AK" ||
                shippingRegion == "KY" ||
                shippingRegion == "AL");
        }

        public bool ProcessOrder(Order order)
        {
            // -- sanity checks
            if (order == null)
            {
                return false;
            }

            // -- is empty order?
            if (order.Items.Count == 0)
            {
                return false;
            }

            if (order.Customer == null)
            {
                return false;
            }


            bool is_nonprofit = order.Customer.IsNonProfit;
            decimal tax_amount = 0;
            decimal shipping_amount = 0;
            decimal weight = 0;
            int reward_points = 0;

            foreach (var item in order.Items)
            {
                // -- tax status
                var item_total_cost = item.Product.Price * item.Quantity;
                if (!is_nonprofit && !IsProductTaxExempt(item.Product, order.ShippingRegion))
                {
                    tax_amount += item_total_cost * Constants.TaxRate;
                }

                // -- reward points
                reward_points += (int)(item_total_cost / 2);

                // -- shipping
                weight += item.Product.Weight * item.Quantity;

                // -- alcohol restrictions
                if (item.Product.Type == ProductType.Alcohol)
                {
                    int age = DateUtil.GetAgeToday(order.Customer.BirthDate);

                    if (age < Constants.DrinkingAge)
                    {
                        return false;
                    }
                    if (!CanShipAlcohol(order.ShippingRegion))
                    {
                        return false;
                    }
                }
                // -- food restrictions
                if (item.Product.Type == ProductType.Food && order.ShippingRegion == "HI")
                {
                    return false;
                }
            }


            if (IsDoubleRewards(order))
            {
                reward_points *= 2;
            }

            if (order.ShippingRegion == "AK" || order.ShippingRegion == "HI")
            {
                shipping_amount = 35;
            }
            else if (weight < 20)
            {
                shipping_amount = 10;
            }
            else
            {
                shipping_amount = 20;
            }

            if (tax_amount > 0)
            {
                order.LineItems.Add(new LineItem(LineItemType.Tax, tax_amount));
            }

            order.LineItems.Add(new LineItem(LineItemType.Shipping, shipping_amount));
            order.LineItems.Add(new LineItem(LineItemType.RewardsPoints, reward_points));

            return true;
        }
        
    }
}