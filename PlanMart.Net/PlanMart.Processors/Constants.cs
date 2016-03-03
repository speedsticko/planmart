using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanMart.Processors
{
    public class Constants
    {
        public const decimal TaxRate = 0.08M;
        public const int DrinkingAge = 21;

        public const decimal ShipFeeUnder20Lbs = 10M;
        public const decimal ShipFee20AndOverLbs = 20M;
        public const decimal ShipFeeNonContinental = 35M;
    }
}
