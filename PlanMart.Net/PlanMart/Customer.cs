using System;

namespace PlanMart
{
    public class Customer
    {
        /// <summary>
        /// The date of birth of the customer 
        /// </summary>
        public DateTime BirthDate { get; }

        /// <summary>
        /// Is the customer representing a non-profit organization
        /// </summary>
        public bool IsNonProfit { get; }

        public Customer(DateTime birthDate, bool isNonProfit)
        {
            BirthDate = birthDate;
            IsNonProfit = isNonProfit;
        }
    }
}