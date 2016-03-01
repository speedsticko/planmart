using System;

namespace PlanMart.Processors
{
    /// <summary>
    /// Your implementation of IOrderProcessor should go here.
    /// </summary>
    public class PlanMartOrderProcessor : IOrderProcessor
    {
        public const decimal TaxRate = 0.08M;
        public const int DrinkingAge = 21;

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

            if (IsMemorialDay(order.Placed) || IsVeteransDay(order.Placed) || IsABlackFriday(order.Placed))
            {
                critera_satisfied += 1;
            }

            return critera_satisfied >= 3;
            
        }

        public static DateTime EasterSunday(int year)
        {
            int day = 0;
            int month = 0;

            int g = year % 19;
            int c = year / 100;
            int h = (c - (int)(c / 4) - (int)((8 * c + 13) / 25) + 19 * g + 15) % 30;
            int i = h - (int)(h / 28) * (1 - (int)(h / 28) * (int)(29 / (h + 1)) * (int)((21 - g) / 11));

            day = i - ((year + (int)(year / 4) + i + 2 - c + (int)(c / 4)) % 7) + 28;
            month = 3;

            if (day > 31)
            {
                month++;
                day -= 31;
            }

            return new DateTime(year, month, day);
        }

        private bool IsABlackFriday(DateTime placed)
        {
            // Black Friday (partying), the last Friday before Christmas
            DateTime black_friday1 = LastDayOfWeekBefore(DayOfWeek.Friday, new DateTime(placed.Year, 12, 25));
            if((placed - black_friday1).Days == 0)
            {
                return true;
            }

            // Black Friday(shopping), the Friday after U.S.Thanksgiving Day.
            DateTime thanksgiving = new DateTime(placed.Year, 11, 1);
            while(thanksgiving.DayOfWeek != DayOfWeek.Thursday)
            {
                thanksgiving = thanksgiving.AddDays(1);
            }
            thanksgiving = thanksgiving.AddDays(21);// jump 3 weeks to 4th Thursday

            DateTime blackfriday2 = thanksgiving.AddDays(1);

            if((placed - blackfriday2).Days == 0)
            {
                return true;
            }

            // Good Friday or Black Friday, a Christian observance of Jesus' crucifixion
            if ((placed - EasterSunday(placed.Year).AddDays(-2)).Days == 0)
            {
                return true;
            }

            return false;
        }

        private bool IsVeteransDay(DateTime placed)
        {
            return placed.Day == 11 && placed.Month == 11;
        }

        private DateTime LastDayOfWeekBefore(DayOfWeek day, DateTime before)
        {
            DateTime tmp_date = before;
            DayOfWeek dayOfWeek = tmp_date.DayOfWeek;
            while (dayOfWeek != DayOfWeek.Monday)
            {
                tmp_date = tmp_date.AddDays(-1);
                dayOfWeek = tmp_date.DayOfWeek;
            }
            return tmp_date;
        }

        private bool IsMemorialDay(DateTime placed)
        {
            // last monday in May 
            DateTime memorial_day = LastDayOfWeekBefore(DayOfWeek.Monday, new DateTime(placed.Year, 5, 31));
            return (memorial_day - placed).Days == 0;
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
                    tax_amount += item_total_cost * TaxRate;
                }

                // -- reward points
                reward_points += (int)(item_total_cost / 2);

                // -- shipping
                weight += item.Product.Weight * item.Quantity;

                // -- alcohol restrictions
                if (item.Product.Type == ProductType.Alcohol)
                {
                    int age = GetCustomerAge(order);

                    if (age < DrinkingAge)
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

        private static int GetCustomerAge(Order order)
        {
            DateTime birth_date = order.Customer.BirthDate;
            DateTime now = DateTime.Today;
            int age = now.Year - birth_date.Year;
            if (now < birth_date.AddYears(age)) age--;
            return age;
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
    }
}